using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIManagerScripts
{
    public class LoadScene : MonoBehaviour
    {
        public void ChangeToScene(string sceneName)
        {
            UIManagerScripts.LoadingScreen.LoadScene(sceneName);
        }

        public void LoadNewScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}