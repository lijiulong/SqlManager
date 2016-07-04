using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;

namespace Franksoft.SqlManager
{
    public class NameValueCollectionConfigurationSection : ConfigurationSection
    {
        private const string COLLECTION_PROP_NAME = "";

        public IEnumerable<KeyValuePair<string, string>> GetNameValueItems()
        {
            foreach (string key in this.ConfCollection.AllKeys)
            {
                NameValueConfigurationElement confElement = this.ConfCollection[key];
                yield return new KeyValuePair<string, string>
                    (confElement.Name, confElement.Value);
            }
        }

        [ConfigurationProperty(COLLECTION_PROP_NAME, IsDefaultCollection = true)]
        protected NameValueConfigurationCollection ConfCollection
        {
            get
            {
                return (NameValueConfigurationCollection)base[COLLECTION_PROP_NAME];
            }
        }
    }

    internal class Initializer
    {
        private const string MODEL_DIRECTORY_KEY = "SqlManager.ModelDirectory";

        private const string MODELS_SECTION_NAME = "Models";

        private const string STANDALONEQUERIES_SECTION_NAME = "StandaloneQueries";

        private List<string> Models { get; set; }

        private List<string> StandaloneQueries { get; set; }

        private List<string> AllFiles { get; set; }

        public static Initializer Instance { get; private set; }

        public string ModelDirectory { get; private set; }

        public ICollection<string> RegisteredFiles
        {
            get
            {
                return this.AllFiles;
            }
        }

        static Initializer()
        {
            if (Instance == null)
            {
                Instance = new Initializer();
            }
        }

        private Initializer()
        {
            InitializeMembers();
            InitializeConfiguration();
        }

        private void InitializeConfiguration()
        {
            string modelDirectory = ConfigurationManager.AppSettings[MODEL_DIRECTORY_KEY];
            if (!string.IsNullOrEmpty(modelDirectory))
            {
                this.ModelDirectory = modelDirectory;
            }

            var models = ConfigurationManager.GetSection(MODELS_SECTION_NAME) as ModelFileSection;
            if (models != null)
            {
                foreach (ModelFilePathElement path in models.Pathes)
                {
                    this.Models.Add(path.Path);
                }
            }

            var standaloneQueries = ConfigurationManager.GetSection(STANDALONEQUERIES_SECTION_NAME) as ModelFileSection;
            if (standaloneQueries != null)
            {
                foreach (ModelFilePathElement path in standaloneQueries.Pathes)
                {
                    this.StandaloneQueries.Add(path.Path);
                }
            }

            this.AllFiles.AddRange(this.Models);
            this.AllFiles.AddRange(this.StandaloneQueries);

            if (!string.IsNullOrEmpty(this.ModelDirectory))
            {
                var files = Directory.GetFiles(this.ModelDirectory);
                this.AllFiles.AddRange(files);
            }

            // Remove duplicated pathes
            Dictionary<string, string> pathes = new Dictionary<string, string>();
            foreach(string path in this.AllFiles)
            {
                pathes[path] = null;
            }
            this.AllFiles.Clear();
            this.AllFiles.AddRange(pathes.Keys);
        }

        private void InitializeMembers()
        {
            this.Models = new List<string>();
            this.StandaloneQueries = new List<string>();
            this.AllFiles = new List<string>();
        }
    }
}
