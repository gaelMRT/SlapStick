using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject sparkleParticle;
    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Sword")){
            ContactPoint contact = other.contacts[0];
            GameObject sparkle = Instantiate(sparkleParticle,contact.point,Quaternion.identity);
            sparkle.GetComponent<ParticleSystem>().Play();
        }
        if(other.collider.CompareTag("BlocSlice")){
            
        }
    }
}
