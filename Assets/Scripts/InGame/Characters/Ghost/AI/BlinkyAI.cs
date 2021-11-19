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

            // Maybe null when this script is used in cutscenes.
            if (level != null)
            {
                level.Pacman.GetComponent<PacmanCollision>().EatenPellete += PacManCollision_EatenPellete;
            }
        }

        private void PacManCollision_EatenPellete()
        {
            pelletsEaten++;
        }

        public override Vector2Int GetScatterTile()
        {
            initialPellets = characterMovement.Arena.InitialNumberOfPellets;

            if (initialPellets > 0 && (initialPellets - pelletsEaten) <= pelletsRemainingToStayInChase)
            {
                return GetChaseTile();
            }

            return characterMovement.Arena.TopRightTile;
        }

        public override void EnableFastAngryMode()
        {
            GetComponent<CharacterMovement>().CharacterAnimator.SetBool(CharacterAnimatorParameterNames.CruiseElroy, true);
        }

        public override Vector2Int GetChaseTile()
        {          
            return pacmanCharacterMovement.GetTileIn(); 
        }
    }
}
