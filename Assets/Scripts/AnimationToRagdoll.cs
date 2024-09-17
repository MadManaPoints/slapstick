using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationToRagdoll : MonoBehaviour
{
    GameManager gameManager;
    PlayerMovement player;
    [SerializeField] String playerName;
    [SerializeField] Collider myCollider;
    [SerializeField] float respawnTime = 5.0f;
    Rigidbody[] rigidbodies;
    bool isRagdoll;
    Animator anim;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find(playerName).GetComponent<PlayerMovement>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        ToggleRagdoll(true);
    }

    void Update()
    {
        RagdollTest();
        Animations();
    }

    

    void RagdollTest(){
        if(!isRagdoll && Input.GetKeyDown(KeyCode.Space)){
            ToggleRagdoll(false);
            //StartCoroutine(GetBackUp());
        }
    }

    public void ToggleRagdoll(bool isAnimating){
        isRagdoll = !isAnimating;
        myCollider.enabled = isAnimating;

        foreach(Rigidbody bone in rigidbodies){
            bone.isKinematic = isAnimating;
        }
        anim.enabled = isAnimating;
    }

    private IEnumerator GetBackUp(){
        yield return new WaitForSeconds(respawnTime);
        ToggleRagdoll(true);
        
    }

    void Animations(){
        if(anim.enabled){
            if(gameManager.fight && !gameManager.roundOver){
                if(GameObject.Find("Player2").GetComponent<PlayerMovement>().P1Hit && playerName == "Player"){
                    anim.SetTrigger("Hit");
                } else if(GameObject.Find("Player").GetComponent<PlayerMovement>().P2Hit && playerName == "Player2"){
                    anim.SetTrigger("Hit");
                } else {
                    anim.SetBool("FightIdle", true);
                }
            } else if(player.dir != Vector3.zero){
                anim.SetBool("Walk", true);
            } else if(playerName == "Player2" && GameObject.Find("Player").GetComponent<PlayerMovement>().P1IsDed){
                anim.SetBool("Walk", false);
                anim.SetBool("Dance", true);
            } else if(playerName == "Player" && GameObject.Find("Player2").GetComponent<PlayerMovement>().P2IsDed){
                anim.SetBool("Walk", false);
                anim.SetBool("Dance", true);
            } else {
                anim.SetBool("Walk", false);
            }
        }
    }
}
