using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class BlockSpwaner : MonoBehaviour
{
    public enum BuildMode
    {
        None = 0,
        Foundation,
        Wall,
        Enviroment
    };
    public BuildMode buildMode = BuildMode.None;

    public enum HitType
    {
        None = 0,
        Ground,
        Foundation,
        Wall
    };

    public enum FA_UseDir
    {
        None = 0,
        Forward ,
        Back,
        Left,
        Right,
        All = int.MaxValue
    }
    public HitType hitType = HitType.None;
    public FA_UseDir useDir = FA_UseDir.None;
    public string tagOfHitObject = ""; // 부딪힌 물체의 태그를 저장할 변수

    public WallData woodWallData; // 생성할 큐브에 사용할 WoodWall 스크립터블 오브젝트
    public FoundationData foundationData;
    public float lengthMul = 3f; // 생성할 벽의 길이

    public float interactDistance = 7.0f; // 상호작용 가능한 최대 거리

    [SerializeField] Vector3 downSpawnPoint = new Vector3 (0, 0.8f, 0); //토대의 크기에 맞게 약간 하단에 생성
    [SerializeField] Vector3 testVec = Vector3.zero;            //확인용 변수

    [SerializeField] SpawnedFoundation spawnedFoundation;
    [SerializeField] bool isSpawnAble_FA = true;   //생성할 위치에 foundation(토대)가 없다면 생성가능

    RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수


    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Tab))     //테스트용 건축모드 변경 tab
        {
            switch(buildMode)
            {
                case BuildMode.Wall:
                    buildMode = BuildMode.Foundation;
                    break;

                case BuildMode.Foundation:
                    buildMode = BuildMode.Wall;
                    break;
                default:
                    buildMode = BuildMode.Foundation;
                    break;
            }
        }

        // Cinemachine Virtual Camera를 통해 Ray 발사
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 부딪힌 물체의 태그를 가져옴 디버그용 = 확인용
            tagOfHitObject = hit.collider.gameObject.tag;

            //현재 ray에 닿고있는 물체의 타입 판단
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                hitType = HitType.Wall;
            }
            else if (hit.collider.gameObject.CompareTag("Foundation"))
            {
                hitType = HitType.Foundation;
            }
            else if (hit.collider.gameObject.CompareTag("Ground"))
            {
                hitType = HitType.Ground;
            }
            else
            {
                hitType = HitType.None;
            }

            //건축모드(buildMode) 에 따라서 다른 기능 실행
            switch (buildMode)
            {
                case BuildMode.None:
                    //fa_preview 미리보기 숨기기
                    BaseCampManager.Instance.FA_preview_Hide();
                    break;

                case BuildMode.Wall:
                    //fa_preview 미리보기 숨기기
                    BaseCampManager.Instance.FA_preview_Hide();
                    WallSpawn(WallPosition());
                    if (hitType == HitType.Foundation)  //토대에 생성할 때
                    {

                    }
                    if (hitType == HitType.Wall)        //벽에서 생성할때
                    {

                    }
                    break;

                case BuildMode.Foundation:
                    //생성될 위치 미리보기
                    BaseCampManager.Instance.FA_preview_Show();
                    if (hitType == HitType.Foundation)  //토대에 생성할 때
                    {
                        // 부딪힌 Foundation 오브젝트의 위치
                        spawnedFoundation = hit.collider.gameObject.GetComponent<SpawnedFoundation>();
                        Vector3 foundationCenterPosition = spawnedFoundation.currentPosition;

                        // 새로운 Foundation 오브젝트의 위치 계산
                        Vector3 spawnPosition = CalculateNewFoundationPosition(foundationCenterPosition, hit.point);
                        BaseCampManager.Instance.fa_preview.transform.position = spawnPosition;

                        FoundationSpwan_FA(spawnPosition);
                    }
                    else if (hitType == HitType.Ground) //땅에서 생성할때
                    {
                        testVec = hit.point;
                        Vector3 spawnPosition = hit.point - downSpawnPoint;
                        
                        BaseCampManager.Instance.fa_preview.transform.position = spawnPosition;
                        FoundationSpawn_Ground(spawnPosition);
                    }
                    break;
            }

        }
    }

    void OldRay()
    {
        RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수

        // Cinemachine Virtual Camera를 통해 Ray 발사
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            //// 충돌한 지점의 법선 벡터를 구함
            //Vector3 normal = hit.normal;

            //// 벽의 모서리에서 출발하는 벡터를 계산
            //Vector3 cornerVector = transform.forward + transform.up * wallLength;

            //// 두 벡터의 내적을 계산하여 벽을 생성할 방향 벡터 결정
            //Vector3 spawnDirection = Vector3.Cross(cornerVector, normal);

            //// 벽을 생성하는 위치 설정
            //Vector3 spawnPosition = hit.point + spawnDirection.normalized * wallLength * 0.5f;

            //// 벽을 생성
            //WallSpawn(spawnPosition);
        }
    }

    void WallSpawn(Vector3 _spawnPosition)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // WoodWall 스크립터블 오브젝트에 저장된 정보를 이용하여 큐브를 생성
            GameObject cube = Instantiate(woodWallData.wallPrefab, _spawnPosition, Quaternion.identity);

            // 큐브의 스케일을 설정하여 크기 조정
            cube.transform.localScale = new Vector3(woodWallData.width, woodWallData.height, woodWallData.depth);

            // 큐브의 머티리얼 설정
            Renderer renderer = cube.GetComponent<Renderer>();
            renderer.material = woodWallData.wallMaterial;
        }
    }

    public Vector3 WallPosition()
    {
        Vector3 result = Vector3.zero;

        return result;
    }

    void FoundationSpawn_Ground(Vector3 _spawnPosition)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // WoodWall 스크립터블 오브젝트에 저장된 정보를 이용하여 큐브를 생성
            GameObject cube = Instantiate(foundationData.foundationPrefab, _spawnPosition, Quaternion.identity);
            GameObject child = cube.transform.GetChild(0).gameObject;
            // 큐브의 스케일을 설정하여 크기 조정
            cube.transform.localScale = new Vector3(foundationData.width, foundationData.height, foundationData.depth);

            // 큐브의 머티리얼 설정
            Renderer renderer = child.GetComponent<Renderer>();
            renderer.material = foundationData.foundationMaterial;
        }
    }

    void FoundationSpwan_FA(Vector3 spawnPosition)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (useDir)
            {
                case FA_UseDir.Forward:
                    isSpawnAble_FA = spawnedFoundation.Check_Forward();
                    break;
                case FA_UseDir.Back:
                    isSpawnAble_FA = spawnedFoundation.Check_Back();
                    break;
                case FA_UseDir.Left:
                    isSpawnAble_FA = spawnedFoundation.Check_Left();
                    break;
                case FA_UseDir.Right:
                    isSpawnAble_FA = spawnedFoundation.Check_Right();
                    break;
            }
            if (isSpawnAble_FA)    //생성할 위치에 foundation(토대)가 없다면 생성가능
            {
                // Foundation 오브젝트 생성
                GameObject cube = Instantiate(foundationData.foundationPrefab, spawnPosition, Quaternion.identity);
                GameObject child = cube.transform.GetChild(0).gameObject;
                // 큐브의 스케일을 설정하여 크기 조정
                cube.transform.localScale = new Vector3(foundationData.width, foundationData.height, foundationData.depth);

                // 큐브의 머티리얼 설정
                Renderer renderer = child.GetComponent<Renderer>();
                renderer.material = foundationData.foundationMaterial;
            }
        }

    }

    Vector3 CalculateNewFoundationPosition(Vector3 centerPosition, Vector3 hitPosition)
    {
        Vector3 returnVec = centerPosition;
        // 새로운 Foundation 오브젝트의 위치 계산
        // 여기에 적절한 로직을 추가하여 centerPosition을 기준으로 새로운 위치를 계산합니다.
        // y값을 무시하고 x와 z 값만을 사용하여 위치의 차이를 구합니다.
        float difX = hitPosition.x - centerPosition.x;
        float difZ = hitPosition.z - centerPosition.z;

        // difX와 difZ 중 절대값이 더 큰 값을 선택하여 적용합니다.
        if (Mathf.Abs(difX) > Mathf.Abs(difZ))
        {
            // x가 더 큰 경우
            if (difX > 0)
            {
                returnVec += Vector3.right * lengthMul;
                useDir = FA_UseDir.Right;
                //isSpawnAble_FA = spawnedFoundation.Check_Right();

            }
            else
            {
                returnVec += Vector3.left * lengthMul;
                useDir = FA_UseDir.Left;
               // isSpawnAble_FA = spawnedFoundation.Check_Left();
            }
                
        }
        else
        {
            // z가 더 큰 경우
            if (difZ > 0)
            {
                returnVec += Vector3.forward * lengthMul;
                useDir = FA_UseDir.Forward;
               // isSpawnAble_FA = spawnedFoundation.Check_Forward();
            }
            else
            {
                returnVec += Vector3.back * lengthMul;
                useDir = FA_UseDir.Back;
               // isSpawnAble_FA = spawnedFoundation.Check_Back();
            }
             
        }
        return returnVec;
    }
}
