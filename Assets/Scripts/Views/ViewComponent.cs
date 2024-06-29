using UnityEngine;
using UnityEngine.Events;

namespace Views
{
    public abstract class ViewComponent : MonoBehaviour
    {
        public virtual void Initialize(UnityEvent onShow, UnityEvent onHide)
        {
            onShow.AddListener(Show);
            onHide.AddListener(Hide);
        }

        protected virtual void Show()
        {
            gameObject.SetActive(true);
        }

        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}