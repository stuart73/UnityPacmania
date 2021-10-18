using UnityEngine;
using Pacmania.InGame.UI;
using Pacmania.GameManagement;

namespace Pacmania.Cutscenes
{
    public class CompleteScreen : MonoBehaviour
    {
        private const float sceneLengthInSeconds = 8.5f;

        private void Start()
        {
            FindObjectOfType<Hud>().StartScreenFadeout();
        }

        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad > sceneLengthInSeconds)
            {
                Game.Instance.CurrentSession.StartNextScene();
            }
        }
    }
}
