using UnityEngine;

namespace Pacmania.InGame.Characters.Pacman
{
    [RequireComponent(typeof(Jumping))]
    public class JumpWhenPacmanJumps : MonoBehaviour
    {
        private Jumping jumpingComponent;

        private void Awake()
        {
            jumpingComponent = GetComponent<Jumping>();
        }

        private void Start()
        {
            Level level = FindObjectOfType<Level>();
            if (level != null)
            {
                level.Pacman.GetComponent<Jumping>().Jumped += Pacman_Jumped;
            }
        }

        private void Pacman_Jumped()
        {
            // Pacman jumped, therefore we should jump.
            jumpingComponent.Jump();       
        }
    }
}
