using UnityEngine;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame.Characters.Ghost.AI
{
    public class BlinkyAI : GhostAI
    {
        [SerializeField] int pelletsRemainingToStayInChase = 40;
        private int pelletsEaten = 0; 
        private int initialPellets = 0;
   
        protected override void Start()
        {
            base.Start();

            Level level = FindObjectOfType<Level>();
            if (level != null)
            {
                level.Pacman.GetComponent<PacmanCollision>().EatenPellete += PacManCollision_EatenPellete;
            }
        }

        private void PacManCollision_EatenPellete()
        {
            pelletsEaten++;
        }

        public override Vector2Int ScatterPosition()
        {
            initialPellets = characterMovement.Arena.InitialNumberOfPellets;

            if (initialPellets > 0 && (initialPellets - pelletsEaten) <= pelletsRemainingToStayInChase)
            {
                return ChasePosition();
            }

            return characterMovement.Arena.TopRightTile;
        }

        public override void EnableFastAngryMode()
        {
            GetComponent<CharacterMovement>().Animator.SetBool("Cruise Elroy", true);
        }

        public override Vector2Int ChasePosition()
        {          
            return pacmanCharacterMovement.GetTileIn(); 
        }
    }
}
