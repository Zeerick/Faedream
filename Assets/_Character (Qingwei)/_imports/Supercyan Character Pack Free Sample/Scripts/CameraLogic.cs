using System.Collections.Generic;
using UnityEngine;

namespace _imports.Supercyan_Character_Pack_Free_Sample.Scripts
{
    public class CameraLogic : MonoBehaviour {

    
        public float rotationSpeedX = 1f;  // index of camera horizontal rotation speed
        public float rotationSpeedY = 1f;  // index of camera vertical rotation speed
        public float upperPitchConstraint = 90f;  // limit of angle of camera rotation going up
        public float lowerPitchConstraint = 90f;  // limit of angle of camera rotation going down
        public float buffering;  // to be implemented

        private float x = 0f;  // basically represents horizontal rotation reference (positive for going right)
        private float y = 0f;  // basically represents vertical rotation reference (positive for going upward)
    
    
        private Transform m_currentTarget;
        private float m_distance = 2f;
        private float m_height = 1;
        private float m_lookAtAroundAngle = 180;

        [SerializeField] private List<Transform> m_targets;
        private int m_currentIndex;

        private void Start () {
            if(m_targets.Count > 0)
            {
                m_currentIndex = 0;
                m_currentTarget = m_targets[m_currentIndex];
            }
        }

        private void SwitchTarget(int step)
        {
            if(m_targets.Count == 0) { return; }
            m_currentIndex+=step;
            if (m_currentIndex > m_targets.Count-1) { m_currentIndex = 0; }
            if (m_currentIndex < 0) { m_currentIndex = m_targets.Count - 1; }
            m_currentTarget = m_targets[m_currentIndex];
        }

        public void NextTarget() { SwitchTarget(1); }
        public void PreviousTarget() { SwitchTarget(-1); }

        private void Update () {
            if (m_targets.Count == 0) { return; }
        }
    
    
    
        void FixedUpdate()
        {
            x += Input.GetAxisRaw("Mouse X") * rotationSpeedX;
            y += Input.GetAxisRaw("Mouse Y") * rotationSpeedY;
            y = Mathf.Clamp(y, -lowerPitchConstraint, upperPitchConstraint);
        
            Quaternion rotation = Quaternion.Euler(-y, x, 0);
        
            transform.rotation = rotation;
        }
    
    
    
    

        private void LateUpdate()
        {
            if(m_currentTarget == null) { return; }

            float targetHeight = m_currentTarget.position.y + m_height;
            float currentRotationAngle = m_lookAtAroundAngle;

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            Vector3 position = m_currentTarget.position;
            position -= currentRotation * Vector3.forward * m_distance;
            position.y = targetHeight;

            transform.position = position;
            transform.LookAt(m_currentTarget.position + new Vector3(0, m_height, 0));
        }
    }
}
