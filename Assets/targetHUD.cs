using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class targetHUD : MonoBehaviour
{
    
    private VRTK_TransformFollow followScript;
    private GameObject HUDGamObj;
    private Canvas HUDCanvas;
    private pushOrder nextTarget;
    private Transform headsetTransform;
    public float spawnDistance = 3f;


    void Awake()
    {
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }
    void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    // Use this for initialization
    void OnEnable()
    {       
        nextTarget = GameObject.FindObjectOfType<pushOrder>();

        headsetTransform = VRTK_DeviceFinder.HeadsetTransform();        
       
        createCanvas();

        //Empty gameobject follows Headset's position and rotation
        GameObject offset = new GameObject("Offset");
        followScript = new VRTK_TransformFollow();
        followScript.gameObjectToChange = offset;
        followScript.gameObjectToFollow = headsetTransform.gameObject;
        followScript.followsPosition = true;
        followScript.followsRotation = true;

        //Put the HUD in front of the headset        
        Vector3 playerPos = headsetTransform.position;
        Vector3 playerDirection = headsetTransform.forward;        
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        //Make the HUD child of the offset object, so it will receive transform updates from it
        HUDGamObj.transform.position = spawnPos;
        HUDGamObj.transform.SetParent(offset.transform);

    }

    private void createCanvas()
    {
        GameObject myText;
        Text text;
        RectTransform rectTransform;

        // Canvas
        HUDGamObj = new GameObject("HUDCanvas");       
        HUDCanvas = HUDGamObj.AddComponent<Canvas>();

        //CNG
        HUDCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        HUDGamObj.AddComponent<CanvasScaler>();
        HUDGamObj.AddComponent<GraphicRaycaster>();

        // Text
        myText = new GameObject("myText");
        myText.transform.parent = HUDGamObj.transform;

        text = myText.AddComponent<Text>();
        text.text = "Current Target: " + nextTarget.nextMural.name;
        text.fontSize = 100;

        // Text position
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);


    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void showTarget(GameObject nextMural)
    {
        HUDGamObj.GetComponentInChildren<Text>().text = "Current Target: " + nextMural.name;
    }

    internal void showGameOverMessage()
    {
        HUDGamObj.GetComponentInChildren<Text>().text = "All animals found. Test Over.";
    }
}
