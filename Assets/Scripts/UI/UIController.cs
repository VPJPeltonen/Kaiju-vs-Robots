using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject StartButton;
    public TextMeshProUGUI PhaseText,FinishText;
    public void ShowStart(){
        StartButton.SetActive(true);
    }

    public void StartButtonPressed(){
        StartButton.SetActive(false);
    }

    public void UpdatePhase(string phase){
        PhaseText.text = phase;
    }

    public void ShowFinish(bool win){
        FinishText.gameObject.SetActive(true);
        if(win){
            FinishText.text = "Victory!";
        }else{
            FinishText.text = "You have been defeated!";
        }
    }
}
