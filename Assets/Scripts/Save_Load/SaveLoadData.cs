using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadData  {

    public static void savePostion(cube Cube){
        // Create BinaryFormater
        BinaryFormatter bf = new BinaryFormatter();


        // Open file to save to
        FileStream stream = new FileStream(Application.persistentDataPath + "/position.sav", FileMode.Create);

        CubePosition data = new CubePosition(Cube);

        // Serialize data
        bf.Serialize(stream, data);

        stream.Close();
    }

	
}


[Serializable]
public class CubePosition {
    public float[] stats;

    public CubePosition(cube Cube){
        stats = new float[3];

        stats[0] = Cube.xPos;
        stats[1] = Cube.yPos;
        stats[2] = Cube.zPos;




    }
}
