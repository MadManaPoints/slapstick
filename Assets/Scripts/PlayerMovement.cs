using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    AnimationToRagdoll ragdoll;
    Rigidbody playerRb;
    public Vector3 move;
    float hInput;
    float vInput;
    float speed = 2.0f;
    float maxSpeed;
    void Start()
    {
        ragdoll = GameObject.Find("Eva_Ragdoll_Animated").GetComponent<AnimationToRagdoll>();
        playerRb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        move = new Vector3(hInput, 0f, vInput);

        playerRb.AddForce(move * speed, ForceMode.Impulse);

        if(playerRb.velocity.magnitude > maxSpeed){
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); 
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Opp"){
            //Debug.Log("YERR");
            ragdoll.ToggleRagdoll(false);
        }
    }
}
