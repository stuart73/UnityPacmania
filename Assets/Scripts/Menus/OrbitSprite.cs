using UnityEngine;
using UnityEngine.Serialization;

namespace Pacmania.Menus
{
    public class OrbitSprite : MonoBehaviour
    {
        [SerializeField] private float middleX = 0.55f;
        [SerializeField] private float middleY = 0.95f;
        [SerializeField] private float diameterX = 1.1f;
        [SerializeField] private float diameterY = 0.42f;
        [SerializeField] private float speed = 3.8f;
        [SerializeField] private float timeOffset = 0;

        private void Update()
        {
            float x = middleX + (Mathf.Sin((Time.timeSinceLevelLoad + timeOffset) * speed) * diameterX - (diameterX / 2));
            float y = middleY + (Mathf.Cos((Time.timeSinceLevelLoad + timeOffset) * speed) * diameterY - (diameterY / 2));

            transform.position = new Vector3(x, y, 0);
        }
    }
}
