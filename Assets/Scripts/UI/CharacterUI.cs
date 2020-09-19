using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    public Character Model;
    public GameObject[] ActionIndicators;
    public GameObject[] HPIndicators;

    // Update is called once per frame
    void Update()
    {
        UpdateActions();
        UpdateHealth();
    }

    private void UpdateActions(){
        int actions = Model.Actions;
        switch(actions){
            case 0:
                ActionIndicators[0].SetActive(false);
                ActionIndicators[1].SetActive(false);
                break;
            case 1:
                ActionIndicators[0].SetActive(true);
                ActionIndicators[1].SetActive(false);
                break;
            case 2:
                ActionIndicators[0].SetActive(true);
                ActionIndicators[1].SetActive(true);
                break;
        }
    }

    private void UpdateHealth(){
        int health = Model.health;
        switch(health){
            case 0:
                HPIndicators[0].SetActive(false);
                HPIndicators[1].SetActive(false);
                HPIndicators[2].SetActive(false);
                HPIndicators[3].SetActive(false);                
                break;
            case 1:
                HPIndicators[0].SetActive(true);
                HPIndicators[1].SetActive(false);
                HPIndicators[2].SetActive(false);
                HPIndicators[3].SetActive(false);                
                break;
            case 2:
                HPIndicators[0].SetActive(true);
                HPIndicators[1].SetActive(true);
                HPIndicators[2].SetActive(false);
                HPIndicators[3].SetActive(false);                
                break;
            case 3:
                HPIndicators[0].SetActive(true);
                HPIndicators[1].SetActive(true);
                HPIndicators[2].SetActive(true);
                HPIndicators[3].SetActive(false);                
                break;
            case 4:
                HPIndicators[0].SetActive(true);
                HPIndicators[1].SetActive(true);
                HPIndicators[2].SetActive(true);
                HPIndicators[3].SetActive(true);                
                break;
        }
    }
}
