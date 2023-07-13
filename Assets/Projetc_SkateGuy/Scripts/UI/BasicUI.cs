using UnityEngine;
using UnityEngine.EventSystems;

namespace SkateGuy.UIs
{
    public class BasicUI : MonoBehaviour
    {
        [SerializeField]
        protected GameObject m_UIRoot = null;
        [SerializeField]
        protected GameObject m_SelectedUIOnOpen = null;
        [SerializeField]
        private bool m_AutoInitializeOnStart = false;
        protected bool _IsInitialize = false;
        public bool IsInitialize
        {
            get { return _IsInitialize; }
        }

        protected virtual void Start()
        {
            if (m_AutoInitializeOnStart)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            if (_IsInitialize)
            {
                return;
            }
            //  Do Initialize 
            DoInitialize();
            //  When successm set _IsInitialize to true.
            _IsInitialize = true;
        }

        protected virtual void DoInitialize()
        {
  
        }

        public virtual void Open()
        {
            m_UIRoot.SetActive(true);
            if (m_SelectedUIOnOpen != null)
            {
                SetSelectedGameObject(m_SelectedUIOnOpen);
            }
        }

        public virtual void Close()
        {
            m_UIRoot.SetActive(false);
        }

        protected void SetSelectedGameObject(GameObject gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
