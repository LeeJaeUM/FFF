using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public ConnectorPosition connectorPosition;
    public SelectedBuildType connectorParentType;

    public bool isConnectedToFloor = false;
    public bool isConnectedToWall = false;
    public bool canConnectTo = true;

    [SerializeField] private bool canConnectToFloor = true;
    [SerializeField] private bool canConnectToWall = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = isConnectedToFloor ? (isConnectedToFloor ? Color.red : Color.green) : (!isConnectedToFloor ? Color.green : Color.yellow);
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2f);
    }

    public void UpdateConnectors(bool rootCall = false)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2f);

        isConnectedToFloor = !canConnectToFloor;
        isConnectedToWall = !canConnectToWall;

        foreach (Collider collider in colliders)
        {
            if(collider.GetInstanceID() == GetComponent<Collider>().GetInstanceID())
            {
                continue;
            }
            if(collider.gameObject.layer == gameObject.layer)   //같은 레이어를 가지고 있는 지 확인
            {
                Connector foundConnector = collider.GetComponent<Connector>();

                if (foundConnector.connectorParentType == SelectedBuildType.floor)
                    isConnectedToFloor = true;
                if (foundConnector.connectorParentType == SelectedBuildType.wall)
                    isConnectedToWall= true;

                if (rootCall)
                    foundConnector.UpdateConnectors();
            }
        }

        canConnectTo = true;

        if (isConnectedToFloor && isConnectedToWall)
            canConnectTo = false;
    }

}
[System.Serializable]
public enum ConnectorPosition
{
    Left,
    Right,
    Top,
    Bottom
}
