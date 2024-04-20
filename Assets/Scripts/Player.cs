using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public new BoxCollider2D collider;
    [HideInInspector]
    public float basePushbackSpeed;

    [HideInInspector]
    public Vector2 nextFrameMove = Vector2.zero;

    public AudioSource audioSource;
    public AudioClip flapClip;
    public AudioClip hitClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 nextPosition = rigidBody.position + nextFrameMove;
        nextPosition.y = Mathf.Clamp(nextPosition.y, -1, 1);
        if(nextPosition.y != rigidBody.position.y)
        {
            audioSource.clip = flapClip;
            audioSource.Play();
        }
        rigidBody.MovePosition(nextPosition);
        nextFrameMove = Vector2.zero;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        audioSource.clip = hitClip;
        if(!audioSource.isPlaying)
            audioSource.Play();
        nextFrameMove += (Vector2.right * basePushbackSpeed * GameManager.globalSpeedMultiplier * Time.deltaTime);
    }
}
