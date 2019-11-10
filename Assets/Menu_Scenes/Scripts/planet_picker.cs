using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet_picker : MonoBehaviour
{
    public GameObject PlanetHalo;
    public Camera MainCam;

    // Start is called before the first frame update
    void Start()
    {
        //PlanetHalo.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        PlanetHalo.GetComponent<Animator>().Play("Halo_in");
    }

    void OnMouseExit()
    {
        PlanetHalo.GetComponent<Animator>().Play("Halo_out");
    }

    void OnMouseDown()
    {
        MainCam.GetComponent<Animator>().Play("camera_zoomin");
    }
}
