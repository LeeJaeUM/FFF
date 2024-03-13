using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpwaner : MonoBehaviour
{
    public WallData woodWallData; // 생성할 큐브에 사용할 WoodWall 스크립터블 오브젝트
    public FoundationData foundationData;
    public float wallLength = 2f; // 생성할 벽의 길이

    public float interactDistance = 7.0f; // 상호작용 가능한 최대 거리
    public string tagOfHitObject = ""; // 부딪힌 물체의 태그를 저장할 변수

    void Update()
    {
        RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수

        // Cinemachine Virtual Camera를 통해 Ray 발사
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 부딪힌 물체의 태그를 가져옴
            tagOfHitObject = hit.collider.gameObject.tag;
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                Debug.Log("tgdggd");
            }
        }
    }

    void WallRay()
    {
        RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수

        // Cinemachine Virtual Camera를 통해 Ray 발사
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 충돌한 지점의 법선 벡터를 구함
            Vector3 normal = hit.normal;

            // 벽의 모서리에서 출발하는 벡터를 계산
            Vector3 cornerVector = transform.forward + transform.up * wallLength;

            // 두 벡터의 내적을 계산하여 벽을 생성할 방향 벡터 결정
            Vector3 spawnDirection = Vector3.Cross(cornerVector, normal);

            // 벽을 생성하는 위치 설정
            Vector3 spawnPosition = hit.point + spawnDirection.normalized * wallLength * 0.5f;

            // 벽을 생성
            WallSpawn(spawnPosition);
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

    void FaoundationSpawn(Vector3 _spawnPosition)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // WoodWall 스크립터블 오브젝트에 저장된 정보를 이용하여 큐브를 생성
            GameObject cube = Instantiate(foundationData.foundationPrefab, _spawnPosition, Quaternion.identity);

            // 큐브의 스케일을 설정하여 크기 조정
            cube.transform.localScale = new Vector3(foundationData.width, foundationData.height, foundationData.depth);

            // 큐브의 머티리얼 설정
            Renderer renderer = cube.GetComponent<Renderer>();
            renderer.material = foundationData.foundationMaterial;
        }
    }
}
