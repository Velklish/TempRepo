using Mirror;
using UnityEngine;

public class Knockable : NetworkBehaviour
{
    public float knockBackForce = 10.0f;
    public float knockbackTime = 0.5f;
    public float gravityScale = 3.0f;
    public GameObject hitEffect;
    
    private CharacterController controller;
    private Vector3 moveDirection;
    private float knockBackCounter;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent(typeof(CharacterController)) as CharacterController;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter >= 0)
        {
            knockBackCounter -= Time.deltaTime;
            moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            //transform.position += moveDirection * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root != gameObject.transform.root)
        {
            switch (other.tag)
            {
                case "Bat": Knockback(other); break;
                default: break;
            }
        }
    }

    private void Knockback(Collider other)
    {
        Instantiate(hitEffect, gameObject.transform.position, gameObject.transform.rotation);
        var vector = other.transform.position - transform.position;
        
        vector.y += 1;
        vector = vector.normalized;
        knockBackCounter = knockbackTime;
        vector.x = -vector.x;
        vector.z = -vector.z;
        moveDirection = vector * knockBackForce;
    }
}
