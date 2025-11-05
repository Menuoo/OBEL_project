using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EasyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    PlayerController player;

    private void Start()
    {
        player = EnemyManager.instance.GetPlayer();
    }

    void Update()
    {
        if (player != null) {
            Move();
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
}
