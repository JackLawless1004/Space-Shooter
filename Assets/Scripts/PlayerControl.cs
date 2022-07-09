using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    Vector2 startingPosition;
    //Array to hold any gun attached to ship
    Gun[] guns;

    float moveSpeed = 8;

    int hits = 4;
    bool invincible = false;
    float invincibeTimer = 0;
    float invincibleTime = 2;

    bool moveUp;
    bool moveDown;
    bool moveLeft;
    bool moveRight;
    bool timeSlow;

    bool shoot;

    //Flickering ship during invinsibility
    SpriteRenderer spriteRenderer;

    GameObject shield;
    int gunLevel = 0;

    private void Awake()
    {
        startingPosition = transform.position;
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // shield is a child of ship
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        //On ship spawn, let's ship find all gun scripts in gun child objects
        guns = transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            gun.isActive = true;
            if (gun.gunLevelRequirment != 0)
            {
                gun.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        timeSlow = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


        if (shoot)
        {
            //False so player has to press key again to shoot, True for hold down shoot forever until let go
            shoot = false;
            //for each gun found in guns, shoot
            foreach(Gun gun in guns)
            {
                if (gun.gameObject.activeSelf)
                {
                    gun.Shoot();

                }
            }
        }
        // Invinsible block after player gets hit
        if (invincible)
        {
            if (invincibeTimer >= invincibleTime)
            {
                invincibeTimer = 0;
                invincible = false;
                spriteRenderer.enabled = true;
            }
            else
            {
                invincibeTimer += Time.deltaTime;
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        // Move potential based on move speed; How far we move this frame
        float moveAmount = moveSpeed * Time.fixedDeltaTime;

        if (timeSlow)
        {
            Time.timeScale = 0.15f;
        }
        else
        {
            Time.timeScale = 1;
        }

        //Based on User Input
        Vector2 move = Vector2.zero;

        if (moveUp)
        {
            move.y += moveAmount;
        }
        if (moveDown)
        {
            move.y -= moveAmount;
        }
        if (moveLeft)
        {
            move.x -= moveAmount;
        }
        if (moveRight)
        {
            move.x += moveAmount;
        }
        // Used to counteract the increased movement gained when going diagonal since diagonal is farther movement than x or y
        float moveMagnitude = Mathf.Sqrt(move.x * move.x + move.y * move.y);
        if (moveMagnitude > moveAmount)
        {
            float ratio = moveAmount / moveMagnitude;
            move *= ratio;
        }
        pos += move;
        // Keep Player in screen boundaries
        if (pos.x <= 1.3f)
        {
            pos.x = 1.3f;
        }
        if (pos.x >= 17.75f)
        {
            pos.x = 17.75f;
        }
        if (pos.y <= 0.25f)
        {
            pos.y = 0.25f;
        }
        if (pos.y >= 9.3f)
        {
            pos.y = 9.3f;
        }
        transform.position = pos;
    }

    void ActivateShield()
    {
        shield.SetActive(true);
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
    }


    //See whether or not ship has shield
    bool HasShield()
    {
        return shield.activeSelf;
    }

    void AddGuns()
    {
        gunLevel++;
        foreach (Gun gun in guns)
        {
            if (gun.gunLevelRequirment <= gunLevel)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                // Deactivates all gun if they are not at this lvl e.g. ship dies
                gun.gameObject.SetActive(false);
            }
        }
    }

    void SpeedIncrease()
    {
        moveSpeed++;
    }

    void ResetShip()
    {
        transform.position = startingPosition;
        DeactivateShield();
        gunLevel = -1;
        AddGuns();
        moveSpeed = 8;
        hits = 4;
        Level.instance.ResetLevel();
    }

    // Hit/Dmg detection and decrease health + activates invincible code
    void Hit(GameObject gameObjectHit)
    {
        if (HasShield())
        {
            DeactivateShield();
        }
        else
        {
            if (!invincible)
            {
                hits--;
                if (hits == 0)
                {
                    ResetShip();
                }
                else
                {
                    invincible = true;
                }
                Destroy(gameObjectHit);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ship dies to enemy bullets
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (bullet.isEnemy)
            {
                Hit(bullet.gameObject);
            }
        }
        //Ship dies when running into enemies
        Destructable destructable = collision.GetComponent<Destructable>();
        if (destructable != null)
        {
            
            Hit(destructable.gameObject);
        }

        PowerUps powerUp = collision.GetComponent<PowerUps>();
        if (powerUp)
        {
            if (powerUp.activateShield)
            {
                ActivateShield();
            }
            if (powerUp.addGuns)
            {
                AddGuns();
            }
            if (powerUp.speedIncrease)
            {
                SpeedIncrease();
            }
            Level.instance.AddScore(powerUp.pointValue);
            Destroy(powerUp.gameObject);
        }

    }
}
