using System;
using YoutubeExplode;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime.ConstrainedExecution;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YouDLl
{
    public partial class MainForm : Form
    {
        sbyte progessBarRed = 2;
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            webView.Create(panel1.Handle);
            ModifyProgressBarColor.SetState(progressBar1, progessBarRed);
        }

        private async void btnGo_Click(object sender, EventArgs e)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var title = video.Title;
            var author = video.Author.ChannelTitle;
            var views = video.Engagement.ViewCount.ToString();
            var likes = video.Engagement.LikeCount.ToString();
            var dislikes = video.Engagement.DislikeCount.ToString();
            var date = video.UploadDate.Date.ToString();
            var videoId = url.Split("?v=")[1];

            webView.Url = "https://www.youtube.com/embed/" + videoId;

            this.Text = "Youdli - " + title;



            lblTitle.Text = title;
            lblAuthor.Text = author;
            lblViews.Text = views;
            lblLike.Text = likes;
            lblDislike.Text = dislikes;
            lblDate.Text = date;

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
            var fileName = $"{title}.{streamInfo.Container.Name}";
            var progress = new Progress<double>(p =>
            {
                progressBar1.Value = Convert.ToInt32(p * 100);
            });

            await youtube.Videos.Streams.DownloadAsync(streamInfo, ofd.SelectedPath + "\\" + fileName, progress);
            MessageBox.Show("the Video download Completed");
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            txtPath.Text = ofd.SelectedPath.ToString();
        }
    }

}