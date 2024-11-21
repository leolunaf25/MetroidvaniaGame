using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float followRange = 10f; // Distancia para comenzar a seguir al jugador
    public float attackRange = 2f; // Distancia para atacar al jugador
    public float speed = 2f; // Velocidad del jefe
    public float attackCooldown = 1.5f; // Tiempo entre ataques

    private Animator animator;
    private float lastAttackTime = 0f; // Tiempo del último ataque
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calcular la distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isAttacking)
            return; // Si está atacando, no puede moverse

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= followRange)
        {
            FollowPlayer();
        }
        else
        {
            Idle();
        }
    }

    void FollowPlayer()
    {
        animator.SetBool("walk", true); // Activar animación de caminar
        animator.SetBool("idle", false);

        // Mover al jefe hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Cambiar la dirección del sprite si es necesario
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;
            animator.SetTrigger("blaster"); // Activar animación de ataque
            lastAttackTime = Time.time;

            // Después de la animación, permitir otros estados
            Invoke("FinishAttack", 0.5f); // Ajusta el tiempo al final de la animación
        }
    }

    void Idle()
    {
        animator.SetBool("walk", false);
        animator.SetBool("idle", true);
    }

    void FinishAttack()
    {
        isAttacking = false;
    }
}
