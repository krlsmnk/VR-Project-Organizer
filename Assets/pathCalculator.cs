using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class pathCalculator : MonoBehaviour {

    public TextAsset pathFile;
    public TextAsset pathDifferenceFile, cellsTravelledFile;

    private int[] shortestPaths = {8,4,10,10,4,7,6,6};
    private int[] currentPath = {0,0,0,0,0,0,0,0};
    private string[] targets = {"Monkey", "Horse", "Bird", "Elephant", "Giraffe", "Turtle", "Frog", "Walrus"};
    private List<string> cellsTraversed = new List<string>();

	// Use this for initialization
	void Start () {
		
        string path = AssetDatabase.GetAssetPath(pathFile);
        string pathDiffFilePath = AssetDatabase.GetAssetPath(pathDifferenceFile);
        string pathCellsTravlFile = AssetDatabase.GetAssetPath(cellsTravelledFile);
        

        //Read the text from directly from the test.txt file
        string[] lines = System.IO.File.ReadAllLines(path); 
             
        int currPathLeg = 0;
        int whichTarget = 0;
        foreach (string line in lines)
        {
            //if(line == "Walrus") break;
            // print the line
            //Debug.Log(line);
            if(line.Length==2){
                currentPath[currPathLeg]+=1; //take step
                cellsTraversed.Add(line); //add current location to path map
            }
            else if(line == targets[whichTarget]){
                if(line != "Walrus") { 
                currPathLeg+=1; //correct target reached, go to next leg of path
                whichTarget += 1; //looking for next target
                //Debug.Log("currPathLeg:" + currPathLeg + ". whichTar: " + whichTarget);
                }
            }
                
        }

        //calculate path difference from shortest possible and write it to common file
            string pathRow = "";
            for (int i = 0; i < currentPath.Length; i++)
            {
                //Debug.Log("Leg #" + i + " :" + (currentPath[i] - shortestPaths[i]));
                pathRow += (currentPath[i] - shortestPaths[i]) + ",";
            }
            File.AppendAllText(pathDiffFilePath, "\n" + pathRow);
        //sort path
        //count duplicates
            string cellsRow = "";
            cellsTraversed.Sort();
            








            //process empty
            IEnumerable<string> enumList = cellsTraversed;
            string totalPath = RunningTotal(cellsTraversed);
            File.AppendAllText(pathCellsTravlFile, totalPath + "\n");












            string[] cellsTraversedArray = cellsTraversed.ToArray();
            string currentCell = "";
            int currentCount = 0;
            for (int i = 0; i < cellsTraversedArray.Length; i++)
            {
                if(cellsTraversedArray[i] != currentCell){
                    if(currentCell!="") cellsRow += (currentCell + ": " + currentCount) +",";
                    currentCell = cellsTraversedArray[i];
                    currentCount = 1;
                }
                else currentCount +=1;
            }   cellsRow += (currentCell + ": " + currentCount) + ","; //remember to print last cell              

    }//end of start()


  
    public static string RunningTotal(IEnumerable<string> source)
{
    var counter = new Dictionary<string, int>();
        string[] firstKey = {"A","B","C","D","E","F","G","H","I"};
        string[] secKey = {"1","2","3","4","5","6","7","8","9"};
        addKeys(firstKey, secKey, counter);
        
        

    foreach(var s in source)
    {
        if(counter.ContainsKey(s))
        {
            counter[s]++;
        }
        else
        {
            counter.Add(s, 1);
        }               
        //yield return Tuple.Create(s, counter[s]);
    }

    string totalPath = "";
    foreach (KeyValuePair<string, int> kvp in counter)
    {
        //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        Debug.Log(kvp.Key + ": " + kvp.Value);
        totalPath += kvp.Value.ToString() + ",";
    }
        //Debug.Log(totalPath);        
        return totalPath;
}

    private static void addKeys(string[] firstKey, string[] secKey, Dictionary<string, int> counter)
    {
        for (int i = 0; i < firstKey.Length; i++)
        {
            for (int j = 0; j < secKey.Length; j++)
            {
                counter.Add(firstKey[i] + secKey[j], 0);
            }            
        }        
    }
}
