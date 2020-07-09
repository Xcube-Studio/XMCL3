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
        public static string GamePath
        {
            get
            {
                if (UseDefaultDirectory)
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";
                else return (string)Json.Read("Files", "GamePath");
            }
        }
        public static string JavaPath { get { return (string)Json.Read("Files", "JavaPath"); } }
        public static bool CompleteResource { get { return (bool)Json.Read("Files", "CompleteResource"); } }
        public static bool UseDefaultDirectory { get { return (bool)Json.Read("Files", "UseDefaultDirectory"); } }
        public static string DownloadSource { get { return (string)Json.Read("Files", "DownloadSource"); } }
        public static bool MoreValueEnabled { get { return (bool)Json.Read("JVM", "MoreValueEnabled"); } }
        public static string Value { get { return (string)Json.Read("JVM", "Value"); } }
        public static int Memory { get { return (int)Json.Read("JVM", "Memory"); } }
        public static bool AutoMemory { get { return (bool)Json.Read("JVM", "AutoMemory"); } }
        public static string UUID { get { return (string)Json.Read("Login", "Choose"); } }
        public static string ClientToken { get { return (string)Json.Read("Login", "ClientToken"); } }
        public static bool IsFullScreen { get { return (bool)Json.Read("Video", "IsFullScreen"); } }
        public static string PrimaryHueMidBrush { get { return (string)Json.Read("Individualization", "PrimaryHueMidBrush"); } }
        public static string PrimaryHueLightBrush { get { return (string)Json.Read("Individualization", "PrimaryHueLightBrush"); } }
        public static string PrimaryHueDarkBrush { get { return (string)Json.Read("Individualization", "PrimaryHueDarkBrush"); } }
        public static bool UseSystemTheme { get { return (bool)Json.Read("Individualization", "UseSystemTheme"); } }
        public static string Background { get { return (string)Json.Read("Individualization", "Background"); } }
        public static bool AutoHideLaucher { get { return (bool)Json.Read("Individualization", "AutoHideLaucher"); } }
        public static bool AcrylicCard { get { return (bool)Json.Read("Individualization", "AcrylicCard"); } }
        public static bool Demo { get { return (bool)Json.Read("Game", "Demo"); } }
        public static string ServerIP { get { return (string)Json.Read("Game", "ServerIP"); } }
        public static string LatestVerison { get { return (string)Json.Read("Game", "LatestVerison"); } }
    }
}
