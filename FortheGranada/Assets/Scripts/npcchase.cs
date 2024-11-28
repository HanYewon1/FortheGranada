using UnityEngine;

public class npcchase : MonoBehaviour
{
    npcsight npc_sight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc_sight = GetComponent<npcsight>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    void Chase()
    {
        //player detected
        if (npc_sight.DetectPlayer == true)
        {

        }
    }
}
