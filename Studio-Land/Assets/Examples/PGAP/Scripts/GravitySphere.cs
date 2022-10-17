using UnityEngine;

public class GravitySphere : GravitySource {

	[SerializeField]
	float gravity = 9.81f;

	[SerializeField, Min(0f)]
	float outerRadius = 10f, outerFalloffRadius = 15f;

    [SerializeField, Min(0f)]
	float innerFalloffRadius = 1f, innerRadius = 5f;

    float innerFalloffFactor, outerFalloffFactor;

    void Awake () {
		OnValidate();
	}

	// Function that Unity calls when the script is loaded or a value changes in the Inspector.
	void OnValidate () {
		outerRadius = Mathf.Max(outerRadius, innerRadius);					// Outer radius >= inner radius
		outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);	// Outer falloff radius >= outer radius
		// TODO: innerRadius and innerFalloffRadius
		
		outerFalloffFactor = 1f / (outerFalloffRadius - outerRadius);
		// TODO: innerFalloffFactor
	}
	
	void OnDrawGizmos () {
		Vector3 center = transform.position;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(center, outerRadius);
		// Only draw the outer falloff radius's sphere if it doesn't intersect with the outer radius's
		if (outerFalloffRadius > outerRadius) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(center, outerFalloffRadius);
		}

		if (innerRadius > 0f && innerRadius < outerRadius) {
			// TODO: Draw a wire sphere for the innerRadius
		}
		// TODO: Draw a wire sphere for the inner falloff radius if it doesn't intersect with the inner radius's sphere
		if (true) {

		}
	}

    public override Vector3 GetGravity (Vector3 playerPosition) {
		// Get vector from the player's position to the center of the sphere
		Vector3 playerToCenter = transform.position - playerPosition;
		float distance = playerToCenter.magnitude;

		// We only get gravity from this sphere if the player is the right distance from the center
		if (distance > outerFalloffRadius || distance < innerFalloffRadius) {
			return Vector3.zero;
		}

		float g = gravity / distance;
		// If the player is further than the outer radius but not further than the outer falloff radius, use a reduced gravity
		if (distance > outerRadius) {
			g *= 1f - (distance - outerRadius) * outerFalloffFactor;
		}
		// TODO: Implement inner falloff reduced gravity
		
		return g * playerToCenter;
	}
}