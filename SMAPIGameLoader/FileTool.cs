﻿using Android.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMAPIGameLoader;

internal static class FileTool
{
    public static string ExternalFilesDir => Application.Context.GetExternalFilesDir(null).AbsolutePath;
}
