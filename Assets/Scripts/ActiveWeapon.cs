using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;


public class ActiveWeapon : MonoBehaviour
{
    public Transform crosshairTarget;
    public UnityEngine.Animations.Rigging.Rig handIk;
    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    RaycastWeapon weapon;
    public Animator rigController;
    // Start is called before the first frame update
    void Start()
    {

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(weapon != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
            weapon.UpdateWeapon(Time.deltaTime, crosshairTarget.position);

            if (Input.GetKeyDown(KeyCode.X))
            {
                bool isHolsted = rigController.GetBool("holster_weapon");
                rigController.SetBool("holster_weapon", !isHolsted);
            }
        }
   
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        rigController.Play("equip_" + weapon.weaponName);
    }

    public void DropWeapon()
    {
        var currentWeapon = weapon;
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            weapon = null;
        }
    }
}
