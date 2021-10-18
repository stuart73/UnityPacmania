using UnityEngine;
using UnityEngine.UI;
using Pacmania.GameManagement;

namespace Pacmania.Cutscenes
{
    public class ScrollingText : MonoBehaviour
    {
        [TextArea(15, 15)]
        [SerializeField] private string text = default;
        [SerializeField] private Text gameOverText = default;
        [SerializeField] private GameObject template = default;

        private GameObject[] lines;
        private float scrollOffset = 0;
        private const float scrollSpeed = 1.6f;
        private const float scrollOffsetToShowGameOverText = 3720.0f;
        private const float scrollOffsetToEndScene= 3960.0f;
        private const float scrollXPos = 80.0f;
        private const float numberOfPixelsPerScrollLine = 32.0f;

        private void Start()
        {
            string[] li = text.Split('\n');

            if (li.Length >0)
            {
                lines = new GameObject[li.Length];

                for (int i = 0; i < li.Length; i++)
                {
                    lines[i] = Instantiate(template, this.transform);
                    lines[i].GetComponent<Text>().text = li[i];
                }
            }
            template.SetActive(false);
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                RectTransform rt = lines[i].GetComponent<RectTransform>();
                rt.localPosition = new Vector3(scrollXPos, (-i * numberOfPixelsPerScrollLine) + scrollOffset, 0);
            }
            scrollOffset += scrollSpeed;

            if (scrollOffset > scrollOffsetToShowGameOverText)
            {
                gameOverText.enabled = true;
            }
            if (scrollOffset > scrollOffsetToEndScene)
            {
                Game.Instance.CurrentSession.GameOver();
            }
        }
    }
}
