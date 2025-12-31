using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform targetPoint;
    [SerializeField] float health = 70;
    int enemyId;
    int lastAttack = -1;


    public bool attackTaken;
    public bool isDead = false;
    public bool isAggro = false;

    SceneItemSave itemSave;


    private void Start()
    {
        enemyId = EnemyManager.instance.AddEnemy(this);

        itemSave = GetComponent<SceneItemSave>();
        if (itemSave != null)
        {
            if (!itemSave.state.isActive)
            {
                Die(true);
                transform.position = new Vector3(itemSave.state.posRot[0], itemSave.state.posRot[1], itemSave.state.posRot[2]);
                transform.rotation = Quaternion.Euler(0f, itemSave.state.posRot[3], 0f);
            }
        }
    }


    private void Update()
    {
        Die(false);
    }

    void Die(bool isForced)
    {
        if (health <= 0 || isForced)
        {
            EnemyManager.instance.RemoveEnemy(enemyId);

            controller.enabled = false;
            isDead = true;

            if (!isForced)
            {
                if (itemSave != null)
                {
                    itemSave.state.isActive = false;
                    itemSave.state.posRot[0] = transform.position.x;
                    itemSave.state.posRot[1] = transform.position.y;
                    itemSave.state.posRot[2] = transform.position.z;
                    itemSave.state.posRot[3] = transform.eulerAngles.y;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Knife knife = other.gameObject.GetComponent<Knife>();
        if (knife != null)
        {
            int attackId;
            int dmgTaken = knife.GetDamage(out attackId);

            TakeDamage(attackId, dmgTaken);
        }

        zPlayerAlert alert = other.gameObject.GetComponent<zPlayerAlert>();
        if (alert != null)
        {
            BecomeAggro();
        }
    }

    public void TakeDamage(int attackId, int dmgTaken)
    {
        if (lastAttack != attackId)
        {
            lastAttack = attackId;
            health -= dmgTaken;

            attackTaken = true;
            BecomeAggro();
        }
    }

    public void BecomeAggro()
    {
        isAggro = true;
    }

    public float GetHealth() => health;

    public Vector3 GetTargetPos() => targetPoint.position;
}
