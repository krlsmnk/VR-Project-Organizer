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

        public void Select(UnityEngine.GameObject hand)
        {
            //Debug.Log("Selected");
            GhostScript.createGC(thisClonable, hand.transform);
        }//end of select

        public void UnSelect(UnityEngine.GameObject caller)
        {
            //Debug.Log("UnSelected");
        }//end of unselect
    }
}//end of namespace
