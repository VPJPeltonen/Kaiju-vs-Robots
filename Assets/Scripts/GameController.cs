using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Character Player;

    void Update(){

    }


    //once map is generated find objects
    public void mapGenerated(){
        Player.moveToStart();
    }
}