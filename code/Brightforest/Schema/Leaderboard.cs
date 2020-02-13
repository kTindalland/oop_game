using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Brightforest.Schema
{
    public class Leaderboard
    {
        public Leaderboard()
        {
            Scores = new List<Score>();
        }

        public List<Score> Scores { get; set; }
    }
}
