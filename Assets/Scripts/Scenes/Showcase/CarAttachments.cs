//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using AnvelApi;
using CAVS.Anvel;

namespace CAVS.ProjectOrganizer.Scenes.Showcase.AnvelObject
{
    public class CarAttachment : MonoBehaviour
    {
        private bool attached;

        private FixedJoint joint;

        private AnvelObject obj;

        private AnvelControlService.Client connection;

        [SerializeField]
        GameObject model;
        [SerializeField]
        GameObject car;
        [SerializeField]
        string objectName;
        [SerializeField]
        string anvelAssetName;
        [SerializeField]
        string carName;

        public void CreateObject()
        {
            connection = ConnectionFactory.CreateConnection();
            model = Instantiate(model) as GameObject;

            obj = AnvelObject.CreateObject(connection, objectName, anvelAssetName, connection.GetObjectDescriptorByName(carName));
        }

        public void RemoveObject()
        {
            obj.RemoveObject();
            obj = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent<Rigidbody>() != null && !attached)
            {
                joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                attached = true;
                float offsetX = model.transform.position.x - car.transform.position.x;
                float offsetY = model.transform.position.y - car.transform.position.y;
                float offsetZ = model.transform.position.z - car.transform.position.z;
                obj.SetObjectPosition(offsetX,offsetY,offsetZ);
            }
        }

        private void OnJointBreak(float breakForce)
        {
            attached = false;
            this.gameObject.AddComponent<FixedJoint>();
            joint = this.gameObject.GetComponent<FixedJoint>();
            joint.breakForce = 1;
            joint.breakTorque = 1;
        }

    }
}
