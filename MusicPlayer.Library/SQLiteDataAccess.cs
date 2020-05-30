using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace MusicPlayer.Library
{
    public class SQLiteDataAccess
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        
        /// <summary>
        /// loads last played music tracks
        /// </summary>
        /// <returns></returns>
        public static List<LastOpenedModel> LoadLastTrackList()
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                var trackList = connection.Query<LastOpenedModel>("SELECT * FROM LastOpened", new DynamicParameters());

                return trackList.ToList();
            }
        }


        /// <summary>
        /// saves actual tracklist
        /// </summary>
        /// <param name="lastOpeneds"></param>
        public static void SaveLastTrackList(List<LastOpenedModel> lastOpeneds)
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("DELETE FROM LastOpened");

                foreach (var item in lastOpeneds)
                {
                    connection.Execute("INSERT INTO LastOpened VALUES (@FilePath)", item);
                }

            }
        }

    }
}
