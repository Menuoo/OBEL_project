using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    [SerializeField] EnemyBase enemy;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] EnemyAttack attackCtrl;

    PlayerController player;

    private static int attackHash = Animator.StringToHash("attack");
    private static int flinchHash = Animator.StringToHash("flinch");

    public bool inAction { get; private set; }


    private void Start()
    {
        inAction = false;
        player = EnemyManager.instance.GetPlayer();
    }

    void Update()
    {
        if (player != null) {    //  - - - --  -  HAVE TO ADD IDLING AND SEEING/HEARING PLAYER

            float dist = (player.transform.position - transform.position).magnitude;

            if (!inAction && dist <= 1.1f)
            {
                Attack();
            }
            else if (!inAction)
            {
                Move();
            }
        }
    }

    void Move()
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;
        dir.y = -1f;

        controller.Move(dir * movementSpeed * Time.deltaTime);

        transform.LookAt(player.transform.position);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    void Attack()
    {
        if (animator.GetBool(attackHash))
            return;

        inAction = true;
        animator.SetBool(attackHash, inAction);
        attackCtrl.Begin();
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
    }

}
