using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float timer=1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timeOut());
    }

    IEnumerator timeOut(){
        yield return new WaitForSeconds(timer);
        Destroy(this);
    }

}
