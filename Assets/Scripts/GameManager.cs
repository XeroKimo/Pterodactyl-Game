using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HUDScreen hud;
    public Player player;
    public Dinosaur dinosaur;
    public BackgroundScroll background;
    public float scoreUpPerTime = 3.0f / 60.0f;
    public Vector2 debrisSpawnTimeDelayRange;
    public Vector2 doubleDebrisSpawnTimeDelayRange = new Vector2(0.2f, 0.5f);
    public float baseDebrisSpeed = 8;
    public float maxGlobalSpeedMultiplier = 2.5f;

    public List<Debris> debrisPrefabs;

    public AudioSource TheDinoSong;

    private List<Debris> managedDebris = new List<Debris>();
    private List<Debris> debrisToDelete = new List<Debris>();

    private float nextDebrisSpawnTime;
    private float debrisSpawnTimer = 0;
    private float nextDoubleDebrisSpawnTime;
    private float doubleDebrisTimer = 0;
    private int? doubleDebrisLane;
    private float scoreUpTimer = 0;
    private float restartTimer = 0;
    private int score;
    private int? highscore;

    private bool firstStart = true;

    bool Playing() => !hud.retryButton.gameObject.activeSelf && !hud.pressStartText.gameObject.activeSelf;

    public static float globalSpeedMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        hud.retryButton.gameObject.SetActive(false);
        hud.pressStartText.gameObject.SetActive(true);
        nextDebrisSpawnTime = Random.Range(debrisSpawnTimeDelayRange.x, debrisSpawnTimeDelayRange.y);
        background.enabled = false;
        player.basePushbackSpeed = baseDebrisSpeed;
        dinosaur.onDebrisOverlapped += (collider, debris) =>
        {
            debrisToDelete.Add(debris);
        };
        dinosaur.onPlayerOverlapped += (collider) =>
        {
            //play animation
            dinosaur.End();
            player.End();
            GameOver();
        };

        hud.retryButton.onClick.AddListener(() =>
        {
            Restart();
        });
    }

    private void Update()
    {
        if(!Playing())
        {
            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                UnityEngine.Debug.Log("WHATTTT!");
                if(restartTimer >= 30 && firstStart == false)
                {
                    Restart();
                } else if(firstStart==true) {
                    firstStart = false;
                    FirstStart();
                }
            }
            //Debug.Log(restartTimer);
            restartTimer +=  Time.fixedDeltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.nextFrameMove += Vector2.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.nextFrameMove -= Vector2.up;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!Playing())
            return;

        foreach(Debris debris in managedDebris)
        {
            debris.rigidBody.MovePosition(debris.rigidBody.position + (Vector2.right * baseDebrisSpeed * globalSpeedMultiplier * Time.deltaTime));
        }

        foreach (Debris debris in debrisToDelete)
        {
            managedDebris.Remove(debris);
            Destroy(debris.gameObject);
        }

        debrisToDelete.Clear();

        debrisSpawnTimer += Time.fixedDeltaTime;
        if(debrisSpawnTimer > nextDebrisSpawnTime)
        {
            int selectedLane = Random.Range(-1, 2);
            Debris newDebris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Count)], new Vector3(-15, selectedLane, 0), Quaternion.identity);
            managedDebris.Add(newDebris);
            nextDebrisSpawnTime = Random.Range(debrisSpawnTimeDelayRange.x, debrisSpawnTimeDelayRange.y);
            debrisSpawnTimer = 0;

            if(Random.value <= 0.33f)
            {
                doubleDebrisLane = selectedLane;
                while (doubleDebrisLane.Value == selectedLane)
                    doubleDebrisLane = Random.Range(-1, 2);

                nextDoubleDebrisSpawnTime = Random.Range(doubleDebrisSpawnTimeDelayRange.x, doubleDebrisSpawnTimeDelayRange.y);
                doubleDebrisTimer = 0;
            }
        }

        if(doubleDebrisLane.HasValue)
        {
            doubleDebrisTimer += Time.fixedDeltaTime;
            if (doubleDebrisTimer > nextDoubleDebrisSpawnTime)
            {
                Debris newDebris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Count)], new Vector3(-15, doubleDebrisLane.Value, 0), Quaternion.identity);
                managedDebris.Add(newDebris);
                doubleDebrisLane = null;
            }
        }

        scoreUpTimer += Time.fixedDeltaTime;

        if(scoreUpTimer >= scoreUpPerTime)
        {
            scoreUpTimer -= scoreUpPerTime;
            score += 1;
            if(score % 150 == 0)
            {
                globalSpeedMultiplier *= 1.1f;
                globalSpeedMultiplier = Mathf.Min(globalSpeedMultiplier, maxGlobalSpeedMultiplier);
                dinosaur.animator.speed = globalSpeedMultiplier;
                player.animator.speed = globalSpeedMultiplier;
                TheDinoSong.pitch = globalSpeedMultiplier;
            }
        }
        hud.SetScoreText(score, highscore);
    }

    private void GameOver()
    {
        hud.retryButton.gameObject.SetActive(true);
        background.enabled = false;
    }

    private void FirstStart()
    {
        background.enabled = true;
        hud.retryButton.gameObject.SetActive(false);
        hud.pressStartText.gameObject.SetActive(false);
        globalSpeedMultiplier = 1;
        score = 0;
    }

    private void Restart()
    {
        restartTimer = 0;
        //Debug.Log("Restarting");
        background.enabled = true;
        hud.retryButton.gameObject.SetActive(false);
        hud.pressStartText.gameObject.SetActive(false);
        TheDinoSong.pitch = 1;

        foreach(Debris debris in managedDebris)
        {
            Destroy(debris.gameObject);
        }
        managedDebris.Clear();
        if(highscore.HasValue && highscore.Value < score)
        {
            highscore = score;
        }
        else if(score > 0)
        {
            highscore = score;
        }

        dinosaur.Restart();
        player.Restart();
        
        globalSpeedMultiplier = 1;
        score = 0;
    }
}
