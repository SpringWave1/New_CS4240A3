using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public OVRInput.Controller Controller;
    public string buttonName;
    public int gunPower;
    public GameObject muzzleFlashPrefab;
    public GameObject rightHand;
    public bool gunGrabbing;

    private GameObject gun;
    private GameObject shootPoint;
    private bool isShooting;
    private bool isGrabbing;
    

    void Start () 
    {
        shootPoint = transform.GetChild(0).gameObject;
        rightHand =  GameObject.Find("CustomHandRight");
        isShooting = false;

        // used to indicate the gun
        gunGrabbing = false;
    }


    //Shoot function 
    IEnumerator Shoot() 
    {
        isShooting = true;

        //creat the bullet
        GameObject bullet = Instantiate(Resources.Load("Bullet2", typeof(GameObject))) as GameObject;
        //set bullet transform as the same as the shoot point
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.position = shootPoint.transform.position;
        bullet.transform.rotation = shootPoint.transform.rotation;

        // add fire
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject fire;
            fire = Instantiate(muzzleFlashPrefab, shootPoint.transform.position, shootPoint.transform.rotation);

            //Destroy the muzzle flash effect
            Destroy(fire, 0.3f);
        }
        
        //add force to the bullet
        rb.AddForce(shootPoint.transform.forward * gunPower);

        //after shoot
        GetComponent<AudioSource>().Play ();
        Destroy (bullet, 1);
        //wait for 0.3s to shoot again
        yield return new WaitForSeconds (0.3f);
        isShooting = false;
    }
    

    // Update is called once per frame
    void Update () 
    {
        // test shoot point direction
        RaycastHit hit;
        Debug.DrawRay(shootPoint.transform.position, shootPoint.transform.forward, Color.green);

        isGrabbing = rightHand.GetComponent<Grab>().grabbing && gunGrabbing;
        // shoot
        if (isGrabbing && Input.GetAxis(buttonName) == 1) 
        {
            if (!isShooting) {
                StartCoroutine ("Shoot");
            }      
        }    
    }
}
