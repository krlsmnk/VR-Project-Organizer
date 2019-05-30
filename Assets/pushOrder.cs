using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class pushOrder : MonoBehaviour {
    public GameObject nextMural;
    private mural[] murals;
    int muralNum;
    private GameObject HUDGamObj;
    private Canvas HUDCanvas;
    private Text HUDText;
    private Image HUDImage;

    void Awake()
    {
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }
    void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    void OnEnable()
    {
       
    }


    // Use this for initialization
    void Start () {
        murals = GameObject.FindGameObjectWithTag("mural").GetComponentsInChildren<mural>();
        nextMural = murals[0].gameObject;
        muralNum = 0;

        for (int i = 0; i < murals.Length; i++) {
            murals[i].touchOrder = i;
        }

        //HUDGamObj = (GameObject)Instantiate(Resources.Load("HUDCanvas"));
        HUDGamObj = GameObject.FindObjectOfType<pushOrder>().gameObject;
        HUDCanvas = HUDGamObj.GetComponent<Canvas>();
        HUDText = HUDGamObj.GetComponentInChildren<Text>();
        HUDText.text = "Current Target: " + nextMural.name;
        HUDImage = HUDGamObj.GetComponentInChildren<Image>();
        HUDImage.material = nextMural.GetComponent<Image>().material;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void advanceMural()
    {
        if (muralNum == murals.Length - 1) endSimulation();
        else
        {
            nextMural = murals[++muralNum].gameObject;
            showTarget(nextMural);
        }
    }

    private void endSimulation()
    {
        Debug.Log("Completion Time: " + Time.realtimeSinceStartup);
        showGameOverMessage();
    }

    internal void showTarget(GameObject nextMural)
    {
        HUDText.text = "Please Find: " + nextMural.name;
        HUDImage.material = nextMural.GetComponent<Image>().material;
    }

    internal void showGameOverMessage()
    {
        HUDText.text = "All animals found. Test Over.";
        HUDImage.material = null;
    }
}
