#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

public class MeticaIOSBuildPostProcessor
{
    // Adjust this path to match where your framework ends up in the Xcode project relative to the project root.
    // Unity typically mirrors the Assets structure inside the "Frameworks" folder.
    private const string FrameworkPath = "Frameworks/MeticaSdk/Plugins/iOS/MeticaSDKFramework.xcframework";

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
            return;

        string pbxProjectPath = PBXProject.GetPBXProjectPath(buildPath);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(pbxProjectPath);

        // 1. Get the GUID of the Main App Target (where the app bundle lives)
        string mainTargetGuid = proj.GetUnityMainTargetGuid();
        
        // 2. Get the GUID of the UnityFramework Target (where your C# code lives)
        string unityFrameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();

        // 3. Find the file GUID of your .xcframework
        // Note: Unity should have already added the file reference to the project if it was checked in the Inspector.
        // We just need to find it.
        string fileGuid = proj.FindFileGuidByProjectPath(FrameworkPath);

        if (string.IsNullOrEmpty(fileGuid))
        {
            // Fallback: If Unity didn't add it to the project structure, we might need to search for it manually 
            // or warn the user. For now, let's try to find it by name if the path failed.
            // This happens if you move the file or if Unity flattens paths.
            fileGuid = proj.FindFileGuidByProjectPath("Frameworks/Plugins/iOS/MeticaSDKFramework.xcframework");
            
            if (string.IsNullOrEmpty(fileGuid))
            {
                UnityEngine.Debug.LogError("Metica PostProcess: Could not find MeticaSDKFramework.xcframework in the Xcode project. " +
                                           "Ensure 'iOS' is checked in the Plugin Inspector.");
                return;
            }
        }

        // 4. Ensure it is LINKED to the UnityFramework target (so your code can call it)
        // Unity usually does this automatically, but we force it to be safe.
        proj.AddFileToBuild(unityFrameworkTargetGuid, fileGuid);

        // 5. Ensure it is EMBEDDED in the Main App Target (crucial for dynamic xcframeworks)
        // This effectively sets "Embed & Sign" in the "Frameworks, Libraries, and Embedded Content" section.
        PBXProjectExtensions.AddFileToEmbedFrameworks(proj, mainTargetGuid, fileGuid);

        // Save changes
        proj.WriteToFile(pbxProjectPath);
        
        UnityEngine.Debug.Log("Metica PostProcess: Successfully embedded MeticaSDKFramework.xcframework into the Xcode project.");
    }
}
#endif
