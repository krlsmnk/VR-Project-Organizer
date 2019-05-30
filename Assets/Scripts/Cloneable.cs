﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;
using VRTK.CAVS.ProjectOrganizer.Interation;

namespace CAVS.ProjectOrganizer.Interation
{    
    public class Cloneable : MonoBehaviour, ISelectable
    {
        private GhostClone GhostScript;
        private Transform originalItemTransform, offsetTransform;
        private GameObject thisClonable, globalGhostScript;
        private VRTK_ControllerEvents Hand;

        // Use this for initialization
        void Start()
        {            
            globalGhostScript = new GameObject();
            thisClonable = this.gameObject; 
            GhostScript = FindObjectOfType<GhostClone>();
            if (GhostScript == null)
            {                 
                GhostScript = globalGhostScript.AddComponent<GhostClone>();                
            }
            //Debug.Log(GhostScript.gameObject.name);

        }
        // Update is called once per frame
        void Update()
        {

        }

        public void UnSelect(UnityEngine.GameObject caller)
        {
            //Debug.Log("UnSelected");
        }//end of unselect

        public void SelectPress(GameObject caller)
        {
            Hand = GameObject.FindObjectOfType<VRTK_ControllerEvents>(); //CNG
            //Debug.Log("Selected");
            //Debug.Log(thisClonable.name);
            //Debug.Log(caller.name);
            //CNG GhostScript.createGC(thisClonable, caller.transform);
            GhostScript.createGC(thisClonable, Hand.transform);
        }

        public void SelectUnpress(GameObject caller)
        {
            //Debug.Log("SelectUnpress");
        }

        public void Hover(GameObject caller)
        {
            //Debug.Log("Hover");
        }

        public void UnHover(GameObject caller)
        {
            //Debug.Log("UnHover");
        }
    }
}//end of namespace