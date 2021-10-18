using System.Collections.Generic;
using UnityEngine;

namespace Pacmania.Utilities.Sprites
{
    public class PaletteSwapper : MonoBehaviour
    {
        [SerializeField] private Sprite oldPaletteSprite = default;
        [SerializeField] private Sprite newPaletteSprite = default;

        private List<Color> oldPalette = new List<Color>();
        private List<Color> newPalette = new List<Color>();

        private void Awake()
        {
            if (oldPaletteSprite == null || newPaletteSprite == null)
            {
                return;
            }

            GeneratePaletteForSprite(oldPaletteSprite, oldPalette);
            GeneratePaletteForSprite(newPaletteSprite, newPalette);

            if (oldPalette.Count != newPalette.Count && oldPalette.Count > 0)
            {
                Debug.LogError("Palettes different size or empty in palette swapper, ignoring...(" + oldPalette.Count.ToString() + "," + newPalette.Count.ToString() + ")", this);
                return;
            }

            SpriteRenderer[] spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                DoPaletteSwapForComponent(spriteRenderer);

            }
        }

        private void DoPaletteSwapForComponent(SpriteRenderer rendererComponent)
        {
            Texture2D newTexture = Instantiate(rendererComponent.sprite.texture);

            for (int h = 0; h < newTexture.height; h++)
            {
                for (int w = 0; w < newTexture.width; w++)
                {
                    Color c = newTexture.GetPixel(w, h);
                    if (c.a > 0)
                    { 
                        int index = GetColorPaletteIndex(oldPalette, c);
                        if (index != -1)
                        {
                            newTexture.SetPixel(w, h, newPalette[index]);                       
                        }
                    }
                }  
            }

            newTexture.Apply();
            rendererComponent.sprite = Sprite.Create(newTexture, rendererComponent.sprite.rect, new Vector2(0.5f, 0.5f), 100.0f);
        }

        private int GetColorPaletteIndex(List<Color> palette, Color color)
        {
            int i = 0;
            foreach (Color candColor in palette)
            {
                if (candColor.r == color.r && candColor.g == color.g && candColor.b == color.b)
                {
                    return i;
                }
                i++;
            }

            return -1;
        }

        private void GeneratePaletteForSprite(Sprite sprite, List<Color> palette)
        {
            Texture2D texture = sprite.texture;

            Color white = new Color(1, 1, 1);

            for (int h = 0; h < texture.height; h++)
            {
                for (int w = 0; w < texture.width; w++)
                {
                    Color c = texture.GetPixel(w, h);
                    if (c != white)
                    {
                        palette.Add(c);
                    }
                }
            }
        }
    }
}
