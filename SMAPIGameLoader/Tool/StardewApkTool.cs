using Android.App;
using Android.Content.PM;
using System;

namespace SMAPIGameLoader;

internal static class StardewApkTool
{
    private static PackageInfo _currentPackageInfo;

    /// <summary>
    /// Load APK metadata directly from a file path (sideloaded/third-party).
    /// </summary>
    public static void LoadFromApkPath(string apkPath)
    {
        try
        {
            var pm = Application.Context.PackageManager;
            var info = pm.GetPackageArchiveInfo(apkPath, PackageInfoFlags.MetaData);

            if (info == null)
                throw new Exception("Invalid APK file: " + apkPath);

            info.ApplicationInfo.SourceDir = apkPath;
            info.ApplicationInfo.PublicSourceDir = apkPath;

            _currentPackageInfo = info;
            Console.WriteLine("Loaded Stardew APK from: " + apkPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading APK: " + ex);
            _currentPackageInfo = null;
        }
    }

    /// <summary>
    /// Try to auto-detect an installed Stardew package (any with 'stardew' in its name).
    /// </summary>
    public static void AutoDetectInstalled()
    {
        try
        {
            var pm = Application.Context.PackageManager;
            foreach (var pkg in pm.GetInstalledPackages(PackageInfoFlags.MetaData))
            {
                if (pkg.PackageName.Contains("stardew", StringComparison.OrdinalIgnoreCase))
                {
                    _currentPackageInfo = pkg;
                    Console.WriteLine("Found installed Stardew package: " + pkg.PackageName);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error scanning packages: " + ex);
        }
    }

    public static PackageInfo CurrentPackageInfo => _currentPackageInfo;

    public static bool IsInstalled => _currentPackageInfo != null;

    public static Android.Content.Context GetContext => Application.Context;

    public static string? BaseApkPath => _currentPackageInfo?.ApplicationInfo?.PublicSourceDir;

    public static string? ContentApkPath => BaseApkPath;

    public static Version GameVersionSupport => new Version(1, 6, 15, 0);

    public static Version CurrentGameVersion
    {
        get
        {
            try
            {
                return new Version(_currentPackageInfo?.VersionName ?? "0.0.0.0");
            }
            catch
            {
                return new Version(0, 0, 0, 0);
            }
        }
    }

    public static bool IsGameVersionSupport => CurrentGameVersion >= GameVersionSupport;
}
