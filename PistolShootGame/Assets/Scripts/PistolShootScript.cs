using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShootScript : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletForwardSpeed;
    public float bulletLifeInSecond;
    public int bulletAmountInSecond;

    private float bulletSpawnWaitingTime;

    [Header("Barrel")]
    public int secondBarrelBulletNumber;
    public int thirdBarrelBulletNumber;
    private int numberOfBarrel;

    PistolMoveAndControlScript pistolMoveAndControlScript;


    // Start is called before the first frame update
    void Start()
    {
        SetBulletFiringValues();
        GetScripts();
        StartCoroutine(SpawnBullet());
    }
    public void SetBulletFiringValues()
    {
        SetBulletAmountInSecond(bulletAmountInSecond);
        CheckBulletAmount();
        SetBulletSpawnWaitingTime(bulletAmountInSecond);
    }
    public void SetBulletAmountInSecond(int bulletAmount)
    {
        bulletAmountInSecond = bulletAmount;
    }
    private void CheckBulletAmount()
    {
        if (bulletAmountInSecond <= 1)
            bulletAmountInSecond = 1;
    }
    private void SetBulletSpawnWaitingTime(int bulletAmount)
    {
        bulletSpawnWaitingTime = 1 / (float)bulletAmount;
    }
    private void GetScripts()
    {
        pistolMoveAndControlScript = GetComponent<PistolMoveAndControlScript>();
    }



    IEnumerator SpawnBullet()
    {
        while (pistolMoveAndControlScript.isPlayerAlive)
        {
            if (pistolMoveAndControlScript.isPlayerTouchedScreen)
            {
                SetNumberOfBarrel(1);
                CreateBulletIn("SpawnPointMiddle");

                if (secondBarrelBulletNumber < bulletAmountInSecond)
                {
                    SetNumberOfBarrel(2);
                    CreateBulletIn("SpawnPointLeft");

                    if (thirdBarrelBulletNumber < bulletAmountInSecond)
                    {
                        SetNumberOfBarrel(3);
                        CreateBulletIn("SpawnPointRight");
                    }
                }
            }

            yield return new WaitForSeconds(bulletSpawnWaitingTime * numberOfBarrel);
        }
    }
    private void SetNumberOfBarrel(int v)
    {
        numberOfBarrel = v;
    }
    private void CreateBulletIn(string spawnPointTag)
    {
        Instantiate(
            bulletPrefab, 
            GameObject.FindGameObjectWithTag(spawnPointTag).transform.position, 
            GameObject.FindGameObjectWithTag(spawnPointTag).transform.rotation
            );
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("PistolBoost"))
        {
            MakePistolParent(other);
            IncreasePistolShootWith(other);
            SetBulletFiringValues();
        }

        if (other.transform.root.CompareTag("xPlanes"))
        {
            pistolMoveAndControlScript.isLevelCompleted = true;
        }
    }
    private void MakePistolParent(Collider other)
    {
        other.transform.SetParent(transform);
    }
    private void IncreasePistolShootWith(Collider other)
    {
        if (other.CompareTag("Sum"))
        {
            SetBulletAmountInSecond(bulletAmountInSecond + int.Parse(other.name));
        }

        else if (other.CompareTag("Multiply"))
        {
            SetBulletAmountInSecond(bulletAmountInSecond * int.Parse(other.name));
        }
    }
}
