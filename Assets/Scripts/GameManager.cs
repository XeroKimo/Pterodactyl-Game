using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HUDScreen hud;
    public Player player;
    public Dinosaur dinosaur;
    public BackgroundScroll background;
    public float scoreUpPerTime = 3.0f / 60.0f;
    public Vector2 debrisSpawnTimeDelayRange;

    public List<Debris> debrisPrefabs;
    private List<Debris> managedDebris = new List<Debris>();
    private List<Debris> debrisToDelete = new List<Debris>();

    private float nextDebrisSpawnTime;
    private float debrisSpawnTimer = 0;
    private float scoreUpTimer = 0;
    private int score;
    private int? highscore;

    bool Playing() => !hud.retryButton.gameObject.activeSelf && !hud.pressStartText.gameObject.activeSelf;

    // Start is called before the first frame update
    void Start()
    {
        hud.retryButton.gameObject.SetActive(false);
        hud.pressStartText.gameObject.SetActive(true);
        nextDebrisSpawnTime = Random.Range(debrisSpawnTimeDelayRange.x, debrisSpawnTimeDelayRange.y);
        background.enabled = false;
        dinosaur.onDebrisOverlapped += (collider, debris) =>
        {
            debrisToDelete.Add(debris);
        };
        dinosaur.onPlayerOverlapped += (collider) =>
        {
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
                Restart();
            }
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
            debris.rigidBody.MovePosition(debris.rigidBody.position + (Vector2.right * 3 * Time.deltaTime));
        }

        foreach (Debris debris in debrisToDelete)
        {
            managedDebris.Remove(debris);
            Destroy(debris.gameObject);
        }

        if(player.nextFrameMove.y != 0)
        {
            RaycastHit2D[] results = new RaycastHit2D[1];
            if(player.collider.Cast(new Vector2(0, player.nextFrameMove.y), results, 1) > 0)
            {
                if (results[0].normal.y != 0)
                    player.nextFrameMove.y = 0;
            }
        }

        debrisToDelete.Clear();

        debrisSpawnTimer += Time.fixedDeltaTime;
        if(debrisSpawnTimer > nextDebrisSpawnTime)
        {
            Debris newDebris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Count)], new Vector3(-15, Random.Range(-1, 2), 0), Quaternion.identity);
            managedDebris.Add(newDebris);
            nextDebrisSpawnTime = Random.Range(debrisSpawnTimeDelayRange.x, debrisSpawnTimeDelayRange.y);
            debrisSpawnTimer = 0;
        }
        scoreUpTimer += Time.fixedDeltaTime;

        if(scoreUpTimer >= scoreUpPerTime)
        {
            scoreUpTimer -= scoreUpPerTime;
            score += 1;
        }
        hud.SetScoreText(score, highscore);
    }

    private void GameOver()
    {
        hud.retryButton.gameObject.SetActive(true);
        background.enabled = false;
    }

    private void Restart()
    {
        background.enabled = true;
        hud.retryButton.gameObject.SetActive(false);
        hud.pressStartText.gameObject.SetActive(false);

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
        player.transform.position = new Vector2(-3, 0);

        score = 0;
    }
}
