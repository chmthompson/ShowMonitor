using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ShowMonitor.Model
{
    class ShowAdapter
    {
        private string connection = String.Format("Server={0};Database=tvshow;Uid={1};Pwd={2};", ShowMonitor.Properties.Settings.Default.Host, ShowMonitor.Properties.Settings.Default.Username, ShowMonitor.Properties.Settings.Default.Password);
        private MySqlConnection _mysqlCon = new MySqlConnection();


        public ShowAdapter()
        {
            try
            {
                _mysqlCon.ConnectionString = connection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adds a TVShow Object to the database
        /// </summary>
        /// <param name="show">TVShow show</param>
        /// <returns></returns>
        public int Add(Show show)
        {
            MySqlCommand com = new MySqlCommand();
            com.Connection = _mysqlCon;
            com.CommandText = "INSERT INTO tvshow.Subscribed_Shows (Show_Name,Season,Episode,Quality,LastUpdated) values(@Show_Name, @Season, @Episode,@Quality,@LastUpdated)";
            com.Parameters.Add("@Show_Name", MySqlDbType.VarChar).Value = show.ShowName;
            com.Parameters.Add("@Season", MySqlDbType.Int16).Value = show.Season;
            com.Parameters.Add("@Episode", MySqlDbType.Int16).Value = show.Episode;
            com.Parameters.Add("@Quality", MySqlDbType.VarChar).Value = show.Quality ?? "N/A";
            com.Parameters.Add("@LastUpdated", MySqlDbType.DateTime).Value = DateTime.Now;
          //  if (this.ShowExists(show.ShowName)) {
            //    return -1;
            //} else {
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return Convert.ToInt16(com.LastInsertedId);
            //}
        }

        public bool Update(Show show)
        {
            MySqlCommand com = new MySqlCommand();
            com.Connection = _mysqlCon;
            com.CommandText = "update tvshow.Subscribed_Show set Episode=@Episode,Season=@Season,LastUpdated=@LastUpdated where Show_Name=@Show_Name";
            com.Parameters.Add("@Show_Name", MySqlDbType.VarChar).Value = show.ShowName;
            com.Parameters.Add("@Season", MySqlDbType.Int16).Value = show.Season;
            com.Parameters.Add("@Episode", MySqlDbType.Int16).Value = show.Episode;
            com.Parameters.Add("@LastUpdated", MySqlDbType.DateTime).Value = DateTime.Now;
            com.Connection.Open();
            com.ExecuteNonQuery();
            com.Connection.Close();
            return true;
        }

        public bool Delete(string showName)
        {
            MySqlCommand com = new MySqlCommand();
            com.Connection = _mysqlCon;
            com.CommandText = "delete from tvshow.Subscribed_Shows where Show_Name = @ShowName";
            com.Parameters.Add("@ShowName", MySqlDbType.VarChar).Value = showName;

            com.Connection.Open();
            com.ExecuteNonQuery();
            com.Connection.Close();
            return true;
        }
        public ObservableCollection<Show> GetSubscribedShows()
        {
            MySqlCommand com = new MySqlCommand("Select Show_Name, Season, Episode, Quality, LastUpdated from tvshow.Subscribed_Shows",_mysqlCon);
            ObservableCollection<Show> showResults = new ObservableCollection<Show>();

            try
            {
                com.Connection.Open();
                MySqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    showResults.Add(new Show()
                    {
                        ShowName = reader.GetString(0),
                        Season = reader.GetInt16(1),
                        Episode = reader.GetInt16(2),
                        Quality = reader.GetString(3),
                        LastUpdated = reader.GetDateTime(4)
                    });
                }
                reader.Close();
                com.Connection.Close();
                return showResults;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns true if show name exists in database
        /// </summary>
        /// <param name="showName">Show Name</param>
        /// <returns></returns>
        public bool ShowExists(string showName)
        {
            MySqlCommand com = new MySqlCommand();
            bool exists;
            com.Connection = _mysqlCon;
            com.CommandText = "Select ID from tvshow.Subscribed_Shows where Show_Name = @showName";
            com.Parameters.Add("@showName", MySqlDbType.VarChar).Value = showName;

            com.Connection.Open();
            exists = (bool)com.ExecuteScalar();
            com.Connection.Close();
            
            return (bool)com.ExecuteScalar() ;
        }
    }
}
