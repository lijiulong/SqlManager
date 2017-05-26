using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Franksoft.SqlManager
{
    /// <summary>
    /// An internal class to read application settings and query definition file pathes.
    /// </summary>
    internal sealed class Initializer
    {
        /// <summary>
        /// Static constructor to initialize singleton instance of SqlManager.
        /// </summary>
        static Initializer()
        {
            if (Instance == null)
            {
                Instance = new Initializer();
            }
        }

        /// <summary>
        /// Private constructor ensures singleton.
        /// </summary>
        private Initializer()
        {
            InitializeMembers();
            InitializeConfiguration();
        }

        /// <summary>
        /// Const string value for the name of model registration section in application settings file.
        /// </summary>
        private const string MODEL_REGISTRATION_SECTION_NAME = "ModelRegistrations";

        /// <summary>
        /// Const string value for the default value of ModelDirectory configuration item in application settings file.
        /// </summary>
        private const string MODEL_DIRECTORY_DEFAULT_VALUE = @".\";

        /// <summary>
        /// Const string value for key of ModelDirectory config item in application settings file.
        /// </summary>
        private const string MODEL_DIRECTORY_CONFIG_KEY = "SqlManager.ModelDirectory";

        /// <summary>
        /// Const boolean value for the default value of IgnoreDuplicateKeys
        /// config item in application settings file.
        /// </summary>
        private const bool IGNORE_DUPLICATE_KEYS_DEFAULT_VALUE = false;

        /// <summary>
        /// Const string value for key of IgnoreDuplicateKeys config item in application settings file.
        /// </summary>
        private const string IGNORE_DUPLICATE_KEYS_CONFIG_KEY = "SqlManager.IgnoreDuplicateKeys";

        /// <summary>
        /// Const boolean value for the default value of UseAppDomainForRelativePath
        /// config item in application settings file.
        /// </summary>
        private const bool USE_APPDOMAIN_DEFAULT_VALUE = false;

        /// <summary>
        /// Const string value for key of UseAppDomainForRelativePath config item in application settings file.
        /// </summary>
        private const string USE_APPDOMAIN_CONFIG_KEY = "SqlManager.UseAppDomainForRelativePath";

        /// <summary>
        /// Gets or sets list of registered query definition file pathes.
        /// </summary>
        private List<string> Models { get; set; }

        /// <summary>
        /// Gets the singleton instance of <see cref="Initializer"/>.
        /// </summary>
        public static Initializer Instance { get; private set; }

        /// <summary>
        /// Gets the value of ModelDirectory config item.
        /// </summary>
        public string ModelDirectory { get; private set; }

        /// <summary>
        /// Gets the value of IgnoreDuplicateKeys config item.
        /// </summary>
        public bool IgnoreDuplicateKeys { get; private set; }

        /// <summary>
        /// Gets the value of UseAppDomainForRelativePath config item.
        /// </summary>
        public bool UseAppDomainForRelativePath { get; private set; }

        /// <summary>
        /// Gets the value of base directory for all relative pathes.
        /// </summary>
        public string RelativePathBase { get; private set; }

        /// <summary>
        /// Gets collection of registered query definition file pathes.
        /// </summary>
        public ICollection<string> ModelRegistration
        {
            get
            {
                return this.Models;
            }
        }

        /// <summary>
        /// Gets the full path of relative path, if the path is not relative path, it will be returned unchanged.
        /// </summary>
        /// <param name="relativePath">The relative path need to get full path.</param>
        /// <returns>String value of full path.</returns>
        public string ProcessRelativePath(string relativePath)
        {
            return Path.Combine(this.RelativePathBase, relativePath);
        }

        /// <summary>
        /// Initializes all members in the instance.
        /// </summary>
        private void InitializeMembers()
        {
            this.Models = new List<string>();
            this.ModelDirectory = MODEL_DIRECTORY_DEFAULT_VALUE;
            this.IgnoreDuplicateKeys = IGNORE_DUPLICATE_KEYS_DEFAULT_VALUE;
            this.UseAppDomainForRelativePath = USE_APPDOMAIN_DEFAULT_VALUE;
        }

        /// <summary>
        /// Initializes query definition file pathes and other configuration item values.
        /// </summary>
        private void InitializeConfiguration()
        {
            string useAppDomainForRelativePath = ConfigurationManager.AppSettings[USE_APPDOMAIN_CONFIG_KEY];
            bool isUseAppDomainForRelativePath = USE_APPDOMAIN_DEFAULT_VALUE;
            if (bool.TryParse(useAppDomainForRelativePath, out isUseAppDomainForRelativePath))
            {
                this.UseAppDomainForRelativePath = isUseAppDomainForRelativePath;
            }

            if (this.UseAppDomainForRelativePath)
            {
                this.RelativePathBase = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                FileInfo assembly = new FileInfo(Assembly.GetExecutingAssembly().Location);
                this.RelativePathBase = assembly.DirectoryName;
            }

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

            string ignoreDuplicateKeys = ConfigurationManager.AppSettings[IGNORE_DUPLICATE_KEYS_CONFIG_KEY];
            bool isIgnoreDuplicateKeys = IGNORE_DUPLICATE_KEYS_DEFAULT_VALUE;
            if (bool.TryParse(ignoreDuplicateKeys, out isIgnoreDuplicateKeys))
            {
                this.IgnoreDuplicateKeys = isIgnoreDuplicateKeys;
            }
        }
    }
}
