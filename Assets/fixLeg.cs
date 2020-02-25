using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class fixLeg : MonoBehaviour
{

    private int[] optimumPath = { 8, 4, 10, 10, 4, 7, 6, 6 };
    private int[] legSteps = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public TextAsset oldPath, newestPath;
 
    // Use this for initialization
    void Start()
    {
        string pathToOldFile = AssetDatabase.GetAssetPath(oldPath);
        string pathToNewFile = AssetDatabase.GetAssetPath(newestPath);
        string[] lines = System.IO.File.ReadAllLines(pathToOldFile);

        //for each line (1 trial)
        foreach (string trial in lines)
        {
            //split the line by commas            
            string[] currentLine = trial.Split(',');
            string subject = currentLine[0];
            int offset = 10; // first step in total path
            legSteps = new int[] {  int.Parse(currentLine[2]), 
                                    int.Parse(currentLine[3]),
                                    int.Parse(currentLine[4]),
                                    int.Parse(currentLine[5]),
                                    int.Parse(currentLine[6]),
                                    int.Parse(currentLine[7]),
                                    int.Parse(currentLine[8]),
                                    int.Parse(currentLine[9])};

            //for each leg
            for(int i =0; i<8; i++)
            {
                string subPath = "";
                //add deviation steps + optimum steps for total leg steps
                legSteps[i] += optimumPath[i];

                //grab that many steps from offset
                for(int j = 0; j< legSteps[i]; j++)
                {
                    //offset points to the start of each leg path
                    subPath += currentLine[offset + j] + ",";

                }
                //write current subpath line to new file
                //FORMAT: Subject, Trial, Leg, totalSteps(Leg[i]), deviationSteps(Leg[i]), Subpath
                File.AppendAllText(pathToNewFile, currentLine[0] + "," +
                                                  currentLine[1] + "," +
                                                  i + "," +
                                                  legSteps[i] + "," +
                                                  (legSteps[i] - optimumPath[i]) + "," +
                                                  subPath + "\n");
                offset += legSteps[i];
            }//end of leg
        }//end of trial
    }//end of start
}//end of class