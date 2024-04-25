using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static BlockSpwaner;
using static Connecting;

public class BlockSpwaner : MonoBehaviour
{
    #region enum타입 변수들
    public enum BuildMode   //건축 모드
    {
        None = 0,
        Foundation,
        Wall_Horizontal,
        Wall_Vertical,
        Enviroment
    };

    public enum FA_UseDir   //토대 설치시 주변에 토대가 있는 지 확인 하는 용도
    {
        None = 0,
        Forward ,
        Back,
        Left,
        Right,
        All = int.MaxValue
    }
    public enum MaterialType
    {
        Wood = 0,
        Stone,
        Iron
    }
    public BuildMode buildMode = BuildMode.Foundation;

    //public HitType hitType = HitType.None;
    public FA_UseDir useDir = FA_UseDir.None;
    public MaterialType materialType = MaterialType.Wood;

    #endregion


    public string tagOfHitObject = ""; // 부딪힌 물체의 태그를 저장할 변수

    public BlockData[] blockDatas; // 생성할 큐브에 사용할 WoodWall 스크립터블 오브젝트
    //float lengthMul = 3f; // 생성할 벽의 길이(구버전)
    public float lengthMulti = 1.5f; // 생성할 벽의 길이의 곲(반지름)

    [SerializeField] private float interactDistance = 18.0f; // 건축 상호작용 가능한 최대 거리


    /// <summary>
    /// buildmode가 foundation일때 반투명하게 미리 위치를 보여주는 오브젝트--------------------------
    /// </summary>
    public GameObject fa_preview;
    public GameObject wall_preview_H;
    public GameObject wall_preview_V;
    public GameObject enviroment_preview;
    public GameObject previewObj;

    //생성 가능/불가능 시 미리보기 옵젝에 씌울 적/녹색 머테리얼------------------------------------------
    public Material SpawnAbledMat;
    public Material SpawnDisabledMat;

    Renderer[] previewRenderers;       //미리보기 오브젝트의 머티리얼을 변경하기 위한 렌더러 변수

    [SerializeField]
    EnviromentData[] enviromentDatas = null;
    //[SerializeField] int enviromentIndex = 0;
    [SerializeField] int enviromentIndex = 0; //임시로 public으로 변경
    public int EnviromentIndex
    {
        get => enviromentIndex;
        set
        {
            if (enviromentIndex != value)
            {
                //enviroData 배열의 길이보다 길면 0
                if(value >= enviromentDatas.Length)
                    enviromentIndex = 0;
                else 
                    enviromentIndex = value;

                EniromentPreview_Setting(enviromentIndex);
                //enviroment_preview = 
            }
        }
    }

    RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수

    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private LayerMask buildObjLayer;

    public bool isHigher = true;        //현재 Ray의 히트 위치가 블록 보다 위인지 아래인지
    public bool isAhead = true;         //현재 Ray의 히트 위치가 블록 보다 앞인지 뒤인지
    public bool isRight = true;         //현재 Ray의 히트 위치가 블록 보다 오른쪽인지 왼쪽인지 판단

    public bool canSpawnObj = true;     //생성가능한지 판단
    [SerializeField] bool canDespawn = false;
    Connecting oneConnecting = null;    //현재 포인터에 닿는 커네팅 : 생성 가능한지 판단하기 위해 불러옴

    [SerializeField] EnviroAdjuster adjuster;

    PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        buildObjLayer = LayerMask.GetMask("BuildObj");
    }

    #region InputActions

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.SpawnObj.performed += OnSpawnObj;
        inputAction.Player.BuildMode.performed += OnBuildMode;
        inputAction.Player.DespawnObj.performed += OnDespawnObj;
    }


    private void OnDisable()
    {
        inputAction.Player.DespawnObj.performed -= OnDespawnObj;
        inputAction.Player.BuildMode.performed -= OnBuildMode;
        inputAction.Player.SpawnObj.performed -= OnSpawnObj;
        inputAction.Disable();
    }

    /// <summary>
    /// 마우스 우클릭시 블록 삭제
    /// </summary>
    /// <param name="context"></param>
    private void OnDespawnObj(InputAction.CallbackContext context)
    {
        if(buildMode != BuildMode.None && canDespawn)
        {
            Destroy(hit.collider.transform.root.gameObject);
        }
    }
    private void OnBuildMode(InputAction.CallbackContext context)
    {
        //UI로 변경하기 위해 임시로 막아둠
        switch (buildMode)
        {
            case BuildMode.Wall_Horizontal:
                buildMode = BuildMode.Wall_Vertical;
                break;

            case BuildMode.Wall_Vertical:
                buildMode = BuildMode.Foundation;
                break;

            case BuildMode.Foundation:
                buildMode = BuildMode.Wall_Horizontal;
                break;
            case BuildMode.Enviroment:
                EnviromentIndex++;    
                //EnviromentIndex %= enviromentDatas.Length;    //프로퍼티로는 작동이 안됨 + 인덱스범위를 넘은 뒤에 돌아와서 문제가있음
                break;
            default:
                buildMode = BuildMode.None;
                break;
        }
    }

    /// <summary>
    /// 마우스 좌클릭 시 블록 생성
    /// </summary>
    /// <param name="_"></param>
    private void OnSpawnObj(InputAction.CallbackContext _)
    {
        if (canSpawnObj)
        {
            
            switch (buildMode)
            {
                case BuildMode.Wall_Horizontal:
                    if (!oneConnecting.isConnectedToWall_Ho)
                    {
                        SpawnBuildObj(blockDatas[0].wallPrefab_Ho);
                    }
                    break;

                case BuildMode.Wall_Vertical:
                    Quaternion rotation = Quaternion.Euler(0, 90, 0);
                    // 게임 오브젝트 생성과 함께 회전 적용
                    if (!oneConnecting.isConnectedToWall_Ve)
                    {
                       SpawnBuildObj(blockDatas[0].wallPrefab_Ve);
                    }
                    break;

                case BuildMode.Foundation:
                    if (oneConnecting == null|| !oneConnecting.isConnectedToFloor)
                    {
                       SpawnBuildObj(blockDatas[0].floorPrefab);
                    }
                    else
                    {
                        Debug.Log("층 생성모드 중 조건에 벗어남");
                    }
                    break;
                case BuildMode.Enviroment:
                    if(canSpawnObj)
                        SpawnBuildObj(enviromentDatas[EnviromentIndex].enviroPrefab, true);
                    //Debug.Log("환경요소는 따로 추가해야함");
                    break;
                case BuildMode.None:
                    Debug.Log("건축모드가 아닐때 마우스 클릭함");
                    break;
            }
        }
            oneConnecting = null;
    }

    /// <summary>
    /// 블록 생성 시 Instance로 게임 오브젝트 생성하는 함수
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="isEnviroment"></param>
    void SpawnBuildObj(GameObject prefab, bool isEnviroment = false)
    {
        GameObject newObj = Instantiate(prefab, previewObj.transform.position, Quaternion.identity);
        if (isEnviroment)
        {

            return;
        }
        else
        {   //벽의 머테리얼 설정
            Renderer renderer = newObj.transform.GetChild(0).GetComponent<Renderer>();
            switch (materialType)
            {
                case MaterialType.Wood:
                    renderer.material = blockDatas[0].blockMaterial;
                    break;
                case MaterialType.Stone:
                    renderer.material = blockDatas[1].blockMaterial;
                    break;
                case MaterialType.Iron:
                    renderer.material = blockDatas[2].blockMaterial;
                    break;
            }

            foreach (Connecting connecting in newObj.GetComponentsInChildren<Connecting>())
            {
                connecting.UpdateConnecting(true);
            }
        }
    }

    /// <summary>
    /// UI활성화 시 생성을 막는 함수 BCUI에서 사용
    /// </summary>
    /// <param name="isActive"></param>
    public void ProhibitSpawn(bool isActive)
    {
        if(isActive)
        {
            inputAction.Disable();
        }
        else
        {
            inputAction.Enable();
        }
    }
    #endregion

    private void Start()
    {
        fa_preview = transform.GetChild(0).gameObject;
        wall_preview_H = transform.GetChild(1).gameObject;
        wall_preview_V = transform.GetChild(2).gameObject;
        enviroment_preview = transform.GetChild(3).gameObject;  //예시용 오브젝트 넣어둠
    }

    // Update 문------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        // 환경요소 생성 모드일때
        if (buildMode == BuildMode.Enviroment)
        {
            //EnvrioAble 레이어인 물체만 판단한다.
            if (Physics.Raycast(ray, out hit, interactDistance, LayerMask.GetMask("EnvrioAble")))
            {
                // 부딪힌 물체의 태그를 가져옴 디버그용 = 확인용
                tagOfHitObject = hit.collider.gameObject.tag;

                //프리뷰 오브젝트 레이캐스트 닿는 위치로 이동
                //previewObj.transform.position = hit.point;

                //환경요소에 닿을 때 제거 가능
                canDespawn = true;

                //환경요소에 닿을 때 생성 불가 , 태그 : Respawn
                if (tagOfHitObject == "Respawn")
                {
                    canSpawnObj = false;
                }
                else
                {
                    canSpawnObj = true;
                }

                //프리뷰 색상 함수
                PreviewMatSelect(canSpawnObj);

                adjuster = hit.collider.GetComponent<EnviroAdjuster>();
                if(adjuster != null) 
                    EnviroAdjuset(adjuster.CenterVec, hit.point);
                
            }
            else
            {
                //그 외 제거 불가능
                canDespawn = false;
            }
        }
        // 환경요소 생성 모드가 아닐 때
        else
        {
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // 부딪힌 물체의 태그를 가져옴 디버그용 = 확인용
                tagOfHitObject = hit.collider.gameObject.tag;

                //생성 가능 물체에 닿을 때 제거 가능 / 다른 태그일 때는 제거 불가
                if (hit.collider.CompareTag("Buildables"))
                    canDespawn = true;
                else
                    canDespawn = false;

                switch (buildMode)
                {
                    case BuildMode.None:
                        //preview 미리보기 숨기기
                        Preview_Hide();
                        break;
                    case BuildMode.Foundation:
                        //생성될 위치 미리보기
                        Preview_Setting(fa_preview);
                        previewObj.transform.position = hit.point;
                        ColliderSearch();
                        break;
                    case BuildMode.Wall_Horizontal:
                        Preview_Setting(wall_preview_H);
                        previewObj.transform.position = hit.point;
                        ColliderSearch();
                        break;
                    case BuildMode.Wall_Vertical:
                        Preview_Setting(wall_preview_V);
                        previewObj.transform.position = hit.point;
                        ColliderSearch();
                        break;
                }
            }
            else
            {
                //ray가 현재 아무것도 닿지 않을때
                canDespawn = false;
            }
        }
    }

    /// <summary>
    /// 환경요소용 미리보기 프리팹 세팅
    /// </summary>
    /// <param name="index">EnviromentData배열의 인덱스 BCUI와 MaterialSelectUI에서 액션으로 보냄</param>
    void EniromentPreview_Setting(int index)
    {
        GameObject selectEnviro = Instantiate(enviromentDatas[index].previewPrefab, transform);
        enviroment_preview = selectEnviro;
        Preview_Setting(enviroment_preview);

        //이전 환경요소 프리뷰 삭제
        Transform enviroChild = transform.GetChild(3);
        if (enviroChild != null && enviroChild.gameObject != selectEnviro)
        {
            Debug.Log(enviroChild.gameObject.name);
        }
        else
        {
            Debug.Log("못찾앗어");
        }
        Destroy(enviroChild.gameObject);
    }

    /// <summary>
    /// 미리보기 세팅
    /// </summary>
    /// <param name="select"></param>
    public void Preview_Setting(GameObject select)
    {
        fa_preview.SetActive(false);
        wall_preview_H.SetActive(false);
        wall_preview_V.SetActive(false);
        enviroment_preview.SetActive(false);
        previewObj = select;
        select.SetActive(true);
    }

    /// <summary>
    /// 모든 미리보기 안보이게 - 건축모드가 아닐때 쓰는 함수
    /// </summary>
    public void Preview_Hide()
    {
        previewObj?.SetActive(false);
        fa_preview.SetActive(false);
        wall_preview_H.SetActive(false);
        wall_preview_V.SetActive(false);
        enviroment_preview.SetActive(false);
    }

    /// <summary>
    /// 벽이 생성가능한지 판단하는 함수
    /// </summary>
    void ColliderSearch()
    {
        //미리보기의 위치를 기준으로 반지름이 1인 구 범위 내에 콜라이더를 수집
        Collider[] colliders = Physics.OverlapSphere(previewObj.transform.position, connectorOverlapRadius, buildObjLayer);

        Connecting connecting = null;

        foreach (Collider collider in colliders)    //콜라이더 배열에 COnnecting 에서 생성가능한지 판단하는 bool 체크
        {
            Connecting tempConnecting = null;
            tempConnecting = collider.GetComponent<Connecting>();
            if (tempConnecting != null)
            {
                if (tempConnecting.canBuild)
                {
                    connecting = tempConnecting;
                    break;
                }
            }
        }
        //생성 불가 조건
        if(connecting == null || buildMode == BuildMode.Foundation && connecting.isConnectedToFloor || buildMode == BuildMode.Wall_Horizontal && connecting.isConnectedToWall_Ho 
                                                                                                    || buildMode == BuildMode.Wall_Vertical && connecting.isConnectedToWall_Ve  )
        {
            canSpawnObj = false;
            PreviewMatSelect(canSpawnObj);

            if (hit.collider.transform.root.CompareTag("Ground") && buildMode == BuildMode.Foundation)   //만약 이 때 땅에 닿고있다면
            {
                canSpawnObj = true;
                PreviewMatSelect(canSpawnObj);
            }
        }
       if(connecting != null)
        {
            canSpawnObj = true;
            PreviewMatSelect(canSpawnObj);
            oneConnecting = connecting;
            Check_ConnectingToHitDir(connecting);
            SpawnPositionSelect(connecting);

        }

    }
    /// <summary>
    /// 현재 Ray가 Connecting의 위치와 비교해서 어디에 있는지 판단하는 함수
    /// </summary>
    /// <param name="connecting"></param>
    void Check_ConnectingToHitDir(Connecting connecting)
    {
        if (hit.point.x > connecting.transform.position.x)
            isRight = true;
        else
            isRight = false;
        if (hit.point.y > connecting.transform.position.y)
            isHigher = true;
        else
            isHigher = false;

        if(hit.point.z > connecting.transform.position.z)
            isAhead = true;
        else
            isAhead = false;
    }  
    /// <summary>
    /// 스폰될 벽 또는 층의 위치를 결정하는 함수
    /// </summary>
    /// <param name="connecting">현재 Ray에 닿은 Connecting이 생성가능할때 사용 </param>
    void SpawnPositionSelect(Connecting connecting)
    {
        ///Transform previewConnector_Tr = connecting.transform;
        ///previewObj.transform.position = connecting.transform.position - (previewConnector_Tr.position - previewObj.transform.position);
        //Debug.Log(connecting.name);
        //Floor에 생성할때
        if (connecting.objType == ObjType.Floor)    
        {   //foundation 생성모드일때
            switch (buildMode)
            {
                case BuildMode.Foundation:
                    if (connecting.usedDir == UsedDir.Right)
                    {
                        //Debug.Log("층의 오른쪽 커넷팅임");
                        previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Left)
                    {
                        //Debug.Log("층의 왼쪽이다");
                        previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Forward)
                    {
                       // Debug.Log("층 앞쪽임 Forward");
                        previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Back)
                    {
                       // Debug.Log("층 뒤야 BBBack");
                        previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                    }
                    break;

                case BuildMode.None:// Debug.Log("건축모드가 아니다 = 패스");
                                    break;
                //벽 생성 모드다
                case BuildMode.Wall_Horizontal:
                    if (connecting.usedDir == UsedDir.Left || connecting.usedDir == UsedDir.Right)
                    {
                        canSpawnObj = false;
                        PreviewMatSelect(canSpawnObj);
                        break;
                    }
                    if (isHigher)
                        previewObj.transform.position = connecting.transform.position + Vector3.up * lengthMulti;
                    else
                        previewObj.transform.position = connecting.transform.position + Vector3.down * lengthMulti;
                    break;

                case BuildMode.Wall_Vertical:
                    if (connecting.usedDir == UsedDir.Forward || connecting.usedDir == UsedDir.Back)
                    {
                        canSpawnObj = false;
                        PreviewMatSelect(canSpawnObj);
                        break;
                    }
                    if (isRight)
                        previewObj.transform.position = connecting.transform.position + Vector3.up * lengthMulti;
                    else
                        previewObj.transform.position = connecting.transform.position + Vector3.down * lengthMulti;
                    break;
            }
        }
        //벽에 생성할때
        else if (connecting.objType == ObjType.Wall_Ho || connecting.objType == ObjType.Wall_Ve)
        {
            //Debug.Log("벽에 닿고 있다");
            switch (buildMode)
            {
                case BuildMode.Foundation:
                    //벽의 위 아래를 제외한 곳에선 생성 불가
                    if (connecting.usedDir == UsedDir.Left || connecting.usedDir == UsedDir.Right || connecting.usedDir == UsedDir.Forward || connecting.usedDir == UsedDir.Back)
                    {
                        canSpawnObj = false;
                        PreviewMatSelect(canSpawnObj); break;
                    }
                    switch (connecting.objType)
                    {
                        case ObjType.Wall_Ho:
                            if (isAhead)
                                previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                            else
                                previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                            break;
                        case ObjType.Wall_Ve:
                            if (isAhead)
                                previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                            else
                                previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;
                            break;
                    }
                    break;

                case BuildMode.None: break;
                //벽 생성 모드다
                case BuildMode.Wall_Horizontal:
                    if (connecting.usedDir == UsedDir.Right)
                    {
                        //Debug.Log("벽의 오른쪽 커넷팅임");
                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Left)
                    {
                       // Debug.Log("벽의 왼쪽이다");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Top)
                    {
                       // Debug.Log("벽의 위위위---");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.up * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Bottom)
                    {
                      //  Debug.Log("벽의 아래");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.down * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Forward || connecting.usedDir == UsedDir.Back)
                    {
                      //  Debug.Log("벽의 앞이랑 뒤쪽임 ");
                        if(isRight)
                            previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                        else
                            previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;

                    }
                    else
                    {
                      //  Debug.Log("어느방향도 설정되지 않았다.");
                    }
                    break;
                case BuildMode.Wall_Vertical:
                    if (connecting.usedDir == UsedDir.Right || connecting.usedDir == UsedDir.Left)
                    {
                      //  Debug.Log("벽의 오른쪽,왼쪽 커넷팅임");
                        Debug.Log(connecting.name);
                        if(isAhead)
                            previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                        else
                            previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Top)
                    {
                      //  Debug.Log("벽의 위위위---");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.up * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Bottom)
                    {
                      //  Debug.Log("벽의 아래");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.down * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Forward)
                    {
                      //  Debug.Log("앞쪽임 Forward");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Back)
                    {
                      //  Debug.Log("뒤야 BBBack");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                    }
                    else
                    {
                      //  Debug.Log("어느방향도 설정되지 않았다.");
                    }
                    break;
            }
        }
    }

    /// <summary>
    ///  미리보기의 머테리얼 색상 정하는 함수
    /// </summary>
    /// <param name="isSpawnable">생성 가능할때 true</param>
    void PreviewMatSelect(bool isSpawnable)    
    {

        if (isSpawnable)
        {
            previewRenderers = previewObj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in previewRenderers)
            {
                renderer.material = SpawnAbledMat;
            }
        }
        else
        {
            previewRenderers = previewObj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in previewRenderers)
            {
                renderer.material = SpawnDisabledMat;
            }
        }
    }

    /// <summary>
    /// Enviro의 튀어나온 면을 floor안쪽으로 넣는 함수
    /// </summary>
    /// <param name="checkObjPosition">현재 바닥의 포지션(원점)</param>
    /// <param name="prefabPosition">현재 ray가 닿는 미리보기 프리팹의 위치</param>
    void EnviroAdjuset(Vector3 checkObjPosition, Vector3 prefabPosition)
    {
        float lengthMultiX = lengthMulti;
        float lengthMultiZ = lengthMulti;


        // 두 오브젝트 간의 x 길이와 z 길이 계산(ray의 위치 - 중심 위치)
        float deltaX = prefabPosition.x - checkObjPosition.x;
        float deltaZ = prefabPosition.z - checkObjPosition.z;

        //radius - (1.5 - 길이) 현재는 x,z길이 모두 정사각형으로 퉁쳐서 radius가 하나임
        float radiusX = enviromentDatas[enviromentIndex].radiusX;
        float radiusZ = enviromentDatas[enviromentIndex].radiusZ;

        float newX = 0, newZ = 0;

        //right, left
        if (deltaX > 0)
        {
            float adjX = radiusX + deltaX;
            float checkX = lengthMultiX - adjX;
            if (checkX > 0 || adjuster.isConRight)       
            {
                //벗어나지 않음
                newX = hit.point.x;
            }
            else
            {
                //밖으로 티어나옴 
                newX = hit.point.x + checkX; //마이너스 값이라 더하면 빼짐
   
            }
        }
        else if (deltaX < 0)
        {
            float adjX = radiusX - deltaX;
            float checkX = lengthMultiX - adjX;
            if (checkX > 0 || adjuster.isConLeft)
            {
                //벗어나지 않음
                newX = hit.point.x;
            }
            else
            {
                //밖으로 티어나옴 
                newX = hit.point.x - checkX; //마이너스 값이라 빼면 더해짐

            }
        }

        //forward, back
        if (deltaZ > 0)
        {
            float adjZ = radiusZ + deltaZ;
            float checkZ = lengthMultiZ - adjZ;
            if (checkZ > 0 || adjuster.isConForward)
            {
                //벗어나지 않음
                newZ = hit.point.z;
            }
            else
            {
                //밖으로 티어나옴  
                newZ = hit.point.z + checkZ; //마이너스 값이라 더하면 빼짐
            }
        }
        else if (deltaZ < 0)
        {
            float adjZ = radiusZ - deltaZ;
            float checkZ = lengthMultiZ - adjZ;
            if (checkZ > 0 || adjuster.isConBack)
            {
                //벗어나지 않음
                newZ = hit.point.z;
            }
            else
            {
                //밖으로 티어나옴  
                newZ = hit.point.z - checkZ; //마이너스 값이라 빼면 더해짐
            }
        }

        //위 길이 만큼 안쪽으로 중심을 이동 시킨다.
        previewObj.transform.position = new Vector3(newX, hit.point.y, newZ);

        Debug.Log($"생성될 X : {newX}, 생성될 z : {newZ}");
    }
      

    

}