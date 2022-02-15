using System;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	GameObject[] planets;
	Rigidbody rigidbody;
	
	void Awake () {
		//get all planets
		planets = GameObject.FindGameObjectsWithTag("Planet");
		//get rigidbody of this object (whatever this script is attached to)
		rigidbody = GetComponent<Rigidbody> ();

		// Disable rigidbody gravity and rotation since we will make our own custom adjustments to gravity and rotation
		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	void FixedUpdate () {
		//initialize upVector of gravity to 0 vector
        Vector3 gravityUp = Vector3.zero;
		//our goal is to find which planet we are closest to for adjusting the camera
        float minDistance = 99999f;
		//for each gravity source, we want to apply its gravity and also check for its distance
        for(int i = 0; i < planets.Length; i++) {
			//get each planet
			GravitySource planet = planets[i].GetComponent<GravitySource>();
			//apply the gravity, set a temp vector3 to the up vector of that planet's gravity
            Vector3 gravityUpTemp = planet.ApplyGravity(rigidbody);
			//check if planet is closest planet, if so, set gravityUp to that planet's gravity's up vector
            if (planet.distance(rigidbody) < minDistance) {
                gravityUp = gravityUpTemp; 
                minDistance = planet.distance(rigidbody);
            }
        }
		//apply the rotation, note that the rotation must be multiplied. We rotate from the current rotation to the gravity's up vector
		rigidbody.rotation = Quaternion.FromToRotation(rigidbody.transform.up,gravityUp) * rigidbody.rotation; 

	}
}