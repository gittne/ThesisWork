using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(0, 1, 0);
    }
    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1.5f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.5f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1.5f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1.5f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.L) && IsHost)
        {
            SpawnEnemy();
        }
    }

    [SerializeField] private Transform prefab;

    void SpawnEnemy()
    {
        Transform enemyTransform = Instantiate(prefab);
        enemyTransform.GetComponent<NetworkObject>().Spawn(true);
    }

}
