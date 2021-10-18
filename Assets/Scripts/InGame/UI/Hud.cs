using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pacmania.GameManagement;
using Pacmania.Utilities.UI;

namespace Pacmania.InGame.UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject playerReadyText = default;
        [SerializeField] private GameObject bonusTargetText = default;
        [SerializeField] private GameObject levelCompleteText = default;
        [SerializeField] private GameObject scoreText = default;
        [SerializeField] private GameObject hiScoreText = default;
        [SerializeField] private GameObject gameOverText = default;
        [SerializeField] private GameObject seeYouText = default;
        [SerializeField] private GameObject backPanel = default;
        [SerializeField] private GameObject extendedText = default;
        [SerializeField] private GameObject courageBonusText = default;

        private ShowLivesLeft showLivesLeft;
        private ShowHudFruit showHudFruit;
        private const float secondsShowExtendedText = 1.0f;

        private void Awake()
        {
            showLivesLeft = FindObjectOfType<ShowLivesLeft>();
            showHudFruit = FindObjectOfType<ShowHudFruit>();       
        }

        private void Start()
        {
            CheckHiScore();
        }

        private void FixedUpdate()
        {
            SetScore(Game.Instance.CurrentSession.Score);
            CheckHiScore();
        }

        private void CheckHiScore()
        {
            Game game = Game.Instance;
            int hiScore = game.HighScoreTable.GetHighestScore();
            int currentScore = game.CurrentSession.Score;
            if (currentScore > hiScore)
            {
                hiScore = currentScore;
            }

            SetHiScore(hiScore);
        }

        public void SetPlayerReadyTextVisibility(bool value)
        {
            EnableText(playerReadyText, value);
        }

        public void SetBonusTargetTextVisibility(bool value)
        {
            EnableText(bonusTargetText, value);
        }

        public void SetLevelCompleteTextVisibility(bool value)
        {
            EnableText(levelCompleteText, value);
        }

        public void SetGameOverTextVisibility(bool value)
        {
            EnableText(gameOverText, value);
        }

        public void SetSeeYouTextVisibility(bool value)
        {
            EnableText(seeYouText, value);
        }
        public void SetCourageBonusVisibility(bool value)
        {
            EnableText(courageBonusText, value);  
        }

        public void SetScore(int value)
        {
            SetText(scoreText, value.ToString());
        }

        public void SetHiScore(int value)
        {
            SetText(hiScoreText, value.ToString());
        }

        public void SetBonusTargetText(string value)
        {
            bonusTargetText.GetComponent<Text>().text = value;
        }   

        private void EnableText(GameObject obj, bool value)
        {
            Text[] texts = obj.GetComponentsInChildren<Text>();

            foreach (var text in texts)
            {
                text.enabled = value;
            }
        }

        private void SetText(GameObject obj, string value)
        {
            Text[] texts = obj.GetComponentsInChildren<Text>();

            foreach (var text in texts)
            {
                text.text = value;
            }
        }

        public void RedrawLivesLeft()
        {
            showLivesLeft.ReDraw();
        }

        public void RedrawFruit()
        {
            showHudFruit.ReDraw();
        }
        public void StartScreenFadeout()
        {
            backPanel.GetComponent<FaderImageIn>().StartFade();
        }

        public void ShowExtendedForPeriod()
        {
            IEnumerator routine = ShowTextForPeriodCoroutine(extendedText, secondsShowExtendedText);
            StartCoroutine(routine);
        }

        private IEnumerator ShowTextForPeriodCoroutine(GameObject text, float period)
        {
            EnableText(text, true); 
            yield return new WaitForSeconds(period);
            EnableText(text, false);
        }
    }
}
