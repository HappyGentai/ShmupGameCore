using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkateGuy.Staties;
using SkateGuy.Staties.EnemyState;

namespace SkateGuy.GameElements
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Enemy : MonoBehaviour, IRecycleable
    {
        [SerializeField]
        private Transform m_MoveTarget = null;
        public Transform MoveTarget
        {
            get { return m_MoveTarget; }
        }
        [SerializeField]
        private CircleCollider2D m_HitBox = null; 
        [Header("State")]
        [SerializeField]
        private float m_MoveSpeed = 1f;
        public float MoveSpeed
        {
            get
            {
                return m_MoveSpeed;
            }
        }
        [Header("Launcher")]
        [SerializeField]
        private Launcher[] m_Launchers = null;
        public Launcher[] Launchers
        {
            get { return m_Launchers; }
        }

        private StateController stateController = null;

        private void Start()
        {
            //  Create state controller and set basic state
            stateController = new StateController();
            var adiotState = new EnemyStateAdiotMove(stateController, this, null, Vector2.left);
            stateController.SetState(adiotState);
        }

        private void Update()
        {
            stateController.Track();
        }

        public void Recycle()
        {
            m_HitBox.enabled = false;
            stateController.SetState(null);
        }
    }
}
