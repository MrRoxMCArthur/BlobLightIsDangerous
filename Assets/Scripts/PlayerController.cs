using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_RigidBody;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;
    private bool m_IsGrounded = true;
    private Quaternion targetRotation;

    [SerializeField] private Animator m_Animator;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    [SerializeField] private float m_RotationSpeed = 10f;
    [SerializeField] private float m_Speed = 10.0f;
    [SerializeField] private float m_JumpForce = 400f;

    public bool m_canPlayerMove = true;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // If the player should jump...
        if (m_IsGrounded && Input.GetButtonDown("Jump"))
        {
            m_Animator.SetBool("IsJumping", true);
            // Add a vertical force to the player.
            m_RigidBody.AddForce(new Vector3(0f, m_JumpForce, 0f));
            m_IsGrounded = false;
        }
        else if(m_IsGrounded && !Input.GetButtonDown("Jump"))
            m_Animator.SetBool("IsJumping", false);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * m_RotationSpeed);
    }

    void FixedUpdate()
    {
        float mH = Input.GetAxis("Horizontal");
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector3(mH * m_Speed, m_RigidBody.velocity.y, m_RigidBody.velocity.z);
        m_RigidBody.velocity = Vector3.SmoothDamp(m_RigidBody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        if (mH > 0f && !m_FacingRight || mH < 0f && m_FacingRight)
        {
            Flip();
            m_FacingRight = !m_FacingRight;
        }

        if (mH != 0f)
            m_Animator.SetBool("IsRunning", true);
        else
            m_Animator.SetBool("IsRunning", false);

    }

    void OnCollisionEnter(Collision collision)
    {
        m_IsGrounded = true;
    }

    private void Flip()
    {
        targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90f, transform.rotation.eulerAngles.z);
        if (transform.rotation.eulerAngles.y < 0f || transform.rotation.eulerAngles.y > 260f)
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90f, transform.rotation.eulerAngles.z);
    }
}