using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CAVS.ProjectOrganizer.Images;

namespace CAVS.ProjectOrganizer.Project
{

    public static class ProjectFactory
    {

        private static char lineSeperater = '\n'; // It defines line seperate character
        private static char fieldSeperator = ','; // It defines field seperate chracter

        public static TextItem[] BuildItemsFromCSV(string csvPath)
        {
            string fileData = System.IO.File.ReadAllText(csvPath);
            string[] records = fileData.Split(lineSeperater);
            string[] titles = records[0].Split(fieldSeperator);

            // Skip the first line of the csv cause those are titles of columns
            TextItem[] items = new TextItem[records.Length - 1];

            for (int i = 1; i < records.Length; i++)
            {
                Dictionary<string, string> valuesOfItem = new Dictionary<string, string>();
                string[] fields = records[i].Split(fieldSeperator);
                for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                {
                    valuesOfItem.Add(titles[fieldIndex], fields[fieldIndex]);
                }

                items[i - 1] = new TextItem(fields[5], "", valuesOfItem);

            }

            return items;
        }

        public static PictureItem[] BuildItemsFromCSV(string csvPath, int iconColumn)
        {
            string fileData = System.IO.File.ReadAllText(csvPath);
            string[] records = fileData.Split(lineSeperater);
            string[] titles = records[0].Split(fieldSeperator);

            // Skip the first line of the csv cause those are titles of columns
            PictureItem[] items = new PictureItem[records.Length - 1];

            int numOfItemsWaitingOn = 0;

            for (int i = 1; i < records.Length; i++)
            {
                Dictionary<string, string> valuesOfItem = new Dictionary<string, string>();
                string[] fields = records[i].Split(fieldSeperator);
                for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                {
                    valuesOfItem.Add(titles[fieldIndex], fields[fieldIndex]);
                }


                if (iconColumn > -1)
                {
                    numOfItemsWaitingOn++;
                    ImageLoader.LoadImage(fields[iconColumn], delegate (string url, Texture2D image)
                    {
                        numOfItemsWaitingOn--;
                        // Debug.Log(i);
                        items[i - 1] = new PictureItem(fields[5], image, valuesOfItem);
                    });
                }

            }

            int timeout = 100000;

            // Wait till we've gotten all our items..
            while (numOfItemsWaitingOn > 0 && timeout > 0) { timeout--; }

            return items;

        }





    }

}