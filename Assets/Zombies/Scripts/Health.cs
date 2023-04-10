using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int hp = 100;

    [SerializeField] private bool isDead = false;

    public bool IsDead {
        get { return isDead; }
    }

    private Animator animator;
    private NavMeshAgent agent;

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount) {
        this.hp -= damageAmount;

        if (animator != null) {
            animator.SetTrigger("Hit"); 
        }

        if (this.hp <= 0 ) {
            this.hp = 0;
            this.isDead = true;

            if (animator != null ) {
                animator.SetInteger("DeathNum", Random.Range(1, 2));
                animator.SetBool("Dead", true);
            }

            if (agent != null ) {
                agent.enabled = false;
            }
        }
    }
}
