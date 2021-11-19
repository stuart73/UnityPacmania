
namespace Pacmania.GameManagement
{
    public class Game
    {
        private static Game instance;
        public const int FramesPerSecond = 60; 

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        private Game()
        {
            // By default we create a player game session. This allows us to play/test individual levels from the unity editor
            // as if we were in game and not in attact/demo mode.
            CreateNewPlayerGameSession();

            HighScoreTable.Load();
        }

        public GameSession Player1Session { get; private set; }

        public GameSession CurrentSession { get; private set; }

        public HighScoreTable HighScoreTable { get; private set; } = new HighScoreTable();

        public void CreateNewPlayerGameSession()
        {
            Player1Session = new PlayerGameSession();  
            CurrentSession = Player1Session;
        }

        public void CreateNewDemoGameSession(int startSequenceIndex)
        {
            Player1Session = new DemoGameSession(startSequenceIndex);

            // Copy the score from the current session.  I.e. in a demo game the score shown at 
            // the top was the last played game's score.
            if (CurrentSession != null)
            {
                Player1Session.Score = CurrentSession.Score;
            }

            CurrentSession = Player1Session;
        }
    }
}
