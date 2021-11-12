using UnityEngine;
using UnityEngine.Serialization;

namespace Pacmania.Menus
{
    public class OrbitSprite : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 5.0f)] private float middleX = 0.55f;
        [SerializeField] [Range(0.0f, 5.0f)] private float middleY = 0.95f;
        [SerializeField] [Range(0.0f, 5.0f)] private float diameterX = 1.1f;
        [SerializeField] [Range(0.0f, 5.0f)] private float diameterY = 0.42f;
        [SerializeField] [Range(0.0f, 8.0f)] private float speed = 3.8f;
        [SerializeField] [Range(-3.0f, 3.0f)] private float timeOffset = 0;

        private void Update()
        {
            float x = middleX + (Mathf.Sin((Time.timeSinceLevelLoad + timeOffset) * speed) * diameterX - (diameterX / 2));
            float y = middleY + (Mathf.Cos((Time.timeSinceLevelLoad + timeOffset) * speed) * diameterY - (diameterY / 2));

            transform.position = new Vector3(x, y, 0);
        }
    }
}
