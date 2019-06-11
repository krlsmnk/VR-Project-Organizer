using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class mural : MonoBehaviour {

    public int touchOrder;
    private VRTK_InteractableObject touchScript;
    private Rigidbody thisRB;
    private AudioSource thisAudSrc;
    private AudioClip correct, incorrect;
    
    // Use this for initialization
	void Start () {
        thisRB = this.gameObject.AddComponent<Rigidbody>();
        thisRB.isKinematic = true;

        touchScript = this.gameObject.AddComponent<VRTK_InteractableObject>();
        touchScript.isGrabbable = false;

        thisAudSrc = this.gameObject.AddComponent<AudioSource>();
        correct = (AudioClip)Resources.Load("sounds/correct");
        incorrect = (AudioClip)Resources.Load("sounds/incorrect");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "controller")
        {
            if (correctPushOrder()) thisAudSrc.PlayOneShot(correct);
            else thisAudSrc.PlayOneShot(incorrect);
        }
        //else Debug.Log("Trigger of non-controller detected.");
    }

    private bool correctPushOrder()
    {
        pushOrder muralTracker = FindObjectOfType<pushOrder>();
        if (muralTracker.nextMural == this.gameObject)
        {
            gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("CheckMark.png");                        
            muralTracker.advanceMural();            
            return true;
        }

        return false;
    }
}