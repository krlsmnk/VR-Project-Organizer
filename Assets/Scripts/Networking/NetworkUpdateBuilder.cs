using System;
using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Netowrking
{
    public class NetworkUpdateBuilder 
    {

        Dictionary<string, Object> values;

        public NetworkUpdateBuilder()
        {
            values = new Dictionary<string, Object>();
        }

        public NetworkUpdateBuilder AddEntry(string key, string value)
        {
            if (values.ContainsKey(key))
            {
                throw new System.Exception("Attempting to add an entry that already exists in the builder.");
            }
            values.Add(key, value);
            return this;
        }

        public NetworkUpdateBuilder AddEntry(string key, UnityEngine.Vector3 value)
        {
            if (values.ContainsKey(key))
            {
                throw new System.Exception("Attempting to add an entry that already exists in the builder.");
            }
            var v3 = new Dictionary<string, object>() {
                { "x", value.x },
                { "y", value.y },
                { "z", value.z }
            };
            values.Add(key, v3);
            return this;
        }

        public NetworkUpdateBuilder AddEntry(string key, Dictionary<string, object> value)
        {
            if (values.ContainsKey(key))
            {
                throw new System.Exception("Attempting to add an entry that already exists in the builder.");
            }
            values.Add(key, value);
            return this;
        }

        public NetworkUpdate Build()
        {
            return new NetworkUpdate(values);
        }

    }
}
