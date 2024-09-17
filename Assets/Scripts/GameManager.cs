using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography;

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
    void Start()
    {
        playerOne = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerTwo = GameObject.Find("Player2").GetComponent<PlayerMovement>();
        targetTime = Random.Range(3.0f, 5.0f);
    }

    void Update()
    {
        text1.transform.position = playerOne.transform.position + offset;
        text2.transform.position = playerTwo.transform.position + offset;

        ScoreTracker();

        if(startIndex == 2){
            fight = true;
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
            firstPress = false;
        }
    }

    void ScoreTracker(){
        if(P1Score == 0 || P2Score == 0){
            roundOver = true;
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
}