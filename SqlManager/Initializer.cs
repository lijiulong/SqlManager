using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Franksoft.SqlManager
{
    internal class Initializer
    {
        private const string MODEL_DIRECTORY_DEFAULT_VALUE = @".\";

        private const string MODEL_DIRECTORY_KEY = "SqlManager.ModelDirectory";

        private const string MODEL_REGISTRATION_SECTION_NAME = "ModelRegistrations";

        private List<string> Models { get; set; }

        public static Initializer Instance { get; private set; }

        public string ModelDirectory { get; private set; }

        public ICollection<string> ModelRegistration
        {
            get
            {
                return this.Models;
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

            var models = ConfigurationManager.GetSection(MODEL_REGISTRATION_SECTION_NAME) as ModelFileSection;
            if (models != null)
            {
                foreach (ModelFilePathElement path in models.Pathes)
                {
                    this.Models.Add(path.Path);
                }
            }

            if (!string.IsNullOrEmpty(this.ModelDirectory) && Directory.Exists(this.ModelDirectory))
            {
                var files = Directory.GetFiles(this.ModelDirectory, "*.xml");
                foreach (string path in files)
                {
                    this.Models.RemoveAll(s => s == path);
                    this.Models.Add(path);
                }
            }
        }

        private void InitializeMembers()
        {
            this.Models = new List<string>();
            this.ModelDirectory = MODEL_DIRECTORY_DEFAULT_VALUE;
        }
    }
}
