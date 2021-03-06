using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWeapon : MonoBehaviour
{
    RaycastWeapon currentWeapon;
    Animator animator;
    MeshSockets sockets;
    WeaponIK weaponIK;
    Transform currentTarget;
    bool weaponActive = false;
    public float inaccuracy = 0.0f;

    private void Start()
    {

        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIK = GetComponent<WeaponIK>();
    }

    private void Update()
    {
        if(currentTarget && currentWeapon && weaponActive)
        {
            Vector3 target = currentTarget.position + weaponIK.targetOffset;
            target += Random.insideUnitSphere * inaccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enabled)
    {
        if (currentWeapon == null) return;

        if (enabled)
        {
            currentWeapon.StartFiring();
        }
        else
        {
            currentWeapon.StopFiring();
        }
    }
    public void Equip(RaycastWeapon weapon)
    {
        currentWeapon = weapon;
        sockets.Attach(weapon.transform,MeshSockets.SocketId.Spine);
    }

    public void ActivateWeapon()
    {
        StartCoroutine(EquipWeapon());
    }

    IEnumerator EquipWeapon()
    {
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
        weaponActive = true;
    }   
    
    public void DeactivateWeapon()
    {
        SetTarget(null);
        SetFiring(false);
        StartCoroutine(HolsterWeapon());
    }

    IEnumerator HolsterWeapon()
    {
        weaponActive = false;
        animator.SetBool("Equip", false);
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
    }

    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void OnAnimationEvent(string eventName)
    {
        if(eventName == "equipWeapon")
        {
            Debug.Log("EquipWeapon");
            sockets.Attach(currentWeapon.transform,MeshSockets.SocketId.RightHand);
        }
    }

    public void SetTarget(Transform target)
    {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }
}
