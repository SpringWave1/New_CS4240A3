using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public OVRInput.Controller Controller;
    public string buttonName;
    public GameObject rightHand;
    public bool gunGrabbing;

    private GameObject gun;
    private GameObject shootPoint;
    private bool isShooting;
    private bool isGrabbing;
    


    void Start () {
        shootPoint = transform.GetChild(0).gameObject;
        //set isShooting bool to default as false
        rightHand =  GameObject.Find("CustomHandRight");
        isShooting = false;
        gunGrabbing = false;
    }


    //Shoot function
    IEnumerator Shoot() {
        isShooting = true;

        //instantiate the bullet
        GameObject bullet = Instantiate(Resources.Load("Bullet2", typeof(GameObject))) as GameObject;
        //Get the bullet's rigid body component and set its position and rotation equal to that of the shootPoint
        bullet.layer = 0;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = shootPoint.transform.rotation;
        bullet.transform.position = shootPoint.transform.position;
        //add force to the bullet
        rb.AddForce(shootPoint.transform.forward * 2100f);
        //play the gun shot sound and gun animation
        GetComponent<AudioSource>().Play ();
        //destroy the bullet after 1 second
        Destroy (bullet, 1);
        // //wait for 1 second and set isShooting to false so we can shoot again
        yield return new WaitForSeconds (0.3f);
        isShooting = false;
    }
    

    // Update is called once per frame
    void Update () {
    
        //declare a new RayCastHit
        RaycastHit hit;

        Debug.DrawRay(shootPoint.transform.position, shootPoint.transform.forward, Color.green);

        isGrabbing = rightHand.GetComponent<Grab>().grabbing && gunGrabbing;
        //cast a ray from the spawnpoint in the direction of its forward vector
        if (isGrabbing && Input.GetAxis(buttonName) == 1) {
            if (!isShooting) {
                StartCoroutine ("Shoot");
            }      
        }    
    }
}
