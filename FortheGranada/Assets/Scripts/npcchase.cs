using UnityEngine;
using System.Collections.Generic;

public class npcchase : MonoBehaviour
{
    npcsight npc_sight;
    npccontroller npc_controller;

    public float stopChase;

    private Transform target;
    private int[,] gridMap = {
        { 0, 0, 0, 1, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 1, 0, 0, 0 },
        { 0, 0, 0, 1, 0 },
        { 0, 1, 0, 0, 0 }
    };

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
    }
}
