using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Schema;
using System.IO;
using XML_Handling;

namespace Brightforest.Managers
{
    public class LeaderboardManager
    {
        private Leaderboard _leaderboard;
        private XML_Handler<Leaderboard> _xmlHandler;

        public LeaderboardManager()
        {
            var filepath = @"C:\Users\kaiti\source\repos\oop_game\data\leaderboard.xml"; // TODO: MAKE FILEPATH RELATIVE

            // Handle the xml stuff pls ty
            _xmlHandler = new XML_Handler<Leaderboard>(filepath);

            // Check if the file exists or not
            if (!File.Exists(filepath))
            {
                // If not, create one and put the leaderboard in it
                _leaderboard = new Leaderboard();
                _xmlHandler.Serialise(_leaderboard);
            }
            else
            {
                // if there is, get the leaderboard
                _leaderboard = _xmlHandler.Deserialise();
            }
        }

        // Add a new score to the leaderboard
        public void AddScore(string name, long score)
        {
            _leaderboard.Scores.Add(new Score()
            {
                Name = name,
                PlayerScore = score
            });

            _xmlHandler.Serialise(_leaderboard);
        }

        // Self explanatory
        public List<Score> GetAllScores()
        {
            return _leaderboard.Scores;
        }

        // get and sort the top ten scores.
        public List<Score> GetTopTenScores()
        {
            var allScores = _leaderboard.Scores;

            var sortedScores = allScores.OrderByDescending(r => r.PlayerScore).ToList();

            return sortedScores.Take(10).ToList();
        }
    }
}
