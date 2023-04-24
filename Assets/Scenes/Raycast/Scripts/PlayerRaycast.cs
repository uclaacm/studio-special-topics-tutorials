using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
enum DebugRayType {
    Debug,
    DebugContinuous,
    Gizmos,
    Handles,
    HandlesDotted
}

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] DebugRayType debugRayType = DebugRayType.Debug;

    List<Ray> rays = new List<Ray>();

    void Update()
    {
        // LMB
        if (Input.GetMouseButtonDown(0))
        {
            var ray = new Ray(transform.position, transform.forward);

            if(debugRayType == DebugRayType.Debug)
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            }
            else
            {
                rays.Add(ray);
            }

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayerMask))
            {
                Destroy(hit.collider.gameObject);
            }
        }

        if(debugRayType == DebugRayType.DebugContinuous) {
            foreach (var ray in rays)
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            }
        }
    }

    void OnDrawGizmos()
    {
        if(debugRayType == DebugRayType.Gizmos)
        {
            foreach (var ray in rays)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(ray.origin, ray.direction * 100);
            }
        }
#if UNITY_EDITOR
        else if (debugRayType == DebugRayType.Handles)
        {
            foreach(var ray in rays)
            {
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(
                    ray.origin,
                    ray.origin + 100 * ray.direction
                );
            }
        }
        else if(debugRayType == DebugRayType.HandlesDotted)
        {
            foreach (var ray in rays)
            {
                Handles.color = Color.red;
                Handles.DrawDottedLine(
                    ray.origin,
                    ray.origin + 100 * ray.direction,
                    10
                );
            }
        }
#endif
    }
}