using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIteamController : MonoBehaviour
{
    public AICharacter[] TeamMembers;
    public PlayerTeamController EnemyManager;
    public MapController MapGrid;

    private List<NodeController> AttackNodes = new List<NodeController>();

    public void moveActiveCharacter(AICharacter activeCharacter){
        TeamMembers[0].hunt();
    }

    public void Move(){
        foreach(Character character in TeamMembers){
            if(!character.knockedOut){
                character.hunt();
                character.hunt();
            }
        }  
    }

    public void Attack(){
        foreach(NodeController Node in AttackNodes){
            Node.TriggerDamage();
        }
        AttackNodes.Clear();
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

    public void moveToStart(){
        foreach(Character character in TeamMembers){
            character.moveToStart();
        }   
    }

}
