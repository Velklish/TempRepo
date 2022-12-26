using Mirror;
using UnityEngine;

public class Box : NetworkBehaviour
{
    public GameObject HitEffect;
    
    public void FixedUpdate()
    {
        var pos = this.transform.position;
        if (pos.y < -10)
        {
            Destroy(this);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        var curTransform = this.transform;
        var pos = curTransform.position;
        var rot = curTransform.rotation;

        if (isServer)
        {
            var obj = Instantiate(HitEffect, pos, rot);
            NetworkServer.Spawn(obj);
        }

        Destroy(gameObject);
    }
}
