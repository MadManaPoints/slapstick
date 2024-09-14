using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationToRagdoll : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] String playerName;
    [SerializeField] Collider myCollider;
    [SerializeField] float respawnTime = 5.0f;
    Rigidbody[] rigidbodies;
    bool isRagdoll;
    Animator anim;

    void Start()
    {
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
            if(player.dir != Vector3.zero){
                anim.SetBool("Walk", true);
                anim.SetBool("Dance", false);
            } else if(Input.GetKeyDown(KeyCode.T)){
                anim.SetBool("Dance", true);
                anim.SetBool("Walk", false);   
            } else if(Input.GetKeyDown(KeyCode.P)){
                anim.SetBool("Dance", false);
            } else {
                anim.SetBool("Walk", false);
            }
        }
    }
}
