using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
        private bool isOpen = false;
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
            }
        }

        //  On close and open event
        public UnityEvent OnUIOpen = new UnityEvent();
        public UnityEvent OnUIClose = new UnityEvent();

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
            OnUIOpen?.Invoke();
            IsOpen = true;

            UIManager.AddOpenUI(this);
        }

        public virtual void Close()
        {
            OnUIClose?.Invoke();
            m_UIRoot.SetActive(false);
            IsOpen = false;
        }

        protected void SetSelectedGameObject(GameObject gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
