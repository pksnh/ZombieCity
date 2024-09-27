using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunActiveButton : MonoBehaviour
{
    // Start is called before the first frame update

    public Gun Shotgun1;
    public Gun Shotgun2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseShotgun1()
    {
        Shotgun1.gameObject.SetActive(true);
        Shotgun2.gameObject.SetActive(false);
    }
    public void UseShotgun2()
    {
        Shotgun1.gameObject.SetActive(false);
        Shotgun2.gameObject.SetActive(true);
    }
}
