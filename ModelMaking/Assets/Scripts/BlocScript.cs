using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocScript : MonoBehaviour
{
    public float speed = 1.0F;
    private Rigidbody rgdbdy;
    // Start is called before the first frame update
    void Start()
    {
        rgdbdy = gameObject.GetComponent<Rigidbody>();
        rgdbdy.velocity = new Vector3(0,0,-speed);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("rigidbody : " + rgdbdy.velocity.magnitude);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        
    }
}
