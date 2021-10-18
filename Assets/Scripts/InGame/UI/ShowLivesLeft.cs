using UnityEngine;
using Pacmania.GameManagement;

namespace Pacmania.InGame.UI
{
    public class ShowLivesLeft : MonoBehaviour
    {
        [SerializeField] private GameObject miniPacman = default;
        private const float pixelsSpacePerPacman = 60.0f;

        public void ReDraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            int lives = Game.Instance.CurrentSession.Lives;

            for (int i=0; i < lives-1; i++)
            {
                GameObject newObject = Instantiate(miniPacman, transform);
                RectTransform recTransform = newObject.GetComponent<RectTransform>();
                recTransform.localPosition = new Vector3(recTransform.localPosition.x + (pixelsSpacePerPacman * i), recTransform.localPosition.y, recTransform.localPosition.z);   
            }
        }
    }
}
