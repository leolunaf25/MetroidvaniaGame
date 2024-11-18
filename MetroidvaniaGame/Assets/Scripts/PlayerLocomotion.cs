using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    // Variables de movimiento
    private float horizontal;
    private float speed = 5f; // Velocidad ajustada para un metroidvania
    private float jumpingPower = 8f;

    // Componentes y referencias
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    void Update()
    {
        // Captura del input horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

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
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }
    }

    private void UpdateAnimator()
    {
        // Actualizar animaciones según el estado
        animator.SetBool("grounded", IsGrounded());
        animator.SetFloat("velocityY", rb.linearVelocity.y);

        if (horizontal != 0)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }
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
