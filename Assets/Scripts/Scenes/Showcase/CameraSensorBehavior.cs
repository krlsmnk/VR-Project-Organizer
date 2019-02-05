using UnityEngine;
using CAVS.Anvel;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class CameraSensorBehavior : AnvelSensorBehavior
    {
        protected override void CreateApprorpiateAnvelObject()
        {
            string name = $"Camera - {Random.Range(0, 1000000)}";
            objectWeArecontrolling = AnvelObject.CreateObject(connection, name, AssetName.Sensors.API_Camera, parent.ObjectDescriptor());
            objectWeArecontrolling.UpdateTransform(transform.localPosition, transform.localRotation);

            GameObject displayPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(displayPlane.GetComponent<Collider>());
            displayPlane.transform.position = transform.forward * 10f;
            displayPlane.transform.SetParent(transform);
            displayPlane.transform.LookAt(transform);
            displayPlane.transform.Rotate(Vector3.up * 180f);
            displayPlane.transform.localScale = Vector3.one * 4f;
            LiveCameraDisplay.Build(displayPlane, new ClientConnectionToken(), name);
        }

        protected override string PropertyKeyForModifying()
        {
            return "Quality";
        }

        protected override Vector2 PropertyRangeForModifying()
        {
            return new Vector2(10, 20);
        }

        protected override float PropertyStartingValueForModifying()
        {
            return 15;
        }

        public CameraConfig GetConfig()
        {
            return new CameraConfig(transform.position, transform.rotation.eulerAngles);
        }

        internal void Set(CameraConfig cameraConfig)
        {
            transform.localPosition = cameraConfig.Position;
            transform.localEulerAngles = cameraConfig.Rotation;
        }
    }

}