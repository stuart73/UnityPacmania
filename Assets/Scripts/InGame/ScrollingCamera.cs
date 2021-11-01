using UnityEngine;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame
{
    public class ScrollingCamera : MonoBehaviour
    {
        private CharacterMovement pacmanCharacterMovement;
        private Vector3 initialPacmanScreenPosition;
        private Vector3 initialCameraPostion;

        private void Awake()
        {
            pacmanCharacterMovement = FindObjectOfType<PacmanController>().GetComponent<CharacterMovement>();
        }

        private void Start()
        {
            initialPacmanScreenPosition = pacmanCharacterMovement.transform.position;
            initialCameraPostion = transform.position;
        }

        private void Update()
        {
            Vector3 pacmanArenaPosition = pacmanCharacterMovement.ArenaPosition;
            pacmanArenaPosition.z = 0; // ignore any jumping part;

            Vector3 pacmanScreenPosition = pacmanCharacterMovement.Arena.GetTransformPositionFromArenaPosition(pacmanArenaPosition);
            pacmanScreenPosition.z = 0;

            transform.position = initialCameraPostion + (pacmanScreenPosition - initialPacmanScreenPosition);
        }
    }
}
