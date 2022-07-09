using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Will move bullet to right
    public Vector2 direction = new Vector2 (1,0);
    public float speed = 2;

    public GameObject bullet;
    public GameObject hitEffect;
    public Vector2 velocity;

    public bool isEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        //Will destroy game object after 3 seconds
        Destroy(gameObject, 3);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Change speed and direction whenever we want
        velocity = direction * speed;
    }

    public void FixedUpdate()
    {
        Vector2 pos = transform.position;

        pos += velocity * Time.fixedDeltaTime;

        transform.position = pos;
    }
}
