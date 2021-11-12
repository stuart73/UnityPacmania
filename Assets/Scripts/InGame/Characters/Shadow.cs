using UnityEngine;
using Pacmania.GameManagement;
using Pacmania.InGame.Arenas;

namespace Pacmania.InGame.Characters
{
    [RequireComponent(typeof(Animator))]
    public class Shadow : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 5.0f)] private float yOffSetPixels = 1;

        // Used in cut scenes
        [SerializeField] private bool arenaZPositionUsesAnimationY = false;
        public bool ArenaZPositionUsesAnimationY
        {
            get => arenaZPositionUsesAnimationY;
            set => arenaZPositionUsesAnimationY = value;
        }
        // Used in cut scenes
        [SerializeField] private Transform animatedParent = null;

        private CharacterMovement movement;
        private Animator animator;

        public bool UseEyesShadow { get; set; } = false;

        private void Awake()
        {
            movement = GetComponentInParent<CharacterMovement>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float zOffset = movement.ArenaPosition.z;

            if (arenaZPositionUsesAnimationY == true && animatedParent != null)
            {
                zOffset += animatedParent.position.y * Arena.spritePixelPerUnit;
            }

            if (UseEyesShadow == true)
            {
                animator.SetInteger("ShadowNumber", 3);
            }
            else
            {
                if (zOffset < 6)
                {
                    animator.SetInteger("ShadowNumber", 1);
                }
                else if (zOffset < 9)
                {
                    animator.SetInteger("ShadowNumber", 2);
                }
                else if (zOffset < 12)
                {
                    animator.SetInteger("ShadowNumber", 3);
                }
                else if (zOffset < 16)
                {
                    animator.SetInteger("ShadowNumber", 4);
                }
                else if (zOffset < 20)
                {
                    animator.SetInteger("ShadowNumber", 5);
                }
                else
                {
                    animator.SetInteger("ShadowNumber", 6);
                }
            }

            Vector3 newPos = transform.position;
            newPos.y = transform.parent.position.y - (zOffset / 100.0f) + (yOffSetPixels / 100.0f);
            newPos.x = transform.parent.position.x + 0.06f;
            transform.position = newPos;
        }
    }
}
