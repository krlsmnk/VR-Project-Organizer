using UnityEngine;
using CAVS.Anvel;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class CreateCameraOnCollision : CreateAnvelObjectOnCollision
    {
        protected override void CreateApprorpiateAnvelObject()
        {
            string name = $"Camera - {Random.Range(0, 1000000)}"; 
            objectWeArecontrolling = AnvelObject.CreateObject(connection, name, AnvelAssetName.Sensors.API_Camera, parent.ObjectDescriptor());
            objectWeArecontrolling.UpdateTransform(transform.localPosition, transform.localRotation.eulerAngles);

            GameObject displayPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(displayPlane.GetComponent<Collider>());
            displayPlane.transform.position = transform.forward * 10f;
            displayPlane.transform.SetParent(transform);
            displayPlane.transform.LookAt(transform);
            displayPlane.transform.Rotate(Vector3.up * 180f);
            displayPlane.transform.localScale = Vector3.one* 4f;
            LiveCameraDisplay.Build(displayPlane, new ClientConnectionToken(), name);
        }
    }

}