using UnityEngine;

namespace SkateGuy.GameElements.EnemyLogicData
{
    [System.Serializable]
    public class EnemyTypeCurveMoveLogicData
    {
        [SerializeField]
        private double m_CurveEndPosX = 0;
        [SerializeField]
        private double m_CurveEndPosY = 0;
        public Vector2 CurveEndPos
        {
            get
            {
                return new Vector2((float)m_CurveEndPosX, (float)m_CurveEndPosY);
            }
            private set { }
        }

        [SerializeField]
        private double m_CurveAidPosAX = 0;
        [SerializeField]
        private double m_CurveAidPosAY = 0;
        public Vector2 CurveAidPosA
        {
            get { return new Vector2((float)m_CurveAidPosAX, (float)m_CurveAidPosAY); }
            private set { }
        }

        [SerializeField]
        private double m_CurveAidPosBX = 0;
        [SerializeField]
        private double m_CurveAidPosBY = 0;
        public Vector2 CurveAidPosB
        {
            get { return new Vector2((float)m_CurveAidPosBX, (float)m_CurveAidPosBY); }
            private set { }
        }

        [SerializeField]
        private double m_SpeedScale = 0;
        public  float SpeedScale
        {
            get { return (float)m_SpeedScale; }
            private set { }
        }

        [SerializeField]
        private bool m_FireWhenMove = false;
        public bool FireWhenMove
        {
            get { return m_FireWhenMove; }
            private set { }
        }

        public EnemyTypeCurveMoveLogicData(Vector2 _CurveEndPos,
            Vector2 _CurveAidPosA, Vector2 _CurveAidPosB, float _SpeedScale, bool _FireWhenMove)
        {
            m_CurveEndPosX = _CurveEndPos.x;
            m_CurveEndPosY = _CurveEndPos.y;
            m_CurveAidPosAX = _CurveAidPosA.x;
            m_CurveAidPosAY = _CurveAidPosA.y;
            m_CurveAidPosBX = _CurveAidPosB.x;
            m_CurveAidPosBY = _CurveAidPosB.y;
            m_SpeedScale = _SpeedScale;
            m_FireWhenMove = _FireWhenMove;
        }
    }
}
