using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    private Transform headTransform;
    private Transform cameraTransform;

    [Header("Bobbing")]
    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    [Range(0, 1)] public float headBobSmoothing = 0.1f;

    //State
    private float walkingTime;
    private float idleTime;
    private Vector3 targetCameraPosition;


    // Start is called before the first frame update
    void Start()
    {
        headTransform = transform.GetChild(0);
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.isWalking)
        {
            walkingTime = 0;
            idleTime += Time.deltaTime;
            //Calculate camera's target position
            targetCameraPosition = headTransform.position + CalculateHeadBobOffset(idleTime);
        } else
        {
            idleTime = 0;
            walkingTime += Time.deltaTime;
            //Calculate camera's target position
            targetCameraPosition = headTransform.position + CalculateHeadBobOffset(walkingTime);
        }

        //Interpolate position
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, headBobSmoothing);

        //Snap position if close enough
        if((cameraTransform.position - targetCameraPosition).magnitude <= 0.001)
        {
            cameraTransform.position = targetCameraPosition;
        }
    }

    private Vector3 CalculateHeadBobOffset(float time)
    {
        float horizontalOffset = 0;
        float verticalOffset = 0;
        Vector3 offset = Vector3.zero;

        if(time > 0)
        {
            if(PlayerController.isWalking)
            {
                if(PlayerController.isSprinting)
                {
                    horizontalOffset = Mathf.Cos(time * bobFrequency * 1.5f) * bobHorizontalAmplitude;
                    verticalOffset = Mathf.Sin(time * bobFrequency * 2 * 1.5f) * bobVerticalAmplitude;
                } else
                {
                    horizontalOffset = Mathf.Cos(time * bobFrequency) * bobHorizontalAmplitude;
                    verticalOffset = Mathf.Sin(time * bobFrequency * 2) * bobVerticalAmplitude;
                }
            } else
            {
                //horizontalOffset = Mathf.Cos(time * bobFrequency * 0) * bobHorizontalAmplitude;
                verticalOffset = Mathf.Sin(time * bobFrequency) * bobVerticalAmplitude;
            }


            offset = headTransform.right * horizontalOffset + headTransform.up * verticalOffset;
        }

        return offset;
    }
}
