using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.CAVS.ProjectOrganizer.Interation;
using VRTK.Examples;

public class DICanvas : MonoBehaviour {

    private GameObject ghostClone, thisCanvas;
    private Transform headsetTransform;
    private VRTK_InteractableObject IOScript;
    public GameObject affectedClone;
    public RigidbodyConstraints RBConstraints;

    // Use this for initialization
    void Start () {
        if(thisCanvas == null) thisCanvas = new GameObject();
        if(headsetTransform == null) headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

        VRTK_Button[] buttons = GameObject.FindObjectsOfType<VRTK_Button>();
        foreach (VRTK_Button thisButton in buttons)
        {
            if (thisButton.gameObject == null) Destroy(thisButton);
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject != null && headsetTransform != null) {
            transform.rotation = new Quaternion(transform.rotation.x, Quaternion.LookRotation(transform.position - headsetTransform.position).y, transform.rotation.z, transform.rotation.w);
            //transform.rotation = Quaternion.LookRotation(transform.position - headsetTransform.position);
            //transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
        //gameObject.transform.LookAt(headsetTransform);

        /*
        if (ghostClone != null && ghostClone.GetComponent<Rigidbody>() != null)
        {
            affectedClone = ghostClone;
            Rigidbody RigBody = ghostClone.GetComponent<Rigidbody>();
            RBConstraints = RigBody.constraints;

            if (ghostClone.GetComponent<GhostClone>().RBConstraints != RBConstraints) ghostClone.GetComponent<GhostClone>().RBConstraints = RBConstraints;
        }
        */
	}

    void LateUpdate()
    {
        
    }

    public void createDICanvas(GameObject gClone, Transform spawnLocation) {
        //Debug.Log(gClone.name + spawnLocation.gameObject.name); //CNG
        headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

        //"There can be only one."
        cleanupOldWindows();

        //Instantiate the canvas at the target position
        thisCanvas = (GameObject)Instantiate(Resources.Load("DistanceInteractionCanvas"));
        thisCanvas.transform.position = spawnLocation.position;
        thisCanvas.transform.position = new Vector3(thisCanvas.transform.position.x, headsetTransform.position.y, thisCanvas.transform.position.z);

        //CNG
        //Add IOScript to the canvas so it can be grabbed
        IOScript = thisCanvas.AddComponent<VRTK_InteractableObject>();
        IOScript.isGrabbable = true;
        IOScript.holdButtonToGrab = true;
        IOScript.stayGrabbedOnTeleport = true;
        IOScript.allowedGrabControllers = VRTK_InteractableObject.AllowedController.Both;
        IOScript.disableWhenIdle = false;
        IOScript.touchHighlightColor = Color.cyan;
        IOScript.isKinematic = true;

        //copy this script, and attach it to the canvas
        thisCanvas.AddComponent<DICanvas>();

        //thisCanvas.AddComponent<CustomButtonReaction>(); //CNG

        //continue setup from the new script (which is ON the canvas), rather than this one, which hangs out in the scene
        thisCanvas.GetComponent<DICanvas>().setupDICanvas(gClone, thisCanvas);
    }

    private void cleanupOldWindows()
    {
        GameObject[] temporary = GameObject.FindGameObjectsWithTag("temporary");
        foreach (GameObject thisTemp in temporary)
        {
            if (thisTemp.GetComponent<DICanvas>() != null) Destroy(thisTemp);
        }
        VRTK_Button[] buttons = GameObject.FindObjectsOfType<VRTK_Button>();
        foreach (VRTK_Button thisButton in buttons)
        {
            if (thisButton.gameObject == null) Destroy(thisButton);
        }
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
        setupButtons(thisCanvas);
    }

    private void setupButtons(GameObject myCanvas)
    {
        thisCanvas = myCanvas;

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
                    Debug.Log("Button without valid name.");
                    break;
            }            
        }

    }

}
