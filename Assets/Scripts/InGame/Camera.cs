using UnityEngine;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame
{
    public class Camera : MonoBehaviour
    {
        private CharacterMovement pacmanCharacterMovement;
        private Vector3 initialPosition;
        private Vector3 initialCameraPostion;

        private void Awake()
        {
            pacmanCharacterMovement = FindObjectOfType<PacmanController>().GetComponent<CharacterMovement>(); ;   
        }

        private void Start()
        {
            initialPosition = pacmanCharacterMovement.transform.position;
            initialCameraPostion = transform.position;

          //  UnityEngine.Camera.main.rect = new Rect(0, 0.125f, 1, 0.75f);

        }

        private void Update()
        {
            Vector3 pacmanArenaPosition = pacmanCharacterMovement.ArenaPosition;
            Vector3 pacmanScreenPosition = pacmanCharacterMovement.Arena.GetTransformPositionFromArenaPosition( new Vector3(pacmanArenaPosition.x, pacmanArenaPosition.y, 0));
            pacmanScreenPosition.z = 0;

            transform.position = initialCameraPostion + (pacmanScreenPosition - initialPosition);
        }
    }
}
