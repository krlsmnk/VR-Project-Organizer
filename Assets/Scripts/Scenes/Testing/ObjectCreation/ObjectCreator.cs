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
        AnvelControlService.Client connection = ConnectionFactory.CreateConnection();
    }
	
	// Update is called once per frame
	void OnCollision(Collider col) {
        Debug.Log("Collision");
        obj = new AnvelObject(connection, "newObject", "Generic 4x4");
        obj.SetObjectPosition(0,0,0);
    }
}
