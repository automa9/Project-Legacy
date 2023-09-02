using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timer=0.01f;
    public float muzzletimer = 0.10f;
    private float t;
    
    public GameObject muzzle;
    private MeshRenderer projectile;

    // Start is called before the first frame update

    void Awake()
    {
        projectile = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        projectile.enabled = false;
        StartCoroutine(timeOut());
        Destroy(muzzle,muzzletimer);
        GameObject newMuzzle = Instantiate(muzzlePrefab, transform.position,transform.rotation);
        Destroy(newMuzzle,muzzletimer);
    }

    private void Update ()
    {
        t += Time.deltaTime; 

        if (t >= 0.5)
        {
            projectile.enabled = true;
            //launch train, an so one conditions
        }
    }

    IEnumerator timeOut(){
        yield return new WaitForSeconds(timer);
        Destroy(this);
    }


}
