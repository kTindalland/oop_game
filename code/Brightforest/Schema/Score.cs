using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightforest.Schema
{
    public struct Score
    {
        // Individual player info for leaderboard
        public string Name { get; set; }
        public long PlayerScore { get; set; }
    }
}
