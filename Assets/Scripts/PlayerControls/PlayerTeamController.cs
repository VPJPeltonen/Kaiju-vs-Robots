using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamController : MonoBehaviour
{
    public Character[] TeamMembers;
    public bool ActiveTeam = false;
    public MapController MapGrid;
    public GameController Game;
    public Character activeCharacter;
    private string mode = "normal";
    private int activeCharacterID;

    public void moveToStart(){
        foreach(Character character in TeamMembers){
            character.moveToStart();
        }
    }

    public void playerAction(int X, int Y){
        if(ActiveTeam){
            Character target = MapGrid.getOccupant(X,Y);
            switch(mode){
                case "normal":
                    List<GameObject> distance = MapGrid.getPath(activeCharacter.getCurrentNode(),X,Y);
                    if(distance.Count <= activeCharacter.moveRange+1 && distance.Count != 0){
                        //checks if the node has someone in it. Attack it or move near it
                        if (target != null){
                            activeCharacter.moveAndAttack(target); 
                        }else{
                            activeCharacter.clickMove(X, Y);
                        }
                    }
                    break;
                /*case "casting":
                    power newPower = powerStorage.getPower(activeAbility);
                    GameObject targetNode = grid.getNode(X,Y);
                    if(newPower == null){break;}
                    int dist = grid.getPath(activeCharacter.getCurrentNode(),X,Y,false).Count;
                    //checks if node is in range and has no blocking objects between it and source
                    if (dist <= newPower.range+1 && grid.checLine(activeCharacter.getCurrentNode(),targetNode)){
                        activeCharacter.cast(activeAbility,targetNode);
                        combatUI.setCursorMode("normal","none");
                    }else if(newPower.range == 0 && dist == 1){
                        //Debug.Log("range "+dist);
                        activeCharacter.cast(activeAbility,targetNode);
                        combatUI.setCursorMode("normal","none");
                    }
                    break;*/
            }
        }
    }




    public void StartTurn(){
        ActiveTeam = true;
        bool lost = true;
        foreach(Character character in TeamMembers){
            character.Actions = 2;
            if(!character.knockedOut){
                lost = false;
            }
        }
        if(lost){
            Game.GameEnd("Enemy Wins");
            return;
        }
        activeCharacter = TeamMembers[0];
        activeCharacterID = 0;
        activeCharacter.SetAsActiveCharacter();
        if(activeCharacter.knockedOut){
            NextCharacter();
        }
    }

    private void EndTurn(){
        MapGrid.resetNodeColors();
        ActiveTeam = false;
        Game.PlayerTurnOver();
        Debug.Log("end turn");
    }

    public void NextCharacter(){
        activeCharacterID += 1;
        if (activeCharacterID >= TeamMembers.Length){
            EndTurn();
            return;
        }
        Debug.Log(activeCharacterID);
        activeCharacter = TeamMembers[activeCharacterID];
        activeCharacter.SetAsActiveCharacter();
        if(activeCharacter.knockedOut){
            NextCharacter();
        }
    }
}
