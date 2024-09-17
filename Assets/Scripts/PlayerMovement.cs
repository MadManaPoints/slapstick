using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager gameManager;
    AnimationToRagdoll ragdoll;
    Rigidbody playerRb;
    public Vector3 hMove;
    public Vector3 vMove;
    public Vector3 dir;
    [SerializeField] String obj;
    float speed = 3.0f;
    float maxSpeed;
    bool up, down,left, right;
    bool[] directions;
    bool isMoving;
    public bool P1IsDed;
    public bool P2IsDed;
    public bool P1Hit;
    public bool P2Hit;
    bool spar;
    bool inPos;
    bool chooseKeys;
    Vector3 playerOneSparPos = new Vector3(-0.5f, 0.217f, -0.4f);
    Vector3 playerTwoSparPos = new Vector3(0.7f, 0.217f, -0.4f);
    [SerializeField] bool playerOne;
    [SerializeField] KeyCode moveUp, moveLeft, moveDown, moveRight;
    KeyCode[] keys;
    KeyCode currentKey;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ragdoll = GameObject.Find(obj).GetComponent<AnimationToRagdoll>();
        playerRb = GetComponent<Rigidbody>();
        directions = new bool[]{up, down, left, right};
        keys = new KeyCode[]{moveUp, moveLeft, moveDown, moveRight};
    }

    void Update(){
        if(!inPos){
            Controls();
        }
        SparPos();
        TrackScore();
        
    }
    void FixedUpdate()
    {        
        playerRb.AddForce(dir * speed, ForceMode.Impulse);

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
        }
    }

    void SparPos(){
        if(spar){
            if(playerOne && !inPos){
                gameManager.P1Text.text = "Press R";
            } else if(!inPos){
                gameManager.P2Text.text = "Press Y";
            }

            if(playerOne && Input.GetKeyDown(KeyCode.R)){
                playerRb.isKinematic = true;
                transform.position = playerOneSparPos;
                transform.localEulerAngles = new Vector3(0f, 90.0f, 0f);
                if(!inPos){
                    gameManager.P1Text.text = "";
                    gameManager.startIndex += 1;
                }
                inPos = true;
            } else if(!playerOne && Input.GetKeyDown(KeyCode.Y)){
                playerRb.isKinematic = true;
                transform.position = playerTwoSparPos;
                transform.localEulerAngles = new Vector3(0f, 270.0f, 0f);
                if(!inPos){
                    gameManager.P2Text.text = "";
                    gameManager.startIndex += 1;
                }
                inPos = true;
            }
        }

        if(playerOne){
            if(gameManager.P1Score == 0){
                P1IsDed = true;
                ragdoll.ToggleRagdoll(false);
            }
        } else if(gameManager.P2Score == 0){
            P2IsDed = true;
            ragdoll.ToggleRagdoll(false);
        }
    }

    void TrackScore(){
        if(gameManager.fight && !gameManager.firstPress && gameManager.timerEnd){
            KeyCode firstKey = gameManager.firstKey;
            KeyCode secondKey = gameManager.secondKey;

            gameManager.P1Text.text = firstKey.ToString();
            gameManager.P2Text.text = secondKey.ToString();

            if(!gameManager.firstPress){
                if(playerOne){
                    if(Input.GetKeyDown(firstKey)){
                        gameManager.firstPress = true;
                        P2Hit = true;
                        gameManager.timerEnd = false;
                        gameManager.P2Score -= 1;
                    
                    }
                } else{
                    if(Input.GetKeyDown(secondKey)){
                        gameManager.firstPress = true;
                        P1Hit = true;
                        gameManager.timerEnd = false;
                        gameManager.P1Score -= 1;
                    }
                }
            }
            
        } else if(gameManager.fight && !gameManager.timerEnd){
            gameManager.P1Text.text = "";
            P1Hit = false;
            gameManager.P2Text.text = "";
            P2Hit = false;
        }
    }

    void Controls(){
        up = Input.GetKey(moveUp);
        left = Input.GetKey(moveLeft);
        down = Input.GetKey(moveDown);
        right = Input.GetKey(moveRight);

        for(int i = 0; i < directions.Length; i++){
            if(Input.GetKeyDown(keys[i])){
                currentKey = keys[i];
            }
        }

        if(up || left || down || right){
            isMoving = true;
        } else{
            isMoving = false;
        }

        Movement();
        PlayerRotation();
    }

    void Movement(){
        if(isMoving){
            if(currentKey == keys[0]){
                dir = new Vector3(0f, 0f, 1.0f);
            }
            if(currentKey == keys[1]){
                dir = new Vector3(-1.0f, 0f, 0f);
            }
            if(currentKey == keys[2]){
                dir = new Vector3(0f, 0f, -1.0f);
            }
            if(currentKey == keys[3]){
                dir = new Vector3(1.0f, 0f, 0f);
            }
        } else{
            dir = Vector3.zero;
        }
    }

    void PlayerRotation(){
        if(currentKey == keys[0]){
            transform.localEulerAngles = Vector3.zero;
        }
        if(currentKey == keys[1]){
            transform.localEulerAngles = new Vector3(0f, 270.0f, 0f);
        }
        if(currentKey == keys[2]){
            transform.localEulerAngles = new Vector3(0f, 180.0f, 0f);
        }
        if(currentKey == keys[3]){
            transform.localEulerAngles = new Vector3(0f, 90.0f, 0f);
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Opp"){
            ragdoll.ToggleRagdoll(false);
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Spar"){
            spar = true;
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Spar"){
            spar = false;
        }
    }
}
