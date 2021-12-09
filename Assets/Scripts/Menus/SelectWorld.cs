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
        private bool worldSelected = false;
        private int lastRenderFrame = -1;
        private bool flickerOn = false;
        private int numberOfFramesBetweenChange = 20;
        private int allowChangeCount = 0;

        private void Awake()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                numberOfFramesBetweenChange = 0;
            }
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (!value.started || worldSelected == true)
            {
                return;
            }

            Vector2 inputMovement = value.ReadValue<Vector2>();

            if (inputMovement.y < 0 && allowChangeCount >= numberOfFramesBetweenChange)
            {
                currentSelectIndex++;
                if (currentSelectIndex > 3)
                {
                    if (Application.platform != RuntimePlatform.Android)
                    {
                        currentSelectIndex = 0;
                    }
                    else
                    {
                        currentSelectIndex = 3;
                    }
                }
                allowChangeCount = 0;
            }
            else if (inputMovement.y > 0 && allowChangeCount >= numberOfFramesBetweenChange)
            {
                currentSelectIndex--;

                if (currentSelectIndex < 0)
                {
                    if (Application.platform != RuntimePlatform.Android)
                    {
                        currentSelectIndex = 3;
                    }
                    else
                    {
                        currentSelectIndex = 0;
                    }
                }
                allowChangeCount = 0;
            }
        }

        public void OnInputTrigger(InputAction.CallbackContext value)
        {
            if (!value.started || worldSelected == true) return;

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
            HighlightSelected();
            Game.Instance.CurrentSession.StartNextScene();       
        }

        private void FixedUpdate()
        {
            ClearSelected();
         
            int renderFrame = Time.frameCount;
            if (renderFrame != lastRenderFrame)
            {
                lastRenderFrame = renderFrame;
                flickerOn = !flickerOn;
            }

            if (worldSelected == false || flickerOn)
            {
                HighlightSelected();
            }
            allowChangeCount++;
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
