using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class headsetBacktrack : MonoBehaviour {

    TextWriter tw;

	// Use this for initialization
	void Start () {
		this.gameObject.AddComponent<CapsuleCollider>();
        this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        if(this.gameObject.name == "Headset")this.gameObject.GetComponent<CapsuleCollider>().radius = 20f;
        else this.gameObject.GetComponent<CapsuleCollider>().radius = 2f;
        this.gameObject.GetComponent<CapsuleCollider>().height = 3f;

        if(this.gameObject.GetComponent<Rigidbody>()==null) this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void writeLine(string line) 
    { 
        tw.WriteLine(line + ",");           
    }


    public void Setup(string filename)
    {
        tw = new StreamWriter(filename + ".txt");
    }

    public void DoneRun()
    {
        tw.Close();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("On trigger enter: " + other.name + "This: " + this.name);
        if (other.GetComponentInParent<triggerColliderScript>()!= null)
        {
            Debug.Log(other.transform.parent.name + other.name);
            tw.WriteLine(other.transform.parent.name + other.name);
        }
        else
        {
            Debug.Log(other.name);
            tw.WriteLine(other.name);
        }
    }

}
