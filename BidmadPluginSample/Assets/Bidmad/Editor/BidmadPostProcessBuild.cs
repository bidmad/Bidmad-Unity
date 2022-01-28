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
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "275upjj5gd.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "294l99pt4k.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "2fnua5tdw4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "2u9pt9hc89.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3rd42ekr43.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4468km3ulz.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "44jx6755aq.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "44n7hlldy6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4fzdc2evr5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4pfyvq9l8r.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "523jb4fst2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5l3tpt7t6e.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5lm9lj6jb7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6964rsfnh4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6g9af3uyq4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "74b6s63p6l.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7rz58n8ntl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "7ug5zh24hu.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "84993kbrcf.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8s468mfl3y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9nlqeag3gk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9rd848q2bz.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9t245vhmpl.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "a7xqa6mtl2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "c6k4g5qg8m.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cg4yq2srnc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cj5566h2ga.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "e5fvkxwrpn.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ejvt5qm6ak.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "g28c52eehv.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "g2y4y55b64.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "gta9lk7p23.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "hs6bdukanm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "kbd757ywx3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "kbmxgpxpgc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "klf5c3l5u5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "m8dbw4sv7c.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mlmmfzh3r3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mtkv5xtk9e.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n6fk4nfna4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n9x2a789qt.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ppxm28t8ap.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "prcb7njmu6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "pwa73g5rt2.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "pwdxu55a5a.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "qqp299437r.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "r45fhb6rf7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "rx5hdcabgc.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "t38b2kh725.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "tl55sbb4fm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "u679fj5vs4.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "uw77j35x4d.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v72qych5uu.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "wg4vff78zm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "wzmmz9fp6w.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "yclnxrl5pm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ydx93a7ass.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3qcr597p9d.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3qy4746246.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "3sh42y64q3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "424m5254lk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "4dzt52r2t5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "578prtvx9j.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5a6flpkh64.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "8c4e2ghe7u.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "av6w8kgt66.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "cstr6suwn9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f38h382jlk.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "p78axxw29g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "s39g8k73mm.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v4nxqhlyqp.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "zq492l623r.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "24t9a8vw3c.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "32z4fx6l9h.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "54nzkqm89y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "6xzpu9s2p8.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "79pbpufp6p.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9b89h5y424.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "c3frkrj4fj.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "feyaarzu9v.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ggvn48r87g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "glqzh8vgby.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "k674qkevps.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "ludvb6z3bs.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "rvh3l7un93.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x8jxxk4ff5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "xy9t38ct57.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "zmvfpc5aq8.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "n38lu8286q.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v9wttpbfk9.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "22mmun2rn5.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "238da6jt44.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "488r3q3dtq.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "5tjdwbrq8w.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f7s53z58qe.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "lr83yxwka7.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "mp6xlyr22a.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "v79kvwwj4g.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "w9q455wk68.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x44k69ngh6.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "bvpn9ufa9b.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "252b5q8x7y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "9g2aggbj52.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "dzg6xy7pwj.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "f73kdq92p3.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "hdw39hrw9y.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "krvm3zuq6h.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "x8uqf25wch.skadnetwork");
                skAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "y45688jllp.skadnetwork");

                plist.WriteToFile(plistPath);
            }
        }
    }