using UnityEngine;

public class npcchase : MonoBehaviour
{
    npcsight npc_sight;
    npccontroller npc_controller;

    public float stopChase;

    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc_sight = GetComponent<npcsight>();
        npc_controller = GetComponent<npccontroller>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    void Chase()
    {
        //player detected
        if (npc_sight.DetectPlayer == true && Vector2.Distance(transform.position, target.position) >= stopChase) 
        {
            npc_controller.moveSpeed *= 1.5f; //평소 속도의 1.5배
            transform.position = Vector2.MoveTowards(transform.position,target.position,Time.deltaTime);
        }
    }
}
