using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float bulletSpeed;
    private float bulletLifeSeconds;


    private void Awake()
    {
        SetBulletSpeed();
        SetBulletLifeTime();
    }
    private void SetBulletSpeed()
    {
        bulletSpeed = GameObject.FindGameObjectWithTag("Pistol").GetComponent<PistolShootScript>().bulletForwardSpeed;
    }
    private void SetBulletLifeTime()
    {
        bulletLifeSeconds = GameObject.FindGameObjectWithTag("Pistol").GetComponent<PistolShootScript>().bulletLifeInSecond;
    }


    // Start is called before the first frame update
    void Start()
    {
        BulletMove();
        DestroyBulletInLifeTime();
    }
    private void BulletMove()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }
    private void DestroyBulletInLifeTime()
    {
        Destroy(gameObject, bulletLifeSeconds);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("Barrels"))
        {
            DecreaseBarrelPowerWithHit(other);
            Destroy(gameObject);
        }

        if (other.gameObject.transform.root.CompareTag("Trap"))
        {
            Destroy(gameObject);
        }
    }
    private void DecreaseBarrelPowerWithHit(Collider other)
    {
        other.transform.parent.GetComponent<BarrelScript>().barrelPower--;
    }
}
