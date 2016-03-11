using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using log4net;


namespace CommonUtils
{
    public class SettingsReader
    {
        private const string PROJ_FILE_NAME = "SettingsReader";
        private static ILog Logger = LogManager.GetLogger(typeof(SettingsReader));

        private ExeConfigurationFileMap fileMap = null;
        private Configuration config = null;
        private ConfigurationSectionGroupCollection sectionGroups = null;

        public SettingsReader(string settingsXML)
        {            
            if (File.Exists(settingsXML))
            {
                fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = settingsXML;
                config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                sectionGroups = config.SectionGroups;
            }
            else
            {
                throw new Exception("An error occurred while trying to read stored settings." + Environment.NewLine +
                    "The configuration file: " + settingsXML + " was not found." + Environment.NewLine +
                     "Error CNF-336 in " + PROJ_FILE_NAME + ".SettingsReader()");
            }            
        }

        public String GetSettingValue(string settingElement)
        {
            var sve = GetSettingValueElement(settingElement);
            return sve.ValueXml.InnerText;            
        }

        public IEnumerable<string> GetSettingValues(string keyName)
        {
            var valueElement = GetSettingValueElement(keyName);
            var xElement = XElement.Parse(valueElement.ValueXml.InnerXml);
            return xElement.Descendants("string").Select(x => x.Value);
        }

        private SettingValueElement GetSettingValueElement(string keyName)
        {
            foreach (ConfigurationSectionGroup group in sectionGroups)  // Loop over all groups
            {
                if (!group.IsDeclared) // Only the ones which are actually defined in app.config
                    continue;
                foreach (ConfigurationSection section in group.Sections) // get all sections inside group
                {
                    var clientSection = section as ClientSettingsSection;
                    if (clientSection == null)
                        continue;
                    
                    foreach (SettingElement set in clientSection.Settings)
                    {
                        if (set.Name.Equals(keyName))
                        {
                            return set.Value;                                
                        }
                    }
                }
            }
            throw new Exception("An error occurred while trying to read a stored setting." + Environment.NewLine +
                "No value was found for: " + keyName + "." + Environment.NewLine +
                 "Error CNF-337 in " + PROJ_FILE_NAME + ".GetSettingValueElement()");
        }

        
    }
}
