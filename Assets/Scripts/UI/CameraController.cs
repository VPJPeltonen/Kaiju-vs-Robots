using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed = 10;
    public float maxLeft,maxRight,maxDown,maxUp;
    private Ray ray;
    private RaycastHit hit;
    private string state = "normal";
    private float speed = 10.0F;
    private float startTime;
    private float journeyLength;

    private Vector3 startPos,targetPos;

    // Update is called once per frame
    void Update(){
        switch(state){
            case "normal":
                moveCamera();
                highLightNodes();
                break;
            case "moving to active":
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
                if(transform.position == targetPos){state = "normal";}
                break;
            default:
                moveCamera();
                highLightNodes();
                break;
        }

    }

    //moves to active character
    public void moveToCharacter(float xPos){
        targetPos = new Vector3(xPos, transform.position.y, transform.position.z);
        startPos = transform.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos, targetPos);
        state = "moving to active";
    }

    //checks if mouse is over a map node and tells it to highlight itself
    private void highLightNodes(){
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.name == "Trigger"){
                NodeTrigger node = hit.collider.GetComponent<NodeTrigger>();
                node.highlight();
            }
        }
    }



    //handles moving the camera with keyboard
    private void moveCamera(){
        //get keyboard
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        //get mouse position
        Vector3 mousePos = Input.mousePosition;
        if(mousePos.x < Screen.width/20){xDir = -1;}
        if(mousePos.x > Screen.width - (Screen.width/20)){xDir = 1;}
        //if(mousePos.y < Screen.height/20){yDir = -1;}
        //if(mousePos.y > Screen.height - (Screen.height/20)){yDir = 1;}
        //check for limits of the camera position so it wont wander too far from map
        if(transform.position.x < maxLeft){xDir = Mathf.Clamp(xDir,0,1);}
        if(transform.position.x > maxRight){xDir = Mathf.Clamp(xDir,-1,0);}
        //if(transform.position.z < maxDown){yDir = Mathf.Clamp(yDir,0,1);}
        //if(transform.position.z > maxUp){yDir = Mathf.Clamp(yDir,-1,0);}
        //move camera
        transform.position = transform.position + new Vector3(Time.deltaTime * cameraSpeed * xDir, 0, Time.deltaTime * cameraSpeed * yDir);
    }
}
