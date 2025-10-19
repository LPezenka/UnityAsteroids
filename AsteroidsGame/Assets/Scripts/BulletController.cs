using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed = 5.0f;
    public Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        transform.position += (Vector3)(direction * speed * dt);
    }
}
