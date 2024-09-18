using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int startIndex = 0;
    public bool fight;
    public int P1Score = 3;
    public int P2Score = 3;
    public bool firstPress;
    public bool timerEnd;
    public bool roundOver;
    float targetTime;
    PlayerMovement playerOne, playerTwo;
    public KeyCode[] P1Keys = new KeyCode[6];
    public KeyCode[] P2Keys = new KeyCode[6];
    public KeyCode firstKey, secondKey;
    [SerializeField] GameObject text1, text2;
    [SerializeField] Vector3 offset = new Vector3(0f, -1.0f, 0f);
    public TextMeshProUGUI P1Text, P2Text;
    [SerializeField] Image[] lives = new Image[6];
    Color[] a = new Color[3];
    Color[] b = new Color[3];
    bool checkOne, checkTwo;
    AudioSource aud;
    [SerializeField] AudioClip oneBell, threeBells, punch;
    [SerializeField] GameObject restart; 
    void Start()
    {
        playerOne = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerTwo = GameObject.Find("Player2").GetComponent<PlayerMovement>();

        aud = GetComponent<AudioSource>();

        for(int i = 0; i < 3; i++){
            a[i] = lives[0].color;
            b[i] = lives[3].color;
        }

        targetTime = Random.Range(3.0f, 5.0f);

        restart.SetActive(false);
    }

    void Update()
    {
        SetFadeIn();

        text1.transform.position = playerOne.transform.position + offset;
        text2.transform.position = playerTwo.transform.position + offset;

        ScoreTracker();
        ReduceLives();

        if(startIndex == 2){
            if(!checkOne || !checkTwo){
                FadeIn(); 
            }
            if(!fight){
                aud.PlayOneShot(oneBell, 0.7f);
                fight = true;
            }
        }

        if(fight && !timerEnd){
            if(!playerOne.P1IsDed && !playerTwo.P2IsDed){
                targetTime -= Time.deltaTime;
                if(targetTime <= 0){
                    firstKey = P2Keys[RandomKey1()];
                    secondKey = P1Keys[RandomKey2()];
                    targetTime = Random.Range(3.0f, 7.0f);
                    timerEnd = true;
                }
            }
        }

        if(firstPress){
            timerEnd = false;
            aud.PlayOneShot(punch, 0.3f);
            firstPress = false;
        }

        if(roundOver){
            restart.SetActive(true);
        }
        RestartGame();
    }

    void ScoreTracker(){
        if(P1Score == 0 || P2Score == 0){
            if(!roundOver){
                aud.PlayOneShot(threeBells, 0.7f);
                roundOver = true;
            }
        }
    }

    void ReduceLives(){
        for(int i = 0; i < 3; i++){
            if(i == P1Score){
                a[i].a = 0f;
            }
            if(i == P2Score){
                b[i].a = 0f;
            }
        }
    }

    void SetFadeIn(){
        for(int i = 0; i < 3; i++){
            lives[i].color = a[i];
            lives[i + 3].color = b[i];
        }
    }

    void FadeIn(){
        for(int i = 0; i < 3; i++){
            if(a[i].a < 1.0f){
                a[i].a += Time.deltaTime;
            } else {
                a[i].a = 1.0f;
                checkOne = true;
            }
            if(b[i].a < 1.0f){
                b[i].a += Time.deltaTime;
            } else {
                b[i].a = 1.0f;
                checkTwo = true;
            }
        }
    }

    public int RandomKey1(){
        int random = Random.Range(0, P1Keys.Length);
        return random;
    }

    public int RandomKey2(){
        int random = Random.Range(0, P2Keys.Length);
        return random;
    }


    void RestartGame(){
        if(roundOver && Input.GetKeyDown(KeyCode.P)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}