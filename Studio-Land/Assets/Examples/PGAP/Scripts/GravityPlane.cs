using UnityEngine;

public class GravityPlane : GravitySource {

	[SerializeField]
	float gravity = 9.81f;
    [SerializeField, Min(0f)]
	float range = 1f;

    void OnDrawGizmos () {
        Vector3 scale = transform.localScale;
		scale.y = range;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
		Vector3 size = new Vector3(1f, 0f, 1f);
		
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(Vector3.zero, size);
        if (range > 0f) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(Vector3.up, size);
        }
	}

	public override Vector3 GetGravity (Vector3 position) {
		// TODO: Vector3.up is not the correct vector here because it's the global up, not up for the player!
		Vector3 up = Vector3.up;

		float distance = Vector3.Dot(up, position - transform.position);

		// TODO: Return 0 (a 0-vector) if the distance from the player to the plane is greater than the plane's range
		if (true) {
			
		}
		
		// Gravity should be negative (downward), so inverse the inputted gravity
		float g = -gravity;

		// TODO: Apply the plane's gravity if the distance is positive and within range of the plane
		// BONUS: Make the gravity weaker when the player is further away (but still within range)
		if (true) {
			
		}

		// TODO: Apply the gravity scalar to the local up vector by multiplying
		return Vector3.zero;
	}
}