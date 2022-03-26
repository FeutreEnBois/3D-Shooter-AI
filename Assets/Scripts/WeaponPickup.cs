using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if(activeWeapon != null)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            activeWeapon.Equip(newWeapon);
            Destroy(gameObject);
        }
        
        AiWeapon aiWeapon = other.gameObject.GetComponent<AiWeapon>();
        if(aiWeapon != null)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            aiWeapon.Equip(newWeapon);
            Destroy(gameObject);
        }
    }
}
