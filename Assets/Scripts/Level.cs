using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    //If there's only going to be a Level asset, make instance
    public static Level instance;
    //Counts how many destructable objects are left
    uint numDestructables = 0;
    bool startNextLevel = false;
    float nextLevelTimer = 4;

    string[] levels = { "Tutorial", "Level 1" };
    int currentLevel = 1;

    public int score = 0;
    Text scoreText;

    private void Awake()
    {
        //No un-needed instances will be created, will only have the original one i.e. only 1 ship on level load
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (startNextLevel)
        {
            if (nextLevelTimer <= 0)
            {
                currentLevel++;
                if (currentLevel <= levels.Length)
                {
                    string sceneName = levels[currentLevel - 1];
                    //Will load scene in background while current one is playing, for seamless transition
                    SceneManager.LoadSceneAsync(sceneName);
                }
                else
                {
                    Debug.Log("Game Over!");
                }
                nextLevelTimer = 4;
                startNextLevel = false;
            }
            else
            {
                nextLevelTimer -= Time.deltaTime;
            }
        }

    }

    public void ResetLevel() 
    {
        foreach(Bullet b in GameObject.FindObjectsOfType<Bullet>())
        {
            Destroy(b.gameObject);
        }
        numDestructables = 0;
        score = 0;
        AddScore(score);
        string sceneName = levels[currentLevel - 1];
        SceneManager.LoadScene(sceneName);
    }

    public void AddScore(int amountToAdd)
    {
        score += amountToAdd;
        scoreText.text = score.ToString();
    }

    public void AddDestructable()
    {
        numDestructables++;
    }

    public void RemoveDestructable()
    {
        numDestructables--;

        if (numDestructables == 0)
        {
            startNextLevel = true;
        }
    }
}
