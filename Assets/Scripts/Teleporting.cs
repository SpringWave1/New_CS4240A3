using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporting : MonoBehaviour
{   
    public OVRInput.Controller Controller;
    public Teleporter teleporter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            teleporter.ToggleDisplay(true);
        }

        if(OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            
            teleporter.ToggleDisplay(false);
            teleporter.Teleport();
        }

    }
}
