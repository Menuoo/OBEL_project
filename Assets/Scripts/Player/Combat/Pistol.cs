using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Pistol : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] WeaponControls weaponControls;

    [SerializeField] Light shotLight;
    [SerializeField] float shotDelay = 0.1f;

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
            if (target == null)
            {
                TryLockOn();
            }
        }
    }

    void TryLockOn()
    {
        Vector3 playerForward = playerController.transform.forward;

        foreach (var pair in EnemyManager.instance.enemyList)
        { 
            
        }
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
