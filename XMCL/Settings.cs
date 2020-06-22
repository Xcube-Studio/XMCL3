using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMCL.Core;

namespace XMCL
{
    public class Settings
    {
        public static string GamePath() 
        {
            if (UseDefaultDirectory)
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";
            else return (string)Json.Read("Files", "GamePath");
        }
        public static string JavaPath = (string)Json.Read("Files", "JavaPath");
        public static bool CompleteResource = (bool)Json.Read("Files", "CompleteResource");
        public static bool UseDefaultDirectory = (bool)Json.Read("Files", "UseDefaultDirectory");
        public static string DownloadSource = (string)Json.Read("Files", "DownloadSource");

        public static bool MoreValueEnabled = (bool)Json.Read("JVM", "MoreValueEnabled");
        public static string Value = (string)Json.Read("JVM", "Value");
        public static int Memory = (int)Json.Read("JVM", "Memory");
        public static bool AutoMemory = (bool)Json.Read("JVM", "AutoMemory");

        public static string UUID = (string)Json.Read("Login", "Choose");
        public static string ClientToken = (string)Json.Read("Login", "ClientToken");

        public static bool IsFullScreen = (bool)Json.Read("Video", "IsFullScreen");

        public static string PrimaryHueMidBrush = (string)Json.Read("Individualization", "PrimaryHueMidBrush");
        public static string PrimaryHueLightBrush = (string)Json.Read("Individualization", "PrimaryHueLightBrush");
        public static string PrimaryHueDarkBrush = (string)Json.Read("Individualization", "PrimaryHueDarkBrush");
        public static bool UseSystemTheme = (bool)Json.Read("Individualization", "UseSystemTheme");
        public static string Background = (string)Json.Read("Individualization", "Background");
        public static bool AutoHideLaucher = (bool)Json.Read("Individualization", "AutoHideLaucher");
        public static bool AcrylicCard = (bool)Json.Read("Individualization", "AcrylicCard");

        public static bool Demo = (bool)Json.Read("Files", "UseDefaultDirectory");
        public static string ServerIP = (string)Json.Read("Game", "ServerIP");
        public static string LatestVerison = (string)Json.Read("Game", "LatestVerison");
    }
}
