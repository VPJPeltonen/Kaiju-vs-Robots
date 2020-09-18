using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeTrigger : MonoBehaviour
{
    public Character Player;
    public NodeController mainNode;

    void Start(){
        Player = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    void OnMouseDown(){
        if ( ! EventSystem.current.IsPointerOverGameObject()){
            Player.playerAction(mainNode.x,mainNode.y);
            Debug.Log("click");
        }
    }

}

