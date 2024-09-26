using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class EntityMovement : MonoBehaviour
    {
        public CharacterController Controller { get; private set; }

        public bool IsGrounded => m_grounded;
        
        [SerializeField] private bool m_grounded;
        [SerializeField] private float m_groundedCheckLength = 1.1f;
        private RaycastHit m_groundedHit;
        
        private Vector3 m_velocity;
        private float m_gravity = -30.0f;
        private float m_terminalVelocity = -13.0f;
        
        protected virtual void Awake()
        {
            Controller = GetComponent<CharacterController>();
        }
        
        protected virtual void Update()
        {
            m_grounded = CheckIsGrounded();
            
            ApplyGravity();
        }
        
        protected void Move(Vector3 direction, float movementSpeed)
        {
            Controller.Move(transform.TransformDirection(direction) * (movementSpeed * Time.deltaTime));
        }

        protected void Jump(float jumpStrength)
        {
            m_velocity.y = jumpStrength;
        }
        
        private void ApplyGravity()
        {
            m_velocity.y += m_gravity * Time.deltaTime;
            if (m_velocity.y <= m_terminalVelocity)
                m_velocity.y = m_terminalVelocity;

            if (IsGrounded && m_groundedHit.distance <= 0.05f)
                m_velocity.y = 0.0f;

            Controller.Move(m_velocity * Time.deltaTime);
        }
        
        private bool CheckIsGrounded()
        {
            Debug.DrawRay(transform.position, Vector3.down * m_groundedCheckLength, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out m_groundedHit, m_groundedCheckLength))
            {
                return true;
            }

            return false;
        }
    }
}
