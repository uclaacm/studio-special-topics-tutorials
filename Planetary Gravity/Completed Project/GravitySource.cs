
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    //we can set gravity and range
    public float gravity = -9.8f;
    public float range = 5f;
    //called by each gravity body

   
    public Vector3 ApplyGravity(Rigidbody body) {
        //quaternion.fromtorotation - current direction and direction we want to face
        //represents the up axis for the body
        Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;
		
		// Apply downwards gravity to body
        if(Vector3.Distance(body.position, transform.position) < range) {
		    body.AddForce(gravityUp * gravity);
            return gravityUp;
        }
        return Vector3.zero;
		// align bodies up axis with the centre of planet
    }

    public float distance(Rigidbody body) {
         float radius = GetComponent<SphereCollider>().radius;
        return Vector3.Distance(body.position, transform.position + radius*(body.position - transform.position).normalized);
    }
}