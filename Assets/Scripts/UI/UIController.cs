using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject StartButton;

    public void ShowStart(){
        StartButton.SetActive(true);
    }

    public void StartButtonPressed(){
        StartButton.SetActive(false);
    }
}
