﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : Character
{
    private AIteamController Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindWithTag("AIManager").GetComponent<AIteamController>();
        Indicator.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        switch(state){
            case "moving":
                movementAnimation();
                break;
        }
    }

    public override void hunt(){
       /* if(stunTurns >= 1){
            stunTurns -= 1;
            actions -= 2;
            if(stunTurns <= 0){
                stunEffect.SetActive(false);
            }
            return;
        }*/
        NodeController moveTarget = findMoveNode("attack","melee");
        //in case for some reason cant find any reason to attack. shouldnt happen but if for some reason does wont end in eternal loop
        if(moveTarget == null){
            //actions -= 2;
            return;
        }
        if(!isInMeleeRange(moveTarget)){
            //BadGuyWalking();
            NodeController currentPos = currentNode.GetComponent<NodeController>();
            List<GameObject> distance = MapGrid.getPath(currentNode,moveTarget.x,moveTarget.y);
            //if(distance.Count <= moveRange+1 && distance.Count != 0){ checkpunch = true; hurtClosest(); sounds.playSound("punch");}
            startMoving(currentPos.x,currentPos.y,moveTarget.x,moveTarget.y);
            //actions -= 2;
        }else{
            //BadGuyPunchDontMove();
            hurtClosest();
            //sounds.playSound("punch");
            //actions -= 2;
        }
        //counting = true;
        //counter = 0;
    }

    protected void hurtClosest(){
        //AITeamManager AIteamManager = GameObject.FindWithTag("AImanager").GetComponent<AITeamManager>();
        GameObject enemy = Manager.findClosestEnemy(currentNode);
        enemy.GetComponent<Character>().takeDamage(attackPower);
    }

    protected NodeController findMoveNode(string goal,string subgoal){
        switch (goal){
            case "attack":
                //gets two possible targets then randomly picks one
                GameObject[] targets = {Manager.findClosestEnemy(currentNode),Manager.findClosestEnemy(currentNode)};
                GameObject enemy = targets[Random.Range(0,1)];
                NodeController enemyLocation = enemy.GetComponent<Character>().getCurrentNode().GetComponent<NodeController>();
                NodeController attackLocation = null;
                switch (subgoal){
                    case "melee":
                        //checks nodes around the enemy to find node thats the closest
                        attackLocation = findClosestMeleeNode(enemyLocation);
                        return attackLocation;
                    /*case "ranged":
                        //gets path to target and sets the target as node that has the distance of characters attack range
                        List<GameObject> tempPath = MapGrid.getPath(currentNode,enemyLocation.x,enemyLocation.y);
                        attackLocation = tempPath[rangedRange].GetComponent<NodeController>();
                        return attackLocation;
                */
                }
                break;
        }
        return null;
    }

    protected override void reduceActions(int cost){
        //actions -= cost;
        //UI.updateActions(actions);
    }
}