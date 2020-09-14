using UnityEngine;

namespace Game.UIManagerScripts.Login
{
    public class LoginScreenManager : MonoBehaviour
    {
        public GameObject splashScreen;
        public Animator startFadeIn;

        void Start()
        {
            splashScreen.SetActive(true);
            startFadeIn.Play("Start with Splash");
        }
    }
}