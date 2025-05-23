using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
//using UnityEditor.MemoryProfiler;
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

    public enum EnviroMatUsage
    {
        WorkBench = 0,
        Table,
        Stair,
        Box,
        Closet,
        Dresser,
        Chair,
        Lamp
    }

    public BuildMode buildMode = BuildMode.None;

    public FA_UseDir useDir = FA_UseDir.None;

    // 스폰 시 소모할 재료
    [SerializeField] private MaterialType materialType = MaterialType.Wood;

    // BCUI에서 버튼 클릭 시 소모할 재료가 변경되도록 한 프로퍼티
    public MaterialType MaterialTypeP
    {
        get => materialType;
        set
        {
            if(materialType != value)
            {
                materialType = value;
                switch(materialType)
                {
                    case MaterialType.Wood:
                        itemcode = ItemCode.Wood;
                        break;
                    case MaterialType.Stone:
                        itemcode = ItemCode.Ironstone;
                        break;
                    case MaterialType.Iron:
                        itemcode = ItemCode.IronPlanks;
                        break;
                }
            }
        }
    }


    #endregion

    public bool TESTnoUseItem = false;

    public string tagOfHitObject = ""; // 부딪힌 물체의 태그를 저장할 변수

    [Header("벽 생성 Data들")]
    [SerializeField] private float interactDistance = 18.0f; // 건축 상호작용 가능한 최대 거리

    public float lengthMulti = 1.5f; // 생성할 벽의 길이의 곲(반지름)

    public BlockData[] blockDatas; // 생성할 큐브에 사용할 WoodWall 스크립터블 오브젝트

    [Header("Preview Objs")]
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

    private Renderer[] previewRenderers;       //미리보기 오브젝트의 머티리얼을 변경하기 위한 렌더러 변수

    [Header("Enviroment Data")]
    [SerializeField] private EnviromentData[] enviromentDatas = null;
    //[SerializeField] int enviromentIndex = 0;
    [SerializeField] private int enviromentIndex = 0; //임시로 public으로 변경
    public int EnviromentIndex
    {
        get => enviromentIndex;
        set
        {
            if (enviromentIndex != value)
            {
                //회전 체크 초기화
                isRotated = false;

                //enviroData 배열의 길이보다 길면 0
                if(value >= enviromentDatas.Length)
                    enviromentIndex = 0;
                else 
                    enviromentIndex = value;

                EniromentPreview_Setting(enviromentIndex);

            }
        }
    }

    private RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수

    [Header("Ray 관련 변수들")]
    // 겹쳐진 커넥터를 찾을 구의 지름
    private float connectorOverlapRadius = 1;
    [SerializeField] private LayerMask buildObjLayer;

    public bool isHigher = true;        //현재 Ray의 히트 위치가 블록 보다 위인지 아래인지
    public bool isAhead = true;         //현재 Ray의 히트 위치가 블록 보다 앞인지 뒤인지
    public bool isRight = true;         //현재 Ray의 히트 위치가 블록 보다 오른쪽인지 왼쪽인지 판단

    public bool canSpawnObj = true;     //생성가능한지 판단
    [SerializeField] private bool canDespawn = false;
    private Connecting oneConnecting = null;    //현재 포인터에 닿는 커네팅 : 생성 가능한지 판단하기 위해 불러옴

    [SerializeField] private bool isRotated = false;

    [SerializeField] EnviroAdjuster adjuster;
    PlayerInputAction inputAction;

    /// <summary>
    /// 아이템 코드
    /// </summary>
    ItemCode itemcode;

    /// <summary>
    /// 인벤토리
    /// </summary>
    InventoryUI inventoryUI;
    Stage1Manager stage1Manager;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        buildObjLayer = LayerMask.GetMask("BuildObj");
        inventoryUI = FindAnyObjectByType<InventoryUI>();   // 인벤토리UI 참조
    }
    private void Start()
    {
        fa_preview = transform.GetChild(0).gameObject;
        wall_preview_H = transform.GetChild(1).gameObject;
        wall_preview_V = transform.GetChild(2).gameObject;
        enviroment_preview = transform.GetChild(3).gameObject;  //예시용 오브젝트 넣어둠

        stage1Manager = Stage1Manager.Instance;
    }

    #region InputActions

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.SpawnObj.performed += OnSpawnObj;
        inputAction.Player.BuildMode.performed += OnBuildMode;
        inputAction.Player.DespawnObj.performed += OnDespawnObj;
        inputAction.Player.Wheel.performed += OnWheelMouse;
    }


    private void OnDisable()
    {
        inputAction.Player.Wheel.performed -= OnWheelMouse;
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

    /// <summary>
    /// Tab을 눌러 모드 변경
    /// </summary>
    /// <param name="context"></param>
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
                //환경요소일 경우는 Tab으로 다음 환경요소로 넘기기
                EnviromentIndex++;    
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
            if (TESTnoUseItem)
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
                        if (oneConnecting == null || !oneConnecting.isConnectedToFloor)
                        {
                            SpawnBuildObj(blockDatas[0].floorPrefab);
                        }
                        else
                        {
                            Debug.Log("층 생성모드 중 조건에 벗어남");
                        }
                        break;
                    case BuildMode.Enviroment:

                        if (EnviromentIndex == 2)
                        {
                            SpawnBuildObj(enviromentDatas[EnviromentIndex].enviroPrefab, false);
                        }
                        else
                        {
                            //계단을 생성할 때의 조건
                            SpawnBuildObj(enviromentDatas[EnviromentIndex].enviroPrefab, true);
                        }
                        break;

                    case BuildMode.None:
                        //Debug.Log("건축모드가 아닐때 마우스 클릭함");
                        break;
                }
            }
            else
            {
                switch (buildMode)
            {
                case BuildMode.Wall_Horizontal:

                    //재료가 없다면  break문 실행 해서 생성 못함
                    if (!inventoryUI.UseItem(itemcode, 5))
                    {
                            break;
                    }

                    if (!oneConnecting.isConnectedToWall_Ho)
                    {
                        SpawnBuildObj(blockDatas[0].wallPrefab_Ho);
                    }
                    break;

                case BuildMode.Wall_Vertical:

                    //재료가 없다면  break문 실행 해서 생성 못함
                    if (!inventoryUI.UseItem(itemcode, 5))
                    {
                            break;
                    }

                    Quaternion rotation = Quaternion.Euler(0, 90, 0);
                    // 게임 오브젝트 생성과 함께 회전 적용
                    if (!oneConnecting.isConnectedToWall_Ve)
                    {
                       SpawnBuildObj(blockDatas[0].wallPrefab_Ve);
                    }
                    break;

                case BuildMode.Foundation:

                    //재료가 없다면  break문 실행 해서 생성 못함
                    if (!inventoryUI.UseItem(itemcode, 5))
                    {
                            
                        Debug.LogWarning("인벤토리 부족임");

                            break;
                    }
                    #region MyRegion

                
                    //else
                    //{
                    //    Debug.LogWarning("인벤토리 아이템 체크완료");
                    //}

                    //if(oneConnecting == null)
                    //{
                    //    Debug.LogWarning(" oneConnecting 비엇다");
                    //}
                    //else
                    //{
                    //    Debug.LogWarning(" oneConnecting 가 있어서 안됨");
                    //    if (!oneConnecting.isConnectedToFloor)
                    //    {
                    //        Debug.LogWarning("바닥과 연결체크 통과함");
                    //    }
                    //    else
                    //    {
                    //        Debug.LogWarning("바닥과 연결체크 실패함!!");
                    //    }

                    //}


                    #endregion

                    if (oneConnecting == null || !oneConnecting.isConnectedToFloor)
                    {
                       SpawnBuildObj(blockDatas[0].floorPrefab);
                    }
                    else
                    {
                        Debug.Log("층 생성모드 중 조건에 벗어남");
                    }
                    break;
                case BuildMode.Enviroment:

                    //재료가 부족할 시 생성하지 않고 스킵
                    if (!HandleEnviroMatUsage())
                    {
                        stage1Manager.BottomTMPText = ("재료가 부족하다");
                            break;
                    }

                    if (EnviromentIndex == 2) 
                    {
                        SpawnBuildObj(enviromentDatas[EnviromentIndex].enviroPrefab, false);
                    }
                    else
                    {
                        //계단을 생성할 때의 조건
                        SpawnBuildObj(enviromentDatas[EnviromentIndex].enviroPrefab, true);
                    }
                    break;

                case BuildMode.None:
                    //Debug.Log("건축모드가 아닐때 마우스 클릭함");
                    break;
            }
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
            // previewObj의 회전을 가져옴
            Quaternion currentRotation = previewObj.transform.rotation;

            // (추가적으로) 생성된 오브젝트의 회전을 현재 오브젝트와 동일하게 설정
            newObj.transform.rotation = currentRotation;
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

            //계단이 아닐때만 벽 연결조건 체크
            if(buildMode != BuildMode.Enviroment)
            {
                foreach (Connecting connecting in newObj.GetComponentsInChildren<Connecting>())
                {
                    connecting.UpdateConnecting(true);
                }
            }
            else
            {          
                // 계단이라면 회전 적용
                Quaternion currentRotation = previewObj.transform.rotation;

                // (추가적으로) 생성된 오브젝트의 회전을 현재 오브젝트와 동일하게 설정
                newObj.transform.rotation = currentRotation;
                return;

            }
        }
    }

    /// <summary>
    /// 마우스 휠 액션 시 계단 회전
    /// </summary>
    /// <param name="context"></param>
    private void OnWheelMouse(InputAction.CallbackContext context)
    {
        if (buildMode == BuildMode.Enviroment)
        {
            Vector2 wheelVec = context.ReadValue<Vector2>();
            Transform enviroChild = transform.GetChild(3);

            isRotated = !isRotated;

            //위로 휠 했을 때
            if (wheelVec.y > 0)
            {
                // 현재 프리팹 오브젝트의 회전 가져오기
                Quaternion currentRotation = enviroChild.rotation;

                // (0, 90, 0) 회전 값을 쿼터니언으로 만들기
                Quaternion additionalRotation = Quaternion.Euler(0, 90, 0);

                // 현재 회전에 새로운 회전을 더하기
                enviroChild.rotation = currentRotation * additionalRotation;
            }
            else
            {
                // 현재 프리팹 오브젝트의 회전 가져오기
                Quaternion currentRotation = enviroChild.rotation;

                // (0, 90, 0) 회전 값을 쿼터니언으로 만들기
                Quaternion additionalRotation = Quaternion.Euler(0, -90, 0);

                // 현재 회전에 새로운 회전을 더하기
                enviroChild.rotation = currentRotation * additionalRotation;
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

    ///                          --||--------------------------------------\\------------------------------------------||----------------
    ///   --------//--------  -----||----------- Update 문-------------------\\------------------------------------------||----------------------------------------
    ///                          --||--------------------------------------\\------------------------------------------||----------------
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
                if(hit.collider.gameObject.transform.parent != null)
                {
                    if (hit.collider.gameObject.transform.parent.CompareTag("Untagged"))
                    {
                        canDespawn = false;
                    }
                    else
                    {
                        canDespawn = true;
                    }
                }
                else
                {
                    if (hit.collider.gameObject.CompareTag("Respawn"))
                    {
                        canDespawn = true;
                    }
                    else
                    {
                        canDespawn = false;
                    }
                }

                //환경요소에 닿을 때 생성 불가 , 태그 : Respawn
                PreviewMatSelect((tagOfHitObject != "Respawn"));

                adjuster = hit.collider.GetComponent<EnviroAdjuster>();
                if(adjuster != null) 
                    EnviroAdjuset(adjuster.CenterVec, hit.point, isRotated);

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
        if(previewObj != null)
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
            PreviewMatSelect(false);

            if (hit.collider.transform.root.CompareTag("Ground") && buildMode == BuildMode.Foundation)   //만약 이 때 땅에 닿고있다면
            {
                PreviewMatSelect(true);
            }
        }
       if(connecting != null)
        {
            PreviewMatSelect(true);
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
                        PreviewMatSelect(false);
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
                        PreviewMatSelect(false);
                        break;
                    }
                    if (isHigher)
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
                        PreviewMatSelect(false); break;
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
        canSpawnObj = isSpawnable;
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
    /// <param name="checkObjPosition">현재 바닥의 포지션(원점) : 정 가운데 </param>
    /// <param name="prefabPosition">현재 ray가 닿는 미리보기 프리팹의 위치</param>
    /// <param name="isRotated">오브젝트를 회전 시켰는지 확인하는 변수</param>
    void EnviroAdjuset(Vector3 checkObjPosition, Vector3 prefabPosition, bool isRotated)
    {
        //바닥의 길이
        float lengthMultiX = lengthMulti;
        float lengthMultiZ = lengthMulti;

        float radiusX = 0;
        float radiusZ = 0;

        // 두 오브젝트 간의 x 길이와 z 길이 계산(ray의 위치 - 중심 위치)
        float deltaX = prefabPosition.x - checkObjPosition.x;
        float deltaZ = prefabPosition.z - checkObjPosition.z;

        //radius - (1.5 - 길이) 현재는 x,z길이 모두 정사각형으로 퉁쳐서 radius가 하나임
        if (!isRotated)
        {
            radiusX = enviromentDatas[enviromentIndex].radiusX;
            radiusZ = enviromentDatas[enviromentIndex].radiusZ;
        }
        else
        {
            radiusX = enviromentDatas[enviromentIndex].radiusZ;
            radiusZ = enviromentDatas[enviromentIndex].radiusX;
        }

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

    /// <summary>
    /// Enviroment 생성 시 소모할 재료를 확인해서 가능한지 판단하는 함수
    /// </summary>
    /// <returns> 가능하다면 소모 후 true, 안되면 false </returns>
    private bool HandleEnviroMatUsage()
    {
        bool result = false;
        EnviroMatUsage usage = (EnviroMatUsage)EnviromentIndex;

        switch (usage)
        {
            case EnviroMatUsage.WorkBench:
                // WorkBench에 대한 처리 코드 / 나무 6개, 돌 4개
                result = ( (inventoryUI.UseItem(ItemCode.Wood, 6)) 
                        && (inventoryUI.UseItem(ItemCode.Ironstone, 4)) );
                break;
            case EnviroMatUsage.Table:
                // Table에 대한 처리 코드
                result = inventoryUI.UseItem(ItemCode.Wood, 5);
                break;
            case EnviroMatUsage.Stair:
                // Stair에 대한 처리 코드 // 현재 사용 중인 벽의 재료 5개 소모
                result = inventoryUI.UseItem(itemcode, 5);
                break;
            case EnviroMatUsage.Box:
                // Box에 대한 처리 코드
                result = inventoryUI.UseItem(ItemCode.Wood, 3);
                break;
            case EnviroMatUsage.Closet:
                // Closet에 대한 처리 코드
                result = inventoryUI.UseItem(ItemCode.Wood, 10);
                break;
            case EnviroMatUsage.Dresser:
                // Dresser에 대한 처리 코드
                result = inventoryUI.UseItem(ItemCode.Wood, 4);
                break;
            case EnviroMatUsage.Chair:
                // Chair에 대한 처리 코드
                result = inventoryUI.UseItem(ItemCode.Ironstone, 4);
                break;
            case EnviroMatUsage.Lamp:
                // Lamp에 대한 처리 코드
                result = inventoryUI.UseItem(ItemCode.Ironstone, 4)
                        && inventoryUI.UseItem(ItemCode.IronPlanks, 2);
                break;
        }

        return result;
    }


}