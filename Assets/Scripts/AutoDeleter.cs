using UnityEngine;

public class AutoDeleter : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private float startTime;

    private void Awake() {
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > (startTime + lifetime)) {
            Destroy(gameObject, 0.1f);
        }
    }
}
