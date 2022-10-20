using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{
    PistolShootScript pistolShootScript;

    private void Start()
    {
        GetOtherNeededScripts();
    }
    private void GetOtherNeededScripts()
    {
        pistolShootScript = GameObject.FindGameObjectWithTag("Pistol").GetComponent<PistolShootScript>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Barrels") || other.transform.root.CompareTag("Trap"))
        {            
            DecreasePistolShoot();
            pistolShootScript.SetBulletFiringValues();
            DestroyProperly();
        }
    }
    private void DecreasePistolShoot()
    {
        if (CompareTag("Sum"))
        {
            pistolShootScript.SetBulletAmountInSecond(pistolShootScript.bulletAmountInSecond - int.Parse(name));
        }

        else if (CompareTag("Multiply"))
        {
            pistolShootScript.SetBulletAmountInSecond(pistolShootScript.bulletAmountInSecond / int.Parse(name));
        }
    }
    private void DestroyProperly()
    {
        GetComponent<BoxCollider>().enabled = false;
        transform.parent = null;
        Destroy(gameObject);
    }
}
