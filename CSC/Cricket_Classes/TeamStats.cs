using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cricket
{
    public class TeamStats
    {
        public TeamStats()
        { }

        private string fBestScore;
        public string FBestScore
        {
            get { return fBestScore;
            }
            set { fBestScore = value; }
        }
    }
}
