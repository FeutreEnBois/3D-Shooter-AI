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
    Animator anim;
    AnimatorOverrideController overrides;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(weapon != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }
            if (weapon.isFiring)
            {
                weapon.UpadateFiring(Time.deltaTime);
            }
            weapon.UpdateBullets(Time.deltaTime);
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
        }
        else
        {
            handIk.weight = 0;
            anim.SetLayerWeight(1, 0.0f);
        }
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = crosshairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        handIk.weight = 1.0f;
        anim.SetLayerWeight(1, 1.0f);
        Invoke(nameof(SetAnimationDelayed), 0.0001f);
        
    }

    void SetAnimationDelayed()
    {
        overrides["weapon_anim_empty"] = weapon.weaponAnimation;
    }

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimation);
        UnityEditor.AssetDatabase.SaveAssets();
    }
}
