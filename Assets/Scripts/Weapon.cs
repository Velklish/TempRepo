using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject WeaponObject;
    private BoxCollider _collider;
    
    void Start()
    {
        _collider = WeaponObject.GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void AttackStart()
    {
        _collider.enabled = true;
    }

    public void AttackEnd()
    {
        _collider.enabled = false;
    }
}
