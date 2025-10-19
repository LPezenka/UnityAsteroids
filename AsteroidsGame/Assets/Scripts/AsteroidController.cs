using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public int hitPoints;
    public Vector2 startPosition;
    public Camera cam;
    public Rigidbody2D rb;
    public GameObject gameController;
    public int generation = 1;
    
    private GameController gc;


    private void OnEnable()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = gameController.GetComponent<GameController>();
        //transform.position = startPosition;
        int xDir = Random.Range(0, 101);
        int yDir = Random.Range(0, 101);
        direction = new Vector2(xDir, yDir);
        direction.Normalize();
        if (rb)
        {
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
            //rb.linearVelocity = direction * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += (Vector3)(direction * speed * Time.deltaTime);
        CorrectCameraPosition();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hitPoints--;
            if (hitPoints <= 0)
            {
                int generation = this.generation;
                Destroy(gameObject);
                gc.TriggerExplosion(collision.transform);
                gc.SpawnAsteroidWave(transform.position, generation + 1);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            
            gameController.GetComponent<GameController>().KillPlayer(collision.transform.position);
            Destroy(collision.gameObject);
        }
    }

    void CorrectCameraPosition()
    {
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        if (transform.position.x < min.x)
        {
            transform.position = new Vector3(max.x, transform.position.y, transform.position.z);
        }

        if (transform.position.x > max.x)
        {
            transform.position = new Vector3(min.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < min.y)
        {
            transform.position = new Vector3(transform.position.x, max.y, transform.position.z);
        }

        if (transform.position.y > max.y)
        {
            transform.position = new Vector3(transform.position.x, min.y, transform.position.z);
        }
    }
}
