using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject marker; 
    public LocomotionController LocomotionController {get; private set;}
    public LayerMask layerMask;
    public float angle = 45f; 
    public float strength = 10f; // ray length
    private int maxCount = 100; 
    private float delta = 0.08f;
    private LineRenderer lineRenderer;
    private Vector3 velocity; 
    private Vector3 groundPos; 
    private Vector3 lastNormal; 
    private List<Vector3> pointList = new List<Vector3>(); 
    private bool displayActive = false; // don't update path when it's false.

    public bool groundDetected = false;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        marker.SetActive(false);
        LocomotionController = GetComponent<LocomotionController>();
    }

    private void FixedUpdate()
    {
        if (displayActive)
        {
            UpdatePath();
        }
    }


    // Update ray path
    private void UpdatePath()
    {
        groundDetected = false;
        pointList.Clear(); // delete all previous points
        velocity = Quaternion.AngleAxis(-angle, transform.right) * transform.forward * strength;

        RaycastHit hit;
        Vector3 pos = transform.position; // take off position
        pointList.Add(pos);

        while (!groundDetected && pointList.Count < maxCount)
        {
            Vector3 newPos = pos + velocity * delta + 0.5f * Physics.gravity * delta * delta;
            velocity += Physics.gravity * delta;
            pointList.Add(newPos); 

            // linecast between last point and current point
            if (Physics.Linecast(pos, newPos, out hit, layerMask))
            {
                groundDetected = true;
                groundPos = hit.point;
                lastNormal = hit.normal;
            }
            pos = newPos; 
        }

        marker.SetActive(groundDetected);

        // add marker
        if (groundDetected)
        {
            marker.transform.position = groundPos + lastNormal * 0.1f;
            marker.transform.LookAt(groundPos);
            
        }
        
        // Update Line Renderer
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }


    // Teleport target transform to ground position
    public void Teleport()
    {
        if (groundDetected) {
            Quaternion rot = marker.transform.rotation;
            Vector3 pos = marker.transform.position;

            DoTeleport(rot, pos);
        }
    }


    // Active teleporter path
    public void ToggleDisplay(bool active)
    {
        lineRenderer.enabled = active;
        marker.SetActive(active);
        displayActive = active;
    }

    // Main teleport function
    public void DoTeleport(Quaternion rotation, Vector3 position)
    {
        var character = LocomotionController.CharacterController;
        character.enabled = false;
        var characterTransform = character.transform;

        Vector3 destPosition = position;
        destPosition.y = 2.3f;
        Quaternion destRotation = rotation;
        characterTransform.position = destPosition;

        character.enabled = true;
    }




    


}