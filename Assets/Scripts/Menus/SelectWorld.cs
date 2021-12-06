using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Pacmania.GameManagement;

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
        private bool waitforChange = false;
        private const float worldChangeTimeStep = 0.2f;

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (worldSelected == true)
            {
                return;
            }

            Vector2 inputMovement = value.ReadValue<Vector2>();

            if (inputMovement.y == 0)
            {
                waitforChange = false;
            }

            CheckInput(inputMovement);
        }

        public void OnMovementSwipe(InputAction.CallbackContext value)
        {
            if (worldSelected == true || waitforChange == true)
            {
                return;
            }

            Vector2 inputMovement = value.ReadValue<Vector2>();

            CheckInput(inputMovement);

            if (waitforChange == true)
            {
                IEnumerator fader = Wait();
                StartCoroutine(fader);
            }
        }

        private void CheckInput(Vector2 inputMovement)
        {
            if (inputMovement.y < 0 && waitforChange == false)
            {
                currentSelectIndex++;
                if (currentSelectIndex > 3) currentSelectIndex = 0;
                waitforChange = true;
            }
            else if (inputMovement.y > 0 && waitforChange == false)
            {
                currentSelectIndex--;
                if (currentSelectIndex < 0) currentSelectIndex = 3;
                waitforChange = true;
            }
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(worldChangeTimeStep);
            waitforChange = false;
        }

        public void OnInputTrigger()
        {
            PlayerGameSession gameSession = Game.Instance.CurrentSession as PlayerGameSession;

            gameSession.CurrentLevel = selectIndexToStartWorld[currentSelectIndex];
            gameSession.StartedGameOnLevel = gameSession.CurrentLevel;
            gameSession.CourageBonus = courageBonus[currentSelectIndex];
            worldSelected = true;
            StartCoroutine(Startlevel());
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
