using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationToRagdoll : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] Collider myCollider;
    [SerializeField] float respawnTime = 5.0f;
    Rigidbody[] rigidbodies;
    bool isRagdoll;
    bool gettingUp;
    Animator anim;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        ToggleRagdoll(true);
    }

    void Update()
    {
        RagdollTest();
        Animations();
        GetIt();
    }

    void GetIt(){
        if(Input.GetKeyDown(KeyCode.T)){
            anim.SetBool("Dance", true);
        } else if(Input.GetKeyDown(KeyCode.P)){
            anim.SetBool("Dance", false);
        }
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
        gettingUp = true;
        ToggleRagdoll(true);
        
    }

    void Animations(){
        if(anim.enabled){
            if(player.move != Vector3.zero){
                anim.SetBool("Walk", true);
            } else {
                anim.SetBool("Walk", false);
            }

            if(gettingUp){
                anim.applyRootMotion = true;
                anim.SetBool("Stand", true);
            }
        }
    }
}
