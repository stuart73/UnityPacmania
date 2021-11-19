using System;

namespace Pacmania.GameManagement
{
    [System.Serializable]
    public class HighScoreTableEntry
    {
        public int Score;
        public int Round;
        public string Initials;

        public HighScoreTableEntry(int score, int round, string initials)
        {
            Score = score;
            Round = round;
            Initials = initials;
        }
        
        public string RoundString
        {
            get
            {
                if (Round == 1000)
                {
                    return "ALL";
                }
                return Round.ToString();
            }
        }
    }
}
