using System.Collections.Generic;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class AnvelObjectManager
    {
        private static List<AnvelObject> anvelObjects;

        private static AnvelObjectManager instance;
        public static AnvelObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnvelObjectManager();
                }
                return instance;
            }
        }

        private AnvelObjectManager()
        {
            anvelObjects = new List<AnvelObject>();
        }

        public void RegisterCreatedObject(AnvelObject anvelObject)
        {
            anvelObjects.Add(anvelObject);
        }

        public void DeleteAllObjectsWeCreatedInAnvel()
        {
            for (int anvelIndex = 0; anvelIndex < anvelObjects.Count; anvelIndex++)
            {
                anvelObjects[anvelIndex].RemoveObject();
            }
        }

        public AnvelObject GetObjectByName(string name)
        {
            for (int anvelIndex = 0; anvelIndex < anvelObjects.Count; anvelIndex++)
            {
                if (anvelObjects[anvelIndex].ObjectName().Equals(name))
                {
                    return anvelObjects[anvelIndex];
                }
            }
            return null;
        }

    }

}