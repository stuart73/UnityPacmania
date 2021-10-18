using UnityEngine;
using Pacmania.InGame.Characters;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    [RequireComponent(typeof(CharacterMovement))]
    public class GhostAI : MonoBehaviour
    {
        protected CharacterMovement characterMovement;
        protected CharacterMovement pacmanCharacterMovement;
        public virtual Vector2Int ScatterPosition() { return Vector2Int.zero; }
        public virtual Vector2Int ChasePosition() { return Vector2Int.zero; }

        protected virtual void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            pacmanCharacterMovement = FindObjectOfType<PacmanController>().GetComponent<CharacterMovement>();
        }

        public virtual void EnableFastAngryMode() { }

        protected virtual void Start() { }
    }
}