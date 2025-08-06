#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;

public class npcattack : MonoBehaviour
{
    public GameObject weaponPrefab;
    public float weaponSpeed = 8f;
    public float attackRange = 3f;
    public float Cooltime = 1f;

    private npcsight npc_sight;
    private Transform target;
    private float lastAttackTime;
    private float epsilon = 0.2f; // 허용 오차 거리
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //난이도마다 다른 공격 속도, 쿨타임 차이
        if (GameManager.Instance.diff == 1)
        {
            weaponSpeed = 6f;
            Cooltime = 2f;
        }
        npc_sight = GetComponent<npcsight>();

        //무기 프리팹 없는 경우 오류
        if (weaponPrefab == null)
        {
            Debug.LogError("Weapon prefab could not be loaded from Resources folder. Check the path and filename.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (npc_sight.DetectPlayer && targetInRange())
        {
            Attack();
        }
    }



    void Attack() //공격
    {
        //쿨타임 안지났거나, 무기 프리팹 없으면 공격 안함
        if (Time.time - lastAttackTime < Cooltime || weaponPrefab == null) return;

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        audiomanager.Instance.npcattack.Play();

        //플레이어 방향 계산, 발사체 회전
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

    //공격 범위 확인
    bool targetInRange()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange + epsilon;
    }
}
