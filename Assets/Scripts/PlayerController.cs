using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_RigidBody;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;
    private bool m_IsGrounded = true;
    private bool m_IsRunning = false;
    private Quaternion m_TargetRotation;
    private int m_JumpCount = 0;

    [SerializeField] private Animator m_Animator;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    [SerializeField] private float m_RotationSpeed = 10f;
    [SerializeField] private float m_Speed = 10.0f;
    [SerializeField] private float m_JumpForce = 400f;
    [SerializeField] private int m_PossibleJumbs = 2;

    public bool m_canPlayerMove = true;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_TargetRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetButtonDown("Speed"))
            m_IsRunning = true;
        else
            m_IsRunning = false;

            // If the player should jump...
            if (m_IsGrounded && Input.GetButtonDown("Jump") || (m_JumpCount < m_PossibleJumbs) && Input.GetButtonDown("Jump"))
        {
            ++m_JumpCount;
            m_Animator.SetBool("IsJumping", true);
            // Add a vertical force to the player.
            m_RigidBody.AddForce(new Vector3(0f, m_IsRunning ? m_JumpForce + 500: m_JumpForce, 0f));
            m_IsGrounded = false;
        }
        else if (m_IsGrounded && !Input.GetButtonDown("Jump"))
        {
            m_Animator.SetBool("IsJumping", false);
            m_JumpCount = 0;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRotation, Time.deltaTime * m_RotationSpeed);
    }

    void FixedUpdate()
    {
        float mH = Input.GetAxis("Horizontal");
        float moveingSpeed = mH * m_Speed + (m_IsRunning ? mH * m_Speed : 0);
        // Move the character by finding the target velocity

        Vector3 targetVelocity = new Vector3(moveingSpeed, m_RigidBody.velocity.y, m_RigidBody.velocity.z);
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
        m_TargetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90f, transform.rotation.eulerAngles.z);
        if (transform.rotation.eulerAngles.y < 0f || transform.rotation.eulerAngles.y > 260f)
            m_TargetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90f, transform.rotation.eulerAngles.z);
    }
}