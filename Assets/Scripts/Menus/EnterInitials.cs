using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using Pacmania.GameManagement;
using Pacmania.Audio;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.UI;


namespace Pacmania.Menus
{
    public class EnterInitials : MonoBehaviour
    {
        [SerializeField] private Text rankText = default;
        [SerializeField] private Text scoreText = default;
        [SerializeField] private Text roundText = default;
        [SerializeField] private Text[] initialsText = default;
        [SerializeField] private Color initialsColor = default;
        [SerializeField] private Color initialsFlashColor = default;

        private const int startCharIndex = 5;
        private const int secondsDoingEndFadeOut = 4;
        private const int numberOfFramesBetweenCharacterChange = 10;

        private static char[] chars = new char[] { ' ', ',', '!', '&', (char)39, '.', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                                             'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private int currentCharIndex = startCharIndex;
        private int currentInitialIndex = 0;
        private int flashCount = 0;
        private int allowInitialChangeCount = 0;

        private void Start()
        {
            FindObjectOfType<AudioManager>().Play(SoundType.EnterInitialsMusic);

            // Fill in details from the new entry in the high score table into this scene.
            int positionIndex = Game.Instance.HighScoreTable.NewEntryIndex;
            HighScoreTableEntry newEntry = Game.Instance.HighScoreTable.Entries[positionIndex];

            rankText.text = "NO." + (positionIndex + 1).ToString();
            scoreText.text = newEntry.Score.ToString();
            roundText.text = newEntry.RoundString;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (!value.started) return;
            Vector2 inputMovement = value.ReadValue<Vector2>();

            if (currentInitialIndex >=3)
            {
                return;
            }

            if (inputMovement.y < 0 && allowInitialChangeCount >= numberOfFramesBetweenCharacterChange)
            {
                MoveToPreviousCharacer();
            }
            else if (inputMovement.y > 0 && allowInitialChangeCount >= numberOfFramesBetweenCharacterChange)
            {
                MoveToNextCharacter();
            }
        }

        public void OnInputTrigger(InputAction.CallbackContext value)
        {
            if (!value.started) return;

            RememberInitialAndMoveToNextCharacter();
            if (currentInitialIndex >= 3)
            {
                CopyInitialsIntoHighScoreTable();
                EndScene();
            }
        }

        private void RememberInitialAndMoveToNextCharacter()
        {
            initialsText[currentInitialIndex].color = initialsColor;

            currentInitialIndex++;
            currentCharIndex = startCharIndex;
        }

        private void MoveToNextCharacter()
        {
            currentCharIndex++;
            if (currentCharIndex >= chars.Length)
            {
                currentCharIndex = 0;
            }
            initialsText[currentInitialIndex].text = chars[currentCharIndex].ToString();
            allowInitialChangeCount = 0;
        }

        private void MoveToPreviousCharacer()
        {
            currentCharIndex--;
            if (currentCharIndex < 0)
            {
                currentCharIndex = chars.Length - 1;
            }
            initialsText[currentInitialIndex].text = chars[currentCharIndex].ToString();
            allowInitialChangeCount = 0;
        }

        private void CopyInitialsIntoHighScoreTable()
        {
            Game game = Game.Instance;
            string initials = initialsText[0].text + initialsText[1].text + initialsText[2].text;
            game.HighScoreTable.Entries[Game.Instance.HighScoreTable.NewEntryIndex].Initials = initials;      
        }

        private void EndScene()
        {
            // Save out
            Game.Instance.HighScoreTable.Save();

            // Hide pacman
            FindObjectOfType<PacmanController>().gameObject.SetActive(false);

            // Fade out music and arena, then go back to highscore in demo mode
            FindObjectOfType<AudioManager>().FadeOut(SoundType.EnterInitialsMusic);
            FindObjectOfType<Hud>().StartScreenFadeout();

            IEnumerator waitCoroutine = WaitThenStartDemoGameCoroutine();
            StartCoroutine(waitCoroutine);
        }

        private void FixedUpdate()
        {
            // Have we finished entering initials?
            if (currentInitialIndex >= 3)
            {
                return;
            }

            // Make current initial character flash  (changes every 4 frames).
            flashCount++;
            if (flashCount % 8 < 4)
            {
                initialsText[currentInitialIndex].color = initialsFlashColor;
            }
            else
            {
                initialsText[currentInitialIndex].color = initialsColor;
            }

            allowInitialChangeCount++;
        }

        private IEnumerator WaitThenStartDemoGameCoroutine()
        {
            yield return new WaitForSeconds(secondsDoingEndFadeOut);

            Game.Instance.CreateNewDemoGameSession(0);
            SceneManager.LoadScene(SceneNames.HighScore);
        }
    }
}
