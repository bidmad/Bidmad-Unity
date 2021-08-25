using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

/// <summary> 
/// This class is responsible for making sure that the Xcode Project is set up for Swift 5.0
/// And Setting up Info.Plist
/// </summary> 

public static class BidmadPostProcessBuild {
        [PostProcessBuildAttribute ( 45 )]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath) {
            if(buildTarget == BuildTarget.iOS) {
                // We need to tell the Unity build to look at the write build file path and specifically reference the exposed Swift header file for it to work 
                var projectPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var project = new PBXProject();
                project.ReadFromFile(projectPath);
                var target =
                #if UNITY_2019_3_OR_NEWER
                    project.GetUnityMainTargetGuid();
                #else
                    project.TargetGuidByName("Unity-iPhone");
                #endif

                // We specifically reference the generated Swift to Objective-C header 
                project.SetBuildProperty(target, "SWIFT_VERSION", "5.0");

                project.WriteToFile(projectPath);

                // We now set up a plist
                string plistPath = buildPath + "/Info.plist";
                PlistDocument plist = new PlistDocument(); 
                plist.ReadFromFile(plistPath);
                PlistElementDict rootDict = plist.root;

                // Set the IDFA request description
                string trackingDescription = "Your data will be used to provide you a better and personalized ad experience.";
                rootDict.SetString("NSUserTrackingUsageDescription", trackingDescription);

                // Set GADApplicationIdentifier (Please contact ADOP for your own ADMOB APPID)
                rootDict.SetString("GADApplicationIdentifier", "YOUR GOOGLE ADMOB APPID");

                // Set SKAdNetwork
                var skAdNetworkArray = rootDict.CreateArray("SKAdNetworkItems");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cstr6suwn9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v9wttpbfk9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n38lu8286q.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4dzt52r2t5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "bvpn9ufa9b.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "2u9pt9hc89.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4468km3ulz.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4fzdc2evr5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7ug5zh24hu.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8s468mfl3y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9rd848q2bz.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9t245vhmpl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "av6w8kgt66.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f38h382jlk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "hs6bdukanm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "kbd757ywx3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ludvb6z3bs.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "m8dbw4sv7c.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mlmmfzh3r3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "prcb7njmu6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "t38b2kh725.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "tl55sbb4fm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "wzmmz9fp6w.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "yclnxrl5pm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ydx93a7ass.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "238da6jt44.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "22mmun2rn5.skadnetwork");
                
                plist.WriteToFile(plistPath);
            }
        }
    }