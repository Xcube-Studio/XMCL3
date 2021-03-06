﻿namespace XMCL
{
    public class Settings
    {
        public static string GamePath
        {
            get
            {
                if ((bool)Json.ReadPath(GamePathName, "RelativePath"))
                    return System.IO.Path.GetFullPath((string)Json.ReadPath(GamePathName, "Path"));
                else return (string)Json.ReadPath(GamePathName, "Path");
            }
        }
        public static string GamePathName { get { return (string)Json.Read("Files", "GamePathName"); } }
        public static string JavaPath { get { return (string)Json.Read("Files", "JavaPath"); } }
        public static bool CompleteResource { get { return (bool)Json.Read("Files", "CompleteResource"); } }
        public static XL.Core.Tools.DownloadSource DownloadSource
        {
            get
            {
                if (Json.Read("Files", "DownloadSource").ToString() == "Mojang")
                    return XL.Core.Tools.DownloadSource.Mojang;
                else if (Json.Read("Files", "DownloadSource").ToString() == "BMCLAPI")
                    return XL.Core.Tools.DownloadSource.BMCPAPI;
                else if (Json.Read("Files", "DownloadSource").ToString() == "Mcbbs")
                    return XL.Core.Tools.DownloadSource.Mcbbs;
                else return XL.Core.Tools.DownloadSource.Mojang;
            }
        }
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
        public static bool BGP { get { return (bool)Json.Read("Individualization", "BackgroundPlus"); } }
        public static bool Demo { get { return (bool)Json.Read("Game", "Demo"); } }
        public static string ServerIP { get { return (string)Json.Read("Game", "ServerIP"); } }
        public static string LatestVerison { get { return (string)Json.Read("Game", "LatestVerison"); } }
    }
}
