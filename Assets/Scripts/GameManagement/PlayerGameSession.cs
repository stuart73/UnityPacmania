using UnityEngine;
using UnityEngine.SceneManagement;
using Pacmania.Audio;
using Pacmania.InGame.UI;
using Pacmania.InGame;

namespace Pacmania.GameManagement
{
    public class PlayerGameSession : GameSession
    {
        public int CourageBonus { get; set; } = 0;
        public int HighScoreRank { get; } = 9;

        // Represents if we've seen all 4 world intros
        private bool[] shownIntroForWorlds = new bool[4] { false, false, false, false };
        private bool shownCompleteAllWorldsScene = false;
    
        private const int extraLivePoints = 100000;

        public override void StartNextScene()
        {
            // Do we need to show a cutscene before proceding to the next level?
            if (shownCompleteAllWorldsScene == false && (CurrentLevel == 8 || CurrentLevel == 20))
            {
                shownCompleteAllWorldsScene = true;
                SceneManager.LoadScene("CompleteScreen");
            }
            else if (shownIntroForWorlds[0] == false && (CurrentLevel == 1 || CurrentLevel == 8))
            {
                shownIntroForWorlds[0] = true;
                SceneManager.LoadScene("BlocktownIntro");
            }
            else if (shownIntroForWorlds[1] == false && (CurrentLevel == 2 || CurrentLevel == 11))
            {
                shownIntroForWorlds[1] = true;
                SceneManager.LoadScene("PacmansParkIntro");
            }
            else if (shownIntroForWorlds[2] == false && (CurrentLevel == 4 || CurrentLevel == 14))
            {
                shownIntroForWorlds[2] = true;
                SceneManager.LoadScene("SandboxIntro");
            }
            else if (shownIntroForWorlds[3] == false && (CurrentLevel == 6 || CurrentLevel == 17))
            {
                shownIntroForWorlds[3] = true;
                SceneManager.LoadScene("JungleStepsIntro");
            }
            else if (CurrentLevel == 20)
            {
                // Completed game, start credits screen scene.  This scene will eventually call our GameOver method.
                SceneManager.LoadScene("CreditsScreen");
            }
            else
            {
                // Start the next level.
                SceneManager.LoadScene("Level"+ CurrentLevel.ToString());
            }
        }

        public override void CompletedLevel()
        {
            CurrentLevel++;

            // If we have just finished the first level of revisting all the worlds, then reset the completed all words flag.
            if (CurrentLevel == 9)
            {
                shownCompleteAllWorldsScene = false;
            }
            StartNextScene();
        }

        public override void AddScore(Level level, int amount)
        {
            int oldScore = Score;
            Score += amount;

            if ((oldScore / extraLivePoints) != (Score / extraLivePoints))
            {
                Lives += 1;

                Hud hud = level.Hud;
                if (hud != null)
                {
                    hud.ShowExtendedForPeriod();
                    hud.RedrawLivesLeft();
                }

                level.AudioManager.Play(SoundType.ExtendedJingle);
            }
        } 

        public override void GameOver()
        {
            // Check if player's score was good enough for highscore.
            if ( Game.Instance.HighScoreTable.DoesGameSessionMakeTable(Game.Instance.CurrentSession) == true)
            {
                SceneManager.LoadScene("EnterInitials");
                return;
            }
  
            // Not good enough for highscore, then go straight back into demo sequence.
            Game.Instance.CreateNewDemoGameSession(0);
            SceneManager.LoadScene("HighScore");
        }
    }
}
