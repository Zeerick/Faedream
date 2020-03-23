using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is a basic 3rd-person perspective implementation.
 */
public class CameraMovement : MonoBehaviour
{
    public GameObject target;  // Target which the camera follows.
    public Vector2 cameraRotationCenter = new Vector3(0.5f, 1f);  // Position relative to the target where the camera rotates around.
                                                                  // x stands for horizontal shift (right being positive).
                                                                  // y stands for vertical shift (upward being positive).
                                                                  // There is no z value, as it is offset by cameraDefaultDistance.
    
    public float rotationSpeedX = 1f;   // Index of camera horizontal rotation speed.
    public float rotationSpeedY = 1f;   // Index of camera vertical rotation speed.
    public float upperPitchConstraint = 70f;    // Limit of angle of camera rotation going up.
    public float lowerPitchConstraint = 70f;    // Limit of angle of camera rotation going down.
    
    public float cameraDefaultDistance = 3f;  // Default distance between camera and camera rotation center.
    public float cameraZoomInConstraint = 1f;   // Minimum distance of camera distance from cameraRotationCenter,
    public float cameraZoomOutConstraint = 5f;  // Maximun distance of camera distance from cameraRotationCenter,

    private float mouseInputX = 0f;  // Basically represents horizontal rotation reference (positive for going right).
    private float mouseInputY = 0f;  // Basically represents vertical rotation reference (positive for going upward).
    private float mouseScroll = 0f;  // Representing mouse wheel srolling input.

    private float cameraDistance;

    void Awake()
    {
        // Below are in case that the camera rotates to slightest upside-down, which is generally a sensible setting.
        if (upperPitchConstraint > 90)
        {
            throw new Exception("upperPitchConstraint should be less than 90. Found: " + upperPitchConstraint);
        }
        if (lowerPitchConstraint < -90)
        {
            throw new Exception("lowerPitchConstraint should be greater than -90. Found: " + lowerPitchConstraint);
        }

        if (lowerPitchConstraint > upperPitchConstraint)
        {
            throw new Exception("lowerPitchConstraint (" + lowerPitchConstraint + ") should be less than upperPitchConstraint (" + upperPitchConstraint + "). ");
        }

        cameraDistance = cameraDefaultDistance;
    }

    void FixedUpdate()
    {
        mouseInputX += Input.GetAxisRaw("Mouse X") * rotationSpeedX;    // Read input of mouse moving horizontally.
        mouseInputY += Input.GetAxisRaw("Mouse Y") * rotationSpeedY;    // Read input of mouse moving vertically.
        mouseInputY = Mathf.Clamp(mouseInputY, -lowerPitchConstraint, upperPitchConstraint);    // Apply constraints to camera pitch.

        transform.rotation = Quaternion.Euler(-mouseInputY, mouseInputX, 0);    // Formally rotates camera.

        // This block will guarantee that, with camera rotating around, it always looks at the desired relative position, despite non-default values in x of cameraRotationCenter. 
        // For example, if camera is set shifting to the right, the player will always present in left half of the screen. 
        float rotationY_angle = (float) (transform.rotation.eulerAngles.y * Math.PI / 180);    // Mathematically calculates the delta horizontal rotation angle of camera. 
        Vector3 cameraRotationCenter_local = new Vector3(
            (float) (Math.Cos(rotationY_angle) * cameraRotationCenter.x), 
            cameraRotationCenter.y, 
            (float) - (Math.Sin(rotationY_angle) * cameraRotationCenter.x)
            );  
        // Calculates by each frame the relative position of local camera rotation center.
        
        Vector3 cameraLookPoint = target.transform.position + cameraRotationCenter_local;  // Converts the relative position into global position.

        mouseScroll = Input.mouseScrollDelta.y; // Read mouse wheel scroll input by each frame.
        cameraDistance -= mouseScroll * 0.2f;   // Pass the zooming information to cameraDistance with coefficient 0.2
        cameraDistance = Mathf.Clamp(cameraDistance, cameraZoomInConstraint, cameraZoomOutConstraint);  // Constraint camera distance between given values.
        if(Input.GetMouseButtonDown(2))
        {
            cameraRotationCenter = new Vector2(-cameraRotationCenter.x, cameraRotationCenter.y);    // Switching camera between left and right shoulders.
        }

        transform.position = cameraLookPoint - transform.forward * cameraDistance;  // Keeps camera behind the player by given cameraDefaultDistance. 
    }

    private void OnCollisionEnter(Collision other)
    {
        // To be implemented. This tackles the camera positioning when it hits the environment, which means the environment is in the way of camera view.
        throw new NotImplementedException();
    }
}
