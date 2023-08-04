using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFX : MonoBehaviour
{

    public float timeout = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BloodTimeout());
    }

    IEnumerator BloodTimeout()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(timeout);

        Destroy(gameObject);
    }
}
