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
                Debug.Log("currPathLeg:" + currPathLeg + ". whichTar: " + whichTarget);
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
            File.AppendAllText(pathCellsTravlFile, "\n" + cellsRow);    

    }//end of start()

}
