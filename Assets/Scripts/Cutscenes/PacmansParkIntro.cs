using UnityEngine;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Pacman;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.Characters.Ghost.AI;
using Pacmania.GameManagement;

namespace Pacmania.Scripts.Cutscenes
{
    public class PacmansParkIntro : MonoBehaviour
    {
        [SerializeField] private Animator inkyAnimtor = default;
        [SerializeField] private Animator blinkyAnimator = default;
        [SerializeField] private GameObject ghostPack = default;
        [SerializeField] private GameObject frontRow = default;
        [SerializeField] private GameObject innerPack = default;

        private Animator pacmanAnimator;
        private const float introLengthInSeconds = 12.8f;
        private const float firstPhaseLengthInSeconds = 5.0f;
        private const float yPositionWhenGhostsLookUp = 0.14f;

        private void Awake()
        {
            pacmanAnimator = FindObjectOfType<PacmanController>().GetComponent<Animator>();
        }

        private void Start()
        {
            SetInkyBlinkyFrigten();
            SetAllBlinkysToCruiseElroyState();
            PauseAnimationsForGhostsInInnerPack();
            FacePacmanInkyBlinkyLeft();
            FaceGhostPackRight();
        }

        private void FaceGhostPackRight()
        {
            foreach (GhostController ghostController in ghostPack.GetComponentsInChildren<GhostController>())
            {
                Animator ghostAnimator = ghostController.GetComponent<Animator>();
                ghostAnimator.SetFloat(CharacterAnimatorParameterNames.Horizontal, 1);
                ghostAnimator.SetFloat(CharacterAnimatorParameterNames.Vertical, 0);
            }
        }

        private void SetInkyBlinkyFrigten()
        {
            inkyAnimtor.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Frigten);
            blinkyAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Frigten);
        }

        private void PauseAnimationsForGhostsInInnerPack()
        {
            foreach (Animator innerGhostAnimator in innerPack.GetComponentsInChildren<Animator>())
            {
                innerGhostAnimator.GetComponent<Animator>().speed = 0;
            }
        }
        private void SetAllBlinkysToCruiseElroyState()
        {
            BlinkyAI[] ghostPackMovements = ghostPack.GetComponentsInChildren<BlinkyAI>();
            foreach (BlinkyAI blinky in ghostPackMovements)
            {
                blinky.GetComponent<Animator>().SetBool(CharacterAnimatorParameterNames.CruiseElroy, true);
            }
        }
        private void FacePacmanInkyBlinkyLeft()
        {
            pacmanAnimator.SetFloat(CharacterAnimatorParameterNames.Horizontal, -1);
            pacmanAnimator.SetFloat(CharacterAnimatorParameterNames.Vertical, 0);

            inkyAnimtor.SetFloat(CharacterAnimatorParameterNames.Horizontal, -1);
            inkyAnimtor.SetFloat(CharacterAnimatorParameterNames.Vertical, 0);

            blinkyAnimator.SetFloat(CharacterAnimatorParameterNames.Horizontal, -1);
            blinkyAnimator.SetFloat(CharacterAnimatorParameterNames.Vertical, 0);
        }

        private void FixedUpdate()
        {
            // Face pacman right if finished first phase.
            if (Time.timeSinceLevelLoad > firstPhaseLengthInSeconds)
            {
                pacmanAnimator.SetFloat("Horizontal", 1);
            }

            UpdateGhostsEyes();

            if (Time.timeSinceLevelLoad > introLengthInSeconds)
            {
                Game.Instance.CurrentSession.StartNextScene();     
            } 
        }

      
        private void UpdateGhostsEyes()
        {
            float pacmanYPos = pacmanAnimator.transform.root.position.y;

            CharacterMovement[] ghostPackMovements = frontRow.GetComponentsInChildren<CharacterMovement>();
            foreach (CharacterMovement ghost in ghostPackMovements)
            {             
                if (pacmanYPos > yPositionWhenGhostsLookUp)
                {
                    ghost.GetComponent<CharacterMovement>().CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.LookingUp);
                }
                else
                {
                    ghost.GetComponent<CharacterMovement>().CharacterAnimator.SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Normal);
                }
            }
        }
    }
}
