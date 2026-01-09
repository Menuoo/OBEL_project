using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemySimple enemy;
    [SerializeField] bool longAttack = false;
    [SerializeField] Collider attackCollider;
    [SerializeField] int damage = 20;

    Vector3 orig = Vector3.zero;
    Vector3 lastLoc = Vector3.zero;
    Vector3 nextLoc = Vector3.zero;
    float longTimer = 0f; // has to be synced with animation somewhat
    bool back = false;


    bool isActive = false;
    //int id = 0;
    //int attackId = 0;

    private void Start()
    {
        enemy = GetComponentInParent<EnemySimple>();
        orig = transform.localPosition;
        attackCollider.enabled = false;
    }

    private void LateUpdate()
    {
        if (longAttack)
        {
            if (isActive)
            {
                if (longTimer > 0.25f)
                {
                    back = true;
                }


                if (!back) 
                    longTimer += Time.deltaTime;
                else 
                    longTimer -= Time.deltaTime * 2f;


                if (longTimer >= 0f)
                transform.position = Vector3.Lerp(lastLoc, nextLoc, longTimer / 0.25f);
            }
            else
                transform.localPosition = orig;
        }
    }

    public void Begin()
    {
        attackCollider.enabled = true;
        isActive = true;
    }

    public void End()
    {
        if (longAttack)
        { 
            nextLoc = orig;
        }

        attackCollider.enabled = false;
        isActive = false;
    }

    public void Long(Vector3 playerLoc) //float dist)
    {
        isActive = true;
        longTimer = -10f;

        playerLoc.y += 0.2f;
        nextLoc = playerLoc;
    }

    public void Extend()
    {
        longTimer = 0f;
        lastLoc = transform.position;
        back = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerInformation player = other.GetComponent<PlayerInformation>();
        if (player != null)
        {
            if (isActive)
            {
                //enemy.PlaySound(8);
                //enemy.PlaySound(10);
                //enemy.PlaySound(15);

                player.AlterHP(-damage);
                isActive = false;
            }
        }
    }

    /*public void SetId(int newId)
    { 
        id = newId;
    }*/
}
