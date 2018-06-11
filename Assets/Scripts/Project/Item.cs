using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CAVS.ProjectOrganizer.Project
{

    public abstract class Item
    {

        protected abstract GameObject getGameobjectReference();

        private readonly string title;

		Dictionary<string, string> values;

        public Item(string title)
        {
			this.values = new Dictionary<string, string>(System.StringComparer.InvariantCultureIgnoreCase);
            this.title = title;
        }

        public Item(string title, Dictionary<string, string> values)
        {
            this.values = new Dictionary<string, string>(System.StringComparer.InvariantCultureIgnoreCase);
            foreach(var keyValPair in values)
            {
                this.values.Add(keyValPair.Key.ToLower(), keyValPair.Value);
            }
            this.title = title;
        }

        /// <summary>
        /// Merges two item properties together and returns an entirely new 
        /// item. If two items contain the same property name but different
        /// values, then we create two properties in the resulting merge
        /// being '<originalPropertyName>.A' and '<originalPropertyName>.B'
        /// 
        /// Mostly operates as a FULL OUTER JOIN in SQL
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Item Merge(Item other)
        {
            if (other == null)
            {
                return this;
            }

            Dictionary<string, string> resultingValues = new Dictionary<string, string>();
            List<string> conflicts = new List<string>();
            foreach (var keypair in values)
            {
                string otherValue = other.GetValue(keypair.Key);
                if (otherValue == null || otherValue == keypair.Value)
                {
                    resultingValues.Add(keypair.Key, keypair.Value);
                }
                else
                {
                    conflicts.Add(keypair.Key);
                    resultingValues.Add(string.Format("{0}.A", keypair.Key), keypair.Value);
                    resultingValues.Add(string.Format("{0}.B", keypair.Key), otherValue);
                }
            }

            foreach (var keypair in other.values)
            {
                if (resultingValues.ContainsKey(keypair.Key) || conflicts.Contains(keypair.Key))
                {
                    continue;
                }
                resultingValues.Add(keypair.Key, keypair.Value);
            }

            return new TextItem(string.Format("{0}.{1}", this.title, other.title), "merge", resultingValues);
        }

        /// <summary>
        /// Gets the title, something to represent the node
        /// </summary>
        /// <returns>The title.</returns>
        public string GetTitle()
        {
            return title;
        }

        /// <summary>
        /// Attempts to retrieve value of the field passed in.
        /// </summary>
        /// <param name="field">field associated with the value (key in the dict)</param>
        /// <returns>value if found, else null</returns>
        public string GetValue(string field)
        {
            if (values.ContainsKey(field.ToLower()))
            {
                return values[field.ToLower()];
            }
            return null;
        }

        /// <summary>
        /// Get all values associated with the item
        /// </summary>
        /// <returns>all values</returns>
        public Dictionary<string, string> GetValues()
        {
            return this.values;
        }

        /// <summary>
        /// Builds a graphical representation of the object inside of the scene
        /// </summary>
        /// <returns>The item.</returns>
        public ItemBehaviour Build()
        {
            return this.Build(Vector3.zero, Vector3.zero);
        }

        /// <summary>
        /// Builds a graphical representation of the object inside of the scene
        /// </summary>
        /// <returns>The item.</returns>
        public ItemBehaviour Build(Vector3 position, Vector3 rotation)
        {
            GameObject node = GameObject.Instantiate(getGameobjectReference());
            node.transform.name = this.GetTitle();
            node.transform.position = position;
            node.transform.rotation = Quaternion.Euler(rotation);

            // The canvas could actually be added on at this step, instead of finding a reference..
            node.transform.Find("Canvas").Find("Text").GetComponent<Text>().text = this.GetTitle();

            return BuildItem(node);
        }

        protected abstract ItemBehaviour BuildItem(GameObject node);

        public override string ToString()
        {
            string result = "Item(";
            foreach(var keypair in values)
            {
                result = string.Format("{0}\n  {1}: {2}", result, keypair.Key, keypair.Value);
            }
            return result + "\n)";
        }

        // Invalid key: cargo capacity (cu ft). Keys must not contain '/', '.', '#', '$', '[', or ']'
        string[] charsToRemove = new string[] { "/", ".", "#", "$", "[", "]", "\n"};

        private string SanitizeKey(string key)
        {
            string santized = key;
            foreach (var c in charsToRemove)
            {
                santized = santized.Replace(c, string.Empty);
            }
            return santized.Trim();
        }

        public string ToJson()
        {
            string result = "{\n";
            int i = 0; 
            foreach (var keyvalPair in values)
            {
                result += string.Format("\"{0}\": \"{1}\"", SanitizeKey(keyvalPair.Key), keyvalPair.Value.Trim());
                if (i < values.Count - 1)
                {
                    result += ",\n";
                } else
                {
                    result += "\n";
                }
                i ++;
            }
            result += "}";
            return result;
        }

    }

}