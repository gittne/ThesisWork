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

    private void Start()
    {
        brain = GetComponentInParent<SCR_EnemyBrain>();
        animator = GetComponentInParent<SCR_EnemyAnimator>();

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            KillPlayers(other.gameObject);

            if (brain.enemyState == SCR_EnemyUtilities.EnemyState.FINISHING)
            {
                SCR_MultiplayerOverlord.Instance.RespawnPlayers();
            }
        }
    }

    void KillPlayers(GameObject player)
    {
        player.transform.LookAt(transform.position);
        transform.LookAt(player.transform.position);

        if (SCR_MultiplayerOverlord.Instance != null)
        {
            Debug.Log("multiplayer kill.");
            player.GetComponent<SCR_First_Person_Controller>().CommencePlayerDeath(player);
            StartCoroutine(KillPlayerCutScene(true));
        }
        else
        {
            Debug.Log("singleplayer kill.");
            player.GetComponent<SCR_First_Person_Controller_Singleplayer>().CommencePlayerDeath(player);
            StartCoroutine(KillPlayerCutScene(false));
        }
    }

    IEnumerator KillPlayerCutScene(bool isMultiplayer)
    {
        animator.PlayKillAnimation();
        brain.EnterKillingState();
        Debug.Log("KILL THEM");
        yield return new WaitForSeconds(5);

        if(isMultiplayer)
            brain.CommenceMultiplayerFinish();
        else
            brain.CommenceRoam();
    }
}
