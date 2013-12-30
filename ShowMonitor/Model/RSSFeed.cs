#region Using
using System;
using System.Xml;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Collections.ObjectModel;
#endregion

// This class could be combined into just a viewmodel
namespace ShowMonitor.Model
{
    delegate ObservableCollection<Show> getRssFeedAsync(string url);

    class RSSFeed
    {
        #region Constructors / Destructors
        public RSSFeed()
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Grabs the RSS feed from the url
        /// </summary>
        /// <param name="url">URL of TV RSS Site</param>
        /// <returns>ObservableCollection<Show></returns>
        public ObservableCollection<Show> getRSSFeed(string url)
        {
            try
            {
                ObservableCollection<Show> tvList = new ObservableCollection<Show>();
                using (XmlReader reader = XmlReader.Create(url, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
                {
                    foreach (SyndicationItem item in SyndicationFeed.Load(reader).Items)
                    {
                        tvList.Add(new Show(item));
                    }
                }
                return tvList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}