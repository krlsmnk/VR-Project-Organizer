using UnityEngine;
using AnvelApi;
using CAVS.Anvel;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Start Position 
        Point3 start = new Point3();
        start.X = 0; start.Y = 0; start.Z = 0;

        //Start Orientation 
        Euler ori = new Euler();
        ori.Roll = 0; ori.Pitch = 0; ori.Yaw = 0;

        AnvelControlService.Client anvelVehicleConnection = ConnectionFactory.CreateConnection();

        //Sensor Name 
        string sensorName = "myLidar";

        //Create the Sensor 
        ObjectDescriptor myObj = anvelVehicleConnection.CreateObject("API 3D Lidar", sensorName, 0, start, ori, true);

        //When you create a sensor, the base class pointer is returned, but we need the LIDAR object so we have to ask for it by name and type 
        ObjectDescriptor lid = anvelVehicleConnection.GetObjectDescriptorByTypeAndName("APILidar", sensorName);

        Debug.Log(lid.ObjectKey);
        Debug.Log(myObj.ObjectKey);

        //Set Desired Properties by name using the lidar object key 
        anvelVehicleConnection.SetProperty(lid.ObjectKey, "HorizontalFOV", "90");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
