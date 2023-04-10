using System.Collections;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    [SerializeField] private Transform attachPoint;
    [SerializeField] private GameObject baseballPrefab;
    [SerializeField] private Transform pitchingTarget;
    [SerializeField] private float throwingForce = 200f;
    [SerializeField] private float throwDelay = 5f;

    private Animator animator;
    private GameObject grabbedBaseball = null;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void PickUpBall() {
        grabbedBaseball = Instantiate(baseballPrefab, attachPoint);
        grabbedBaseball.transform.position = attachPoint.position;
        var ballRB = grabbedBaseball.GetComponent<Rigidbody>();
        ballRB.isKinematic = true;
    }

    public void Release() {
        grabbedBaseball.transform.SetParent(null);
        Rigidbody ballRB = grabbedBaseball.GetComponent<Rigidbody>();
        ballRB.isKinematic = false;

        Vector3 direction = (pitchingTarget.position - grabbedBaseball.transform.position).normalized;
        ballRB.AddForce(throwingForce * direction, ForceMode.Force);

        StartCoroutine(WaitThenThrow(throwDelay));
    }

    public IEnumerator WaitThenThrow(float delay) {
        float timePassed = 0f;

        do {
            timePassed += Time.deltaTime;
            yield return null;
        } while (timePassed < delay);

        animator.SetTrigger("Throw");

        yield return null;
    }
}
