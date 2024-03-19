using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Build Objects")]
    [SerializeField] private List<GameObject> floorObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> wallObjects = new List<GameObject>();

    [Header("Build Settings")]
    [SerializeField] private SelectedBuildType currentBuildType;
    [SerializeField] private LayerMask connectorLayer;

    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;       //생서이 가능할 떄의 머테리얼
    [SerializeField] private Material ghostMaterialInvalid;     //생성이 불가능 할 때의 머테리얼
    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private float maxGroundAngle = 45f;    //90이 아니라 언덕에는 불가능

    [Header("Internal State")]
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private int currentBuildingIndex;
    private GameObject ghostBuildGameObject;
    private bool isGhostInValidPosition = false;
    private Transform ModelParent = null;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            switch(currentBuildType)
            {
                case SelectedBuildType.floor:
                    currentBuildType = SelectedBuildType.wall;
                    isBuilding = false;
                    isBuilding = true;
                    break; 
                case SelectedBuildType.wall:
                    currentBuildType = SelectedBuildType.floor;
                    isBuilding = false;
                    isBuilding = true;
                    break; 
                default:
                    currentBuildType = SelectedBuildType.floor;
                    isBuilding = false;
                    isBuilding = true;
                    break;
            }
        }
        ///. 건설 모드일 때에는 GhostBuild() 함수를 호출하여 건물을 미리보기로 표시하고, 마우스 왼쪽 버튼을 클릭하면 PlaceBuild() 함수를 호출하여 실제로 건물을 설치합니다.
        if (isBuilding)
        {
            GhostBuild();

            if (Input.GetMouseButtonDown(0))
                PlaceBuild();
        }
        else if (ghostBuildGameObject)
        {
            Destroy(ghostBuildGameObject);
            ghostBuildGameObject = null;
        }
    }
    /// <summary>
    /// 현재 선택된 건물을 미리보기로 생성하고, 마우스 위치에 따라 이동시킵니다. 미리보기 건물의 위치가 유효한지 확인하고, 그에 따라 건물을 유효한 위치에 스냅하거나 무효한 위치에 배치합니다
    /// </summary>
    private void GhostBuild()
    {
        GameObject currentBuild = GetCurrentBuild();
        CreateGhostPrefab(currentBuild);

        MoveGhostPrefabToRaycast();
        CheckBuildValidity();
    }

    /// <summary>
    /// 이 함수는 현재 선택된 건물을 반환합니다. 인덱스에 맞춰 obj의 재질 선택
    /// </summary>
    /// <returns></returns>
    private GameObject GetCurrentBuild()
    {
        switch (currentBuildType)
        {
            case SelectedBuildType.floor:
                return floorObjects[currentBuildingIndex];
            case SelectedBuildType.wall:
                return wallObjects[currentBuildingIndex];
        }
        return null;
    }

    /// <summary>
    /// 현재 선택된 건물에 대한 미리보기 프리팹을 생성합니다. 
    /// 미리보기 프리팹은 유효한 위치에 배치될 때까지 건설 모드에서 이동합니다.
    /// </summary>
    /// <param name="currentBuild"></param>
    private void CreateGhostPrefab(GameObject currentBuild)
    {
        if (ghostBuildGameObject == null)
        {
            ghostBuildGameObject = Instantiate(currentBuild);

            ModelParent = ghostBuildGameObject.transform.GetChild(0);

            GhostifyModel(ModelParent, ghostMaterialValid);
            GhostifyModel(ghostBuildGameObject.transform);
        }
    }
    /// <summary>
    /// 마우스 포인터의 위치에 따라 미리보기 건물을 이동시킵니다.
    /// </summary>
    private void MoveGhostPrefabToRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ghostBuildGameObject.transform.position = hit.point;
        }
    }

    /// <summary>
    /// 건물을 배치할 위치의 유효성을 검사합니다. 
    /// </summary>
    private void CheckBuildValidity()
    {
        Collider[] colliders = Physics.OverlapSphere(ghostBuildGameObject.transform.position, connectorOverlapRadius, connectorLayer);
        if (colliders.Length > 0)
        {
            GhostConnectBuild(colliders);
        }
        else
        {
            GhostSeperateBuild();
        }
    }
    #region 겹치는 Connector가 있다면-----------
    /// <summary>
    /// 이 함수는 건물을 설치할 때 미리보기 건물이 연결될 수 있는지 확인합니다.
    /// </summary>
    /// <param name="colliders"></param>
    private void GhostConnectBuild(Collider[] colliders)
    {
        Connector bestConnector = null;

        foreach (Collider collider in colliders)
        {
            Connector connector = collider.GetComponent<Connector>();

            if (connector.canConnectTo)
            {
                bestConnector = connector;
                break;
            }
        }

        if (bestConnector == null || currentBuildType == SelectedBuildType.floor && bestConnector.isConnectedToFloor || currentBuildType == SelectedBuildType.wall && bestConnector.isConnectedToWall)
        {
            GhostifyModel(ModelParent, ghostMaterialValid);
            isGhostInValidPosition = false;
            return;
        }

        SnapGhostPrefabToConnector(bestConnector);
    }
    /// <summary>
    /// 이 함수는 미리보기 건물을 연결 부분에 정확히 맞춥니다.
    /// </summary>
    /// <param name="connector"></param>
    private void SnapGhostPrefabToConnector(Connector connector)
    {
        Transform ghostConnector_Tr = FindSnapConnector(connector.transform, ghostBuildGameObject.transform.GetChild(1));
        ghostBuildGameObject.transform.position = connector.transform.position - (ghostConnector_Tr.position - ghostBuildGameObject.transform.position);

        if (currentBuildType == SelectedBuildType.wall)
        {
            Quaternion newRotation = ghostBuildGameObject.transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, connector.transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
            ghostBuildGameObject.transform.rotation = newRotation;
        }

        GhostifyModel(ModelParent, ghostMaterialValid);
        isGhostInValidPosition = true;
    }
    /// <summary>
    /// 이 함수는 건물의 연결 부분을 찾아 반환합니다.
    /// </summary>
    /// <param name="snapConnector"></param>
    /// <param name="ghostConnectorParent"></param>
    /// <returns></returns>
    private Transform FindSnapConnector(Transform snapConnector, Transform ghostConnectorParent)
    {
        ConnectorPosition OppositieConnectorTag = GetOppositePosition(snapConnector.GetComponent<Connector>());
        foreach (Connector connector in ghostConnectorParent.GetComponentsInChildren<Connector>())
        {
            if (connector.connectorPosition == OppositieConnectorTag)
            {
                return connector.transform;
            }
        }
        return null;
    }


    /// <summary>
    /// 이 함수는 주어진 연결 부분의 반대편 연결 부분을 반환합니다
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
    private ConnectorPosition GetOppositePosition(Connector connector)
    {
        ConnectorPosition position = connector.connectorPosition;

        if (currentBuildType == SelectedBuildType.wall && connector.connectorParentType == SelectedBuildType.floor)
            return ConnectorPosition.Bottom;

        if (currentBuildType == SelectedBuildType.floor && connector.connectorParentType == SelectedBuildType.wall && connector.connectorPosition == ConnectorPosition.Top)
        {
            if (connector.transform.root.rotation.y == 0)
            {
                return GetConnectorClosestToPlayer(true);
            }
            else
            {
                return GetConnectorClosestToPlayer(false);
            }
        }

        switch (position)
        {
            case ConnectorPosition.Left:
                return ConnectorPosition.Right;
            case ConnectorPosition.Right:
                return ConnectorPosition.Left;
            case ConnectorPosition.Top:
                return ConnectorPosition.Bottom;
            case ConnectorPosition.Bottom:
                return ConnectorPosition.Top;
            default: return ConnectorPosition.Bottom;
        }

    }
    //이 함수는 플레이어에 가장 가까운 연결 부분을 반환합니다.
    private ConnectorPosition GetConnectorClosestToPlayer(bool topBottom)
    {
        Transform cameraTransform = Camera.main.transform;

        if (topBottom)
            return cameraTransform.position.z >= ghostBuildGameObject.transform.position.z ? ConnectorPosition.Bottom : ConnectorPosition.Top;
        else
            return cameraTransform.position.x >= ghostBuildGameObject.transform.position.x ? ConnectorPosition.Left : ConnectorPosition.Right;
    }

    #endregion

    /// <summary>
    /// 이 함수는 미리보기 건물이 지면에 연결되어 있는지 확인하고, 유효성을 검사합니다
    /// </summary>
    private void GhostSeperateBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (currentBuildType == SelectedBuildType.wall)
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (hit.collider.transform.root.CompareTag("Buildables"))
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (Vector3.Angle(hit.normal, Vector3.up) < maxGroundAngle)
            {
                GhostifyModel(ModelParent, ghostMaterialValid);
                isGhostInValidPosition = true;
            }
            else
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    /// <summary>
    /// 머테리얼을 바꾸는 함수
    /// </summary>
    /// <param name="modelParent"></param>
    /// <param name="ghostMaterial"></param>
    private void GhostifyModel(Transform modelParent, Material ghostMaterial = null)
    {
        if(ghostMaterial != null)
        {
            foreach(MeshRenderer meshRenderer in modelParent.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = ghostMaterial;
            }
        }
        else
        {
            foreach(Collider modelColliders in modelParent.GetComponentsInChildren<Collider>())
            {
                modelColliders.enabled = false;
            }
        }
    }
    /// <summary>
    /// 건물을 실제로 배치합니다. 미리보기 건물이 유효한 위치에 있을 때 호출
    /// </summary>
    private void PlaceBuild()
    {
        if (ghostBuildGameObject != null & isGhostInValidPosition)
        {
            GameObject newBuild = Instantiate(GetCurrentBuild(), ghostBuildGameObject.transform.position, ghostBuildGameObject.transform.rotation);

            Destroy(ghostBuildGameObject);
            ghostBuildGameObject = null;

            //isBuilding = false;

            foreach (Connector connector in newBuild.GetComponentsInChildren< Connector>() )
            {
                connector.UpdateConnectors(true);
            }
        }
    }


}
[System.Serializable]
public enum SelectedBuildType
{
    floor,
    wall
}