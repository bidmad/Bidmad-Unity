using System;
using UnityEditor;
using UnityEngine;

namespace Bidmad.Editor
{
    public static class BidmadPackageExporter
    {
        public static void Export()
        {
            try
            {
                string outPath = Environment.GetEnvironmentVariable("BIDMAD_PACKAGE_OUTPUT");
                if (string.IsNullOrEmpty(outPath))
                {
                    Debug.LogError("BIDMAD_PACKAGE_OUTPUT env var not set");
                    EditorApplication.Exit(2);
                    return;
                }

                string[] folders = new[]
                {
                    "Assets/Bidmad/Editor",
                    "Assets/Bidmad/Scripts/Bidmad",
                    "Assets/ExternalDependencyManager",
                    "Assets/Plugins/iOS/Bidmad",
                };

                AssetDatabase.ExportPackage(folders, outPath, ExportPackageOptions.Recurse);
                Debug.Log("Bidmad package exported to: " + outPath);
                EditorApplication.Exit(0);
            }
            catch (Exception ex)
            {
                Debug.LogError("BidmadPackageExporter failed: " + ex);
                EditorApplication.Exit(1);
            }
        }
    }
}
