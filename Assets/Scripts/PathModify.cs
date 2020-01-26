using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PathModify : MonoBehaviour {
    
    public TextAsset newPathDifferenceFile, cellsTravelledInOrder;
    private List<string> cellsTraversed = new List<string>();
    public List<TextAsset> docsToReturn;
    private List<string> pillars;
    public string SubjectNumber;
    private int trialNum =0;
    private int[] shortestPaths = {8,4,10,10,4,7,6,6};
    private int[] currentPath = {0,0,0,0,0,0,0,0};
    private string[] targets = {"Monkey", "Horse", "Bird", "Elephant", "Giraffe", "Turtle", "Frog", "Walrus"};

	// Use this for initialization
	void Start () {
		pillars = new List<string> {"B2","B4","B6","B8",
                                    "D2","D4","D6","D8",
                                    "F2","F4","F6","F8",
                                    "H2","H4","H6","H8"};
        string pathDiffFilePath = AssetDatabase.GetAssetPath(newPathDifferenceFile);
        string pathCellsTravlFile = AssetDatabase.GetAssetPath(cellsTravelledInOrder);

        //generate all file paths from fileList
        List<string> filePaths = new List<string>();
        foreach(TextAsset thisAsset in docsToReturn)
        {
            filePaths.Add(AssetDatabase.GetAssetPath(thisAsset));
        }                
        
        //for each file
         foreach(string path in filePaths)
        {

            //Read the text from directly from the current .txt file
            string[] lines = System.IO.File.ReadAllLines(path);

            //reset pathDiff values
            int currPathLeg = 0;
            int whichTarget = 0;
            currentPath = new int[] {0,0,0,0,0,0,0,0};

            //write current filename to output
            //File.AppendAllText(pathCellsTravlFile, path + "\n");

            //write apparent subjectNum and trialNum to output
            File.AppendAllText(pathCellsTravlFile, ("\n" + SubjectNumber + "," + ++trialNum) + ",");

            // for each entry in the file
            foreach (string line in lines)
            {
                //determine if it is a relevant collision
                if((line.Length==2) && (!isPillar(line)))
                {
                    //if so, write it to the output file
                    File.AppendAllText(pathCellsTravlFile, line + ",");
                    currentPath[currPathLeg]++; //took a valid step on [this] leg of the path
                }
                //if it's a target collision, calculate pathDiff
                else if(line == targets[whichTarget]){
                    if(line != "Walrus") { 
                    currPathLeg++; //correct target reached, go to next leg of path
                    whichTarget++; //looking for next target                    
                    }
                }
            }

            //append apparent SubjectNum and TrialNum to pathDif file
            File.AppendAllText(pathDiffFilePath, ("\n" + SubjectNumber + "," + trialNum) + ",");
            
            //calculate path difference from shortest possible and write it to common file
            string pathRow = "";
            for (int i = 0; i < currentPath.Length; i++)
            {
                //Debug.Log("Leg #" + i + " :" + (currentPath[i] - shortestPaths[i]));
                if(currentPath[i] - shortestPaths[i] < 0) pathRow += "0,";
                else pathRow += (currentPath[i] - shortestPaths[i]) + ",";                
            }
            File.AppendAllText(pathDiffFilePath, pathRow);

        }//end of for each file in directory


	}


    //checks if a specific cell is one of the columns or not
    private bool isPillar(string line)
    {
        foreach(string pillar in pillars) { 
            //is this cell one of the known pillars?
            if(line.CompareTo(pillar) == 0) return true;
        }
        //if not any of the pillars, return false
        return false;
    }
}
