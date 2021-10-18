using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Pacmania.GameManagement;

namespace Pacmania.Menus
{
    public class HighScore : MonoBehaviour
    {
        [SerializeField] private Text[] initialsText = default;
        [SerializeField] private Text[] scoresText = default;
        [SerializeField] private Text[] roundText = default;

        private void Start()
        {
            HighScoreTable table = Game.Instance.HighScoreTable;

            if (initialsText.Length != table.Entries.Count ||
                scoresText.Length != table.Entries.Count ||
                roundText.Length != table.Entries.Count)
            {
                Debug.LogError("High score table entries length does not match scorboard screen entries length", this);
            }

            for (int i=0; i< table.Entries.Count; i++)
            {
                HighScoreTableEntry entry = table.Entries[i];
                initialsText[i].text = entry.Initials;
                scoresText[i].text = entry.Score.ToString();
                roundText[i].text = entry.RoundString; 
            }

            StartCoroutine(WaitCoroutine());

        }
        private void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                (Game.Instance.CurrentSession as DemoGameSession)?.StartPlayerGame();
            }
        }
        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(4);
            Game.Instance.CurrentSession.StartNextScene();
        }
    }
}
