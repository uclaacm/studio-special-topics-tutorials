using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [System.NonSerialized] public Vector2 Direction;

    [SerializeField] LayerMask enemyLayerMask;

    [Min(0)]
    [SerializeField] float speed;

    [Min(0)]
    [SerializeField] float lifeTime;

    [Min(0)]
    [SerializeField] float damage;

    float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        transform.position += (Vector3)Direction * speed * Time.deltaTime;
        if(Time.time > startTime + lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // in the layermask, the layer is the bit number
        // 1 << n sets the nth bit to 1
        if ((enemyLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<Enemy>().Hit(damage);
            Destroy(gameObject);
        }
    }
}
