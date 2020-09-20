using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{ 
    public int startX,startY,moveRange,attackPower,health,physicalResist,psychicResist,castRange;
    public MapController MapGrid;
    public GameObject Indicator,CharacterSprite;
    public PlayerTeamController PlayerManager;
    public ParticleSystem DamageEffect;
    public GameObject currentNode;
    public Animator anim;
    protected Transform startNode, nextNode, endNode;
    protected float movementAnimationSpeed = 4.0f;
    protected float startTime,journeyLength;
    protected List<GameObject> path;
    protected int pathPoint;
    protected string state;
    public bool knockedOut = false;
    private int actions = 2;

    public int Actions { get => actions; set => actions = value;}

    void Start(){
        Indicator.SetActive(false);
    }

    void Update(){
        switch(state){
            case "moving":
                movementAnimation();
                anim.SetBool("walking", true);
                break;
            case "default":
                anim.SetBool("walking", false);
                break;
        }
    }

    //returns the node the character is on
    public GameObject getCurrentNode(){ return currentNode; } 

    public virtual void hunt(){}

    //for checking if character is in melee range
    protected bool isInMeleeRange(NodeController targetLocation){
        bool inRange = false;
        NodeController characterNode = currentNode.GetComponent<NodeController>();
        //array with coordinates for nodes surrounding target location
        Vector2[] nearNodes = surroundingNodes(targetLocation);
        foreach(Vector2 node in nearNodes){
            if(characterNode.x == node.x && characterNode.y == node.y){
                inRange = true;
            }
        }
        return inRange;
    }

    //gives array of node locations surrounding given node
    protected Vector2[] surroundingNodes(NodeController centerNode){
        Vector2[] nearNodes = new Vector2[]{
            new Vector2(centerNode.x-1,centerNode.y),
            new Vector2(centerNode.x-1,centerNode.y+1),
            new Vector2(centerNode.x,centerNode.y+1),
            new Vector2(centerNode.x+1,centerNode.y+1),
            new Vector2(centerNode.x+1,centerNode.y),
            new Vector2(centerNode.x+1,centerNode.y-1),
            new Vector2(centerNode.x,centerNode.y-1),
            new Vector2(centerNode.x-1,centerNode.y-1)
        };
        return nearNodes;
    }

    public void SetAsActiveCharacter(){
        Indicator.SetActive(true);
        adjustGrid();
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

    public void moveAndAttack(Character target){
        if(actions <= 0){return;}
        NodeController enemyLocation = target.getCurrentNode().GetComponent<NodeController>();
        //check if no need to move
        if(!isInMeleeRange(enemyLocation)){
            NodeController moveNode = findClosestMeleeNode(enemyLocation);
            startMoving(currentNode.GetComponent<NodeController>().x,currentNode.GetComponent<NodeController>().y,moveNode.x,moveNode.y);
            if(actions >= 2 && target.gameObject.tag == "Enemy"){
                reduceActions(2);
                target.takeDamage(attackPower);
                //Punch();
            }else{
                reduceActions(1);
            }
        }else{
            if(target.gameObject.tag == "Enemy"){
                target.takeDamage(attackPower);
                //Punch();
            }
            reduceActions(1);
        }
        //adjustGrid();
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

    public void cast(int X, int Y){
        if(actions != 2){
            return;
        }
        
        NodeController startNode = currentNode.GetComponent<NodeController>();
        List<NodeController> targetNodes = new List<NodeController>();
        if(X == startNode.x){
            if(Y > startNode.y){
                for(int i=1; i < castRange;i++){
                    GameObject node = MapGrid.getNode(startNode.x,startNode.y+i);
                    if(node != null){
                        targetNodes.Add(node.GetComponent<NodeController>());
                    }
                }
            }else{
                for(int i=1; i <castRange;i++){
                    GameObject node = MapGrid.getNode(startNode.x,startNode.y-i);
                    if(node != null){
                        targetNodes.Add(node.GetComponent<NodeController>());
                    }
                }
            }
        }
        if(Y == startNode.y){
            if(X > startNode.x){
                for(int i=1; i <castRange;i++){
                    
                    GameObject node = MapGrid.getNode(startNode.x+i,startNode.y);
                    if(node != null){
                        targetNodes.Add(node.GetComponent<NodeController>());
                    }
                }
            }else{
                for(int i=1; i < castRange;i++){
                    GameObject node = MapGrid.getNode(startNode.x-i,startNode.y);
                    if(node != null){
                        targetNodes.Add(node.GetComponent<NodeController>());
                    }
                }
            }

        }
        foreach(NodeController node in targetNodes){
            node.PlayerSpecial();
        }
        reduceActions(2);
    }

    public void clickMove(int targetX, int targetY){
        //if(actions <= 0){return;}
        startMoving(currentNode.GetComponent<NodeController>().x,currentNode.GetComponent<NodeController>().y,targetX,targetY);
        reduceActions(1);
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

    //checks the nodes around target node to find one thats closest
    protected NodeController findClosestMeleeNode(NodeController targetLocation){
        NodeController meleeNode = null;
        int lowestDist = 0;
        //array with coordinates for nodes surrounding target location
        Vector2[] nearNodes = surroundingNodes(targetLocation);
        //go trough the nodes to find closest one
        foreach(Vector2 node in nearNodes){
            GameObject playerNode = MapGrid.getNode((int)node.x,(int)node.y);
            //in case the node doesnt exist maybe not try checking it 
            if (playerNode == null){continue;}
            NodeController proposedNode = playerNode.GetComponent<NodeController>();
            if (!proposedNode.passable){continue;}
            int distance = MapGrid.getPath(currentNode,proposedNode.x,proposedNode.y).Count;
            if (meleeNode == null && proposedNode.occupant == null){
                meleeNode = proposedNode;
                lowestDist = distance;
                continue;
            }
            if (distance < lowestDist && proposedNode.occupant == null){
                meleeNode = proposedNode;
                lowestDist = distance;
            }
        }
        return meleeNode;
    }

    //shows hit effetct and checks if the damage is enough to knock the character out
    public virtual void takeDamage(int damage, string damageType = "physical"){
        //sounds.playSound("defeat");
        switch(damageType){
            case "physical":
                var damageMod = (100f-physicalResist)/100f;
                damage = (int)Mathf.Round(damage*damageMod);
                break;
            case "psychic":
                damage = (int)Mathf.Round(damage*((100-psychicResist)/100));
                break;
        }
        /*if(shieldDuration > 0){
            Debug.Log("Shield activated. Damage preshield: " + damage + " shield left: " + shieldDuration);
            damage = Mathf.Clamp(damage-shieldAmount,0,1000);
            shieldDuration -= 1; 
            Debug.Log("Shield activated. Damage postshield: " + damage + " shield left: " + shieldDuration);
        }*/
        health -= damage;
        DamageEffect.Emit(10);
        //healthBar.SetHealth(health);
        //UI.showDamage(-damage);
        //hitIndicator.showHit();
        //checks if the damage is enough to knock the character out. If character has points of undying he will lose one instead of dying
        if(health <= 0){ 
            /*if(undying > 0){
                undying -= 1;
                health = 1;
                healthBar.SetHealth(health);
            }else{*/
            getDefeated();
        }
    }

    protected virtual void getDefeated(){
        CharacterSprite.SetActive(false);
        //healthBar.gameObject.SetActive(false);
        //stunEffect.SetActive(false);
        currentNode.GetComponent<NodeController>().occupant = null;
        currentNode = null;
        knockedOut = true;
    }

    protected virtual void reduceActions(int cost){
        actions -= cost;
        if(actions <= 0){
            Indicator.SetActive(false);
            PlayerManager.NextCharacter();
            return;
        }
        adjustGrid();
        //UI.updateActions(actions);
    }

    private void adjustGrid(){
        if(actions > 0){
            MapGrid.showMoveRange(moveRange,currentNode);
            Debug.Log("show");
        }else{
            MapGrid.resetNodeColors();
            Debug.Log("reset");
        }
    }

}
