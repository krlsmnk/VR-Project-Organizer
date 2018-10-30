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
        model = Instantiate(model) as GameObject;
        obj = AnvelObject.CreateObject(connection, "camera", "SampleCamera", connection.GetObjectDescriptorByName("newObject"));
    }
	
	// Update is called once per frame
	public void UpdateTransformInAnvel() {
        Debug.Log("Anvel Transform Update");
        
        obj.UpdateTransform(model.transform.position, model.transform.eulerAngles);
    }
}
