using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAVS.Anvel;
using CAVS.Anvel.Lidar;
using CAVS.Anvel.Vehicle;
using AnvelApi;
using CAVS.ProjectOrganizer.Scenes.Showcase;

public class ObjectCreator : MonoBehaviour {

    AnvelObject obj;
    AnvelControlService.Client connection;

    [SerializeField]
    GameObject model;

    // Use this for initialization
    public void CreateObject () {
        connection = ConnectionFactory.CreateConnection();
        Instantiate(model);
        obj = new AnvelObject(connection, "camera", "SampleCamera", connection.GetObjectDescriptorByName("newObject"));
    }
	
	// Update is called once per frame
	public void UpdatePosition() {
        Debug.Log("Position Update");
        
        obj.SetObjectPosition(model.transform.position.x, model.transform.position.y, model.transform.position.z);
        obj.SetObjectRotation(model.transform.rotation.x, model.transform.rotation.y, model.transform.rotation.z);
    }
}
