using UnityEngine;
using System.Collections;
using Pacmania.GameManagement;

namespace Pacmania.Menus
{
    public class LogoMenu : MonoBehaviour
    {
        private const float secondsInLogoScreen = 4.0f;

        private void Start()
        {
            // If the current game session is not a demo one, then make it so.  By default it will be set to a player session
            // allowing us to test levels in the Unity editor.
            if (Game.Instance.CurrentSession is DemoGameSession == false)
            {
                Game.Instance.CreateNewDemoGameSession(1);
            }

            StartCoroutine(WaitCoroutine());
        }

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(secondsInLogoScreen);
            Game.Instance.CurrentSession.StartNextScene() ;
        }

        public void OnInputTrigger()
        {
            (Game.Instance.CurrentSession as DemoGameSession)?.StartPlayerGame();
        }      
    }
}
