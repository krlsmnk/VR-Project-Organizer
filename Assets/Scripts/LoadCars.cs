using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoadCars : MonoBehaviour {

	List<Car> listOfCars = new List<Car>();
	//grab the CSV from the Unity folder
	public TextAsset cardata  = Resources.Load<TextAsset> ("carData");


	// Use this for initialization
	void Start () {
		//split the data into rows
		string[] data = cardata.text.Split ('\n');

		//for each row, skipping header
		for (int i = 1; i < data.Length; i++) {

			//split each row into individual values
			string[] row = data [i].Split (',');
			Car c = new Car ();
			//assign individual values to class values
			int.TryParse(row[0], out c.identification);
			c.make = row[1];
			c.model = row[2];
			c.year = row [3];
			c.trim = row [4];
			c.description = row [5];
			c.carClass = row [6];
			c.url = row [7];
			c.bType = row [8];
			double.TryParse(row[9], out c.length);
			double.TryParse(row[10], out c.width);
			double.TryParse(row[11], out c.height);
			double.TryParse(row[12], out c.wheelbase);
			double.TryParse(row[13], out c.curbWeight);
			double.TryParse(row[14], out c.grossWeight);
			c.cylinders = row [15];
			int.TryParse(row[16], out c.horsepower);
			int.TryParse(row[17], out c.valves);
			c.valveTiming = row [18];
			c.driveType = row [19];
			c.transmission = row [20];
			c.engineType = row [21];
			c.fuelType = row [22];
			double.TryParse(row[23], out c.fuelCapacity);
			c.mpg = row [24];
			double.TryParse(row[25], out c.frontHead);
			double.TryParse(row[26], out c.frontHip);
			double.TryParse(row[27], out c.frontLeg);
			double.TryParse(row[28], out c.frontShoulder);
			double.TryParse(row[29], out c.rearHead);
			double.TryParse(row[30], out c.rearHip);
			double.TryParse(row[31], out c.rearLeg);
			double.TryParse(row[32], out c.rearShoulder);
			double.TryParse(row[33], out c.cargoCapacity);
			double.TryParse(row[34], out c.maxSpeed);
			double.TryParse(row[35], out c.drag);

			//put the constructed car into the list 
			//so it can be accessed later during runtime
			listOfCars.Add (c);

		}//end of for each row
	
		foreach (Car c in listOfCars) {
			Debug.Log (c.model);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
