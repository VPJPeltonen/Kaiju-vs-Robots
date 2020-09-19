using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeTrigger : MonoBehaviour
{
    public PlayerTeamController Player;
    public NodeController mainNode;

    void Start(){
        Player = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerTeamController>();
    }

    void OnMouseDown(){
        if ( ! EventSystem.current.IsPointerOverGameObject() && Player.ActiveTeam){
            //Debug.Log("click");
            //Debug.Log(mainNode.x + " " + mainNode.y);
            Player.playerAction(mainNode.x,mainNode.y);
            //Debug.Log("click");
        }
    }

}

