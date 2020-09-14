using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UIManagerScripts.Button
{
    public class InteractablePanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Animator buttonAnimator;

        void Start()
        {
            buttonAnimator = this.GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed"))
            {
                // do nothing because it's clicked
            }

            else
            {
                buttonAnimator.Play("Hover");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed"))
            {
                // do nothing because it's clicked
            }

            else
            {
                buttonAnimator.Play("Normal");
            }
        }
    }
}
