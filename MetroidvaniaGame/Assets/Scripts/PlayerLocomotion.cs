using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    // Variables de movimiento
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 8f;

    // Componentes y referencias
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    // Estado del personaje
    private bool isGrounded;

    void Update()
    {
        // Captura del input horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        // Actualizar el estado de si está en el suelo
        isGrounded = IsGrounded();

        // Lógica de salto
        HandleJump();

        // Actualización de las animaciones
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        // Control del giro del sprite
        if (horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0f;
        }
    }

    private void HandleJump()
    {
        // Salto solo si está en el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }
    }

    private void UpdateAnimator()
    {
        // Animación de caminar
        animator.SetBool("walk", horizontal != 0 && isGrounded);

        // Animación de salto (cuando no está en el suelo)
        animator.SetBool("jump", !isGrounded);
    }

    private bool IsGrounded()
    {
        // Detección de suelo con un círculo
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Método opcional para visualizar el área de detección en el editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
