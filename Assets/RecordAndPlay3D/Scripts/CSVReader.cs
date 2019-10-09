using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour {

	// Use this for initialization
	void Start () {
    
    try
    {
        String st = File.ReadAllText("runtimes.csv");
        Console.WriteLine(st + "\n");
    }
    catch { 
        Console.WriteLine("Could not read file.");       
    }
	}//end of start
	
	// Update is called once per frame
	void Update () {
		
	}
}
