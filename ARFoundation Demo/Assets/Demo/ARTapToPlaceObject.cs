using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject gameObjectIndicator;
    public GameObject gameObjectInstance;

    private GameObject spawnedObjectIndicator;
    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastmanager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isObjectBrought = false;

    private void Awake()
    {
        _arRaycastmanager = GetComponent<ARRaycastManager>();
    }

    public void placeObject()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        _arRaycastmanager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        var hitPose = hits[0].pose;
        var cameraForward = Camera.current.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        hitPose.rotation = Quaternion.LookRotation(cameraBearing);
        

        if(spawnedObject == null)
        {
            spawnedObject = Instantiate(gameObjectInstance, hitPose.position, hitPose.rotation);
        }
        else
        {
            spawnedObject.transform.position = hitPose.position;
            spawnedObject.transform.rotation = hitPose.rotation;
            print(hitPose.rotation);
        }
        isObjectBrought = false;   
    }

    public void bringToScren()
    {
        if (spawnedObject != null)
        {
            spawnedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            isObjectBrought = true;
        }   
            
    }

    void Update()
    {
        if (isObjectBrought == true)
        {
            spawnedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
        }

        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        
        _arRaycastmanager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        var placementPose = hits[0].pose;
        var cameraForward = Camera.current.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        placementPose.rotation = Quaternion.LookRotation(cameraBearing);


        if(spawnedObjectIndicator == null)
        {
            spawnedObjectIndicator = Instantiate(gameObjectIndicator, placementPose.position, placementPose.rotation);
        }
        else
        {
            spawnedObjectIndicator.transform.position = placementPose.position;
            spawnedObjectIndicator.transform.rotation = placementPose.rotation;
        }    
    }
}
