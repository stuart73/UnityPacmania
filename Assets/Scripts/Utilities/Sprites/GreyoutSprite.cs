using UnityEngine;

namespace Pacmania.Utilities.Sprites
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GreyoutSprite : MonoBehaviour
    {
        [SerializeField] private Material greyMaterial = default;
        private Material defaultMaterial;
        public bool UsingGreyMateria { get; private set; } = false;
        
        private void Awake()
        {
            defaultMaterial = GetComponent<SpriteRenderer>().material;
        }
        private void Start()
        {
        }

        public void EnableGrey()
        {
            if (enabled == false) return;
            ApplyMeterial(greyMaterial);
            UsingGreyMateria = true;
        }

        public void DisableGrey()
        {
            if (enabled == false) return;
            ApplyMeterial(defaultMaterial);
            UsingGreyMateria = false;
        }

        private void ApplyMeterial(Material material)
        {
            SpriteRenderer[] allChildren = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer child in allChildren)
            {
                child.material = material;
            }
        }
    }
}
