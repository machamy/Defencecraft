using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Ingame;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// 이대로면인게임 안에서만 사용되어야할듯?
/// </remarks>
public class InputManager : MonoBehaviour
{

    /// <summary>
    /// 카메라 앵커
    /// </summary>
    [SerializeField] private QuarterviewCamera qCamera;
    /// <summary>
    /// 실제 카메라
    /// </summary>
    private Camera camera;
    private Transform cameraTransform;
    
    [SerializeField] private float zoomMin = 1F;
    [SerializeField] private float zoomMax = 10F;
    [SerializeField] private float zoomAmount = 10F;
    private float nowZoom = 0F;

    private Vector3 touchStart;
    private bool touchPrev;

    private Vector3 prevMouse;
    
    void Start()
    {
        if (!qCamera)
            return;
        camera = qCamera.GetComponentInChildren<Camera>();
        cameraTransform = camera.transform;
        touchStart = default;
        touchPrev = false;
    }

    // TODO : 터치 테스트
    void Update()
    {
        //TODO 카메라 가동범위 제한
        //TODO 현재 씬 확인 절차 필요(카메라 관련은 카메라 클래스로 옮기는게 나을듯
#if UNITY_EDITOR
        if(qCamera){
            float increment = Input.GetAxis("Mouse ScrollWheel");
            if(increment is <= -0.001f or >= 0.001f)
                zoom(increment);

            

            Vector3 worldDeltaPos = qCamera.transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")));
            worldDeltaPos.y = 0;
            worldDeltaPos.Normalize();
            qCamera.transform.Translate(worldDeltaPos,Space.World);
        }
#else

        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);
            
            Vector3 cameraDeltaPos = cameraTransform.right * touchZero.deltaPosition.x +
                                     cameraTransform.up * touchZero.deltaPosition.y;

            Vector3 worldDeltaPos = camera.GetComponent<QuarterviewCamera>().GetWorldDiretion(cameraDeltaPos);
            
            camera.transform.Translate(worldDeltaPos);
        }
        else 
#endif
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
        if (Input.GetMouseButton(0) && touchPrev)
        {
            Vector3 direction = touchStart - camera.ScreenToWorldPoint(Input.mousePosition);
            qCamera.transform.position += direction;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            touchPrev = true;
            touchStart = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
            touchPrev = false;
    }

    void zoom(float increment)
    {

        nowZoom += increment;
        nowZoom = Math.Clamp(nowZoom, zoomMin, zoomMax);
        cameraTransform.position = qCamera.transform.position + cameraTransform.forward * (nowZoom * zoomAmount);
        
    }
}
