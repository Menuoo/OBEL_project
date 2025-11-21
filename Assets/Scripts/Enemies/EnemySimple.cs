using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    [SerializeField] EnemyBase enemy;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 400f;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] EnemyAttack attackCtrl;

    PlayerController player;

    private static int attackHash = Animator.StringToHash("attack");
    private static int flinchHash = Animator.StringToHash("flinch");
    private static int deadHash = Animator.StringToHash("dead");
    private static int idleHash = Animator.StringToHash("idle");

    public bool inAction { get; private set; }
    bool canAttack = true;

    public bool immobile = false;


    private void Start()
    {
        inAction = false;
        player = EnemyManager.instance.GetPlayer();
    }

    void Update()
    {
        if (enemy.attackTaken)
            TakeDamage();

        if (enemy.isDead)
        {
            Die();
            return;
        }

        if (player != null)
        {    //  - - - --  -  HAVE TO ADD IDLING AND SEEING/HEARING PLAYER



            float dist = (player.transform.position - transform.position).magnitude;

            if (immobile)
                return;

            if (!enemy.isAggro)
            {
                animator.SetBool(idleHash, true);
                return;
            }
            else
            {
                animator.SetBool(idleHash, false);
            }

            if (!inAction && dist <= 1.1f)   // CHANGE LOGIC OF ATTACK DECISION (PROBABLY TO RAYCAST)  ---  OR SEPERATE OBJECT AROUND ATTACK LOCATION TO CHECK DIST
            {
                Attack();
            }
            else if (!inAction)
            {
                Move();
            }
        }
        else 
        {
            enemy.isAggro = false;
            animator.SetBool(idleHash, true);
        }
    }

    void Move()
    {
        //Vector3 dir = player.transform.position - transform.position;
        //dir.y = 0;
        //dir = dir.normalized;
        //dir.y = -1f;

        Quaternion rotation = controller.transform.rotation;
        controller.transform.transform.LookAt(player.transform.position);
        controller.transform.rotation = Quaternion.Lerp(rotation, Quaternion.Euler(0, controller.transform.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);

        controller.Move(controller.transform.forward * movementSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (animator.GetBool(attackHash))
            return;

        inAction = true;
        animator.SetBool(attackHash, inAction);
        attackCtrl.Begin();

        immobile = true;
    }

    void Flinch()
    {
        if (animator.GetBool(flinchHash))
            return;

        inAction = true;
        animator.SetBool(flinchHash, inAction);
        attackCtrl.End();
    }

    public void EndAction()
    {
        Invoke("RealEndAction", 0.1f);
    }

    void RealEndAction() // have to add a delay because unity animation sucks giga ass
    {
        inAction = false;
        animator.SetBool(attackHash, false);
        animator.SetBool(flinchHash, false);
        attackCtrl.End();

        Invoke("Mobile", 0.5f);
    }


    public void Mobile()
    { 
        immobile = false;
    }

    public void TakeDamage()
    {
        Flinch();
        enemy.attackTaken = false;
        Instantiate(EffectManager.instance.GetParticles(0), enemy.GetTargetPos(), Quaternion.identity);
    }

    public void Die()
    {
        animator.SetBool(deadHash, true);
        this.enabled = false;
        //controller.enabled = false;
    }
}
