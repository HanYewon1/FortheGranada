using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class npcattack : MonoBehaviour
{
    public GameObject weaponPrefab;
    public float weaponSpeed = 2f;
    public float Cooltime = 2f;

    private npcsight npc_sight;
    //private npcchase npc_chase;
    private Transform target;
    private float lastAttackTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc_sight = GetComponent<npcsight>();
      //  npc_chase = GetComponent<npcchase>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾� ����, ���� �Ÿ� ���� ��
        if(npc_sight.DetectPlayer==true && Vector2.Distance(transform.position, target.position) <= 4f)//4�� ���÷� ���� ��
        {
            //��Ÿ�� ���� ����
            if(Time.time - lastAttackTime >= Cooltime)
            {
                Attack();
                lastAttackTime = Time.time; 
            }
        }
    }

    void Attack()
    {
        GameObject weapon = Instantiate(weaponPrefab, transform.position,Quaternion.identity);
        Vector2 playerDirection = (target.position - transform.position).normalized;
        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.AddForce(playerDirection * Time.deltaTime);
        }
    }
}
