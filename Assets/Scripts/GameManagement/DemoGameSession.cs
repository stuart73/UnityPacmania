using UnityEngine;
using UnityEngine.SceneManagement;
using Pacmania.InGame;

namespace Pacmania.GameManagement
{
    public class DemoGameSession : GameSession
    {
        private int currentSequenceIndex = 0;

        public DemoGameSession(int startSequenceIndex)
        {
            currentSequenceIndex = startSequenceIndex;
        }

        public override void StartNextScene()
        {
            // All demo levels have 1 life
            Lives = 1;

            currentSequenceIndex++;
            if (currentSequenceIndex >= 9) currentSequenceIndex = 1;

            if (currentSequenceIndex == 1 || currentSequenceIndex == 5)
            {
                SceneManager.LoadScene(SceneNames.Logo);
            }
            else if (currentSequenceIndex == 2)
            {
                SceneManager.LoadScene(SceneNames.LevelPrefix + "2");
            }
            else if (currentSequenceIndex == 3 || currentSequenceIndex == 7)
            {
                SceneManager.LoadScene(SceneNames.HighScore);
            }
            else if (currentSequenceIndex == 4)
            {
                SceneManager.LoadScene(SceneNames.LevelPrefix + "4");
            }
            else if (currentSequenceIndex == 6)
            {
                SceneManager.LoadScene(SceneNames.LevelPrefix + "6");
            }
            else if (currentSequenceIndex == 8)
            {
                SceneManager.LoadScene(SceneNames.LevelPrefix + "1");
            }
        }

        public override void GameOver()
        {
            StartNextScene();
        }

        public override void CompletedLevel()
        {
            Debug.LogError("Completed a demo level which should not occur!");
        }

        public override void AddScore(Level level, int amount)
        { 
            // In demo session adding the score does nothing.
        }

        public void StartPlayerGame()
        {
            Game.Instance.CreateNewPlayerGameSession();
            SceneManager.LoadScene(SceneNames.WorldSelect);
        }
    }
}
