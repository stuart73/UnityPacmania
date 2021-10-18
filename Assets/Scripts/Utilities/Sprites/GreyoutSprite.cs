using UnityEngine;

namespace Pacmania.Utilities.Sprites
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GreyoutSprite : MonoBehaviour
    {
        [SerializeField] private Material greyMaterial = default;
        private Material defaultMaterial;
        
        private void Awake()
        {
            defaultMaterial = GetComponent<SpriteRenderer>().material;
        }
        private void Start()
        {
        }

        public void GreyEnabled(bool value)
        {
            if (enabled == false)
            {
                return;
            }

            Material material = defaultMaterial;
            if (value == true)
            {
                material = greyMaterial;
            }
            GetComponent<SpriteRenderer>().material = defaultMaterial;
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                child.GetComponent<SpriteRenderer>().material = material;
            }
        }
    }
}
