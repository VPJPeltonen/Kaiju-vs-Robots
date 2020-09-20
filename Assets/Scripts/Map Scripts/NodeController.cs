using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public int x,y,F,H,G;
    public bool passable = true;
    public bool dangerous = false;
    public int storedDamage = 0;
    public GameObject debugCube,grid,specialEffect;
    public ParticleSystem DamageEffect;
    //public effectTimer effectCube,fireEffect,psychicEffect;
    public GameObject Parent;
    public Character occupant;
    public GameObject DangerIndicator;
    private float counter = 0f;
    private bool counting = false;

    void Update(){
        if(dangerous){
            DangerIndicator.SetActive(true);
        }else{
            DangerIndicator.SetActive(false);
        }
        if(counting){
            counter += Time.deltaTime;
            if(counter >= 1f){
                specialEffect.SetActive(false);
                counting = false;
            }
        }
    }

    public void TriggerDamage(){
        dangerous = false;
        if(occupant != null){
            occupant.takeDamage(storedDamage);
            storedDamage = 0;
        }
        DamageEffect.Emit(10);
    }

    public void SetCoordinates(int X, int Y){x = X;y = Y;}

    //function for showing if the node is withing move distance from given source tile and move range
    //can also be used to check direct lines between node and target
    public void showRange(int maxRange, int targetX, int targetY, bool directPath=false){
        if(!directPath){
            List<GameObject> route = transform.parent.GetComponent<MapController>().getPath(gameObject, targetX, targetY);
            if(route.Count <= maxRange){
                grid.GetComponent<SpriteRenderer>().color = new Color(0.13f, 0.52f, 0.19f, 0.3f);
            }else{
                showOutOfRange();
            }
        }else{
            MapController gridMap = transform.parent.GetComponent<MapController>();
            List<GameObject> route = gridMap.getPath(gameObject, targetX, targetY,false);
            if(route.Count <= maxRange && gridMap.checLine(gameObject,gridMap.getNode(targetX,targetY))){
                grid.GetComponent<SpriteRenderer>().color = new Color(0.64f, 0.11f, 0.19f, 0.3f);
            }else{
                showOutOfRange();
            }
        }
    }

    public void PlayerSpecial(){
        specialEffect.SetActive(true);
        counting = true;
        counter = 0f;
        if(occupant != null){
            occupant.takeDamage(1);
        }
        DamageEffect.Emit(10);
    }

    //changes color of the node to red
    public void showOutOfRange(){
        grid.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 0f, 0.3f);
    }
/*
    public void showEffect(string effectType = "normal"){
        switch(effectType){
            case "normal":
                effectCube.startTimer();
                break;
            case "fire":
                fireEffect.startTimer();
                break;
            case "psychic":
                psychicEffect.startTimer();
                break;
            default:
                effectCube.startTimer();
                break;
        }
    }
*/
    //resets to default color of the grid
    public void resetColor(){grid.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.3f);}

    //sets path finding related variables back to default
    public void resetPathVariables(){
        Parent = null;
        F = 0;
        H = 0;
        G = 0;
    }

    public void hideDebug(){
        debugCube.SetActive(false);
        Parent = null;
        F = 0;
        H = 0;
        G = 0;
    }
}
