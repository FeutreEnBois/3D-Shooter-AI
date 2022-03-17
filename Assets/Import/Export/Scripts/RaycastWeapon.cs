using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public bool isFiring = false;
    public float fireRate = 25; // in bullet per seconds
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float maxLifeTime = 10f;
    public float damage = 1f;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public TrailRenderer trailRenderer;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime; // when are we suppose to fire a bullet
    List<Bullet> bullets = new List<Bullet>();

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(trailRenderer, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;

        FireBullet();
    }


    public void UpadateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= 0.0)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltatime)
    {
        SimulateBullets(deltatime);
        DestroyBullets();
    }

    private void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }

    private void SimulateBullets(float deltatime)
    {
        bullets.ForEach(b =>
        {
            Vector3 p0 = GetPosition(b);
            b.time += deltatime;
            Vector3 p1 = GetPosition(b);
            RaycastSegment(p0, p1, b);
        });
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = (end - start).magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;

            var rb2b = hitInfo.collider.GetComponent<Rigidbody>();
            if(rb2b)
            {
                rb2b.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }

            var hitbox = hitInfo.collider.GetComponent<Hitbox>();
            if (hitbox)
            {
                hitbox.OnRaycastHit(this, ray.direction);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        muzzleFlash.Emit(1);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

    }

    public void StopFiring()
    {
        isFiring=false;
    }
}
