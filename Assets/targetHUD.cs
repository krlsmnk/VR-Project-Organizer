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
    public Transform headsetTransform;
   
    // Use this for initialization
    void Start()
    {
        //headset = new GameObject("HeadsetTransform");

        nextTarget = GameObject.FindObjectOfType<pushOrder>();

        if (headsetTransform == null) headsetTransform = VRTK_DeviceFinder.HeadsetTransform();        
        if (headsetTransform == null) headsetTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset);
        //if (headsetTransform == null) headsetTransform = Camera.main.transform;
        if (headsetTransform == null) headsetTransform = GameObject.FindObjectOfType<SteamVR_Camera>().transform;

        createCanvas();

        GameObject offset = new GameObject();
        offset.transform.position = headsetTransform.forward;
        offset.transform.SetParent(headsetTransform);

        followScript = new VRTK_TransformFollow();
        followScript.gameObjectToChange = HUDGamObj;
        followScript.gameObjectToFollow = offset;
        followScript.followsPosition = true;
        followScript.followsRotation = true;

    }

    private void createCanvas()
    {
        //CNG
        /*

        GameObject myText;
        Text text;
        RectTransform rectTransform;

        // Canvas
        HUDGamObj = new GameObject();
        HUDGamObj.name = "HUDCanvas";
        HUDCanvas = HUDGamObj.AddComponent<Canvas>();

        //CNG HUDCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        HUDCanvas.renderMode = RenderMode.WorldSpace;
        HUDGamObj.AddComponent<CanvasScaler>();
        HUDGamObj.AddComponent<GraphicRaycaster>();

        //Panel?

        // Text
        myText = new GameObject();
        myText.transform.parent = HUDGamObj.transform;

        text = myText.AddComponent<Text>();
        text.text = "Current Target: " + nextTarget.nextMural.name;
        text.fontSize = 100;

        // Text position
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);
        */

        HUDGamObj = (GameObject)Instantiate(Resources.Load("HUDCanvas"));
        HUDCanvas = HUDGamObj.GetComponentInChildren<Canvas>();
        Text text = HUDGamObj.GetComponentInChildren<Text>();
        text.text = "Current Target: " + nextTarget.nextMural.name;
        Image image = HUDCanvas.GetComponent<Image>();
        image.material = nextTarget.nextMural.GetComponentInChildren<Image>().material;

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void showTarget(GameObject nextMural)
    {
        HUDGamObj.GetComponentInChildren<Text>().text = "Please Find: " + nextMural.name;
        Image image = HUDCanvas.GetComponent<Image>();
        image.material = nextMural.GetComponent<Material>();
    }

    internal void showGameOverMessage()
    {
        HUDGamObj.GetComponentInChildren<Text>().text = "All animals found. Test Over.";
        Image image = HUDCanvas.GetComponent<Image>();
        image.material = null;
    }
}
