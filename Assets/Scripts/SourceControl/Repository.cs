using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CAVS.ProjectOrganizer.SourceControl
{
    public class Repository
    {
        /// <summary>
        /// The name of the folder that contains all the repository information
        /// </summary>
        private string repoName;

        private Dictionary<string, Artifact> artifacts;

        private JsonSerializer serializer;

        public Repository(string repoName)
        {
            this.repoName = repoName;

            artifacts = new Dictionary<string, Artifact>();

            serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            serializer.Converters.Add(new VectorConverter());
            serializer.Converters.Add(new QuaternionConverter());

            if (Directory.Exists(repoName))
            {
                var files = Directory.GetFiles(repoName);
                
                foreach (var file in files)
                {
                    UnityEngine.Debug.Log(file);
                    using (StreamReader sr = new StreamReader(file))
                    using (JsonReader writer = new JsonTextReader(sr))
                    {
                        UnityEngine.Debug.Log(file.Length);
                        UnityEngine.Debug.Log(file);
                        UnityEngine.Debug.Log(repoName);
                        UnityEngine.Debug.Log(repoName.Length);
                        artifacts[file.Substring(repoName.Length + 1, file.Length - 6 - repoName.Length)] = serializer.Deserialize<Artifact>(writer);
                    }
                }
                
            } else
            {
                Directory.CreateDirectory(repoName);
            }
        }

        public Artifact GetArtifact(string id)
        {
            if (artifacts.ContainsKey(id))
            {
                return artifacts[id];
            }
            return null;
        }

        public void SetArtifact(string id, Artifact artifact)
        {
            artifacts[id] = artifact;
        }

        public void Commit()
        {
            foreach (var artifact in artifacts)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(repoName, string.Format("{0}.json", artifact.Key))))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, artifact.Value);
                }
            }
        }

    }
}

