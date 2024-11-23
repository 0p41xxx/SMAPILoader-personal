﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SMAPIGameLoader.Launcher;

internal static class FilePickerHelper
{
    public static FilePickerFileType FileTypeZip = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        { DevicePlatform.Android, new[] { "application/zip" } },
    });
    public static async Task<FileResult> PickZipFile(string title = null)
    {
        if (title == null)
            title = "Please select zip file";

        var options = new PickOptions
        {
            PickerTitle = title,
            FileTypes = FileTypeZip,
        };
        return await FilePicker.PickAsync(options);
    }
}
