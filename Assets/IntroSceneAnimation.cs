using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float DinoMove_Time;
    public float StartTransform_Time;
    public float EndTransform_Time;
    public float MoveInPlayer_Time;
    public float StopMoveInPlayer_Time;

    public float Wait_Time;

    public GameObject InternetConnected;
    public GameObject TitleScreen;

    public GameObject DinoWalking;
    public AudioSource Powerup;
    public AudioSource Sucking;
    // Start is called before the first frame update
    void Start()
    {
        basicDinoAnimator = dinoPrefab.GetComponent<Animator>();
        introPlayerAnimator = playerPrefab.GetComponent<Animator>();
        //Debug.Log(StartTransform_Time);
        //Debug.Log(EndTransform_Time);
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
            //Debug.Log(timeKeeper);
            if(DinoMove_Time>timeKeeper)
            {
                dinoPrefab.transform.Translate(Vector3.right * Time.deltaTime * walkSpeed);
            } else if (StartTransform_Time>timeKeeper)
            {
                DinoWalking.SetActive(false);
                wifiAnimator.SetTrigger("Enabled");
                basicDinoAnimator.SetTrigger("Transform");
                InternetConnected.gameObject.SetActive(true);
                Powerup.PlayDelayed(0.3f);
                Sucking.PlayDelayed(0.6f);
            } 
            if(MoveInPlayer_Time < timeKeeper && StopMoveInPlayer_Time > timeKeeper) {
                //Sucking.Play();
                InternetConnected.gameObject.SetActive(false);
                playerPrefab.transform.Translate(Vector3.right * Time.deltaTime * 5.0f);
                TitleScreen.gameObject.SetActive(true);
            }
            if(Wait_Time<timeKeeper) {
                Debug.Log("Changing Scenes");
                startScene = false;
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }
}
