using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    bool canBeDestroyed = false;
    public int scoreValue = 100;

    public int health;
    public GameObject Effect;
    // Start is called before the first frame update
    void Start()
    {
        Level.instance.AddDestructable();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -2)
        {
            Level.instance.RemoveDestructable();
            Destroy(gameObject);
        }
        // Boundaries for when game object can be destroyed (Not killed outside camera view)
        if (transform.position.x < 17.8f && !canBeDestroyed)
        {
            canBeDestroyed = true;
            Gun[] guns = transform.GetComponentsInChildren<Gun>();
            foreach (Gun gun in guns)
            {
                gun.isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeDestroyed)
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (!bullet.isEnemy)
            {
              Level.instance.AddScore(scoreValue);
              Level.instance.RemoveDestructable();
              Destroy(gameObject);
              Destroy(bullet.gameObject);     
            }
        }
    }
}
