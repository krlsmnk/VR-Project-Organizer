using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class pathCellProcessing : MonoBehaviour {

    [SerializeField] TextAsset cellsVisitedFile, outputCellsVisited;


    private int[] cellsVisitedA = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedB = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedC = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedD = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedE = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedF = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedG = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedH = {0,0,0,0,0,0,0,0,0};
    private int[] cellsVisitedI = {0,0,0,0,0,0,0,0,0};

    private string a,b,c,d,e,f,g,h,i;

	// Use this for initialization
	void Start () {
		string cellsVisited = AssetDatabase.GetAssetPath(cellsVisitedFile);
        string output = AssetDatabase.GetAssetPath(outputCellsVisited);

         //Read the text from directly from the pathDiff.txt file
        string[] linesCellsVisited = System.IO.File.ReadAllLines(cellsVisited); 


        //Sanitize Data
        foreach (string thisTrial in linesCellsVisited)
        {
            a ="000000000";
            b ="";
            c ="";
            d ="";
            e ="";
            f ="";
            g ="";
            h ="";
            i ="";

            List<char> chars = thisTrial.ToList<char>();
            char[] charArray = chars.ToArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if(Char.IsLetter(charArray[i]) && charArray[i] != 'S') checkChar(i, charArray);                
            }
            
            //Debug.Log(myToString(cellsVisitedA));
            //if(a!="") Debug.Log(a.ToCharArray().ToString());

            
            /*
            File.AppendAllText(output, cellsVisitedA.ToString());
            File.AppendAllText(output, cellsVisitedB.ToString());
            File.AppendAllText(output, cellsVisitedC.ToString());
            File.AppendAllText(output, cellsVisitedD.ToString());
            File.AppendAllText(output, cellsVisitedE.ToString());
            File.AppendAllText(output, cellsVisitedF.ToString());
            File.AppendAllText(output, cellsVisitedG.ToString());
            File.AppendAllText(output, cellsVisitedH.ToString());
            File.AppendAllText(output, cellsVisitedI.ToString());
            
            File.AppendAllText(output, "\n" + "\n");
            */
        }
       
   }//end of start

    private string myToString(int[] v)
    {
        string myString = "";
        for (int i = 0; i < v.Length; i++)
        {
            myString += v[i].ToString();
        }
        return myString;
    }

    void checkChar(int charIndex, char[] line)
    {        
        int val2 = -1;
        int val3 = -1;

        int.TryParse(line[charIndex+1].ToString(), out val2);
                
                if(line[charIndex+5]==',') int.TryParse(line[charIndex+4].ToString(), out val3);
                else{
                        int.TryParse(line[charIndex+4].ToString() + line[charIndex+5].ToString(), out val3); 
                }
                if(val2 != -1) {
                    val2--;
                    switch (line[charIndex])
                    {
                        case 'A':
                            cellsVisitedA[val2] = val3;
                            char[] aArray = a.ToCharArray();        
                            Char.TryParse(val3.ToString(), out aArray[val2]);
                            Debug.Log(a);
                            a = aArray.ToString();
                    Debug.Log(a);
                               
                            break;
                        case 'B':
                            cellsVisitedB[val2] = val3;
                            b+= val3.ToString() + ",";
                            break;
                        case 'C':
                            cellsVisitedC[val2] = val3;
                            c+= val3.ToString() + ",";
                            break;
                        case 'D':
                            cellsVisitedD[val2] = val3;
                            d+= val3.ToString() + ",";
                            break;
                        case 'E':
                            cellsVisitedE[val2] = val3;
                            e+= val3.ToString() + ",";
                            break;
                        case 'F':
                            cellsVisitedF[val2] = val3;
                            f+= val3.ToString() + ",";
                            break;
                        case 'G':
                            cellsVisitedG[val2] = val3;
                            g+= val3.ToString() + ",";
                            break;
                        case 'H':
                            cellsVisitedH[val2] = val3;
                            h+= val3.ToString() + ",";
                            break;
                        case 'I':
                            cellsVisitedI[val2] = val3;
                            i+= val3.ToString() + ",";
                            break;
                        default:
                            break;                
                    }        
                }
    }
	
}

