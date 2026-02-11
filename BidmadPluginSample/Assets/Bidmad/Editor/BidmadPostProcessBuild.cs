using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Custom;
using UnityEditor.iOS.Xcode.Custom.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class BidmadPostProcessBuild {
        [PostProcessBuildAttribute ( 150 )]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath) {
        if(buildTarget == BuildTarget.iOS) {
                // We need to tell the Unity build to look at the write build file path and specifically reference the exposed Swift header file for it to work 
                var projectPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var project = new UnityEditor.iOS.Xcode.Custom.PBXProject();
                project.ReadFromFile(projectPath);

                var target = project.TargetGuidByName("Unity-iPhone");

                // We specifically reference the generated Swift to Objective-C header 
                project.SetBuildProperty(target, "SWIFT_VERSION", "5.0");
                
                // Embed dynamic frameworks from Pods folder
                EmbedDynamicFrameworks(project, target, buildPath, projectPath);
                
                project.WriteToFile(projectPath);

                // We now set up a plist
                string plistPath = buildPath + "/Info.plist";
                UnityEditor.iOS.Xcode.Custom.PlistDocument plist = new UnityEditor.iOS.Xcode.Custom.PlistDocument(); 
                plist.ReadFromFile(plistPath);
                UnityEditor.iOS.Xcode.Custom.PlistElementDict rootDict = plist.root;

                // Set the IDFA request description
                string trackingDescription = "Your data will be used to provide you a better and personalized ad experience.";
                rootDict.SetString("NSUserTrackingUsageDescription", trackingDescription);

                // Set GADApplicationIdentifier (Please contact ADOP for your own ADMOB APPID)
                rootDict.SetString("GADApplicationIdentifier", "YOUR GOOGLE ADMOB APPID");

                // Set SKAdNetwork
                var skAdNetworkArray = rootDict.CreateArray("SKAdNetworkItems");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "22mmun2rn5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "238da6jt44.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "24t9a8vw3c.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "24zw6aqk47.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "252b5q8x7y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "275upjj5gd.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "294l99pt4k.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "2fnua5tdw4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "2u9pt9hc89.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "32z4fx6l9h.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3l6bd9hu43.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3qcr597p9d.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3qy4746246.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3rd42ekr43.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3sh42y64q3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "424m5254lk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4468km3ulz.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "44jx6755aq.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "44n7hlldy6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "47vhws6wlr.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "488r3q3dtq.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4dzt52r2t5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4fzdc2evr5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4mn522wn87.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4pfyvq9l8r.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4w7y6s5ca2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "523jb4fst2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "52fl2v3hgk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "54NZKQM89Y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "54nzkqm89y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "578prtvx9j.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5a6flpkh64.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5l3tpt7t6e.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5lm9lj6jb7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5tjdwbrq8w.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6964rsfnh4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6g9af3uyq4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6p4ks3rnbw.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6v7lgmsu45.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6xzpu9s2p8.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6yxyv74ff7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "737z793b9f.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "74b6s63p6l.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7953jerfzd.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "79pbpufp6p.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7fmhfwg9en.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7rz58n8ntl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7ug5zh24hu.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "84993kbrcf.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "89z7zv988g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8c4e2ghe7u.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8m87ys6875.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8r8llnkz5a.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8s468mfl3y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "97r2b46745.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9b89h5y424.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9g2aggbj52.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9nlqeag3gk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9rd848q2bz.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9t245vhmpl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9vvzujtq5s.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9yg77x724h.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "a2p9lx4jpn.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "a7xqa6mtl2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "a8cz6cu7e5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "av6w8kgt66.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "b9bk5wbcq9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "bmxgpxpgc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "bvpn9ufa9b.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "bxvub5ada5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "c3frkrj4fj.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "c6k4g5qg8m.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cg4yq2srnc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cj5566h2ga.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cp8zw746q7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cs644xg564.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cstr6suwn9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "dbu4b84rxf.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "dkc879ngq3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "dzg6xy7pwj.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "e5fvkxwrpn.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ecpz2srf59.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "eh6m2bh4zr.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ejvt5qm6ak.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f38h382jlk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f73kdq92p3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f7s53z58qe.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "feyaarzu9v.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "g28c52eehv.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "g2y4y55b64.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ggvn48r87g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "glqzh8vgby.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "gta8lk7p23.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "gta9lk7p23.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "gvmwg8q7h5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "h65wbv5k3f.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "hb56zgv37p.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "hdw39hrw9y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "hs6bdukanm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "k674qkevps.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "k6y4y55b64.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "kbd757ywx3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "kbmxgpxpgc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "klf5c3l5u5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "krvm3zuq6h.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "lr83yxwka7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ludvb6z3bs.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "m297p6643m.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "m5mvw97r93.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "m8dbw4sv7c.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mlmmfzh3r3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mls7yz5dvl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mp6xlyr22a.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mqn7fxpca7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mtkv5xtk9e.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n38lu8286q.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n66cz3y3bx.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n6fk4nfna4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n9x2a789qt.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "nu4557a4je.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "nzq8sh4pbs.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "p78axxw29g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ppxm28t8ap.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "prcb7njmu6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "pu4na253f3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "pwa73g5rt2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "pwdxu55a5a.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "qqp299437r.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "qu637u8glc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "qwpu75vrh2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "r26jy69rpl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "r45fhb6rf7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "rvh3l7un93.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "rx5hdcabgc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "s39g8k73mm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "s69wq72ugq.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "sczv5946wb.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "su67r6k2v3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "t38b2kh725.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "t6d3zquu66.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "tl55sbb4fm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "tmhh9296z4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "tvvz7th9br.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "u679fj5vs4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "uw77j35x4d.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v4nxqhlyqp.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v72qych5uu.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v79kvwwj4g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v9wttpbfk9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "vcra2ehyfk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "vutu7akeur.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "w9q455wk68.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "wg4vff78zm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "wzmmZ9fp6w.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "wzmmz9fp6w.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x2jnk7ly8j.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x44k69ngh6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x5l83yy675.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x8jxxk4ff5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x8uqf25wch.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "xy9t38ct57.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "y45688jllp.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "y5ghdn5j9k.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "yclnxrl5pm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ydx93a7ass.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "yrqqpx2mcb.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "z24wtl6j62.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "z4gj7hsk7h.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "zmvfpc5aq8.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "zq492l623r.skadnetwork");

                plist.WriteToFile(plistPath);

                RemoveThumbOption(buildTarget, buildPath);
            }
    }
    
    private static void EmbedDynamicFrameworks(UnityEditor.iOS.Xcode.Custom.PBXProject project, string target, string buildPath, string projectPath) {
        string podsPath = Path.Combine(buildPath, "Pods");
        
        if (!Directory.Exists(podsPath)) {
            UnityEngine.Debug.Log("Pods directory not found at: " + podsPath);
            return;
        }
        
        var frameworkDirectories = Directory.GetDirectories(podsPath, "*.framework", SearchOption.AllDirectories);
        UnityEngine.Debug.Log("frameworkDirectories: " + string.Join(", ", frameworkDirectories));

        // Check if the parent folder of each of frameworkDirectories does not contain simulator keyword.
        // If it does, skip the framework.
        var frameworkSearchPaths = new List<string>();

        frameworkSearchPaths.Add("$(SRCROOT)/Pods");

        foreach (var frameworkPath in frameworkDirectories) {
            string parentFolder = Path.GetDirectoryName(frameworkPath);
            if (parentFolder.Contains("simulator") || parentFolder.Contains("tvos") || parentFolder.Contains("maccatalyst") || parentFolder.Contains("X86") || parentFolder.Contains("x86")) {
                UnityEngine.Debug.Log("Skipping framework in simulator or tvos folder: " + frameworkPath);
            } else {
                string? searchablePath = ProcessFramework(project, target, buildPath, podsPath, frameworkPath, projectPath);
                if (searchablePath != null) {
                    frameworkSearchPaths.Add(searchablePath);
                }
            }
        }

        UnityEngine.Debug.Log("frameworkSearchPaths: " + string.Join(", ", frameworkSearchPaths));

        project.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
        
        foreach (var frameworkSearchPath in frameworkSearchPaths) {
            project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", frameworkSearchPath);
        }
    }
    
    private static string? ProcessFramework(UnityEditor.iOS.Xcode.Custom.PBXProject project, string target, string buildPath, string podsPath, string frameworkPath, string projectPath) {
        string frameworkName = Path.GetFileNameWithoutExtension(frameworkPath);
        string relativeFrameworkPath = "Pods/" + GetRelativePath(podsPath, frameworkPath);
        
        // Check if it's a dynamic framework by examining the binary file type
        string binaryPath = Path.Combine(frameworkPath, frameworkName);
        if (File.Exists(binaryPath)) {
            bool isDynamicFramework = IsDynamicFramework(binaryPath);
            
            if (isDynamicFramework) {
                return EmbedFramework(project, target, frameworkName, relativeFrameworkPath, projectPath);
            } else {
                UnityEngine.Debug.Log("Skipped static framework: " + frameworkName);
            }
        }

        return null;
    }
    
    private static string EmbedFramework(UnityEditor.iOS.Xcode.Custom.PBXProject project, string target, string frameworkName, string relativeFrameworkPath, string projectPath) {
        // Add the framework to the project
        string frameworkGuid = project.AddFile(relativeFrameworkPath, relativeFrameworkPath, UnityEditor.iOS.Xcode.Custom.PBXSourceTree.Source);
        
        // Add the framework to the target's frameworks build phase
        project.AddFileToBuild(target, frameworkGuid);
        
        // Use the extension method to properly embed the framework
        PBXProjectExtensions.AddFileToEmbedFrameworks(project, target, frameworkGuid);
        UnityEngine.Debug.Log($"Successfully embedded {frameworkName} using PBXProjectExtensions");

        string searchablePath = "$(SRCROOT)/" + Regex.Replace(relativeFrameworkPath, @"/[^/]+\.framework$", "");

        UnityEngine.Debug.Log("Embedded dynamic framework: " + frameworkName);

        return searchablePath;
    }
    
    private static bool IsDynamicFramework(string binaryPath) {
        try {
            // Use the 'file' command to determine the binary type
            var startInfo = new ProcessStartInfo {
                FileName = "file",
                Arguments = $"\"{binaryPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            using (var process = Process.Start(startInfo)) {
                if (process != null) {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0) {
                        UnityEngine.Debug.LogError($"'file' command failed for {binaryPath}: {error}");
                        return false;
                    }
                    
                    // Check if the output contains "dynamically linked shared library"
                    // This indicates a dynamic framework
                    bool isDynamic = output.Contains("dynamically linked shared library");
                    
                    // Also check for "current ar archive" which indicates a static library
                    bool isStatic = output.Contains("current ar archive");
                    
                    // Log the file type for debugging
                    UnityEngine.Debug.Log($"Framework binary type for {Path.GetFileName(binaryPath)}: {output.Trim()}");
                    
                    if (isDynamic) {
                        UnityEngine.Debug.Log($"Detected dynamic framework: {Path.GetFileName(binaryPath)}");
                        return true;
                    } else if (isStatic) {
                        UnityEngine.Debug.Log($"Detected static framework: {Path.GetFileName(binaryPath)}");
                        return false;
                    } else {
                        // If we can't determine, log a warning and default to static
                        UnityEngine.Debug.LogWarning($"Could not determine framework type for {Path.GetFileName(binaryPath)}, defaulting to static. Output: {output.Trim()}");
                        return false;
                    }
                }
            }
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogError($"Error checking framework type for {binaryPath}: {ex.Message}");
        }
        
        // Default to false (static) if we can't determine
        return false;
    }
    
    private static string GetRelativePath(string basePath, string fullPath) {
        if (fullPath.StartsWith(basePath)) {
            return fullPath.Substring(basePath.Length).TrimStart(Path.DirectorySeparatorChar);
        }
        return fullPath;
    }

    public static void RemoveThumbOption(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            //1.Load the Xcode project into a PBXProject instance, and get the main target.
            var projectPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var project = new UnityEditor.iOS.Xcode.PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            var mainTargetGuid = project.GetUnityMainTargetGuid();

            //OTHER_CFLAGS
            var otherCFlags = project.GetBuildPropertyForAnyConfig(mainTargetGuid, "OTHER_CFLAGS");
            if (otherCFlags != null || otherCFlags != "")
            {
                var newOtherCFlags = otherCFlags.Replace("-mno-thumb", "");
                project.SetBuildProperty(mainTargetGuid, "OTHER_CFLAGS", newOtherCFlags);
                project.WriteToFile(projectPath);
            }
        }
    }
}