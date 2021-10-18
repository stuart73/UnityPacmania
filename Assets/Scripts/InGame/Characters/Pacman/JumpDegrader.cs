using UnityEngine;

namespace Pacmania.InGame.Characters.Pacman
{
    [RequireComponent(typeof(Jumping))]

    public class JumpDegrader : MonoBehaviour
    {
        [SerializeField] private float degradeJumpStartTime = 120;
        [SerializeField] private float degradeJumpEndTime = 240;

        private float startTime = 0;
        private Jumping jumpingComponent;

        private void Start()
        {
            jumpingComponent = GetComponent<Jumping>();
            startTime = Time.time;
        }

        private void FixedUpdate()
        {
            float timeSinceStart = Time.time - startTime;

            if (timeSinceStart >= degradeJumpEndTime)
            {
                jumpingComponent.JumpPower = 0;
                return;
            }

            if (timeSinceStart > degradeJumpStartTime)
            {
                float degradeTimeSpan = degradeJumpEndTime - degradeJumpStartTime;

                jumpingComponent.JumpPower = (degradeJumpEndTime - timeSinceStart) / degradeTimeSpan;
            }
        }
    }
}
