using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UIManagerScripts.Login
{
    public class LoginPanelManager : MonoBehaviour
    {
        public List<GameObject> loginPanel = new List<GameObject>();

        public List<GameObject> loginButton = new List<GameObject>();

        private string panelFadeIn = "Panel In";
        private string panelFadeOut = "Panel Out";

        private string buttonFadeIn = "Hover to Pressed";
        private string buttonFadeOut = "Pressed to Normal";

        private GameObject currentPanel;
        private GameObject nextPanel;

        private GameObject currentButton;
        private GameObject nextButton;

        public int currentPanelIndex = 0;
        private int currentButtonlIndex = 0;

        private Animator currentPanelAnimator;
        private Animator nextPanelAnimator;

        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        void Start()
        {
            currentButton = loginButton[currentPanelIndex];
            currentButtonAnimator = currentButton.GetComponent<Animator>();
            currentButtonAnimator.Play(buttonFadeIn);

            currentPanel = loginPanel[currentPanelIndex];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);
        }

        public void PanelAnim(int newPanel)
        {
            if (newPanel != currentPanelIndex)
            {
                currentPanel = loginPanel[currentPanelIndex];

                currentPanelIndex = newPanel;
                nextPanel = loginPanel[currentPanelIndex];

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();

                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                currentButton = loginButton[currentButtonlIndex];

                currentButtonlIndex = newPanel;
                nextButton = loginButton[currentButtonlIndex];

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }

        public void RegisterPlayer()
        {
            Debug.Log("Register");
        }

        public void LoginPlayer()
        {
            Debug.Log("Login");
            StartCoroutine("LoadMainMenu");
            
        }

        public IEnumerator LoadMainMenu()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}