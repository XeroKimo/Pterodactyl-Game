using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroSceneAnimation : MonoBehaviour
{
    public GameObject dinoPrefab;
    public GameObject playerPrefab;
    private Animator basicDinoAnimator;
    private Animator introPlayerAnimator;
    public Animator wifiAnimator;

    private float timeKeeper = 0.0f;

    private bool startScene = true;
    public float walkSpeed;

    public float StartTransform_Time;
    public float EndTransform_Time;
    public float MoveInPlayer_Time;

    public Canvas InternetConnected;
    // Start is called before the first frame update
    void Start()
    {
        basicDinoAnimator = dinoPrefab.GetComponent<Animator>();
        introPlayerAnimator = playerPrefab.GetComponent<Animator>();
    }

    void TransformDino(){

    }

    void MoveInPlayer(){

    }

    void EndScene()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(startScene)
        {
            timeKeeper += Time.fixedDeltaTime;
            Debug.Log(timeKeeper);
            if(StartTransform_Time>timeKeeper)
            {
                dinoPrefab.transform.Translate(Vector3.right * Time.deltaTime * walkSpeed);
            } else {
                if(EndTransform_Time>timeKeeper)
                {
                    basicDinoAnimator.SetTrigger("Transform");
                    InternetConnected.enabled = true;
                    wifiAnimator.SetTrigger("Enabled");
                } else {
                    
                }
                
            }
        }
        
    }
}
