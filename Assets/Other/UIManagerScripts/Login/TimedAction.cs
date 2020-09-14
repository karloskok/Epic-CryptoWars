using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UIManagerScripts.Login
{
    public class TimedAction : MonoBehaviour
    {
        public float timer = 4;
        public bool enableAtStart;
        public UnityEvent timerAction;

        void Start()
        {
            if(enableAtStart == true)
            {
                StartCoroutine("TimedEvent");
            }
        }

        IEnumerator TimedEvent()
        {
            yield return new WaitForSeconds(timer);
            timerAction.Invoke();
        }

        public void StartIEnumerator ()
        {
            StartCoroutine("TimedEvent");
        }

        public void StopIEnumerator ()
        {
            StopCoroutine("TimedEvent");
        }
    }
}
