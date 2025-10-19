using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class GameController : MonoBehaviour
{
    public Camera cam;
    public List<GameObject> asteroidPrefabs;
    public GameObject explosionPrefab;
    public GameObject laserPrefab;
    public MainMenuController mainMenu;

    public List<GameObject> asteroidsInGame = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject player;
    private Vector3 initialAsteroidScale;
    public AudioClip explosionSFX;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        mainMenu.Show();
    }

    public void StartGame()
    {
        initialAsteroidScale = asteroidPrefabs[0].transform.localScale;
        // Clear Asteroids
        foreach (var go in asteroidsInGame)
        {
            Destroy(go);
        }

        mainMenu.Hide();
        SpawnPlayer(Vector2.zero);
        SpawnAsteroid(new Vector2(-8.0f, 8.0f));
        SpawnAsteroid(new Vector2(8.0f, 8.0f));
        SpawnAsteroid(new Vector2(8.0f, -8.0f));
        SpawnAsteroid(new Vector2(-8.0f, -8.0f));
    }


    public void SpawnLaserBeam(Transform tf, Vector2 _direction)
    {
        var lb = Instantiate(laserPrefab, tf.position + tf.up * 0.5f, tf.rotation);
        lb.GetComponent<BulletController>().direction = _direction;
    }

    public void SpawnPlayer(Vector2 spawnPosition)
    {
        // find object with the tag player and destroy it
        if (player != null)
        {
            Destroy(player);
        }
        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.GetComponent<ShipController>().gameController = this;
        player.GetComponent<ShipController>().camera = cam;
    }

    public void SpawnAsteroidWave(Vector2 spawnPosition, int generation = 1)
    {
        if (generation > 3) return;
        for (int i = 0; i < generation; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            SpawnAsteroid(spawnPosition + offset, generation);
        }
    }

    public void SpawnAsteroid(Vector2 spawnPosition, int generation = 1)
    {
        var asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], spawnPosition, Quaternion.identity);
        var asteroidController = asteroid.GetComponent<AsteroidController>();
        asteroidController.cam = cam;
        asteroidController.generation = generation;
        asteroidController.hitPoints = (10 - generation);
        asteroidController.gameController = this.gameObject;

        Vector3 currentScale = initialAsteroidScale;
        if (generation != 1)
            for (int i = 0; i < generation; i++)
            {
                currentScale *= 0.85f;// ((float)(generation -1.0f) / (generation));
            }

        asteroid.transform.localScale = currentScale;

        asteroidsInGame.Add(asteroid);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerExplosion(Transform tf)
    {
        var ex = Instantiate(explosionPrefab, tf.position, Quaternion.identity);
        Destroy(ex.gameObject, 2.0f);
        audioSource.PlayOneShot(explosionSFX);
        if (asteroidsInGame.Count == 0)
            mainMenu.Show();
    }

    public void KillPlayer(Vector2 Position)
    {
        var ex = Instantiate(explosionPrefab, Position, Quaternion.identity);
        Destroy(ex.gameObject, 2.0f);
        audioSource.PlayOneShot(explosionSFX);
        mainMenu.Show();
    }

}
