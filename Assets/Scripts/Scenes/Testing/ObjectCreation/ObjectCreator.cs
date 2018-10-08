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

    // Use this for initialization
    void Start () {
        connection = ConnectionFactory.CreateConnection();
        Debug.Log("Start");
        obj = new AnvelObject(connection, "newObject", "Generic 4x4", null);
    }
	
	// Update is called once per frame
	void OnCollisionEnter(Collision col) {
        Debug.Log("Collision");
        
        obj.SetObjectPosition(col.transform.position.x, col.transform.position.y, col.transform.position.z);
        obj.SetObjectRotation(col.transform.rotation.x, col.transform.rotation.y, col.transform.rotation.z);
    }
}
