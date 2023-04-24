using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float speed = 3;

    [Space]
    [SerializeField] SpriteRenderer warningSign;

    [Min(0)]
    [SerializeField] float warningRadius = 5;

    [SerializeField] LayerMask enemyLayerMask;


    [Space]
    [SerializeField] GameObject bulletPrefab;

    [Min(0)]
    [SerializeField] float shotInterval;

    float lastShotTime = float.MinValue;

    // last movement in the x direction that we performed
    // this is the direction that the bullet move when fired
    float lastMoveXValue = 1;

    void Reset()
    {
        warningSign = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>())
            .Find(sr => sr.gameObject.name.ToLower().Contains("warning"));
    }

    void Start()
    {
        warningSign.enabled = false;
    }

    void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Movement
        transform.position += ((Vector3)input).normalized * speed * Time.deltaTime;

        if (input.x > 0) lastMoveXValue = 1;
        else if (input.x < 0) lastMoveXValue = -1;


        // Show warning symbol if an enemy is within warningRadius of us
        if (Physics2D.OverlapCircle(transform.position, warningRadius, enemyLayerMask) != null)
        {
            warningSign.enabled = true;
        }
        else
        {
            warningSign.enabled = false;
        }


        // Fire only if we haven't fired in the last lastShotTime seconds
        if(Time.time - lastShotTime > shotInterval && Input.GetButton("Fire1"))
        {
            Fire();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, warningRadius);
    }


    void Fire()
    {
        lastShotTime = Time.time;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Direction = new Vector2(lastMoveXValue, 0);
    }
}
