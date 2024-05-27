using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private Camera camera;
    private Transform cameraTransform;
    private Vector3 cameraOriginVector3;
    
    [SerializeField] private float zoomMin = 1F;
    [SerializeField] private float zoomMax = 10F;
    [SerializeField] private float zoomAmount = 10F;
    private float nowZoom = 0F;

    private Vector3 touchStart;
    
    void Start()
    {
        cameraTransform = camera.transform;
        cameraOriginVector3 = cameraTransform.position;
    }

    // TODO : 터치 테스트
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            touchStart = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01F);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - camera.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {

        nowZoom += increment;
        nowZoom = Math.Clamp(nowZoom, zoomMin, zoomMax);
        cameraTransform.position = cameraOriginVector3 + cameraTransform.forward * (nowZoom * zoomAmount);
        
    }
}
