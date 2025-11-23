using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Pistol : MonoBehaviour
{
    [SerializeField] PlayerInformation playerInfo;
    [SerializeField] PlayerController playerController;
    [SerializeField] WeaponControls weaponControls;
    [SerializeField] Billboard aimTarget;

    [SerializeField] int damage = 10;

    [SerializeField] Light shotLight;
    [SerializeField] float shotDelay = 0.1f;
    [SerializeField] float range = 10f;

    bool canShoot = true;
    int attackId = 9;

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
        if (weaponControls.isAiming) {
            //if (target == null)
            //{
                TryLockOn();
            //}
        }
        else {
            target = null;
        }


        if (target != null) {
            aimTarget.GetComponent<Renderer>().enabled = true;
            aimTarget.transform.position = target.GetTargetPos();
        }
        else {
            aimTarget.GetComponent<Renderer>().enabled = false;
        }
    }


    void TryLockOn()
    {
        Vector3 playerForward = playerController.transform.forward;
        EnemyBase targetEnemy = null;
        //float minDist = range;
        float minimum = 30f;
        float minAngle = minimum;

        foreach (var pair in EnemyManager.instance.enemyList)
        { 
            EnemyBase enemy = pair.Value;
            Vector3 vectorTo = enemy.transform.position - playerController.transform.position;

            if (math.dot(vectorTo, playerForward) < 0) 
            {
                continue;
            }

            float minFactor = math.lerp(1, 0, vectorTo.magnitude / range);
            Debug.Log(vectorTo.magnitude);


            vectorTo.y = 0;
            vectorTo = vectorTo.normalized;


            float angleBetween = Vector3.Angle(vectorTo, playerForward);

            if (angleBetween < minAngle && angleBetween < minFactor * minimum)      // DONE: add distance check, which dictates the maximum allowed angle
            {                                                                       // DONE: max distance = angle must be perfect (and also cant lock on)
                minAngle = angleBetween;
                targetEnemy = enemy;
            }

            /*if (vectorTo.magnitude < minDist)
            { 
                minDist = vectorTo.magnitude;
                targetEnemy = enemy;
            }*/
        }

        target = targetEnemy;
    }


    // returns value between 0 and 1 (instead of -1 and 1), for vertical aim
    public float GetTargetValue()
    {
        if (target != null)
        {
            // lowkey better version

            /*Vector3 targetPos = target.GetTargetPos();
            float rawValue = targetPos.y - transform.position.y;
            rawValue = rawValue / 2f + 0.5f;

            return math.clamp(rawValue, 0f, 1f);*/

            Vector3 targetPos = target.GetTargetPos();
            Vector3 vecTo = targetPos - playerController.transform.position;

            float angle = Vector3.Angle(vecTo, playerController.transform.up);

            float rawValue = 1f - (angle - 45f) / 90f;

            return math.clamp(rawValue, 0f, 1f);
        }

        return 0.7f;
    }


    public void HandleShot(PlayerInput input)
    {
        if (!weaponControls.isAiming || !canShoot)
            return;

        if (!playerInfo.TryShoot())
            return;

        Debug.Log("we shootin out here");

        attackId = (attackId + 1) % 8 + 8;
        target?.TakeDamage(attackId, damage);


        input.PlaySound(1);

        CameraEffects.instance.Shake(1f);
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
