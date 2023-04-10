using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave {
        [SerializeField] private int count = 1;
        [SerializeField] public GameObject zombiePrefab;
        [SerializeField] public Transform spawnLocation;

        private List<GameObject> enemies;

        public void StartSpawn() {
            enemies = new List<GameObject>();
        }

        public bool SpawnNext(Transform target) {
            if (enemies.Count < count) {
                // spawn a new zombie
                var newZombie = Instantiate(zombiePrefab, spawnLocation.position, Quaternion.identity);
                enemies.Add(newZombie);
                TrackPlayer trackPlayer = newZombie.GetComponent<TrackPlayer>();
                trackPlayer.target = target;
                return true;
            } else {
                // no more to spawn
                return false;
            }
        }

        public bool IsDefeated() {
            if (count > enemies.Count) {
                return false;
            }

            for (int i = 0; i < enemies.Count; i++) {
                Health enemyHealth = enemies[i].GetComponent<Health>();
                if (!enemyHealth.IsDead) {
                    return false;
                }
            }
            return true;
        }
    }

    [Header("Wave Data")]
    [SerializeField] private Wave[] waves = null;
    [SerializeField] private int waveIndex = 0;
    [SerializeField] private float lastWaveSpawnTime = 0f;
    [SerializeField] private Transform target;
    [SerializeField] private float delayBetweenSpawns = 1f;

    [Header("Between Waves")]
    [SerializeField] private float delayBetweenWaves = 5f;
    [SerializeField] private bool betweenWaves = true;

    [Header("User Interface")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private string nextWaveMessage = "Prepare for the next wave!";
    [SerializeField] private string competedMessage = "Congratulations, you have survived!";

    private void Start() {
        lastWaveSpawnTime = Time.time;
        messageText.text = nextWaveMessage;
        countdownText.text = Mathf.RoundToInt(delayBetweenWaves).ToString();
    }

    private void Update() {
        if (betweenWaves) {
            // update the countdown
            float secondsRemaining = (lastWaveSpawnTime + delayBetweenWaves) - Time.time;
            countdownText.text = Mathf.RoundToInt(secondsRemaining).ToString();

            if ((lastWaveSpawnTime + delayBetweenWaves) < Time.time) {
                betweenWaves = false;
                lastWaveSpawnTime = Time.time;
                messageText.text = "";
                countdownText.text = "";

                // spawn the next wave
                waves[waveIndex].StartSpawn();
                StartCoroutine(SpawnZombie(target));
            }
        }

        if (!betweenWaves && waves[waveIndex].IsDefeated()) {
            // start the between-wave delay
            betweenWaves = true;
            lastWaveSpawnTime = Time.time;
            waveIndex++;  // TODO: Handle what to do on the last wave

            if (waveIndex < waves.Length) {
                messageText.text = nextWaveMessage;
                countdownText.text = Mathf.RoundToInt(delayBetweenWaves).ToString();
            } else {
                messageText.text = competedMessage;
            }
        }
    }

    public IEnumerator SpawnZombie(Transform target) {
        yield return new WaitForSeconds(delayBetweenSpawns);
        bool spawnAgain = waves[waveIndex].SpawnNext(target);

        if (spawnAgain) {
            StartCoroutine(SpawnZombie(target));
        }
    }
}
