﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMAPIGameLoader;

internal static class FileTool
{
    public static string ExternalFilesDir = Application.Context.GetExternalFilesDir(null).AbsolutePath;
    public static string InternalFilesDir = Application.Context.FilesDir.AbsolutePath;
    public static string ExternalAppDir = ExternalFilesDir.Substring(0, ExternalFilesDir.Length - 6);
    public static string SafePath(string path)
    {
        path = path.Replace("\\", "/");
        path = path.Replace("//", "/");
        return path;
    }
    public static float ConvertBytesToMB(long bytes)
    {
        return bytes / (1024f * 1024f);
    }

    internal static void MakeSureDirectory(string dir)
    {
        Directory.CreateDirectory(dir);
    }
    internal static void MakeSureFilePath(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        MakeSureDirectory(dir);
    }

    internal static void ClearCache()
    {
        try
        {

            var dir = Application.Context.ExternalCacheDir;
            if (dir.Exists())
                dir.Delete();

            dir = Application.Context.CacheDir;
            if (dir.Exists())
                dir.Delete();
        }
        catch (Exception ex)
        {
            ErrorDialogTool.Show(ex);
        }
    }

    internal static void Delete(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    internal static string GetFirstDirNameFromFilePath(string fileName)
    {
        string fileDirPath = Path.GetDirectoryName(fileName);
        string[] dirNames = fileDirPath.Split(Path.DirectorySeparatorChar);
        return dirNames.FirstOrDefault();
    }

    //beware don't use entry.Name, it's not works if it dir entry
    internal static bool IsEntryDirectory(ZipArchiveEntry entry) => entry.FullName.EndsWith("/");
    internal static bool IsEntryFile(ZipArchiveEntry entry) => !IsEntryDirectory(entry);

    internal static void OpenAppFilesExternalFilesDir(string additionPath)
    {
        string initPath = Path.Combine(ExternalFilesDir, additionPath);
        OpenAppFiles(initPath);
    }
    const string BaseDocumentPrimaryPath = "content://com.android.externalstorage.documents/document/primary";
    internal static void OpenAppFiles(string initDirPath)
    {
        const string filesPackageName = "com.google.android.documentsui";
        const string filesActivityName = "com.android.documentsui.files.FilesActivity";
        if (ApkTool.GetPackageInfo(filesPackageName) is null)
        {
            ErrorDialogTool.Show(new($"Not found app {filesPackageName}"), "Try Open Folder Mods");
            return;
        }

        try
        {
            //check path not exist, use external files dir 
            if (Directory.Exists(initDirPath) is false)
            {
                //default at ../files path
                initDirPath = ExternalFilesDir;
            }

            //safe path!!
            //remove path /storage/emulated/0
            initDirPath = initDirPath.Replace("/storage/emulated/0/", "");
            var uriString = $"{BaseDocumentPrimaryPath}%3A{initDirPath.Replace("/", "%2F")}";
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.Parse(uriString), "vnd.android.document/directory");
            intent.SetClassName(filesPackageName, filesActivityName);
            intent.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }
        catch (Exception ex)
        {
            ErrorDialogTool.Show(ex);
        }
    }
}
