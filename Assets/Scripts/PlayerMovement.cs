using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    AnimationToRagdoll ragdoll;
    Rigidbody playerRb;
    public Vector3 hMove;
    public Vector3 vMove;
    public Vector3 dir;
    [SerializeField] String obj;
    float hInput;
    float vInput;
    float speed = 3.0f;
    float maxSpeed;
    bool up, down,left, right;
    bool[] directions;
    bool isMoving;
    bool isDed;
    [SerializeField] bool playerOne;
    [SerializeField] KeyCode moveUp, moveLeft, moveDown, moveRight;
    KeyCode[] keys;
    KeyCode currentKey;
    void Start()
    {
        ragdoll = GameObject.Find(obj).GetComponent<AnimationToRagdoll>();
        playerRb = GetComponent<Rigidbody>();
        directions = new bool[]{up, down, left, right};
        keys = new KeyCode[]{moveUp, moveLeft, moveDown, moveRight};
    }

    void Update(){
        if(!isDed){
            Controls();
        }
    }
    void FixedUpdate()
    {        
        playerRb.AddForce(dir * speed, ForceMode.Impulse);

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
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
            isDed = true;
            ragdoll.ToggleRagdoll(false);
        }
    }
}
