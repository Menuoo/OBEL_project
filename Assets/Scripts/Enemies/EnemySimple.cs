using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySimple : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    [SerializeField] EnemyBase enemy;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 400f;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] EnemyAttack attackCtrl1;
    [SerializeField] EnemyAttack attackCtrl2;

    PlayerController player;

    private static int attackHash = Animator.StringToHash("attack");
    private static int addAttackHash = Animator.StringToHash("addAttack");
    private static int longAttackHash = Animator.StringToHash("longAttack");
    private static int flinchHash = Animator.StringToHash("flinch");
    private static int staggerHash = Animator.StringToHash("stagger");
    private static int deadHash = Animator.StringToHash("dead");
    private static int instaDeadHash = Animator.StringToHash("instaDie");
    private static int idleHash = Animator.StringToHash("idle");

    public bool inAction { get; private set; }
    bool canAttack = true;

    public bool immobile = false;

    float dist = 100f;
    Vector3 moveDir = Vector3.zero;

    // stagger logic
    float maxHP;
    float lastHP;
    [SerializeField] float staggerThreshold;

    // claw
    [SerializeField] float attackDistance = 1.1f;
    bool addAttack = false;

    // branch
    [SerializeField] float attackCD = 4f;
    float attackCooldown = 0f;
    float rotDiff = 180f;


    private void Start()
    {
        maxHP = enemy.GetHealth();
        lastHP = maxHP;

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

        HandleMove();
        attackCooldown += Time.deltaTime;

        if (player != null && !player.dead)
        {   
            dist = (player.transform.position - transform.position).magnitude;


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

            if (!inAction && dist <= attackDistance && (enemyType == EnemyType.Branch || rotDiff < 80f))   
            {
                // CHANGE LOGIC OF ATTACK DECISION (PROBABLY TO RAYCAST)  ---  OR SEPERATE OBJECT AROUND ATTACK LOCATION TO CHECK DIST
                Attack();
            }
            else if (enemyType == EnemyType.Branch && !inAction && rotDiff < 10f && attackCooldown >= attackCD)
            {
                Branch_LongAttack();
                //attackCooldown = 0f;
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

    void HandleMove()
    {
        moveDir.y = -3f;
        controller.Move(moveDir * Time.deltaTime);

        moveDir = Vector3.zero;
    }

    void Move()
    {
        //Vector3 dir = player.transform.position - transform.position;
        //dir.y = 0;
        //dir = dir.normalized;
        //dir.y = -1f;

        Quaternion rotation = controller.transform.rotation;
        controller.transform.transform.LookAt(player.transform.position);

        rotDiff = 180f - math.abs(math.abs(rotation.eulerAngles.y - controller.transform.eulerAngles.y) - 180f);

        controller.transform.rotation = Quaternion.Lerp(rotation, Quaternion.Euler(0, controller.transform.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);

        moveDir = controller.transform.forward * movementSpeed * ((180f - rotDiff) / 180f); // modulated by rotDiff

        HandleMove();
        //controller.Move(controller.transform.forward * movementSpeed * Time.deltaTime);
    }


    void Attack()
    {
        if (animator.GetBool(attackHash))
            return;

        inAction = true;
        animator.SetBool(attackHash, inAction);
        //attackCtrl1.Begin();

        immobile = true;
    }
    void AddAttack()
    {
        attackCtrl1.End();

        addAttack = true;

        inAction = true;
        animator.SetBool(addAttackHash, inAction);
        //attackCtrl2.Begin();

        immobile = true;
    }

    void Claw_AdditionalAttack()
    {
        if (enemyType == EnemyType.Claw && player != null)
        {
            //float dist = (player.transform.position - transform.position).magnitude;

            if (dist <= attackDistance && Random.value < 0.65f && !addAttack)
            {
                AddAttack();
            }
        }
    }

    void Branch_LongAttack()
    {
        if (animator.GetBool(attackHash))
            return;

        inAction = true;
        animator.SetBool(longAttackHash, inAction);
        //attackCtrl1.Begin();
        attackCtrl2.Long(player.transform.position);//enemy.transform.forward * dist);

        immobile = true;
    }

    void Branch_Extension()
    {
        attackCtrl2.Extend();
    }


    void Flinch()
    {
        if (animator.GetBool(flinchHash) || animator.GetBool(staggerHash))
            return;

        //inAction = true;
        animator.SetBool(flinchHash, true); // inAction changed to true
        //attackCtrl.End();
    }

    public void EndFlinch()
    {
        animator.SetBool(flinchHash, false);
    }

    void Stagger()
    {
        if (animator.GetBool(staggerHash))
            return;

        inAction = true;
        animator.SetBool(staggerHash, true); // inAction changed to true
        attackCtrl1.End();
        attackCtrl2.End();
    }


    public void EndAction()
    {
        Invoke("RealEndAction", 0.1f);
    }

    void RealEndAction() // have to add a delay because unity animation sucks giga ass
    {
        if (enemyType == EnemyType.Branch && animator.GetBool(longAttackHash))
            attackCooldown = 0f;

        addAttack = false;
        inAction = false;

        animator.SetBool(attackHash, false);
        animator.SetBool(longAttackHash, false);
        animator.SetBool(addAttackHash, false);
        animator.SetBool(flinchHash, false);
        animator.SetBool(staggerHash, false);
        attackCtrl1.End();
        attackCtrl2.End();

        Invoke("Mobile", 0.3f);
    }


    public void Mobile()
    { 
        immobile = false;
    }

    public void TakeDamage()
    {
        if (lastHP > staggerThreshold && enemy.GetHealth() <= staggerThreshold)
        {
            Stagger();
        }
        else { Flinch(); }

        enemy.attackTaken = false;
        Instantiate(EffectManager.instance.GetParticles(0), enemy.GetTargetPos(), Quaternion.identity);
        lastHP = enemy.GetHealth();
    }

    public void Die()
    {
        if (enemy.GetHealth() > 0)
        {
            animator.SetBool(instaDeadHash, true);
        }
        else
        {
            animator.SetBool(deadHash, true);
            attackCtrl1.End();
            attackCtrl2.End();
        }


        this.enabled = false;
        //controller.enabled = false;
    }

    public void Activate1()
    {
        if (addAttack)
            attackCtrl2.Begin();
        else
            attackCtrl1.Begin();
    }

    public void Finish1()
    {
        if (addAttack)
            attackCtrl2.End();
        else
            attackCtrl1.End();
    }

    /*
    public void Activate2()
    {
        attackCtrl2.Begin();
    }
    public void Finish2()
    {
        attackCtrl2.End();
    }*/
}

public enum EnemyType { Base, Claw, Branch }