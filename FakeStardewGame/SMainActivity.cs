using Android.App;
using Android.Content.PM;
using System;
using System.Reflection;

namespace FakeStardewGame
{
    [Activity(Label = "Fake Stardew Valley", Icon = "@mipmap/ic_launcher", Theme = "@style/Theme.Splash",
        MainLauncher = true, AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = (ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden
            | ConfigChanges.Orientation | ConfigChanges.ScreenLayout
            | ConfigChanges.ScreenSize | ConfigChanges.UiMode))]
    public sealed class SMainActivity : StardewValley.MainActivity
    {
        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            instance = this;

            try
            {
                var smapiLoaderAsm = Assembly.LoadFrom(ApplicationContext.GetExternalFilesDir(null) + "/SMAPILoader.dll");
                smapiLoaderAsm.GetType("SMAPILoader.Program").GetMethod("Start", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
            }
            catch (Exception e)
            {
                Android.Util.Log.Error("SMAPI-Tag", e.ToString());
            }

            base.OnCreate(bundle);
        }
    }
}
