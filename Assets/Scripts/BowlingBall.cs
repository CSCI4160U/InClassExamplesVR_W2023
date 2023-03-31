using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BowlingBall : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BoxCollider outOfBounds;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other) {
        transform.position = spawnPoint.position;
        rb.velocity = Vector3.zero;
    }
}
