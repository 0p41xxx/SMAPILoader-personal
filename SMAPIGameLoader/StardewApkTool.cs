﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAPIGameLoader;

internal static class StardewApkTool
{
    public const string PackageName = "com.chucklefish.stardewvalley";
    public static PackageInfo PackageInfo => ApkTool.GetPackageInfo(PackageName);

    public static bool IsInstalled
    {
        get
        {
            try
            {
                //check if found package
                var version = PackageInfo.VersionName;
                //check if we have 3 apks: [base, split_content & split_config]
                bool haveApksValid = SplitApks?.Count == 2;
                return haveApksValid;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
    public static void StartGame()
    {
        var intent = GetContext.PackageManager.GetLaunchIntentForPackage(PackageName);
        GetContext.StartActivity(intent);
    }
    public static Android.Content.Context GetContext => Application.Context;
    public static string? BaseApkPath => PackageInfo?.ApplicationInfo?.PublicSourceDir;
    public static IList<string>? SplitApks => PackageInfo?.ApplicationInfo?.SplitSourceDirs;

    public static string? ContentApkPath => SplitApks.First(path => path.Contains("split_content"));
    public static string? ConfigApkPath => SplitApks.First(path => path.Contains("split_config"));

    public readonly static Version GameVersionSupport = new Version("1.6.14.3");
    public static Version CurrentGameVersion => new Version(PackageInfo?.VersionName);
    public static bool IsGameVersionSupport => CurrentGameVersion >= GameVersionSupport;
}
