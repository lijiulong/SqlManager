using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Franksoft.SqlManager
{
    internal sealed class Initializer
    {
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

        private const string MODEL_DIRECTORY_DEFAULT_VALUE = @".\";

        private const string MODEL_DIRECTORY_CONFIG_KEY = "SqlManager.ModelDirectory";

        private const string MODEL_REGISTRATION_SECTION_NAME = "ModelRegistrations";

        private const bool IGNORE_DUPLICATE_KEYS_DEFAULT_VALUE = false;

        private const string IGNORE_DUPLICATE_KEYS_CONFIG_KEY = "SqlManager.IgnoreDuplicateKeys";

        private List<string> Models { get; set; }

        public static Initializer Instance { get; private set; }

        public bool IgnoreDuplicateKeys { get; private set; }

        public string ModelDirectory { get; private set; }

        public ICollection<string> ModelRegistration
        {
            get
            {
                return this.Models;
            }
        }

        private void InitializeMembers()
        {
            this.Models = new List<string>();
            this.ModelDirectory = MODEL_DIRECTORY_DEFAULT_VALUE;
            this.IgnoreDuplicateKeys = IGNORE_DUPLICATE_KEYS_DEFAULT_VALUE;
        }

        private void InitializeConfiguration()
        {
            string modelDirectory = ConfigurationManager.AppSettings[MODEL_DIRECTORY_CONFIG_KEY];
            if (!string.IsNullOrEmpty(modelDirectory))
            {
                this.ModelDirectory = modelDirectory;
            }
            this.ModelDirectory = this.ProcessRelativePath(this.ModelDirectory);

            var models = ConfigurationManager.GetSection(MODEL_REGISTRATION_SECTION_NAME) as ModelRegistrationSection;
            if (models != null)
            {
                foreach (ModelRegistrationElement path in models.Pathes)
                {
                    this.Models.Add(this.ProcessRelativePath(path.Path));
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

            string ignoreDuplicateKeys= ConfigurationManager.AppSettings[IGNORE_DUPLICATE_KEYS_CONFIG_KEY];
            bool isIgnoreDuplicateKeys = IGNORE_DUPLICATE_KEYS_DEFAULT_VALUE;
            if(bool.TryParse(ignoreDuplicateKeys, out isIgnoreDuplicateKeys))
            {
                this.IgnoreDuplicateKeys = isIgnoreDuplicateKeys;
            }
        }

        private string ProcessRelativePath(string relativePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }
    }
}
