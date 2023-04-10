using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    [SerializeField] private float groundThreshold = 0.2f;

    [SerializeField] private bool showTrail = true;
    [SerializeField] private GameObject ballImagePrefab;
    [SerializeField] private float ballImageInterval = 0.1f;

    private Vector3 originalPosition;
    private Vector3 hitPosition;
    private Vector3 groundHitPosition = Vector3.zero;
    private bool isGrounded = false;

    private List<GameObject> ballImages;
    private float lastImageTime;

    private void Start() {
        originalPosition = transform.position;

        ballImages = new List<GameObject>();
        lastImageTime = Time.time;
    }

    private void Update() {
        // draw a ball image, if necessary
        if (!isGrounded && showTrail && (Time.time > (lastImageTime + ballImageInterval))) {
            var ballImage = Instantiate(ballImagePrefab, transform.position, transform.rotation);
            ballImages.Add(ballImage);
            lastImageTime = Time.time;
        }    

        // determine if the ball has hit the ground
        if (groundHitPosition != Vector3.zero && transform.position.y <= groundThreshold) {
            groundHitPosition = transform.position;
            isGrounded = true;
            
            float distanceHit = (groundHitPosition - originalPosition).magnitude;
            Debug.Log("Hit the ball " + distanceHit + "m.");
        }
    }
}
