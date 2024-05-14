
using UnityEngine;

/// <summary>
/// Handles the movement of the skill coin.
/// Attach this to the model subobject so that the collider isn't moved as well.
/// </summary>
public class SkillCoinMovement : MonoBehaviour
{
    // specification decided:
    public const float SKILL_COIN_WIDTH = .5f;

    //// For now, only let the skill coin rotate and go up/ & down

    // degrees per second. Can be set in the editor.
    public float rotateSpeed = 60.0f;
    // the distance between the highest and lowest points it can go.
    public float upDownDistance = 2.0f;
    // the speed of going up & down. cycles per second.
    // A cycle is when it goes from the middle to the middle again.
    public float upDownSpeed = 1.0f;

    // going up and down uses a famous periodic function: Sine.
    // height = .5 * upDownDistance * sin(p)
    // this is p.
    private float currentUpDownProgress = 0.0f;

    private void Update()
    {
        // rotate the skill coin.
        transform.Rotate(new Vector3(.0f, rotateSpeed * Time.deltaTime, .0f));

        // make it go up and down.
        currentUpDownProgress += upDownSpeed * Utility.TWO_PI * Time.deltaTime;
        if(currentUpDownProgress > Utility.TWO_PI)
        {
            // to prevent overflow.
            currentUpDownProgress -= Utility.TWO_PI;
        }
        float height = .5f * upDownDistance * Mathf.Sin(currentUpDownProgress);
        Vector3 newPos = transform.position;
        newPos.y = height;
        transform.position = newPos;
    }
}