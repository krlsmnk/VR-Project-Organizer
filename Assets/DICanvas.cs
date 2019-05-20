﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK.Examples;

public class DICanvas : MonoBehaviour {

    private GameObject ghostClone, thisCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void createDICanvas(GameObject gClone, Transform spawnLocation) {
        //Instantiate the canvas at the target position
        thisCanvas = (GameObject)Instantiate(Resources.Load("DistanceInteractionCanvas"));
        thisCanvas.transform.position = spawnLocation.forward;

        //copy this script, and attach it to the canvas
        thisCanvas.AddComponent<DICanvas>();

        //continue setup from the new script (which is ON the canvas), rather than this one, which hangs out in the scene
        thisCanvas.GetComponent<DICanvas>().setupDICanvas(gClone, thisCanvas);
    }

    private void setupDICanvas(GameObject gClone, GameObject myCanvas)
    {
        //Assign GameObject references
        ghostClone = gClone;
        thisCanvas = myCanvas;

        //set tag and name so we can clean it up, find it later, and distinguish it in the editor
        thisCanvas.name = "Distance Interaction Canvas: " + ghostClone.name;
        thisCanvas.tag = "temporary";

        //Hook up the buttons so they reference the correct constraints
        setupButtons();
    }

    private void setupButtons()
    {
        //Get the script that has the OnClick methods for the buttons, so we can assign them
        CustomButtonReaction ButtonScript = thisCanvas.GetComponent<CustomButtonReaction>();               
        //Tell the button script which RidgidBody it will be modifying
        ButtonScript.cloneRigidBody = ghostClone.GetComponent<Rigidbody>();

        //Get all the buttonso on the canvas
        Button[] canvasButtons = thisCanvas.GetComponentsInChildren<Button>();
        //Tell each button which RidgidBody they will be modifying
        foreach (Button thisButton in canvasButtons) {
            switch (thisButton.name) {
                case "posx": thisButton.onClick.AddListener(ButtonScript.posx);
                    break;                
                case "posy":
                    thisButton.onClick.AddListener(ButtonScript.posy);
                    break;
                case "posz":
                    thisButton.onClick.AddListener(ButtonScript.posz);
                    break;
                case "rotx":
                    thisButton.onClick.AddListener(ButtonScript.rotx);
                    break;
                case "roty":
                    thisButton.onClick.AddListener(ButtonScript.roty);
                    break;
                case "rotz":
                    thisButton.onClick.AddListener(ButtonScript.rotz);
                    break;
                case "Translate":
                    thisButton.onClick.AddListener(ButtonScript.translateButton);
                    break;
                case "Rotate":
                    thisButton.onClick.AddListener(ButtonScript.rotateButton);
                    break;
                case "Destroy":
                    thisButton.onClick.AddListener(ButtonScript.destroyButton);
                    break;
                default:
                    break;
            }            
        }

    }

}