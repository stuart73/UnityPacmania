using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardShortcuts : MonoBehaviour
{
    public void QuitGame(InputAction.CallbackContext value)
    {
        if (!value.started) return;

        Application.Quit();
    }
}
