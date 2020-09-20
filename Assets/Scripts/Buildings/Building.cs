using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Character
{
    public GameObject Healthy,Damaged,Damaged2;
    void Start(){

    }

    void Update(){

    }
    
    public void MoveToGrid(MapController MapGrid){
        //generateStats();
        currentNode = MapGrid.getNode(startX,startY);
        //currentNode.GetComponent<NodeController>().passable = false;
        currentNode.GetComponent<NodeController>().occupant = this;
        transform.position = new Vector3(currentNode.transform.position.x,0,currentNode.transform.position.z);
    }

    
    public override void takeDamage(int damage, string damageType = "physical"){
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
        switch(health){
            case 2:
                Healthy.SetActive(false);
                Damaged.SetActive(true);
                Damaged2.SetActive(false);
                break;
            case 1:
                Healthy.SetActive(false);
                Damaged.SetActive(false);
                Damaged2.SetActive(true);
                break;
            case 0:
                Healthy.SetActive(false);
                Damaged.SetActive(false);
                Damaged2.SetActive(false);
                getDefeated();
                break;
        }
        //healthBar.SetHealth(health);
        //UI.showDamage(-damage);
        //hitIndicator.showHit();
        //checks if the damage is enough to knock the character out. If character has points of undying he will lose one instead of dying
        /*
        if(health <= 0){ 
            /*if(undying > 0){
                undying -= 1;
                health = 1;
                healthBar.SetHealth(health);
            }else{
            getDefeated();
        }*/
    }
    protected override void getDefeated(){
        //CharacterSprite.SetActive(false);
        //healthBar.gameObject.SetActive(false);
        //stunEffect.SetActive(false);
        currentNode.GetComponent<NodeController>().occupant = null;
        currentNode = null;
        knockedOut = true;
    }

}
