using Plossum.CommandLine;
using System.IO;

namespace FlashProgramerConsole
{

    [CommandLineManager(Description = "Use this write data to flash chip.")]
    internal class CommandLineOptions
    {
        [CommandLineOption(Name = "h", Aliases = "-help", Description = "Displays this help text")]
        public bool Help { get; set; }

        private string m_configFile;
        private string m_CKLink = "default";

        [CommandLineOption(Name = "f", Aliases = "-config_file", Description = "Specifies the config file", MinOccurs = 1)]
        public string ConfigFile
        {
            get => m_configFile;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOptionValueException("The file must not be empty", false);
                m_configFile = File.Exists(value) ? value : throw new InvalidOptionValueException("The config file not exists!");
            }
        }

        [CommandLineOption(Name = "c", Aliases = "-ck_link", Description = "Specifies the CK Link hardware ID")]
        public string CKLink
        {
            get => m_CKLink;
            set
            {
                m_CKLink = !string.IsNullOrEmpty(value) ? value : throw new InvalidOptionValueException("The CK Link ID must not be empty", false);
            }
        }
    }
}
