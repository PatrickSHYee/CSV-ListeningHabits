using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_ListeningHabits
{
    class Program
    {
        // Global List
        public static List<Play> musicDataList = new List<Play>();
        static void Main(string[] args)
        {
            // initalize dataset into list
            InitList();
            MostPopularArtistByYear("2014");
            // keep console open
            Console.ReadLine();
        }
        /// <summary>
        /// A function to initalize the List from the csv file
        /// needed for testing
        /// </summary>
        public static void InitList()
        {
            // load data
            using (StreamReader reader = new StreamReader("scrobbledata.csv"))
            {
                // Get and don't use the first line
                string firstline = reader.ReadLine();
                // Loop through the rest of the lines
                while (!reader.EndOfStream)
                {
                    musicDataList.Add(new Play(reader.ReadLine()));
                }
            }
        }
        /// <summary>
        /// A function that will return the total ammount of plays in the dataset
        /// </summary>
        /// <returns>total number of plays</returns>
        public static int TotalPlays()
        {
            return musicDataList.Count;
        }
        /// <summary>
        /// A function that returns the number of plays ever by an artist
        /// </summary>
        /// <param name="artistName">artist name</param>
        /// <returns>total number of plays</returns>
        public static int TotalPlaysByArtistName(string artistName)
        {
            // this one is how I code it
            //return musicDataList.Where(x => x.Artist.ToLower().Contains(artistName.ToLower())).ToList().Count;
            // another way discuss in class
            return musicDataList.Count(x => x.Artist.ToLower() == artistName.ToLower());
        }
        /// <summary>
        /// A function that returns the number of plays by a specific artist in a specific year
        /// </summary>(
        /// <param name="artistName">artist name</param>
        /// <param name="year">one year</param>
        /// <returns>total plays in year</returns
        public static int TotalPlaysByArtistNameInYear(string artistName, string year)
        {
            // SELECT artirstName from musicDataList
            // List<Play> theArtist = musicDataList.Where(x => x.Artist.ToLower().Contains(artistName.ToLower())).ToList().Where(y => y.Time.Year.ToString().Contains(year)).ToList();
            // THEN
            // SELECT year from artistName
            //return musicDataList.Where(x => x.Artist == artistName).ToList().Where(y => y.Time.Year.ToString()== year).ToList().Count;  // this is my code.
            return musicDataList.Count(x=>x.Artist.ToLower() == artistName.ToLower() && x.Time.Year.ToString() == year);// using .Count
        }
        /// <summary>
        /// A function that returns the number of unique artists in the entire dataset
        /// </summary>
        /// <returns>number of unique artists</returns>
        public static int CountUniqueArtists()
        {
            return musicDataList.Select(x => x.Artist).ToList().Distinct().ToList().Count;
        }
        /// <summary>
        /// A function that returns the number of unique artists in a given year
        /// </summary>
        /// <param name="year">year to check</param>
        /// <returns>unique artists in year</returns>
        public static int CountUniqueArtists(string year)
        {
            return musicDataList.Where(x=>x.Time.Year.ToString().Contains(year)).ToList().Select(y=>y.Artist).ToList().Distinct().ToList().Count;
        }
        /// <summary>
        /// A function that returns a List of unique strings which contains
        /// the Title of each track by a specific artists
        /// </summary>
        /// <param name="artistName">artist</param>
        /// <returns>list of song titles</returns>
        public static List<string> TrackListByArtist(string artistName)
        {
            return musicDataList.Where(art=>art.Artist == artistName).ToList().Select(track=>track.Title).ToList().Distinct().ToList();
        }
        /// <summary>
        /// A function that returns the first time an artist was ever played
        /// </summary>
        /// <param name="artistName">artist name</param>
        /// <returns>DateTime of first play</returns>
        public static DateTime FirstPlayByArtist(string artistName)
        {
            
            return musicDataList.Where(x => x.Artist.ToLower() == artistName).ToList().OrderBy(x=>x.Time).First().Time;
        }
        /// <summary>
        ///                     ***BONUS***
        /// A function that will determine the most played artist in a specified year. GroupBy lambda expression
        /// </summary>
        /// <param name="year">year to check</param>
        /// <returns>most popular artist in year</returns>
        public static string MostPopularArtistByYear(string year)
        {
            return musicDataList.Where(x => x.Time.Year.ToString() == year).GroupBy(y => y.Artist).OrderByDescending(z=>z.Count()).First().Key;
        }
    }

    public class Play
    {
        // Properties
        public DateTime Time { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public Play(string lineInput)
        {
            // Split using the tab character due to the tab delimited data format
            string[] playData = lineInput.Split('\t');
            
            // Get the time in milliseconds and convert to C# DateTime
            DateTime posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            this.Time = posixTime.AddMilliseconds(long.Parse(playData[0]));

            // need to populate the rest of the properties
            this.Artist = playData[1];  // put the artist name in artist variable
            this.Title = playData[2];
            this.Album = playData[3];
        }
    }
}
