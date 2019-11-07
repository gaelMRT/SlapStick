using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocScript : MonoBehaviour
{
    public float speed = 1.0F;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x,pos.y,pos.z-speed*Time.deltaTime);
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
