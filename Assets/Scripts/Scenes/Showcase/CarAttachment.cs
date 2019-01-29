using UnityEngine;
using AnvelApi;
using CAVS.Anvel;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    /// <summary>
    /// TODO: Not sure if this is being used anymore
    /// </summary>
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
            connection = ConnectionFactory.CreateConnection(new ClientConnectionToken());
            model = Instantiate(model);
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
                obj.UpdateTransform(model.transform.position - car.transform.position, model.transform.rotation);
            }
        }

        private void OnJointBreak(float breakForce)
        {
            attached = false;
            joint = gameObject.AddComponent<FixedJoint>();
            joint.breakForce = 1;
            joint.breakTorque = 1;
        }

    }
}
