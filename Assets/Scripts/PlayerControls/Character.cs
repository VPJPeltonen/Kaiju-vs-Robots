using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{ 
    public int startX,startY,moveRange;
    public MapController MapGrid;
    private GameObject currentNode;
    protected Transform startNode, nextNode, endNode;
    protected float movementAnimationSpeed = 4.0f;
    protected float startTime,journeyLength;
    protected List<GameObject> path;
    protected int pathPoint;
    protected string state;

    void Update(){
        switch(state){
            case "moving":
                movementAnimation();
                break;
        }
    }

    public void moveToStart(){
        //generateStats();
        currentNode = MapGrid.getNode(startX,startY);
        currentNode.GetComponent<NodeController>().occupant = this;
        transform.position = new Vector3(currentNode.transform.position.x,0,currentNode.transform.position.z);
    }

    //handles the movement of the character through its path
    protected void movementAnimation(){
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * movementAnimationSpeed;
        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;
        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startNode.position, nextNode.position, fractionOfJourney);
        //if character has reached its next node move to next node or finish movement
        if (transform.position == nextNode.position){
            //sprites.setSpriteOrder(nextNode.gameObject.GetComponent<nodeController>().x);
            if (nextNode == endNode){
                state = "none"; 
                //sounds.walking = false;
                //teamManager.movementDone();
            }else{
                startNode = nextNode;
                pathPoint++;
                nextNode = path[Mathf.Clamp(path.Count-pathPoint,0,path.Count-1)].transform; 
                startTime = Time.time;
                journeyLength = Vector3.Distance(startNode.position, nextNode.position);
            }
        }
    }


    public void playerAction(int X, int Y){
        //if(activeTeam){
            //Character target = grid.getOccupant(X,Y);
            //switch(mode){
               // case "normal":
                    List<GameObject> distance = MapGrid.getPath(currentNode,X,Y);
                    Debug.Log(distance.Count);
                    if(distance.Count <= moveRange+1 && distance.Count != 0){
                        //checks if the node has someone in it. Attack it or move near it
                        //if (target != null){
                          //  activeCharacter.moveAndAttack(target); 
                        //}else{
                         //   activeCharacter.clickMove(X, Y);
                        //}
                        clickMove(X, Y);
                    }
                   // break;
               /* case "casting":
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
                    break;
                */
    }

    public void clickMove(int targetX, int targetY){
        //if(actions <= 0){return;}
        startMoving(currentNode.GetComponent<NodeController>().x,currentNode.GetComponent<NodeController>().y,targetX,targetY);
        //actions -= 1;
        //UI.updateActions(actions);
        //adjustGrid();
        //WalkingAnimation();
    }

    protected void startMoving(int startX, int startY, int endX, int endY){
        //sounds.walking = true;
        //get path and set the starting node, next node and the last node
        path = MapGrid.getPath(currentNode,endX,endY);
        //if path is empty just inform manager movement is done
        if (path.Count <= 0){
            //teamManager.movementDone();
            state = "none";
            return;
        }
        //set first point in path,next node and the node the character will end its movement on
        pathPoint = 2;
        startNode = path[path.Count-1].transform;
        nextNode =  path[Mathf.Clamp(path.Count-pathPoint,0,100)].transform;
        endNode = path[findEmptySpot()].transform;
        //fix for eternally looping turn
        if(endNode == startNode){
           // actions = 0;
            state = "none";
           // active = false;
           // teamManager.movementDone();
           // sounds.walking = false;
            return;
        }
        //change the characters node from current node to on it will end its movement on
        currentNode.GetComponent<NodeController>().occupant = null;
        currentNode = endNode.gameObject;
        currentNode.GetComponent<NodeController>().occupant = this;
        // Keep a note of the time the movement started and calculate the journey length.
        startTime = Time.time;
        journeyLength = Vector3.Distance(startNode.position, nextNode.position);
        state = "moving";
    }

    //if the path point is empty set the path end to node before it and check again until free node is found
    protected int findEmptySpot(){
        int nodeSpot = Mathf.Clamp(path.Count-moveRange-1,0,100);
        Transform endNode = path[nodeSpot].transform;
        while(true){
            //check if node is empty. If its not move to next
            if(endNode.gameObject.GetComponent<NodeController>().occupant != null){
                nodeSpot++;
                //in case only free node is the start node just stop searching
                if(nodeSpot == path.Count-1){ nodeSpot = path.Count-1;break;}
                endNode = path[Mathf.Clamp(nodeSpot,0,100)].transform;
            }else{
                break;
            }  
        }
        return nodeSpot;
    }

}
