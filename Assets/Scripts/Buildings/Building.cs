using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int startX, startY;
    private GameObject currentNode;
    
    public void MoveToGrid(MapController MapGrid){
        //generateStats();
        currentNode = MapGrid.getNode(startX,startY);
        currentNode.GetComponent<NodeController>().passable = false;
        //currentNode.GetComponent<NodeController>().occupant = this;
        transform.position = new Vector3(currentNode.transform.position.x,0,currentNode.transform.position.z);
    }
}
