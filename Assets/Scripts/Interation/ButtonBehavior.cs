using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;

namespace CAVS.ProjectOrganizer.Interation
{

    public class ButtonBehavior : MonoBehaviour
    {

        private List<Action> subscribers;

        [SerializeField]
        [Range(0.1f, 5f)]
        private float minimumProximityForAnimation;

        [SerializeField]
        private GameObject buttonPiece;

        [SerializeField]
        private GameObject proximityPiece;

        /// <summary>
        /// If we want to immitate pressing the button without the controller
        /// </summary>
        [SerializeField]
        private KeyCode alternativeActivationViaKey;

        private float buttonHitRefractory;

        private float lastButtonHit;

        void Start(){
            lastButtonHit = 0;
            buttonHitRefractory = 0.5f;
        }

        public void Subscribe(Action sub)
        {
            if (sub != null)
            {
                if (subscribers == null)
                {
                    subscribers = new List<Action>();
                }
                subscribers.Add(sub);
            }
        }


        private void CallSubscribers()
        {
            if(Time.time < lastButtonHit + buttonHitRefractory){
                return;
            }
            if (subscribers != null)
            {
                foreach (Action sub in subscribers)
                {
                    if (sub != null)
                    {
                        sub();
                    }
                }
            }
            lastButtonHit = Time.time;
        }

        void OnTriggerEnter(Collider other)
        {
            proximityPiece.GetComponent<MeshRenderer>().material.color = Color.green;
            GetComponent<BoxCollider>().size = new Vector3(0.6f, 3f, 0.6f);
            buttonPiece.transform.Translate(Vector3.up / 15f);
            CallSubscribers();
        }

        void OnTriggerExit(Collider other)
        {
            GetComponent<BoxCollider>().size = new Vector3(0.6f, 2f, 0.6f);
            buttonPiece.transform.Translate(Vector3.down / 15f);
            proximityPiece.GetComponent<MeshRenderer>().material.color = Color.blue;
        }


        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(alternativeActivationViaKey))
            {
                CallSubscribers();
            }

            GameObject[] controllers = new GameObject[] {
                VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.LeftController).gameObject,
                VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.RightController).gameObject
            };

            if (controllers == null)
            {
                return;
            }

            GameObject closest = null;
            float closestDistance = float.MaxValue;
            foreach (GameObject controller in controllers)
            {
                if (controller == null)
                {
                    continue;
                }
                float dist = Vector3.Distance(controller.transform.position, this.buttonPiece.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = controller;
                }
            }

            Vector3 newProximityScale;
            if (closestDistance <= minimumProximityForAnimation)
            {
                float scale = 1.1f - (closestDistance / minimumProximityForAnimation);
                newProximityScale = new Vector3(scale, 1f, scale);
            }
            else
            {
                newProximityScale = Vector3.zero;
            }
            proximityPiece.transform.localScale = newProximityScale;
        }


    }

}