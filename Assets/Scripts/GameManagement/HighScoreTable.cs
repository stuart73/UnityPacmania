using System.Collections.Generic;

namespace Pacmania.GameManagement
{
    public class HighScoreTable
    {
        public int NewEntryIndex { get; private set; } = 0;

        public List<HighScoreTableEntry> Entries = new List<HighScoreTableEntry> {   new HighScoreTableEntry(100000,5, "TAR" ),
                                                                                new HighScoreTableEntry(90000,5, "DAI" ),
                                                                                new HighScoreTableEntry(80000,4, "FFP" ),
                                                                                new HighScoreTableEntry(70000,4, "PAC" ),
                                                                                new HighScoreTableEntry(60000,3, "BOO" ),
                                                                                new HighScoreTableEntry(50000,3, "WAN" ),
                                                                                new HighScoreTableEntry(40000,2, "KAZ" ),
                                                                                new HighScoreTableEntry(30000,2, "ZUN" ),
                                                                                new HighScoreTableEntry(20000,1, "TOM" ),
                                                                                new HighScoreTableEntry(10000,1, "TAM" ) };

        public int GetHighestScore()
        {
            return Entries[0].Score;
        }

        public bool DoesGameSessionMakeTable(GameSession session)
        {
            Game game = Game.Instance;
            for (int i = 0; i < Entries.Count; i++)
            {
                int entryScore = Entries[i].Score;
                int entryRound = Entries[i].Round;
                int playerScore = session.Score;
                int playerRound = session.CurrentLevel;

                if (entryScore < playerScore || (entryScore == playerScore && entryRound < playerRound))
                {
                    var entry = new HighScoreTableEntry(playerScore, playerRound, "...");
                    Entries.Insert(i, entry);
                    Entries.RemoveAt(Entries.Count - 1);
                    NewEntryIndex = i;
                    return true;
                }
            }

            return false;
        }
    }
}
