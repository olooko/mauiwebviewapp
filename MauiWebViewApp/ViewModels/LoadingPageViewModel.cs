using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWebViewApp.ViewModels
{
    public partial class LoadingPageViewModel : ObservableObject
    {
        private const string RELEASES_URL = "https://webview.olooko.xyz/releases";

        private const int BUFF_SIZE = 81920;

        [ObservableProperty]
        private double _progressValue;

        public LoadingPageViewModel()
        {
#if (ANDROID && RELEASE)
            UpdateApk();
#endif
        }

        private async void UpdateApk()
        {
            HttpResponseMessage response = await (new HttpClient()).GetAsync(string.Format("{0}/version.html", RELEASES_URL));
            var versionString = await response.Content.ReadAsStringAsync();
            
            if (CompareUpdatingVersion(AppInfo.Current.VersionString, versionString))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string downloadUrl = string.Format("{0}/{1}-{2}.apk", RELEASES_URL, AppInfo.PackageName, versionString);
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                    httpResponseMessage.EnsureSuccessStatusCode();

                    string outputFileName = Path.Combine(FileSystem.AppDataDirectory, string.Format("{0}.apk", AppInfo.PackageName));

                    var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.None, BUFF_SIZE, true);

                    long totalLength = (long)httpResponseMessage.Content.Headers.ContentLength!;
                    long readLength = 0;

                    while (true)
                    {
                        var buffer = new byte[BUFF_SIZE];

                        int length = await contentStream.ReadAsync(buffer, 0, BUFF_SIZE);

                        if (length == 0) break;

                        await fileStream.WriteAsync(buffer, 0, length);

                        readLength += (long)length;

                        int percent = (int)(readLength * 100 / totalLength);

                        this.ProgressValue = percent / 100d;
                    }

                    fileStream.Close();
                    contentStream.Close();

                    var file = new Java.IO.File(outputFileName);
                    var context = Android.App.Application.Context;

                    var intent = new Android.Content.Intent(Android.Content.Intent.ActionInstallPackage);
                    intent.AddFlags(Android.Content.ActivityFlags.GrantReadUriPermission);
                    intent.AddFlags(Android.Content.ActivityFlags.NewTask);

                    var fileUri = AndroidX.Core.Content.FileProvider.GetUriForFile(context, AppInfo.PackageName + ".provider", file);

                    intent.SetData(fileUri);
                    intent.PutExtra(Android.Content.Intent.ExtraInstallerPackageName, AppInfo.PackageName);

                    context.StartActivity(intent);
                }
            }
        }


        private bool CompareUpdatingVersion(string oldVersion, string newVersion)
        {
            string[] oldVersionSplittedString = oldVersion.Split(".");
            string[] newVersionSplittedString = newVersion.Split(".");

            bool isNeededUpdating = false;

            if (newVersionSplittedString.Length == oldVersionSplittedString.Length)
            {
                for (int i = 0; i < newVersionSplittedString.Length; i++)
                {
                    int oldNumber = Convert.ToInt32(oldVersionSplittedString[i]);
                    int newNumber = Convert.ToInt32(newVersionSplittedString[i]);
                    if (newNumber > oldNumber)
                    {
                        isNeededUpdating = true;
                        break;
                    }
                    else if (newNumber < oldNumber)
                    {
                        break;
                    }
                }
            }

            return isNeededUpdating;
        }

    }
}
