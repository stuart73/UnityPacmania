using UnityEngine;
using UnityEngine.UI;

namespace Pacmania.Utilities.UI
{
    [RequireComponent(typeof(Image))]

    public class FaderImageIn : MonoBehaviour
    {
        [SerializeField] [Range(0.0001f, 0.01f)] private float step = 0.002f;
        private float current = 0;
        private bool fading = false;
        private Image image;
        private bool completed = false;
        
        public void StartFade()
        {
            fading = true;
            if (image)
            {
                image.enabled = true;
            }
        }

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void FixedUpdate()
        {
            if (fading == false && image && !completed)
            {
                return;
            }
            current += step;
            if (current > 1)
            {
                current = 1;
                completed = true;
            }

            image.color = new Color(image.color.r, image.color.g, image.color.b, current);
        }
    }
}
