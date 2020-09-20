using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIteamController : MonoBehaviour
{
    public AICharacter[] TeamMembers;
    public PlayerTeamController EnemyManager;
    public MapController MapGrid;
    public GameController Game;
    public float actionDelay = 1f;
    private List<NodeController> AttackNodes = new List<NodeController>();
    private int currentChar = 0;
    private string state = "idle";
    private float counter = 0f;

    void Update(){
        switch(state){
            case "idle":
                break;
            case "attacking":
                counter += Time.deltaTime;
                if(counter >= actionDelay){
                    foreach(NodeController Node in AttackNodes){
                        Node.TriggerDamage();
                    }
                    AttackNodes.Clear();
                    foreach(AICharacter character in TeamMembers){
                        character.clearAttackNodes();
                    }
                    counter = 0f;
                    state = "moving";
                }
                break;
            case "moving":
                counter += Time.deltaTime;
                if(counter >= actionDelay){
                    Character character = TeamMembers[currentChar];
                    if(!character.knockedOut){
                        character.hunt();
                        character.hunt();
                    }
                    currentChar += 1;
                    counter = 0f;
                    if (currentChar >= TeamMembers.Length){
                        currentChar = 0;
                        state = "idle";
                        Game.AITurnOver();
                    }
                }
                break;
        }
    }

    public void moveActiveCharacter(AICharacter activeCharacter){
        TeamMembers[0].hunt();
    }

    public void Move(){
        state = "moving";
    }

    public void Attack(){
        /*bool lost = true;
        foreach (Character enemy in TeamMembers){
            if(!enemy.knockedOut){
                lost = false;
            }
        }
        if(lost){
            Game.GameEnd("Player Wins");
            return;
        }*/
        state = "attacking";
    }

    public void AddDamageNode(NodeController newNode){
        AttackNodes.Add(newNode);
    }

    //find a closest enemy to the asking character
    public GameObject findClosestEnemy(GameObject attackerNode){
        Character closest = null;
        Character[] playerCharacters = EnemyManager.TeamMembers;
        int lowestDistance = 0;
        //go through all enemies and compare distances 
        foreach (Character enemy in playerCharacters){
            //Character enemyChar = enemy.GetComponent<Character>();
            //if the checked character is knocked out skip it
            if(enemy.knockedOut){continue;}
            NodeController enemyNode = enemy.getCurrentNode().GetComponent<NodeController>();
            int distance = MapGrid.getPath(attackerNode,enemyNode.x,enemyNode.y).Count;
            //if no closest selected yet just set this one as it and move on
            if (closest == null){
                closest = enemy;
                lowestDistance = distance;
                continue;
            }
            if (distance < lowestDistance){
                closest = enemy;
                lowestDistance = distance;
            }
        }
        return closest.gameObject;
    }

    //todo - doesnt work
    public GameObject findClosestBuilding(GameObject attackerNode){
        Character closest = null;
        Character[] playerCharacters = EnemyManager.TeamMembers;
        int lowestDistance = 0;
        //go through all enemies and compare distances 
        foreach (Character enemy in playerCharacters){
            //Character enemyChar = enemy.GetComponent<Character>();
            //if the checked character is knocked out skip it
            if(enemy.knockedOut){continue;}
            NodeController enemyNode = enemy.getCurrentNode().GetComponent<NodeController>();
            int distance = MapGrid.getPath(attackerNode,enemyNode.x,enemyNode.y).Count;
            //if no closest selected yet just set this one as it and move on
            if (closest == null){
                closest = enemy;
                lowestDistance = distance;
                continue;
            }
            if (distance < lowestDistance){
                closest = enemy;
                lowestDistance = distance;
            }
        }
        return closest.gameObject;
    }

    public void moveToStart(){
        foreach(Character character in TeamMembers){
            character.moveToStart();
        }   
    }

}
