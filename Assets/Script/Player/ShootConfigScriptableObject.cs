using UnityEngine;

[CreateAssetMenu(fileName ="ShootConfig",menuName= "Guns/Shoot Configuration",order = 2)]
public class ShootGunScriptableObject : ScriptableObject
{
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0.1f,0.1f,0.1f);
    public float fireRate = 0.25f;
}
