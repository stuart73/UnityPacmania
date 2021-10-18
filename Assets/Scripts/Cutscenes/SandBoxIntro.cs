using UnityEngine;
using Pacmania.GameManagement;

namespace Pacmania.Cutscenes
{
    public class SandBoxIntro : MonoBehaviour
    {
        private const float secondsIntroShownFor = 7.8f;
        private void Update()
        {
            if (Time.timeSinceLevelLoad > secondsIntroShownFor)
            {
                Game.Instance.CurrentSession.StartNextScene();
            }
        }
    }
}
