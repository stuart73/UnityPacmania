using UnityEngine;

namespace Pacmania.InGame.Characters
{
    public class CharacterManager
    {
        public CharacterMovement[] Characters { get; private set; }

        public CharacterManager()
        {
            Characters = UnityEngine.Object.FindObjectsOfType<CharacterMovement>();
        }

        public void HideCharacters()
        {
            if (Characters != null)
            {
                foreach (CharacterMovement character in Characters)
                {
                    character.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
        public void PauseCharacters()
        {
            if (Characters != null)
            {
                foreach (CharacterMovement character in Characters)
                {
                    character.Paused = true;
                }
            }
        }
        public void ResumeCharacters()
        {
            if (Characters != null)
            {
                foreach (CharacterMovement character in Characters)
                {
                    character.Paused = false;
                }
            }
        }
        public void ResetCharactersToStartPositions()
        {
            if (Characters != null)
            {
                foreach (CharacterMovement character in Characters)
                {
                    character.ResetToStartPosition();
                }
            }
        }
    }
}
