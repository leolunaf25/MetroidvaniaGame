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
    public bool isAttacking; // Nuevo estado para el ataque

    void Update()
    {
        // Captura del input horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        // Actualizar el estado de si está en el suelo
        isGrounded = IsGrounded();

        // Manejo del salto
        HandleJump();

        // Manejo del ataque
        HandleAttack();

        // Actualización de las animaciones
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal, solo si no está atacando
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }

        // Control del giro del sprite
        if (horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0f;
        }
    }

    private void HandleJump()
    {
        // Salto solo si está en el suelo y no está atacando
        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }
    }

    private void HandleAttack()
    {
        string[] triggers = { "punch", "kick", "blaster" };

        // Activar animación de ataque al hacer clic izquierdo
        for (int i = 0; i < triggers.Length; i++)
        {
            if (Input.GetMouseButtonDown(i) && !isAttacking)
            {
                isAttacking = true;
                rb.linearVelocity = new Vector2(0, 0);
                animator.SetTrigger(triggers[i]);
                break;
            }
        }
    }
    /*
    private void HandleAttack()
    {
        KeyCode[] attackKeys = { KeyCode.Z, KeyCode.X, KeyCode.C };
        string[] triggers = { "uno", "dos", "tres" };

        for (int i = 0; i < attackKeys.Length; i++)
        {
            if (Input.GetKeyDown(attackKeys[i]) && !isAttacking)
            {
                isAttacking = true;
                rb.velocity = new Vector2(0, 0);
                animator.SetTrigger(triggers[i]);
                break;
            }
        }
    }
    */
    private void UpdateAnimator()
    {
        // Animación de caminar
        animator.SetBool("walk", horizontal != 0 && isGrounded && !isAttacking);

        // Animación de salto (cuando no está en el suelo)
        animator.SetBool("jump", !isGrounded && !isAttacking);
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

    // Método llamado desde el Animator para finalizar el ataque
    public void EndAttack()
    {
        isAttacking = false;
    }
}