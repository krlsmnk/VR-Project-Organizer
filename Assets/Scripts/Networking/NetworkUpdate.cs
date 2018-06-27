using System;
using System.Collections.Generic;


namespace CAVS.ProjectOrganizer.Netowrking
{

    public class NetworkUpdate 
    {

        Dictionary<string, Object> values;

        public NetworkUpdate(Dictionary<string, Object> values)
        {
            this.values = values;
        }

        public Dictionary<string, Object> GetValues()
        {
            return values;
        }

    }

}