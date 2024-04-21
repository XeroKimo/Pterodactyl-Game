using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Dinosaur : MonoBehaviour
{
    public Action<Collider2D, Debris> onDebrisOverlapped;
    public Action<Collider2D> onPlayerOverlapped;
    public AudioSource suckAudio;

    public Animator animator;
    private AnimatorController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Restart()
    {
        animator.speed = 1;
        animator.SetTrigger("Restart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End()
    {
        animator.speed = 1;
        animator.SetTrigger("End");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debris debris = collision.gameObject.GetComponent<Debris>();
        suckAudio.Play();
        if (debris)
        {
            onDebrisOverlapped?.Invoke(collision, debris);
        }
        else if(collision.gameObject.GetComponent<Player>())
        {
            onPlayerOverlapped?.Invoke(collision);
        }
    }
}
