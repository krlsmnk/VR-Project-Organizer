using UnityEngine;
using CAVS.ProjectOrganizer.Interation;
using CAVS.Anvel;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    public class InteractableScreen : RestrainedInteractableObject
    {
        [SerializeField]
        private Vector3 centerPositon;

        [SerializeField]
        private float distanceFromCenter;


        protected AnvelObject objectWeArecontrolling;

        public override Vector3 GetAvailablePositionFromDesired(Vector3 desiredPosition, Quaternion desiredRotation)
        {
            var direction = desiredPosition - centerPositon;
            direction.y = 0;
            return (direction.normalized * distanceFromCenter) + centerPositon;
        }

        public override Quaternion GetAvailableRotationFromDesired(Vector3 desiredPosition, Quaternion desiredRotation)
        {
            return Quaternion.LookRotation(desiredPosition - centerPositon);
        }

        private void Start()
        {
            transform.rotation = GetAvailableRotationFromDesired(transform.position, Quaternion.identity);
            transform.position = GetAvailablePositionFromDesired(transform.position, Quaternion.identity);
        }

        private Vector3 lastPosition;

        private Quaternion lastRotation;

        private void Update()
        {
            if(objectWeArecontrolling == null)
            {
                return;
            }
            Vector3 currentPosition = transform.position;
            Quaternion currentRotation = transform.rotation;
            if ((currentPosition - lastPosition).Equals(Vector3.zero) == false || currentRotation != lastRotation)
            {
                objectWeArecontrolling.UpdateTransform(transform.localPosition, transform.localRotation);
            }
            lastPosition = currentPosition;
            lastRotation = currentRotation;
        }

        public void Initialize(ClientConnectionToken connectionToken)
        {
            objectWeArecontrolling = AnvelObject.CreateObject(ConnectionFactory.CreateConnection(connectionToken), $"Camera - {Random.Range(0, 1000000)}", AssetName.Sensors.API_Camera);
            objectWeArecontrolling.UpdateTransform(transform.localPosition, transform.localRotation);

            LiveCameraDisplay.Build(gameObject, connectionToken, objectWeArecontrolling);
        }

    }

}

