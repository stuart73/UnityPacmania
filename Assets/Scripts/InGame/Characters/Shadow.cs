using UnityEngine;
using Pacmania.InGame.Arenas;

namespace Pacmania.InGame.Characters
{
    [RequireComponent(typeof(Animator))]
    public class Shadow : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 10.0f)] private float xOffsetPixels = 6;
        [SerializeField] [Range(0.0f, 5.0f)] private float yOffsetPixels = 0;

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

            string parameterName = CharacterAnimatorParameterNames.ShadowNumber;
            if (UseEyesShadow == true)
            {
                animator.SetInteger(parameterName, 3);
            }
            else
            {
                if (zOffset < 6)
                {
                    animator.SetInteger(parameterName, 1);
                }
                else if (zOffset < 9)
                {
                    animator.SetInteger(parameterName, 2);
                }
                else if (zOffset < 12)
                {
                    animator.SetInteger(parameterName, 3);
                }
                else if (zOffset < 16)
                {
                    animator.SetInteger(parameterName, 4);
                }
                else if (zOffset < 20)
                {
                    animator.SetInteger(parameterName, 5);
                }
                else
                {
                    animator.SetInteger(parameterName, 6);
                }
            }

            float pixelsPerUnit = Arena.spritePixelPerUnit;

            Vector3 newPos = transform.position;
            newPos.y = transform.parent.position.y - (zOffset / pixelsPerUnit) + (yOffsetPixels / pixelsPerUnit);
            newPos.x = transform.parent.position.x + (xOffsetPixels / pixelsPerUnit);
            transform.position = newPos;
        }
    }
}
