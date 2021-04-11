using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaElementTest
{
    public partial class MainPage : ContentPage
    {
        private static int _currentFileNumber;
        const string _tempFileNamePrefix = "soundbytes";
        private MediaSource MyMediaSource;
        public MainPage()
        {
            InitializeComponent();
            MyMediaSource = new Uri("ms-appx:///ding_sound_effect.mp3");
            mediaElement.Source = MyMediaSource;
        }
        private void mediaElement_MediaEnded(object sender, EventArgs e)
        {

        }
        private string GetNextTempFileName()
        {
            _currentFileNumber++;
            return $"{_tempFileNamePrefix}{_currentFileNumber:D8}";
        }
        private string CreateTempDingFile()
        {
            string tempFileName = GetNextTempFileName();
            Stream stream = GetStreamFromResourceFile("ding-sound-effect.mp3");
            tempFileName = $"{tempFileName}.mp3";
            SaveStreamToTempFile(stream, tempFileName);
            return tempFileName;
        }
        private void SaveStreamToTempFile(Stream InputStream, string FileName)
        {
            string folder = Path.GetTempPath();
            string tempFilePath = Path.Combine(folder, FileName);
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            using (FileStream outputStream = File.Create(tempFilePath))
            {
                InputStream.CopyToAsync(outputStream);
            }
            bool fileExistsFlag = File.Exists(tempFilePath);
            if (fileExistsFlag)
            {
                System.Console.WriteLine("DEBUG - file exists at " + tempFilePath);
            }
        }
            private Stream GetStreamFromResourceFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("MediaElementTest.Assets." + filename);

            return stream;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            string tempFileName = CreateTempDingFile();
            MyMediaSource = new Uri($"ms-appdata:///temp/{tempFileName}");
            System.Console.WriteLine("Setting media source to: " + MyMediaSource.ToString());
            mediaElement.Source = MyMediaSource;
            mediaElement.Play();
        }
    }
}
