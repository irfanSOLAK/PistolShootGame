using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrelScript : MonoBehaviour
{
    [Header("Barrel")]
    public int barrelPower;

    // Start is called before the first frame update
    void Start()
    {
        DetectBarrelPower();
    }
    private void DetectBarrelPower()
    {
        if (gameObject.transform.parent.name == "RandomPowerBarrels")
        {
            barrelPower = Random.Range(10, 41);
        }
        else
        {
            barrelPower = int.Parse(gameObject.transform.parent.name);
        }
    }



    // Update is called once per frame
    void Update()
    {
        WriteBarrelPower();
        CheckBarrelDestroy();
    }

    private void WriteBarrelPower()
    {
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = barrelPower.ToString();
    }
    private void CheckBarrelDestroy()
    {
        if (barrelPower <= 0)
        {
            Destroy(gameObject);
        }
    }
}
