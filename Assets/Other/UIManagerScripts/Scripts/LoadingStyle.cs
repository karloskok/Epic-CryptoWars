using UnityEngine;

namespace UIManagerScripts
{
    public class LoadingStyle : MonoBehaviour
    {
        public void SetStyle(string prefabToLoad)
        {
            UIManagerScripts.LoadingScreen.prefabName = prefabToLoad;
        }
    }
}