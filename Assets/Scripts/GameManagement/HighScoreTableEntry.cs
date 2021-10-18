
namespace Pacmania.GameManagement
{
    public class HighScoreTableEntry
    {
        public HighScoreTableEntry(int score, int round, string initials)
        {
            Score = score;
            Round = round;
            Initials = initials;
        }
        public int Score { get; set; }

        public int Round { get; set; }

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
    
        public string Initials { get; set; }
    }
}
