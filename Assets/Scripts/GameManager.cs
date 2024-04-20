using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HUDScreen hud;
    public Player player;
    public Dinosaur dinosaur;
    public float scoreUpPerTime = 3.0f / 60.0f;

    public List<Debris> debrisPrefabs;
    private List<Debris> managedDebris = new List<Debris>();
    private List<Debris> debrisToDelete = new List<Debris>();

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
        dinosaur.onDebrisOverlapped += (collider, debris) =>
        {
            debrisToDelete.Add(debris);
        };
        dinosaur.onPlayerOverlapped += (collider) =>
        {
            hud.retryButton.gameObject.SetActive(true);
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
            Vector3 playerPosition = player.transform.position;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.nextFrameMove += Vector2.up;
                //playerPosition += Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.nextFrameMove -= Vector2.up;
                //playerPosition -= Vector3.up;
            }
            //playerPosition.y = Mathf.Clamp(playerPosition.y, -1, 1);
            //player.transform.position = playerPosition;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!Playing())
            return;

        foreach(Debris debris in managedDebris)
        {
            debris.rb.MovePosition(debris.rb.position + (Vector2.right * 3 * Time.deltaTime));
        }

        foreach (Debris debris in debrisToDelete)
        {
            managedDebris.Remove(debris);
            Destroy(debris.gameObject);
        }
        debrisToDelete.Clear();

        debrisSpawnTimer += Time.fixedDeltaTime;
        if(debrisSpawnTimer > 1)
        {
            Debris newDebris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Count)], new Vector3(-15, Random.Range(-1, 2), 0), Quaternion.identity);
            managedDebris.Add(newDebris);
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

    private void Restart()
    {
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
