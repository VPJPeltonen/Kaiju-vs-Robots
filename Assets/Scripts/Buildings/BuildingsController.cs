using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsController : MonoBehaviour
{
    public Building[] Buildings;
    public MapController MapGrid;

    public void SetUpMap(){
        foreach(Building building in Buildings){
            building.MoveToGrid(MapGrid);
        }
    }
}
