using UnityEngine;

/// <summary>
/// Just a simple script that lets the movement indicator rotate.
/// </summary>
public class MovementIndicator : MonoBehaviour
{
    // degrees per second. Can be set in the editor.
    public float rotateSpeed = 90.0f;

    private void Update()
    {
        // rotate around the y axis.
        transform.Rotate(new Vector3(.0f, rotateSpeed * Time.deltaTime, .0f));
    }
}