using System.Collections;
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
        private GameObject thisClonable;

        // Use this for initialization
        void Start()
        {
            GhostScript = FindObjectOfType<GhostClone>();
            if (GhostScript == null) GhostScript = new GhostClone();
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
            //Debug.Log("Selected");
            GhostScript.createGC(thisClonable, caller.transform);
        }

        public void SelectUnpress(GameObject caller)
        {
            Debug.Log("SelectUnpress");
        }

        public void Hover(GameObject caller)
        {
            Debug.Log("Hover");
        }

        public void UnHover(GameObject caller)
        {
            Debug.Log("UnHover");
        }
    }
}//end of namespace
