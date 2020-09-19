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
        GameState = "PlayerTurn";
        PLT.StartTurn();
    }

    public void PlayerTurnOver(){
        GameState = "AITurn";
        AIT.Attack();
        AIT.Move();
        GameState = "PlayerTurn";
        PLT.StartTurn();
    }

    private void GenerateAI(){

    }
}