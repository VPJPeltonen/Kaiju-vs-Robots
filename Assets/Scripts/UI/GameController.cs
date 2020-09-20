using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Character Player;
    public GameObject AIGoon;
    public PlayerTeamController PLT;
    public AIteamController AIT;
    public UIController UI;
    public BuildingsController Buildings;
    private bool PlayerTurn;
    private string GameState = "Generating Map";

    void Update(){
		if (Input.GetMouseButtonDown(1)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				//Destroy(hit.transform.gameObject);
                PlayerTeamController Player = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerTeamController>();
                NodeController targetNode = hit.transform.gameObject.GetComponent<NodeTrigger>().mainNode;
                PLT.playerCast(targetNode.x,targetNode.y);
			}
		}
    }

    public void GameEnd(string winner){
        if(winner == "Player Wins"){
            UI.ShowFinish(true);
        }else{
            UI.ShowFinish(false);
        }
    }

    //once map is generated find objects
    public void mapGenerated(){
        PLT.moveToStart();
        AIT.moveToStart();
        Buildings.SetUpMap();
        //StartGame();
        //GenerateAI();
        UI.ShowStart();
    }

    public void StartGame(){
        GameState = "AIMove";
        AIT.Move();
        UI.UpdatePhase("Enemy Turn");
    }

    public void PlayerTurnOver(){
        GameState = "AITurn";
        UI.UpdatePhase("Enemy Turn");
        AIT.Attack();
        Buildings.CheckCity();
        //AIT.Move();
    }

    public void AITurnOver(){
        GameState = "PlayerTurn";
        PLT.StartTurn();
        UI.UpdatePhase("Player Turn");
    }

    private void GenerateAI(){

    }
}