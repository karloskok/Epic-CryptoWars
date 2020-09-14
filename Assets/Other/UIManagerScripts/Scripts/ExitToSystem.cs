using UnityEngine;

namespace UIManagerScripts
{
    public class ExitToSystem : MonoBehaviour
    {
        public void ExitGame()
        {
            Debug.Log("Beep beep, bop bop. Exit system is working :)");
            Application.Quit();
        }
    }
}