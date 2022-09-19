﻿using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib.BUTR.Extensions;

using System.IO;
using System.Xml;

namespace Bannerlord.ButterLib.Helpers
{
    internal static class LocalizedTextManagerUtils
    {
        private delegate XmlDocument? LoadXmlFileDelegate(string path);
        private static readonly LoadXmlFileDelegate? LoadXmlFile =
            AccessTools2.GetDeclaredDelegate<LoadXmlFileDelegate>("TaleWorlds.Localization.LocalizedTextManager:LoadXmlFile");

        private delegate void LoadFromXmlDelegate(XmlDocument doc, string modulePath);
        private static readonly LoadFromXmlDelegate? LoadFromXml =
            AccessTools2.GetDeclaredDelegate<LoadFromXmlDelegate>("TaleWorlds.Localization.LanguageData:LoadFromXml");

        public static void LoadLanguageData()
        {
            if (LoadXmlFile is null || LoadFromXml is null)
                return;

            if (ModuleInfoHelper.GetModulePath(typeof(LocalizedTextManagerUtils)) is not { } modulePath || !Directory.Exists(modulePath))
                return;

            foreach (var file in Directory.GetFiles(modulePath, "language_data._xml", SearchOption.AllDirectories))
            {
                if (LoadXmlFile(file) is { } xmlDocument)
                    LoadFromXml(xmlDocument, modulePath);
            }
        }
    }
}