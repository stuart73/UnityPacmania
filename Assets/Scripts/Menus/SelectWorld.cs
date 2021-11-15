using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pacmania.GameManagement;
using Pacmania.Audio;

namespace Pacmania.Menus
{
    public class SelectWorld : MonoBehaviour
    {
        [SerializeField] private Text blockTownText = default;
        [SerializeField] private SpriteRenderer blockTownHighlightImage = default;

        [SerializeField] private Text pacmansParkText = default;
        [SerializeField] private SpriteRenderer pacmansParkHighlightImage = default;

        [SerializeField] private Text sandboxText = default;
        [SerializeField] private SpriteRenderer sandboxHighlightImage = default;

        [SerializeField] private Text jungleStepsText = default;
        [SerializeField] private SpriteRenderer jungleStepsHighlightImage = default;

        [SerializeField] private Color textSelectColor = default;
        [SerializeField] private Color textNonSelectColor = default;

        private int currentSelectIndex = 0;
        private static readonly int[] selectIndexToStartWorld = { 1, 2, 4, 6 };
        private static readonly int[] courageBonus = { 0, 70000, 150000, 300000 };
        private int flickerCount = 0;
        private bool worldSelected = false;

        private void Update()
        {
            PlayerGameSession gameSession = Game.Instance.CurrentSession as PlayerGameSession;

            if (gameSession == null)
            {
                Debug.LogError("Game session not a PlayerGameSession in select world screen", this);
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && worldSelected == false )
            {
                currentSelectIndex++;
                if (currentSelectIndex > 3) currentSelectIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && worldSelected == false)
            {
                currentSelectIndex--;
                if (currentSelectIndex < 0) currentSelectIndex = 3;         
            }
            else if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
            {
                gameSession.CurrentLevel = selectIndexToStartWorld[currentSelectIndex];
                gameSession.StartedGameOnLevel = gameSession.CurrentLevel;
                gameSession.CourageBonus = courageBonus[currentSelectIndex];
                worldSelected = true;
                StartCoroutine(Startlevel());
            }
        }
        private IEnumerator Startlevel()
        {
            yield return new WaitForSeconds(1.0f);
            Game.Instance.CurrentSession.StartNextScene();
        }
        private void FixedUpdate()
        {
            flickerCount++;
            ClearSelected();

            if (worldSelected == false || (flickerCount % 2) == 0)
            {
                HighlightSelected();
            }
        }
        private void ClearSelected()
        {
            blockTownText.color = textNonSelectColor;
            blockTownHighlightImage.enabled = false;

            pacmansParkText.color = textNonSelectColor;
            pacmansParkHighlightImage.enabled = false;

            sandboxText.color = textNonSelectColor;
            sandboxHighlightImage.enabled = false;

            jungleStepsText.color = textNonSelectColor;
            jungleStepsHighlightImage.enabled = false;
        }

        void HighlightSelected()
        {
            GameSession gameSession = Game.Instance.CurrentSession;
            if (currentSelectIndex==0)
            {
                blockTownText.color = textSelectColor;
                blockTownHighlightImage.enabled = true;
            }
            else if (currentSelectIndex==1)
            {
                pacmansParkText.color = textSelectColor;
                pacmansParkHighlightImage.enabled = true;
            }
            else if (currentSelectIndex==2)
            {
                sandboxText.color = textSelectColor;
                sandboxHighlightImage.enabled = true;
            }
            else if (currentSelectIndex==3)
            {
                jungleStepsText.color = textSelectColor;
                jungleStepsHighlightImage.enabled = true;
            }
        }
    }
}
