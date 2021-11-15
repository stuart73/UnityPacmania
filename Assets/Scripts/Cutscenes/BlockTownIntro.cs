using Pacmania.GameManagement;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Ghost;
using Pacmania.InGame.Characters.Pacman;
using UnityEngine;

namespace Pacmania.Cutscenes
{
    public class BlockTownIntro : MonoBehaviour
    {
        private Transform pacmanTransform;
        private GhostManager ghostManager;
        private const float introLengthInSeconds = 4.0f;
        private const float yPositionWhenGhostsLookUp = 0.14f;

        private void Awake()
        {
            pacmanTransform = FindObjectOfType<PacmanController>().transform;
            ghostManager = FindObjectOfType<GhostManager>();
        }

        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad > introLengthInSeconds)
            {
                Game.Instance.CurrentSession.StartNextScene();
            }

            UpdateGhostsEyes();
        }

        private void UpdateGhostsEyes()
        {
            float pacmanYPos = pacmanTransform.root.position.y;

            foreach (GhostController ghost in ghostManager.Ghosts)
            {
                if (pacmanYPos > yPositionWhenGhostsLookUp)
                {
                    ghost.GetComponent<Animator>().SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.LookingUp);
                }
                else
                {
                    ghost.GetComponent<Animator>().SetInteger(CharacterAnimatorParameterNames.State, GhostAnimationState.Normal);
                }
            }
        }
    }
}
