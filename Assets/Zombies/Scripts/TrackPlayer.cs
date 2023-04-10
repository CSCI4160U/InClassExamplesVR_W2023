using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrackPlayer : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private float closeEnoughDistance = 1f;
    [SerializeField] private float attackCooldown = 2f;
    
    private Animator animator;
    private NavMeshAgent agent;

    private float lastAttackTime;

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent != null) {
            if (Vector3.Distance(target.position, transform.position) < closeEnoughDistance) {
                if (Time.time > (lastAttackTime + attackCooldown)) {
                    // attack
                    if (animator != null) {
                        animator.SetInteger("AttackNum", Random.Range(1, 6));
                        animator.SetTrigger("Attack");
                    }
                    lastAttackTime = Time.time;
                }
            } else {
                agent.SetDestination(target.position);

                if (animator != null) {
                    animator.SetFloat("Forward", agent.velocity.magnitude);
                }
            }
        }
    }
}
