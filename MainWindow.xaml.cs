using System.Collections.Generic;
using System.Net;
using System.Windows;

namespace tvcheck_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string[] URLs = new string[] { "http://fs.to/flist/i1JyOT1po21gODTFfZEmkdq?folder=870916&quality=hdrip" };

        private List<Series> GlobalSeries = null;
        public MainWindow()
        {
            InitializeComponent();
            RefreshAllFromServer();
            FillTheTree();
        }

        private void refreshMenu_Click(object sender, RoutedEventArgs e)
        {
            RefreshAllFromServer();
            FillTheTree();
        }

        private void RefreshAllFromServer()
        {
            List<Series> allResults = new List<Series>();
            
            foreach (string url in URLs)
            {
                Series currentSeries = new Series();
                string[] episodesList = new string[] { };
                using (WebClient client = new WebClient())
                {
                    episodesList = client.DownloadString(url).Split();
                }
                currentSeries.Name = episodesList[0].Substring(50);
                currentSeries.SetUrl(url);
                currentSeries.SetEpisodes(episodesList);

                allResults.Add(currentSeries);
            }
            GlobalSeries = allResults;
        }

        private void FillTheTree()
        {
            foreach (Series currentSeries in GlobalSeries)
            {
                generalTree.ItemsSource = currentSeries.GetEpisodesNames();
            }
        }
    }

    public class Series
    {
        private string _name;

        private string _url;

        private string[] _episodes;

        public string Name {
            get {
                return _name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _name = value;
            }
        }
        public string URL {
            get
            {
                return _url;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _url = value;
            }
        }
        
        public string[] Episodes {
            get
            {
                return this._episodes;
            }
             set
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                    _episodes = value;
            }
        }
        public string GetUrl()
        {
            return this.URL;
        }
        public void SetUrl(string url)
        {
            this.URL = url;
        }
        public string[] GetEpisodesNames() //returns list of episodes in human readable format
        {
            List<string> names = new List<string> { };
            foreach (string name in this.Episodes)
            {
                int slashIndex = name.LastIndexOf("/");
                if (slashIndex > 0)
                    names.Add(name.Substring(slashIndex + 1).Replace("."," "));
            }
            return names.ToArray();
        }
    }
}
