using Pacmania.GameManagement;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.Characters.Ghost.AI;
using Pacmania.InGame.Characters.Pacman;
using UnityEngine;

namespace Pacmania.Cutscenes
{
    public class JungleStepsIntro : MonoBehaviour
    {
        [SerializeField] private GameObject ghostPack = default;
        [SerializeField] private GameObject frontRow = default;
        [SerializeField] private GameObject innerPack = default;
        [SerializeField] private GameObject frightenPack = default;

        private Animator pacmansAnimator;
        private const float introLengthInSeconds = 12.3f;
        private const float firstPhaseLengthInSeconds = 4.0f;
        private const float yPositionWhenGhostsLookUp = 0.14f;

        private void Awake()
        {
            pacmansAnimator = FindObjectOfType<PacmanController>().GetComponent<Animator>();   
        }

        private void Start()
        {
            SetAllBlinkysToCruiseElroyState();
            PauseAnimationsForGhostsInInnerPack();
            SetAllGhostsInFrigtenPackToFrigtenAnimation();
            MakeGhostPackFaceRight();
        }

        private void MakeGhostPackFaceRight()
        {
            // Note, not all Animators in children objects will have horizontal/vertical animations
            foreach (GhostController ghostController in ghostPack.GetComponentsInChildren<GhostController>())
            {
                Animator animator = ghostController.GetComponent<Animator>();
                animator.SetFloat("Horizontal", 1);
                animator.SetFloat("Vertical", 0);
            }
        }

        private void SetAllGhostsInFrigtenPackToFrigtenAnimation()
        {
            // Note, not all Animators in children objects will have state animations
            foreach (GhostController ghostController in frightenPack.GetComponentsInChildren<GhostController>())
            {
                ghostController.GetComponent<Animator>().SetInteger("State", GhostAnimationState.Frigten);
            }
        }

        private void PauseAnimationsForGhostsInInnerPack()
        {
            foreach (Animator innerGhostAnimator in innerPack.GetComponentsInChildren<Animator>())
            {
                innerGhostAnimator.speed = 0;
            }
        }

        private void SetAllBlinkysToCruiseElroyState()
        {
            foreach (BlinkyAI blinky in ghostPack.GetComponentsInChildren<BlinkyAI>())
            {
                blinky.GetComponent<Animator>().SetBool("Cruise Elroy", true);
            }
        }

        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < firstPhaseLengthInSeconds)
            {
                // Face right.
                pacmansAnimator.SetFloat("Horizontal", 1);
                pacmansAnimator.SetFloat("Vertical", 0);
            }
            else
            {
                // Face down.
                pacmansAnimator.SetFloat("Vertical", -1);
                pacmansAnimator.SetFloat("Horizontal", 0);

                // pacman no longer jumping but now moving down so disable this.
                pacmansAnimator.GetComponentInChildren<Shadow>().ArenaZPositionUsesAnimationY = false;
            }
   
            UpdateGhostsEyes();

            if (Time.timeSinceLevelLoad > introLengthInSeconds)
            {
                Game.Instance.CurrentSession.StartNextScene();
            }
        }

        private void UpdateGhostsEyes()
        {
            float pacmanYPos = pacmansAnimator.transform.root.position.y;

            // Note, not all Animators in children objects will have state animations
            foreach (GhostController ghostController in frontRow.GetComponentsInChildren<GhostController>())
            {
                if (pacmanYPos > yPositionWhenGhostsLookUp)
                {
                    ghostController.GetComponent<Animator>().SetInteger("State", GhostAnimationState.LookingUp);
                }
                else
                {
                    ghostController.GetComponent<Animator>().SetInteger("State", GhostAnimationState.Normal);
                }
            }
        }
    }
}
