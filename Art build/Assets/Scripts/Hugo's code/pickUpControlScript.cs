﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//
// ## HUGO BAILEY
// ## Written: Proof of Concept phase
// ## Purpose: Manages player pick-ups (e.g. story items)
//

public class pickUpControlScript : MonoBehaviour
{
    public GameObject camera;

    private RaycastHit hit;
    private GameObject hitObject;
    private item pickUpItem;

    private GameObject display;
    private GameObject noteMenu;
    private starStoneManager generator;

    [SerializeField]
    private GameObject pickUpText;

    [SerializeField]
    private GameObject ladderNotif;

    [SerializeField]
    private GameObject offeringText;

    void Start()
    {
        //Assigns game objects and attached components using 'find object'
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        display = GameObject.FindGameObjectWithTag("display");
        noteMenu = GameObject.FindGameObjectWithTag("noteDisplayManager");
        generator = GameObject.FindGameObjectWithTag("GeneratorManager").GetComponent<starStoneManager>();
        pickUpText.SetActive(false);
        ladderNotif.SetActive(false);
        offeringText.SetActive(false);
    }

    void Update()
    {
        //Raycasts forwards from camera position
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 3f)) 
        {
            hitObject = hit.transform.gameObject;

            if (hitObject.CompareTag("PickUp") && hitObject.GetComponent<item>() == true)
            {
                pickUpText.SetActive(true);
            }
            else
            {
                pickUpText.SetActive(false);
            }

            if (hitObject.CompareTag("Ladder"))
            {
                ladderNotif.SetActive(true);
            }
            else
            {
                ladderNotif.SetActive(false);
            }

            if (hitObject.CompareTag("tutorialOffering"))
            {
                offeringText.SetActive(true);
            }
            else
            {
                offeringText.SetActive(false);
            }



            if (Input.GetKeyDown(KeyCode.E)) //if item is hit AND E is pressed;
            {
                if (hitObject.CompareTag("PickUp") && hitObject.GetComponent<item>() == true)
                {
                    // If the item is of type PickUp and has an attached 'item' component, add it to the notes menu
                    pickUpItem = hitObject.GetComponent<item>();

                    noteMenu.GetComponent<noteMenuManager>().addToMenu(pickUpItem);
                    display.GetComponent<pickUpDropDown>().displayItemPickUp(pickUpItem.icon, pickUpItem.itemName, pickUpItem.summary); //display on drop down menu
                    Destroy(hitObject);
                }

                if (hitObject.CompareTag("tutorialOffering"))
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    TextCutscene.storyIndex = 0;
                    SaveLoadManager.instance.LoadSceneWithFade("IntroScene", false);

                    PlayerPrefs.SetInt("TutorialCompleted", 1);
                }

                // ## ACTIVATES STAR STONES 
                switch (hitObject.tag)
                {
                    case "Blue":
                        generator.activateStone(starStoneManager.starStones.Blue);
                        AudioManager.instance.PlaySoundEffect3D("HealthCollect", hitObject.transform.position);
                        break;

                    case "Purple":
                        generator.activateStone(starStoneManager.starStones.Purple);
                        AudioManager.instance.PlaySoundEffect3D("HealthCollect", hitObject.transform.position);
                        break;

                    case "Orange":
                        generator.activateStone(starStoneManager.starStones.Orange);
                        AudioManager.instance.PlaySoundEffect3D("HealthCollect", hitObject.transform.position);
                        break;

                    case "Pink":
                        generator.activateStone(starStoneManager.starStones.Pink);
                        AudioManager.instance.PlaySoundEffect3D("HealthCollect", hitObject.transform.position);
                        break;

                }
            }
        }
        else
        {
            pickUpText.SetActive(false);
            ladderNotif.SetActive(false);
            offeringText.SetActive(false);
        }

        Debug.DrawLine(camera.transform.position, camera.transform.position + (camera.transform.forward * 3)); //EDITOR DEBUG - drawn representation of raycast
    }
}
