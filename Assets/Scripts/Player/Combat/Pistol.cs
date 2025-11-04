using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;

public class Pistol : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] WeaponControls weaponControls;

    [SerializeField] Light shotLight;
    [SerializeField] float shotDelay = 0.1f;
    [SerializeField] float range = 10f;

    bool canShoot = true;

    EnemyBase target = null;

    private void OnEnable()
    {
        canShoot = true;
        //PlayerInput.OnWeaponActionEvent += HandleShot;
    }

    private void OnDisable()
    {
        //PlayerInput.OnWeaponActionEvent -= HandleShot;
    }


    void Update()
    {
        HandleAim();
    }

    void HandleAim()
    {
        if (weaponControls.isAiming)
        {
            //if (target == null)
            //{
                TryLockOn();
            //}
        }
        else
        {
            target = null;
        }
    }

    void TryLockOn()
    {
        Vector3 playerForward = playerController.transform.forward;
        EnemyBase targetEnemy = null;
        float minDist = range;

        foreach (var pair in EnemyManager.instance.enemyList)
        { 
            EnemyBase enemy = pair.Value;
            Vector3 vectorTo = enemy.transform.position - playerController.transform.position;

            if (math.dot(vectorTo, playerForward) < 0) 
            {
                continue;
            }

            if (vectorTo.magnitude < minDist)
            { 
                minDist = vectorTo.magnitude;
                targetEnemy = enemy;
            }
        }

        if (targetEnemy != null)
            TargetNewEnemy(targetEnemy);
    }

    void TargetNewEnemy(EnemyBase newEnemy)
    {
        target = newEnemy;
    }

    // returns value between 0 and 1 (instead of -1 and 1), for vertical aim
    public float GetTargetValue()
    {
        if (target != null)
        {

            // FOR SURE INCOMPLETE/INCORRECT      --  - - - - -     FIX THIS


            Vector3 targetPos = target.GetTargetPos();
            float rawValue = targetPos.y - transform.position.y;
            rawValue = rawValue / 2f + 0.5f;

            return math.clamp(rawValue, 0f, 1f);
        }

        return 0.5f;
    }


    public void HandleShot(PlayerInput input)
    {
        if (!weaponControls.isAiming || !canShoot)
            return;

        Debug.Log("we shootin out here");



        shotLight.enabled = true;
        canShoot = false;
        Invoke("DisableLight", 0.05f);
        Invoke("EnableShot", shotDelay);
    }


    void DisableLight()
    {
        shotLight.enabled = false;
    }

    void EnableShot()
    {
        canShoot = true;
    }
}
