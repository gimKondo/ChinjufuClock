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

using System.IO;
using System.Windows.Threading;
using Microsoft.Win32;
using Forms = System.Windows.Forms;

namespace ChinjufuClock
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer_;
        String soundDirectory_;
        String charactorPath_;

        public MainWindow()
        {
            InitializeComponent();

            soundDirectory_ = Properties.Settings.Default.SoundDirectory;

            dispatcherTimer_ = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer_.Interval = new TimeSpan(0, 0, 1); // 1秒ごとにイベント発生
            dispatcherTimer_.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer_.Start();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss");
            txtTime.Text = now;
            for (int i = 0; i < 24; ++i )
            {
                String timeStr = i.ToString() + ":00:00";
                String fileName = (30 + i).ToString() + ".mp3";
                if (now == timeStr)
                {
                    if (!ResetCharactor())
                    {
                        MessageBox.Show("適切なパスが設定されていません");
                        return;
                    }
                    PlayVoice(fileName);
                    return;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nowHour = DateTime.Now.TimeOfDay.Hours;
            for (int i = 0; i < 24; ++i)
            {
                String timeStr = i.ToString() + ":00:00";
                String fileName = (30 + i).ToString() + ".mp3";
                if (nowHour == i)
                {
                    if (!ResetCharactor())
                    {
                        MessageBox.Show("適切なパスが設定されていません");
                        return;
                    }
                    PlayVoice(fileName);
                    return;
                }
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new Forms.FolderBrowserDialog())
            {
                if (soundDirectory_.Length != 0)
                {
                    dlg.SelectedPath = soundDirectory_;
                }
                if (Forms.DialogResult.OK == dlg.ShowDialog())
                {
                    soundDirectory_ = dlg.SelectedPath;
                    Properties.Settings.Default.SoundDirectory = soundDirectory_;
                    Properties.Settings.Default.Save();
                }
                
            }
            

        }

        /// <summary>指定のファイルのボイスを鳴らす</summary>
        /// <param name="fileName"></param>
        private void PlayVoice(String fileName)
        {
            MediaPlayer player = new MediaPlayer();
            player.Volume = 0.3;
            player.Open(new Uri(charactorPath_ + fileName, UriKind.Absolute));
            player.Play();
            //player.Close();
        }

        /// <summary>時報担当キャラクタの変更</summary>
        /// <returns></returns>
        private bool ResetCharactor()
        {
            try
            {
                var dirs = Directory.GetDirectories(soundDirectory_);
                Random rnd = new Random(Environment.TickCount);
                var trgDir = dirs[rnd.Next(0, dirs.Length)];
                charactorPath_ = trgDir + @"\";
                txtCharactor.Text = System.IO.Path.GetFileName(trgDir);
            }
            catch(Exception)
            {
                return false;
            }
            
            return true;
        }

    }
}
