using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static BlockSpwaner;
using static Connecting;

public class BlockSpwaner : MonoBehaviour
{
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
    public BuildMode buildMode = BuildMode.None;
    //public HitType hitType = HitType.None;
    public FA_UseDir useDir = FA_UseDir.None;
    public string tagOfHitObject = ""; // 부딪힌 물체의 태그를 저장할 변수

    public WallData woodWallData; // 생성할 큐브에 사용할 WoodWall 스크립터블 오브젝트
    public FoundationData foundationData;
    float lengthMul = 3f; // 생성할 벽의 길이(구버전)
    public float lengthMulti = 1.5f; // 생성할 벽의 길이의 곲

    public float interactDistance = 11.0f; // 건축 상호작용 가능한 최대 거리

    public Material SpawnAbledMat;
    public Material SpawnDisabledMat;

    [SerializeField] Vector3 downSpawnPoint = new Vector3 (0, 0.8f, 0); //토대의 크기에 맞게 약간 하단에 생성
    [SerializeField] Vector3 testVec = Vector3.zero;            //확인용 변수


    RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수
    /// <summary>
    /// buildmode가 foundation일때 반투명하게 미리 위치를 보여주는 오브젝트
    /// </summary>
    public GameObject fa_preview;
    public GameObject wall_preview_H;
    public GameObject wall_preview_V;
    public GameObject previewObj;
    Renderer previewRenderer;

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
    }

    private void OnBuildMode(InputAction.CallbackContext context)
    {
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
            default:
                buildMode = BuildMode.Foundation;
                break;
        }
    }


    private void OnSpawnObj(InputAction.CallbackContext context)
    {
        if (canSpawnObj)
        {
            
            switch (buildMode)
            {
                case BuildMode.Wall_Horizontal:
                    if (!oneConnecting.isConnectedToWall_Ho)
                    {
                        SpawnBuildObj(woodWallData.wallPrefab_Ho);
                    }
                    break;

                case BuildMode.Wall_Vertical:
                    Quaternion rotation = Quaternion.Euler(0, 90, 0);
                    // 게임 오브젝트 생성과 함께 회전 적용
                    if (!oneConnecting.isConnectedToWall_Ve)
                    {
                        SpawnBuildObj(woodWallData.wallPrefab_Ve);
                    }
                    break;

                case BuildMode.Foundation:
                    if (!oneConnecting.isConnectedToFloor)
                    {
                        SpawnBuildObj(foundationData.foundationPrefab);
                    }
                    break;
                default:
                    Debug.Log("건축모드가 아닐때 마우스 클릭함");
                    break;
            }
        }
            oneConnecting = null;
    }

    void SpawnBuildObj(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab, previewObj.transform.position, Quaternion.identity);
        foreach (Connecting connecting in newObj.GetComponentsInChildren<Connecting>())
        {
            connecting.UpdateConnecting(true);
        }
    }

    private void OnDisable()
    {
        inputAction.Player.BuildMode.performed -= OnBuildMode;
        inputAction.Player.SpawnObj.performed -= OnSpawnObj;
        inputAction.Disable();
    }

    #endregion
    private void Start()
    {
        fa_preview = transform.GetChild(0).gameObject;
        wall_preview_H = transform.GetChild(1).gameObject;
        wall_preview_V = transform.GetChild(2).gameObject;
    }


    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 부딪힌 물체의 태그를 가져옴 디버그용 = 확인용
            tagOfHitObject = hit.collider.gameObject.tag;
            switch (buildMode)
            {
                case BuildMode.None:
                    //preview 미리보기 숨기기
                    Preview_Hide();
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
                case BuildMode.Foundation:
                    //생성될 위치 미리보기
                    Preview_Setting(fa_preview);
                    previewObj.transform.position = hit.point;

                    ColliderSearch();


                    break;
            }
        }
    }
    public void Preview_Setting(GameObject select)
    {
        fa_preview.SetActive(false);
        wall_preview_H.SetActive(false);
        wall_preview_V.SetActive(false);
        previewObj = select;
        select.SetActive(true);
    }
    public void Preview_Hide()
    {
        previewObj.SetActive(false);
        fa_preview.SetActive(false);
        wall_preview_H.SetActive(false);
        wall_preview_V.SetActive(false);
    }


    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private LayerMask buildObjLayer;
    public bool isHigher = true;
    public bool isAhead = true;
    public bool isRight = true;
    public bool canSpawnObj = true;     //생성가능한지 판단
    Connecting oneConnecting = null;    //현재 포인터에 닿는 커네팅 : 생성 가능한지 판단하기 위해 불러옴


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
                    Debug.Log("있다");
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
        Debug.Log(connecting.name);
        //Floor에 생성할때
        if (connecting.objType == ObjType.Floor)    
        {   //foundation 생성모드일때
            Debug.Log("층에 닿고있다");
            switch (buildMode)
            {
                case BuildMode.Foundation:
                    Debug.Log("durl");
                    if (connecting.usedDir == UsedDir.Right)
                    {
                        Debug.Log("층의 오른쪽 커넷팅임");
                        previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Left)
                    {
                        Debug.Log("층의 왼쪽이다");
                        previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Forward)
                    {
                        Debug.Log("층 앞쪽임 Forward");
                        previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Back)
                    {
                        Debug.Log("층 뒤야 BBBack");
                        previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                    }
                    break;

                case BuildMode.None: Debug.Log("건축모드가 아니다 = 패스");  break;
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
            Debug.Log("벽에 닿고 있다");
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

                case BuildMode.None: Debug.Log("건축모드가 아니다 = 패스"); break;
                //벽 생성 모드다
                case BuildMode.Wall_Horizontal:
                    if (connecting.usedDir == UsedDir.Right)
                    {
                        Debug.Log("벽의 오른쪽 커넷팅임");
                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Left)
                    {
                        Debug.Log("벽의 왼쪽이다");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Top)
                    {
                        Debug.Log("벽의 위위위---");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.up * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Bottom)
                    {
                        Debug.Log("벽의 아래");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.down * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Forward || connecting.usedDir == UsedDir.Back)
                    {
                        Debug.Log("벽의 앞이랑 뒤쪽임 ");
                        if(isRight)
                            previewObj.transform.position = connecting.transform.position + Vector3.right * lengthMulti;
                        else
                            previewObj.transform.position = connecting.transform.position + Vector3.left * lengthMulti;

                    }
                    else
                    {
                        Debug.Log("어느방향도 설정되지 않았다.");
                    }
                    break;
                case BuildMode.Wall_Vertical:
                    if (connecting.usedDir == UsedDir.Right || connecting.usedDir == UsedDir.Left)
                    {
                        Debug.Log("벽의 오른쪽,왼쪽 커넷팅임");
                        Debug.Log(connecting.name);
                        if(isAhead)
                            previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                        else
                            previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Top)
                    {
                        Debug.Log("벽의 위위위---");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.up * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Bottom)
                    {
                        Debug.Log("벽의 아래");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.down * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Forward)
                    {
                        Debug.Log("앞쪽임 Forward");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.forward * lengthMulti;
                    }
                    else if (connecting.usedDir == UsedDir.Back)
                    {
                        Debug.Log("뒤야 BBBack");

                        Debug.Log(connecting.name);
                        previewObj.transform.position = connecting.transform.position + Vector3.back * lengthMulti;
                    }
                    else
                    {
                        Debug.Log("어느방향도 설정되지 않았다.");
                    }
                    break;
            }
        }
    }
    void PreviewMatSelect(bool isSpawnable)     //미리보기의 머테리얼 색상 정하는 함수
    {
        if (isSpawnable)
        {
            previewRenderer = previewObj.GetComponent<Renderer>();
            previewRenderer.material = SpawnAbledMat;
        }
        else
        {
            previewRenderer = previewObj.GetComponent<Renderer>();
            previewRenderer.material = SpawnDisabledMat;
        }
    }
}