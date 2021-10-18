using UnityEngine;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene("Logo");
            }
            else if (currentSequenceIndex == 2)
            {
                SceneManager.LoadScene("Level2");
            }
            else if (currentSequenceIndex == 3 || currentSequenceIndex == 7)
            {
                SceneManager.LoadScene("HighScore");
            }
            else if (currentSequenceIndex == 4)
            {
                SceneManager.LoadScene("Level4");
            }  
            else if (currentSequenceIndex == 6)
            {
                SceneManager.LoadScene("Level6");
            }
            else if (currentSequenceIndex == 8)
            {
                SceneManager.LoadScene("Level1");
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

        public void StartPlayerGame()
        {
            Game.Instance.CreateNewPlayerGameSession();
            SceneManager.LoadScene("WorldSelect");
        }
    }
}
