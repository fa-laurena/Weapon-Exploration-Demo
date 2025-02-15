using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ShootingGame : MonoBehaviour
{
    public GameObject[] targetPrefabs;
    public Transform spawnArea;
    public TextMeshProUGUI scoreTextMesh; // Use TextMeshProUGUI instead of Text
    public TextMeshProUGUI highScoreTextMesh; // Use TextMeshProUGUI instead of Text
    public Magazine magazine;
    public GameObject totalScoreSceneLoaderPrefab;

    private int score;
    private int highScore;
    private int bulletsLeft;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();

        bulletsLeft = magazine.numberOfBullet;

        StartCoroutine(SpawnTargets());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && bulletsLeft > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the hit object has a specific tag
            if (hitObject.CompareTag("RedTarget"))
            {
                HandleTargetHit(10);
            }
            else if (hitObject.CompareTag("YellowTarget"))
            {
                HandleTargetHit(3);
            }
            else if (hitObject.CompareTag("GreenTarget"))
            {
                HandleTargetHit(1);
            }
        }
    }

    IEnumerator SpawnTargets()
    {
        while (true)
        {
            GameObject targetPrefab = targetPrefabs[Random.Range(0, targetPrefabs.Length)];
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                spawnArea.position.y,
                Random.Range(spawnArea.position.z - spawnArea.localScale.z / 2, spawnArea.position.z + spawnArea.localScale.z / 2)
            );

            Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    private void HandleTargetHit(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScoreText();

        bulletsLeft--;
        // You can update the bullets UI element here if you have one
    }

    public void UpdateScoreText()
    {
        scoreTextMesh.text = "Score: " + score; // Update TextMeshProUGUI text
    }

    public void UpdateHighScoreText()
    {
        highScoreTextMesh.text = "High Score: " + highScore; // Update TextMeshProUGUI text
    }

    public void EndGame()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }

        LastScore.lastScore = score;
        Instantiate(totalScoreSceneLoaderPrefab);
    }
}
