using UnityEngine;

public class HitNearest : MonoBehaviour
{
    public string targetTag = "Target"; // The tag of the target game object
    public float speed = 10f; // Missile speed

    private Transform target; // Transform of the target
    private bool hasTarget = false; // Indicates if the missile has a target

    private void Start()
    {
        FindTarget();
    }

    private void Update()
    {
        if (hasTarget)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;

            // Move the missile towards the target
            transform.Translate(direction * speed * Time.deltaTime);
            // Rotate the missile towards the target
            //Quaternion targetRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        else
        {
            // If target is lost, try to find a new one
            FindTarget();
        }
    }

    private void FindTarget()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
    }
}
