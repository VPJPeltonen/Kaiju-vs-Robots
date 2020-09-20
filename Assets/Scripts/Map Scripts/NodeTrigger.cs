using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeTrigger : MonoBehaviour
{
    public PlayerTeamController Player;
    public NodeController mainNode;
    public GameObject highlightSquare;
    public NodeController MainNode;
    public GameObject square;
    private bool highlighting = false;
    
    void Start(){
        Player = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerTeamController>();
    }

    void Update(){
        if (highlighting){
            highlightSquare.SetActive(true);
            highlighting = false;
        }else{
            highlightSquare.SetActive(false);
        }
    }

    void OnMouseDown(){
        if ( ! EventSystem.current.IsPointerOverGameObject() && Player.ActiveTeam){
            //Debug.Log("click");
            //Debug.Log(mainNode.x + " " + mainNode.y);
            Player.playerAction(mainNode.x,mainNode.y);
            //Debug.Log("click");
        }
    }

    //turns on the highlighting of the node
    public void highlight(){
        highlighting = true;
    }

    //if a blocking object enters the node disable it
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Blocker"){
            square.SetActive(false);
            MainNode.passable = false;
        }
    }

    //enable the node when blocking object leaves
    private void OnTriggerExit(Collider other){
        if(other.tag == "Blocker"){
            square.SetActive(true);
            MainNode.passable = true;
        }
    } 

}

