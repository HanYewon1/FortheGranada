using UnityEngine;
using System.Collections.Generic;

public class npcchase : MonoBehaviour
{
    npcsight npc_sight;
    npccontroller npc_controller;

    public float stopChase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc_sight = GetComponent<npcsight>();
        npc_controller = GetComponent<npccontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (npc_sight.DetectPlayer == true && !npc_controller.isChasing)
        {
            Debug.Log("Start chasing player.");
            npc_controller.isChasing = true;
            npc_controller.StartChasing(); // 추격 시작
            // 추격 로직 (DFS 등)
        }
        else if (!npc_sight.DetectPlayer && npc_controller.isChasing)
        {
            if (npc_sight.Target == null || Vector2.Distance(transform.position, npc_sight.Target.position) > stopChase)
            {
                Debug.Log("Stop chasing player. Returning to patrol.");
                npc_controller.isChasing = false;
                npc_controller.StopChasing(); // 추격 중단 후 순찰 복귀
            }
        }
    }
}

  