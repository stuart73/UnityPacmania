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

         //   Camera.main.rect = new Rect(0, 0.125f, 1, 0.75f);

              StartCoroutine(WaitCoroutine());
        }

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(secondsInLogoScreen);
            Game.Instance.CurrentSession.StartNextScene() ;
        }

        private void Update()
        {
            if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0) == true)
            {
                (Game.Instance.CurrentSession as DemoGameSession)?.StartPlayerGame();
            }
        }      
    }
}
