﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int startX,startY;
    public GameObject[] Bots;
    public AIteamController Manager;
    public GameObject currentNode;
    public MapController MapGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(){
        if(currentNode.GetComponent<NodeController>().occupant != null){
            return;
        }
        int flip = Random.Range(0,3);
        if(flip != 0){
            return;
        }
        flip = Random.Range(0,Bots.Length-1);
        GameObject newEnemy = Instantiate(Bots[flip], transform.position, transform.rotation); 
        newEnemy.transform.SetParent(Manager.transform);
        Manager.TeamMembers.Add(newEnemy.GetComponent<AICharacter>());
        newEnemy.GetComponent<AICharacter>().initChar(startX,startY);
    }

    public void moveToStart(){
        //generateStats();
        currentNode = MapGrid.getNode(startX,startY);
        //currentNode.GetComponent<NodeController>().occupant = this;
        transform.position = new Vector3(currentNode.transform.position.x,0,currentNode.transform.position.z);
    }
}
