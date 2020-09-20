using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsController : MonoBehaviour
{
    public Building[] Buildings;
    public MapController MapGrid;
    public GameController Game;
    public void SetUpMap(){
        foreach(Building building in Buildings){
            building.MoveToGrid(MapGrid);
        }
    }

    public void CheckCity(){
        bool destroyed = true;
        foreach(Building building in Buildings){
            if (!building.knockedOut){
                destroyed = false;
            }
        }        
        if(destroyed){
            Game.GameEnd("Player Wins");
            return;
        }
    }
}
