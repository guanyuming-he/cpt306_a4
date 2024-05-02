using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Two modes.
/// Camera always focuses on the boss, which is stationary.
/// 
/// Camera rotates around Focal in the X-Z plane.
///       hd
/// Cam  ____________
/// |   \    |
/// |    \   |
/// |     \  | vd
/// |      \ |
/// |       \|
///          Focal
///          
/// </summary>
public class CameraManager : MonoBehaviour
{
    /*********************************** STATIC ***********************************/

    // Note: in Unity: x is right, y is up, and z is forward.

    // see the above figure in comments
    public const float VD = 100.0f;
    // see the above figure in comments
    public const float HD = 50.0f;

    public const float PI = UnityEngine.Mathf.PI;
    public const float TWO_PI = 2.0f * UnityEngine.Mathf.PI;
    public const float HALF_PI = .5f * UnityEngine.Mathf.PI;

    /*********************************** FIELDS ***********************************/

    // The main camera
    Camera cam;

    // Its focalPoint on the boss.
    Vector3 focalPoint;

    // How much I should rotate the z axis counterclockwise
    // (when looking down from the top of the y axis)
    // to reach the lookat vector's projection on the X-Z plane.
    // The camera starts with angle = 0.
    // It is restricted between 0 to 2pi radians
    // It is in radians.
    float angle;

    /*********************************** CTOR ***********************************/

    public CameraManager()
    {
        angle = 0.0f;

        throw new System.NotImplementedException("Set focalPoint");
    }

    /*********************************** METHODS ***********************************/

    /// <summary>
    /// Rotates the camera to its right (when I am looking through the camera),
    /// that is, counterclockwise when looking down from the top of Y.
    /// </summary>
    /// <param name="rotateAngle">in radians</param>
    public void rotateLeft(float rotateAngle)
    {
        addAngle(rotateAngle);
        updateTrans();
    }

    /// <summary>
    /// Rotates the camera to its left (when I am looking through the camera)
    /// that is, clockwise when looking down from the top of Y.
    /// </summary>
    /// <param name="rotateAngle">in radians</param>
    public void rotateRight(float rotateAngle)
    {
        addAngle(-rotateAngle);
        updateTrans();
    }

    /*********************************** PRIVATE HELPERS ***********************************/

    /// <summary>
    /// Adds addition to angle, and round angle to [0,2pi) radians
    /// </summary>
    /// <param name="addition"></param>
    private void addAngle(float addition)
    {
        angle += addition;
        if(angle < 0.0f)
        {
            angle += TWO_PI;
        }
        else if(angle >= TWO_PI)
        {
            angle -= TWO_PI;
        }

        Utility.MyDebugAssert(angle >= 0.0f && angle < TWO_PI);
    }

    /// <summary>
    /// Updates the transformation of the camera after a change in angle.
    /// </summary>
    private void updateTrans()
    {
        // change the position.
        float angleBetweenLookatAndX = angle + HALF_PI;
        cam.transform.position =
            new Vector3
            (
                HD * UnityEngine.Mathf.Cos(angleBetweenLookatAndX),
                VD,
                HD * UnityEngine.Mathf.Sin(angleBetweenLookatAndX)
            );
        // make the camera look at the focal point after the position change.
        cam.transform.LookAt(focalPoint);
    }

    /*********************************** MONO ***********************************/
    /// <summary>
    /// Obtain the camera,
    /// and set up it with the boardd
    /// </summary>
    void Start()
    {
        // This is the main camera.
        cam = Camera.main;
        Utility.MyDebugAssert(cam != null, "should not be null");

        // angle = 0.0f; done in the ctor.
        // set up the initial transformation.
        updateTrans();
    }

    /// <summary>
    /// Not used for now.
    /// </summary>
    void Update()
    {

    }
}
