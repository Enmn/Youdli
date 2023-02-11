using System;
using YoutubeExplode;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime.ConstrainedExecution;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using System.Diagnostics;

namespace YouDLl
{
    public partial class MainForm : Form
    {
        ChromiumWebBrowser browser;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webView.Create(panel1.Handle);
        }

        private async void btnGo_Click(object sender, EventArgs e)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(txtUrl.Text);
            var title = video.Title;
            var author = video.Author.ChannelTitle;
            var views = video.Engagement.ViewCount.ToString();
            var date = video.UploadDate.Date.ToString();
            var videoId = txtUrl.Text.Split("?v=")[1];

            webView.Url = "https://www.youtube.com/embed/" + videoId;

            this.Text = "Youdli - " + title;

            lblTitle.Text = title;
            lblAuthor.Text = author;
            lblViews.Text = views;
            lblDate.Text = date;

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
            var fileName = $"{title}.{streamInfo.Container.Name}";
            var progress = new Progress<double>(p =>
            {
                progressBar1.Value = Convert.ToInt32(p * 100);
            });

            await youtube.Videos.Streams.DownloadAsync(streamInfo, ofd.SelectedPath + "\\" + fileName, progress);
            this.Text = "Youdli";
            progressBar1.Value = 0;
            MessageBox.Show("the Video download Completed");
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            txtPath.Text = ofd.SelectedPath.ToString();
        }
    }
}