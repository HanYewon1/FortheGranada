using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class npcattack : MonoBehaviour
{
    public GameObject weaponPrefab;
    public float weaponSpeed = 3f;
    public float attackRange = 4f;
    public float Cooltime = 1f;

    private npcsight npc_sight;
    private Transform target;
    private float lastAttackTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        npc_sight = GetComponent<npcsight>();
    }
    void Update()
    {
        if (npc_sight.DetectPlayer && targetInRange())
        {
            Attack();
        }
    }


    {
        if (Time.time - lastAttackTime < Cooltime) return;

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);

        Vector2 playerDirection = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

        weapon.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(playerDirection * weaponSpeed, ForceMode2D.Impulse);
        }
        lastAttackTime = Time.time;
    }
    bool targetInRange()
    {

            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange;
    }

}
