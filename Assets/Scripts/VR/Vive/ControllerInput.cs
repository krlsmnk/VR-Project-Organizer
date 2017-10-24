﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Comes from: 
// https://www.raywenderlich.com/149239/htc-vive-tutorial-unity
namespace CAVS.ProjectOrganizer.VR.Vive
{

    public class ControllerInput : MonoBehaviour
    {

        /// <summary>
        /// The object we're colliding with when we're not holding anything
        /// </summary>
        private GameObject collidingObject;
        
        /// <summary>
        /// What we're currently holding
        /// </summary>
        private GameObject objectInHand; 

        /// <summary>
        /// The steam controller itself
        /// </summary>
        private SteamVR_TrackedObject trackedObj;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int)trackedObj.index); }
        }

        void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        public void OnTriggerEnter(Collider other)
        {
            SetCollidingObject(other);
        }

        public void OnTriggerStay(Collider other)
        {
            SetCollidingObject(other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (!collidingObject)
            {
                return;
            }

            collidingObject = null;
        }

        private void GrabObject()
        {
            objectInHand = collidingObject;
            collidingObject = null;
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }

        private void ReleaseObject()
        {
            if (GetComponent<FixedJoint>())
            {
                GetComponent<FixedJoint>().connectedBody = null;
                Destroy(GetComponent<FixedJoint>());
                objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
                objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
            }
            objectInHand = null;
        }

        private FixedJoint AddFixedJoint()
        {
            FixedJoint fx = gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 20000;
            fx.breakTorque = 20000;
            return fx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        private void SetCollidingObject(Collider col)
        {
            if (collidingObject || !col.GetComponent<Rigidbody>())
            {
                return;
            }
            collidingObject = col.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (Controller.GetHairTriggerDown())
            {
                if (collidingObject)
                {
                    GrabObject();
                }
            }

            if (Controller.GetHairTriggerUp())
            {
                if (objectInHand)
                {
                    ReleaseObject();
                }
            }
        }
    }

}