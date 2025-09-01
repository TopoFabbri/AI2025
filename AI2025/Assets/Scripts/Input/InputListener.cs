using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Input
{
    public class InputListener : MonoBehaviour
    {
        public static event Action Restart;
        public static event Action ResetPath;

        private void OnRestart()
        {
            Restart?.Invoke();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnResetPath()
        {
            ResetPath?.Invoke();
        }
    }
}
