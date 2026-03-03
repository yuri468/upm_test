# Editor Folder

This folder contains Unity Editor-only code that is excluded from runtime builds.

## What is the Editor Folder?

Unity's [special folder convention](https://docs.unity3d.com/Manual/SpecialFolders.html) - any folder named `Editor` has these properties:

1. **Editor-Only Compilation**: Code here can use the `UnityEditor` namespace
2. **Excluded from Builds**: Not included in APK/IPA/standalone builds
3. **Development Tools**: Build scripts, custom inspectors, editor utilities

## Files in This Folder

- `AndroidBuilder.cs` - Command-line Android build automation
- `MeticaIOSBuildPostProcessor.cs` - iOS build post-processing
- `MeticaDependencies.xml` - Android dependency configuration

## When to Use Editor Folder

Place code here if it:
- Uses `UnityEditor` APIs
- Is only needed during development
- Should not increase runtime build size
