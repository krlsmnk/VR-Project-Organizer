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

        /// <summary>
        ///  
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="key"></param>
        /// <returns>all records when there is a match in either left (table1) or right (table2) table records. Null if left or right side was null</returns>
        public static Item[] FullOuterJoin(Item[] left, Item[] right, string key)
        {
            if(left == null || right == null)
            {
                return null;
            }

            List<Item> resultingSet = new List<Item>();

            List<Item> remainingRight = new List<Item>(right);

            foreach (var item in left)
            {
                if(item == null)
                {
                    continue;
                }

                string keyValue = item.GetValue(key);
                if(keyValue == null)
                {
                    resultingSet.Add(item);
                    continue;
                }

                Item match = null;
                int i = 0;
                while (match == null && i < remainingRight.Count)
                {
                    if (remainingRight[i] != null && remainingRight[i].GetValue(key) == keyValue)
                    {
                        match = remainingRight[i];
                    }
                    i++;
                }

                if(match != null)
                {
                    resultingSet.Add(item.Merge(match));
                    remainingRight.Remove(match);
                } else
                {
                    remainingRight.Add(item);
                }
            }

            foreach(var remainingItem in remainingRight)
            {
                if(remainingItem != null)
                {
                    resultingSet.Add(remainingItem);
                }
            }

            return resultingSet.ToArray();
        }

        /// <summary>
        /// You're 'intersect' operation.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Item[] InnerJoin(Item[] left, Item[] right, string key)
        {
            if (left == null || right == null)
            {
                return null;
            }

            List<Item> resultingSet = new List<Item>();

            foreach (var item in left)
            {
                if (item == null)
                {
                    continue;
                }

                string keyValue = item.GetValue(key);
                if (keyValue == null)
                {
                    continue;
                }

                bool matchFound = false;
                int i = 0;
                while (!matchFound && i < right.Length)
                {
                    if (right[i] != null && right[i].GetValue(key) == keyValue)
                    {
                        matchFound = true;
                        resultingSet.Add(item.Merge(right[i]));
                    }
                    i++;
                }
            }

            return resultingSet.ToArray();
        }

        public static Item[] LeftJoin(Item[] left, Item[] right, string key)
        {
            return SideJoin(left, right, key);
        }

        public static Item[] RightJoin(Item[] left, Item[] right, string key)
        {
            return SideJoin(right, left, key);
        }

        /// <summary>
        /// Mimics the LEFT and RIGHT JOIN key words. If you are trying to 
        /// mimic a LEFT JOIN, you pass in the left side to the first parameter,
        /// if you are trying to mimic a RIGHT JOIN, you pass the right side in
        /// to the first parameter.
        /// </summary>
        /// <param name="larger"></param>
        /// <param name="smaller"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Item[] SideJoin(Item[] larger, Item[] smaller, string key)
        {
            if (larger == null || smaller == null)
            {
                return null;
            }

            List<Item> resultingSet = new List<Item>();

            foreach (var item in larger)
            {
                if (item == null)
                {
                    continue;
                }

                string keyValue = item.GetValue(key);
                if (keyValue == null)
                {
                    continue;
                }

                bool matchFound = false;
                int i = 0;
                while (!matchFound && i < smaller.Length)
                {
                    if (smaller[i] != null && smaller[i].GetValue(key) == keyValue)
                    {
                        matchFound = true;
                        resultingSet.Add(item.Merge(smaller[i]));
                    }
                    i++;
                }

                if(!matchFound)
                {
                    resultingSet.Add(item);
                }
            }

            return resultingSet.ToArray();
        }

        public static TextItem[] BuildItemsFromCSV(string csvPath)
        {
            string fileData = File.ReadAllText(csvPath);
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

        public static TextItem[] BuildItemsFromCSV(string csvPath, bool hasHeader)
        {
            if (hasHeader)
            {
                return BuildItemsFromCSV(csvPath);
            }

            string fileData = File.ReadAllText(csvPath);
            string[] records = fileData.Split(lineSeperater);

            // Skip the first line of the csv cause those are titles of columns
            TextItem[] items = new TextItem[records.Length];

            for (int i = 0; i < records.Length; i++)
            {
                Dictionary<string, string> valuesOfItem = new Dictionary<string, string>();
                string[] fields = records[i].Split(fieldSeperator);
                for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                {
                    valuesOfItem.Add(fieldIndex.ToString(), fields[fieldIndex]);
                }

                items[i] = new TextItem(fields[5], "", valuesOfItem);

            }

            return items;
        }

        public static PictureItem[] BuildItemsFromCSV(string csvPath, int iconColumn)
        {
            string fileData = File.ReadAllText(csvPath);
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