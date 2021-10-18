using System.Text;
using UnityEngine;
using Pacmania.InGame.Characters.Ghost.AI;
using Pacmania.GameManagement;
using UnityEditor;

namespace Pacmania.InGame.Characters.Ghost
{
    [RequireComponent(typeof(GhostManager))]
    public class GhostFastModeTimer : MonoBehaviour
    {
        [SerializeField] private float fastModeStartTime = 60;
        private int frameCount = 0;
        private const float fastModeSpeedMultiplier = 1.07f;

        private void FixedUpdate()
        {
            frameCount++;
            if (frameCount == fastModeStartTime * Game.FramesPerSecond )
            {
                EnableFastMode();
            }
        }

        public void EnableFastMode()
        {
            GhostController[] ghosts = GetComponent<GhostManager>().Ghosts; 
            foreach (GhostController ghost in ghosts)
            {     
                ghost.GetComponent<GhostAI>().EnableFastAngryMode();
                ghost.GetComponent<CharacterMovement>().DefaultSpeed *= fastModeSpeedMultiplier;
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            float secondsCounter = ((fastModeStartTime * Game.FramesPerSecond) - frameCount) / Game.FramesPerSecond;
            if (secondsCounter < 0) secondsCounter = 0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Fast mode begins in:");
       
            stringBuilder.Append(secondsCounter.ToString("0.00"));

            Vector3 position = transform.position;
            position.y -= 0.18f;
            Handles.Label(position, stringBuilder.ToString());
#endif
        }
    }
}
