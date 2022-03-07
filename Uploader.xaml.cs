using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Win32;
using YouTube_Video_Uploader.YouTubeAPI;

namespace YouTube_Video_Uploader
{
    /// <summary>
    /// Interaction logic for Uploader.xaml
    /// </summary>
    public partial class Uploader : Page
    {
        private List<string> paths;
        public Uploader()
        {
             InitializeComponent();
        }
        public Uploader(List<string> files, List<string> paths)
        {
            InitializeComponent();
            this.paths = paths;
              for (int i = 0; i < files.Count; i++)
              { 
                  cb_videos.Items.Add(files[i]);
              }
        }

        #region Events
        private void btn_chooseimage_Click(object sender, RoutedEventArgs e)
        {
           
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            bool? dialogResult = ofd.ShowDialog();
            switch (dialogResult)
            {
                case true:
                    System.Drawing.Image img = System.Drawing.Image.FromFile(ofd.FileName);
                    if (img.Width>=1280 && img.Height>=720)
                    {
                        tb_image_path.Foreground = Brushes.Gray;
                        tb_image_path.Text = ofd.FileName;
                    }
                    else
                    {   
                        tb_image_path.Foreground = (Brush)(new BrushConverter().ConvertFrom("#EA0010")); 
                        tb_image_path.Text = "Image smaller than 1280x720";
                    }
                    break;
            }
        }

        #endregion
        private void ch_nothumbnail_Checked(object sender, RoutedEventArgs e)
        {
          btn_chooseimage.IsEnabled = false;
          tb_image_path.Text = String.Empty;
        }
        private void Ch_nothumbnail_OnUnchecked(object sender, RoutedEventArgs e)
        {
            btn_chooseimage.IsEnabled = true;
        }

        private async void UploadWithoutThumbnail()
        {
            var video = new YouTubeAPI.Video()
            {
                VideoPath = paths[cb_videos.SelectedIndex],
                Title = tb_title.Text,
                Description = tb_description.Text,

                Tags = tb_tags.Text.Split(','),

                Category = (Categories)(cb_categories.SelectedItem),
                Privacy = (Privacies)(cb_privacies.SelectedItem)
            };
            var jsonSecret = @"YouTubeAPI\secret.json";
            var AppName = "YouTube Video Uploader";
            var channelID = tb_channellink.Text.Substring(tb_channellink.Text.LastIndexOf("/channel/") + 9);
            var result = await Credentials.FromSecret(jsonSecret, AppName, channelID).Authorize().Upload(video);
            if (result.Status == Statuses.Failed)
            {
                MessageBox.Show(result.Error);
            }
        }

        private async void UploadWithThumbnail()
        {
            var video = new YouTubeAPI.Video()
            {
                VideoPath = paths[cb_videos.SelectedIndex],
                Title = tb_title.Text,
                Description = tb_description.Text,

                Tags = tb_tags.Text.Split(','),

                Category = (Categories)(cb_categories.SelectedItem),
                Privacy = (Privacies)(cb_privacies.SelectedItem)
            };
            var thumbnail = new YouTubeAPI.Thumbnail()
            {
                ThumbnailPath = tb_image_path.Text,
            };
            var jsonSecret = @"YouTubeAPI\secret.json";
            var AppName = "YouTube Video Uploader";
            var channelID = tb_channellink.Text.Substring(tb_channellink.Text.LastIndexOf("/channel/") + 9);
            var result = await Credentials.FromSecret(jsonSecret, AppName, channelID).Authorize().Upload(video).Upload(thumbnail);
            if (result.Status == Statuses.Failed)
            {
                MessageBox.Show(result.Error);
            }
        }
        private async void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            if (cb_videos.SelectedItem != null && !String.IsNullOrEmpty(tb_title.Text) &&
                 !String.IsNullOrEmpty(tb_channellink.Text) && cb_categories.SelectedItem != null && cb_privacies.SelectedItem != null
                 && !String.IsNullOrEmpty(tb_description.Text) && !String.IsNullOrEmpty(tb_tags.Text))
            {
                if (tb_channellink.Text.StartsWith("https://www.youtube.com/channel/")||
                    tb_channellink.Text.StartsWith("www.youtube.com/channel/") ||
                      tb_channellink.Text.StartsWith("youtube.com/channel/"))
                {
                    if (ch_nothumbnail.IsChecked == true)
                    {
                        UploadWithoutThumbnail();
                    }

                    if (ch_nothumbnail.IsChecked == false)
                    {
                        UploadWithThumbnail();
                    }
                }
            }
        }
    }   
}

