using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;


namespace CAVS.ProjectOrganizer.Interation
{

    public class ButtonBehavior : MonoBehaviour, ISelectable
    {

        private List<Action> subscribers;

        [SerializeField]
        private GameObject buttonPiece;

        private Vector3 buttonPieceStartingPosition;

        [SerializeField]
        private GameObject proximityPiece;

        private Material proximityPieceMaterial;

        /// <summary>
        /// If we want to immitate pressing the button without the controller
        /// </summary>
        [SerializeField]
        private KeyCode alternativeActivationViaKey;

        private float buttonHitRefractory;

        private float lastButtonHit;

        private void Awake()
        {
            subscribers = new List<Action>();
        }

        void Start()
        {
            lastButtonHit = 0;
            buttonHitRefractory = 0.5f;
            proximityPieceMaterial = proximityPiece.GetComponent<MeshRenderer>().material;
            buttonPieceStartingPosition = buttonPiece.transform.position;
        }

        public void Subscribe(Action sub)
        {
            if (sub != null)
            {
                subscribers.Add(sub);
            }
        }


        private void CallSubscribers()
        {
            if (Time.time < lastButtonHit + buttonHitRefractory)
            {
                return;
            }

            foreach (Action sub in subscribers)
            {
                sub?.Invoke();
            }
            lastButtonHit = Time.time;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "controller")
            {
                Select();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "controller")
            {
                UnSelect();
            }
        }

        public void Select()
        {
            buttonPiece.transform.position = buttonPieceStartingPosition + (Vector3.down *.05f);
            CallSubscribers();
            proximityPieceMaterial.color = Color.green;
        }

        public void UnSelect()
        {
            proximityPieceMaterial.color = Color.blue;
            buttonPiece.transform.position = buttonPieceStartingPosition;
        }
    }

}