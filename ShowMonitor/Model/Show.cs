#region Using
using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Net;
#endregion

namespace ShowMonitor.Model
{
    public class Show
    {

        #region Public Properties
        public string Quality { get; set; }
        public string Title { get; set; }
        public string Link { get; set;  }
        public string ShowName { get; set; }
        public string EpisodeDate { get; set; }
        public int Season { get; set; }
        public int Episode { get; set; }
        public DateTime LastUpdated { get; set; }
        #endregion

        #region Constructors & Destructors
        public Show()
        {
        }

        // Constructor that parses the TV show information and sets the show properties
        public Show(SyndicationItem show)
        {
            string[] parseShowInfo = show.Summary.Text.Split(';');
            ShowName = parseShowInfo[0].Remove(0, "Show Name: ".Length);
            Link = show.Links.ElementAt(0).Uri.ToString();
            EpisodeDate = show.PublishDate.Month.ToString() + "/" + show.PublishDate.Day.ToString() + "/" + show.PublishDate.Year.ToString();
            // Match the quality of the show
            {
                if (show.Title.Text.Contains("HDTV")) { 
                    Quality = "HDTV";
                } else if (show.Title.Text.Contains("PDTV")) {
                    Quality = "PDTV";
                }

            }

            // Only grab the season if there is one in the show summary
            if (parseShowInfo[2].Contains("Season:"))
            {
                Season = Convert.ToInt32(parseShowInfo[2].Remove(0, " Season: ".Length));
                // if there is a season, there is a episode number
                Episode = Convert.ToInt32(parseShowInfo[3].Remove(0, " Episode: ".Length));
                // same with episode title
                Title = parseShowInfo[1].Remove(0, " Episode Title: ".Length);
            }
        }
        #endregion

        #region Public Methods
        public bool Download()
        {
            //bool IsDownloaded = false;
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(this.Link), String.Format("{0}\\{1}.torrent",ShowMonitor.Properties.Settings.Default.TorrentFolder,this.ShowName.ToString().Replace(" ","_")));
            return true;
        }

        public override string ToString()
        {
            string showName = ShowName;
            string season, episode;
            // format the season and episode numbers with a prefaced '0' if single digit
            if ((Season != 0) && (Episode != 0))
            {

                season = (Season < 10) ? ("0" + Season) : Season.ToString();
                episode = (Episode < 10) ? ("0" + Episode) : Episode.ToString();
                showName += " [S" + season + "E" + episode + "]";
            }
            else
            {
                showName += " [" + EpisodeDate + "]";
            }
            return showName;
        }
        #endregion
    }
}
