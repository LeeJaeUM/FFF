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
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;
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

        if(isBuilding)
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
    private void GhostBuild()
    {
        GameObject currentBuild = GetCurrentBuild();
        CreateGhostPrefab(currentBuild);

        MoveGhostPrefabToRaycast();
        CheckBuildValidity();
    }

    private void CreateGhostPrefab(GameObject currentBuild)
    {
        if(ghostBuildGameObject == null)
        {
            ghostBuildGameObject = Instantiate(currentBuild);

            ModelParent = ghostBuildGameObject.transform.GetChild(0);

            GhostifyModel(ModelParent, ghostMaterialValid);
            GhostifyModel(ghostBuildGameObject.transform);
        }
    }

    private void MoveGhostPrefabToRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction , Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ghostBuildGameObject.transform.position = hit.point;
        }
    }

    private void CheckBuildValidity()
    {
        Collider[] colliders = Physics.OverlapSphere(ghostBuildGameObject.transform.position, connectorOverlapRadius, connectorLayer);
        if(colliders.Length > 0 )
        {
            GhostConnectBuild(colliders);
        }
        else
        {
            GhostSeperateBuild();
        }
    }
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

    private void SnapGhostPrefabToConnector(Connector connector)
    {
        Transform ghostConnector = FindSnapConnector(connector.transform, ghostBuildGameObject.transform.GetChild(1));
        ghostBuildGameObject.transform.position = connector.transform.position - (ghostConnector.position - ghostBuildGameObject.transform.position);

        if (currentBuildType == SelectedBuildType.wall)
        {
            Quaternion newRotation = ghostBuildGameObject.transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, connector.transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
            ghostBuildGameObject.transform.rotation = newRotation;
        }

        GhostifyModel(ModelParent, ghostMaterialValid);
        isGhostInValidPosition = true;
    }

    private void GhostSeperateBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
           if(currentBuildType == SelectedBuildType.wall)
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

            if(Vector3.Angle(hit.normal, Vector3.up) < maxGroundAngle)
            {
                GhostifyModel(ModelParent, ghostMaterialValid);
                isGhostInValidPosition =true;
            }
            else
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    private Transform FindSnapConnector(Transform snapConnector, Transform ghostConnectorParent)
    {
        ConnectorPosition OppositieConnectorTag = GetOppositePosition(snapConnector.GetComponent<Connector>());
        foreach (Connector connector in ghostConnectorParent.GetComponentsInChildren<Connector>())
        {
            if(connector.connectorPosition == OppositieConnectorTag)
            {
                return connector.transform;
            }
        }
        return null;
    }

    private ConnectorPosition GetOppositePosition(Connector connector)
    {
        ConnectorPosition position = connector.connectorPosition;

        if (currentBuildType == SelectedBuildType.wall && connector.connectorParentType == SelectedBuildType.floor)
            return ConnectorPosition.Bottom;

        if (currentBuildType == SelectedBuildType.floor && connector.connectorParentType == SelectedBuildType.wall && connector.connectorPosition == ConnectorPosition.Top)
        {
            if(connector.transform.root.rotation.y == 0)
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

    private ConnectorPosition GetConnectorClosestToPlayer(bool topBottom)
    {
        Transform cameraTransform = Camera.main.transform;

        if (topBottom)
            return cameraTransform.position.z >= ghostBuildGameObject.transform.position.z ? ConnectorPosition.Bottom : ConnectorPosition.Top;
        else
            return cameraTransform.position.x >= ghostBuildGameObject.transform.position.x ? ConnectorPosition.Left : ConnectorPosition.Right;
    }

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


    private void PlaceBuild()
    {
        if (ghostBuildGameObject != null & isGhostInValidPosition)
        {
            GameObject newBuild = Instantiate(GetCurrentBuild(), ghostBuildGameObject.transform.position, ghostBuildGameObject.transform.rotation);

            Destroy(ghostBuildGameObject);
            ghostBuildGameObject = null;

            isBuilding = false;

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