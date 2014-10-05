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

namespace ChinjufuClock
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer_;
        String soundPath_;

        public MainWindow()
        {
            InitializeComponent();

            ResetCharactor();
            
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
                    ResetCharactor();
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
                    ResetCharactor();
                    PlayVoice(fileName);
                    return;
                }
            }
        }

        /// <summary>指定のファイルのボイスを鳴らす</summary>
        /// <param name="fileName"></param>
        private void PlayVoice(String fileName)
        {
            MediaPlayer player = new MediaPlayer();
            player.Volume = 0.3;
            player.Open(new Uri(soundPath_ + fileName, UriKind.Absolute));
            player.Play();
            //player.Close();
        }

        private void ResetCharactor()
        {
            var dirs = Directory.GetDirectories(@"F:\voice\kancolle\");
            Random rnd = new Random(Environment.TickCount);
            var trgDir = dirs[rnd.Next(0, dirs.Length)];
            soundPath_ = trgDir + @"\";
            txtCharactor.Text = System.IO.Path.GetFileName(trgDir);
        }
    }
}
