using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SCR_EnemyKill_Multiplayer : MonoBehaviour
{
    SCR_EnemyBrain brain;
    SCR_EnemyAnimator animator;
    SCR_EnemyVision vision;

    int playersKilled;

    private void Start()
    {
        brain = GetComponentInParent<SCR_EnemyBrain>();
        animator = GetComponentInParent<SCR_EnemyAnimator>();
    }

    GameObject latestKilledPlayer;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            KillPlayers(other.gameObject);
        }
    }

    void KillPlayers(GameObject player)
    {
        player.transform.position = transform.position + transform.TransformDirection(Vector3.forward) * 1.5f;
        player.transform.LookAt(transform.position);
        transform.LookAt(player.transform.position);

        player.GetComponentInChildren<Animator>().SetTrigger("Die");

        if (SCR_MultiplayerOverlord.Instance != null && player.GetComponent<SCR_First_Person_Controller>() != null)
        {
            Debug.Log("multiplayer kill.");
            SCR_MultiplayerOverlord.Instance.PlayerLifeStatusServerRpc();

            latestKilledPlayer = player;
            StartCoroutine(KillPlayerCutScene(true));
        }
        else
        {
            Debug.Log("singleplayer kill.");
            player.GetComponent<SCR_First_Person_Controller_Singleplayer>().CommencePlayerDeath();
            StartCoroutine(KillPlayerCutScene(false));
        }
    }

    IEnumerator KillPlayerCutScene(bool isMultiplayer)
    {
        animator.PlayKillAnimation();
        brain.EnterKillingState();
        Debug.Log("KILL THEM");
        yield return new WaitForSeconds(5);

        if (isMultiplayer)
        {
            KillPlayerServerRpc();
            yield return new WaitForSeconds(3);
            brain.CommenceMultiplayerFinish();
        }
        else
            brain.ResetMonster();
    }

    [ServerRpc(RequireOwnership = false)]
    void KillPlayerServerRpc()
    {
        latestKilledPlayer.GetComponent<SCR_First_Person_Controller>().PlayerDeathClientRpc();
    }
}
