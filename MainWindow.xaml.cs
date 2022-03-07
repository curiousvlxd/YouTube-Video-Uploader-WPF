using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YouTube_Video_Uploader.YouTubeAPI;
using Application = System.Windows.Application;
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;


namespace YouTube_Video_Uploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog;
        private FolderBrowserDialog folderBrowserDialog;
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.InitialDirectory = "c:\\";
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "mp4 files *.mp4|*.mp4";
            openFileDialog.RestoreDirectory = true;
        }

        #region TitleBarEvents
        private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            this.Close();
        }



        #endregion

        #region WindowEvents

        private void DropFiles(object sender, DragEventArgs e)
        {
            BorderError.Opacity = 0;
            TextBlockError.Opacity = 0;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                List<string> files = new List<string>();
                List<string> paths = new List<string>();
                string[] arr_files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filename in arr_files)
                {
                    if (System.IO.Path.GetExtension(filename) == ".mp4")
                    {
                        files.Add(System.IO.Path.GetFileName(filename));
                        paths.Add(filename);
                    }
                }
                if (files.Count > 0 && paths.Count > 0)
                {
                    Main.Content = new Uploader(files, paths);
                }
                else
                {
                    BorderError.Opacity = 1;
                    TextBlockError.Opacity = 1;
                }
            }
            
        }

        private void ButtonChooseFiles_Click(object sender, RoutedEventArgs e)
        {
            BorderError.Opacity = 0;
            TextBlockError.Opacity = 0;
            List<string> files = new List<string>();
            List<string> paths = new List<string>();
            bool? dialogResult = openFileDialog.ShowDialog();
            switch (dialogResult)
            {
                case true:
                    foreach (var file in openFileDialog.FileNames)
                    {   
                        paths.Add(file);
                        files.Add(System.IO.Path.GetFileName(file));
                    }
                    this.Main.Content = new Uploader(files, paths);
                    break;
                case false:
                   
                    break;
                default:

                    break;
            }
        }

        private void ButtonChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            BorderError.Opacity = 0;
            TextBlockError.Opacity = 0;
            List<string> files = new List<string>();
            List<string> paths = new List<string>();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {   
                string[] arr_files = System.IO.Directory.GetFiles(folderBrowserDialog.SelectedPath);
                foreach (string filename in arr_files)
                {
                    if (System.IO.Path.GetExtension(filename) == ".mp4")
                    {   
                        files.Add(System.IO.Path.GetFileName(filename));
                        paths.Add(filename);
                    }
                }

                if (files.Count > 0 && paths.Count > 0)
                {
                    Main.Content = new Uploader(files, paths);
                }
                else
                {
                    BorderError.Opacity = 1;
                    TextBlockError.Opacity = 1;
                }
                
            }
            //Main.Content = new Uploader();
        }
        private void TitleNavigated(object sender, NavigationEventArgs e)
        {

        }
        private void CommandBinding_Executed_BrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            Main.Content = null;
        }


        #endregion


    }
}
