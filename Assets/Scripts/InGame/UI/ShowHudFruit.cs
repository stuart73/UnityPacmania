using UnityEngine;
using Pacmania.GameManagement;

namespace Pacmania.InGame.UI
{
    public class ShowHudFruit : MonoBehaviour
    {
        [SerializeField] private GameObject[] fruit = default;

        private const float spacingBetweenFruit = 56.0f;
        private const int maxFruitShownAtOnce = 8;

        public void ReDraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            int currentLevel = Game.Instance.CurrentSession.CurrentLevel;
            int startLevel = Game.Instance.CurrentSession.StartedGameOnLevel;

            int count = 0;
            for (int i = currentLevel; i >= startLevel ; i--)
            {
                GameObject newObject = Instantiate(fruit[i-1], transform);
                float x = GetComponent<RectTransform>().rect.width / 2.0f;
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(x - (spacingBetweenFruit * count), 0, 0);
                count++;
                if (count == maxFruitShownAtOnce)
                {
                    break;
                }
            }
        }
    }
}
