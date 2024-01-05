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
        }
    }

    void KillPlayers(GameObject player)
    {
        if (SCR_MultiplayerOverlord.Instance != null)
        {
            Debug.Log("multiplayer kill.");
            //SCR_MultiplayerOverlord.Instance.KillAllPlayersServerRpc();

            player.GetComponent<SCR_First_Person_Controller>().PlayerDie();
            brain.CommenceMultiplayerFinish();
        }
        else
        {
            Debug.Log("singleplayer kill.");
            player.GetComponent<SCR_First_Person_Controller_Singleplayer>().PlayerDie();
            StartCoroutine(KillPlayerCutScene());
            //player.transform.position = new Vector3(0, 0, 0);
        }
    }

    IEnumerator KillPlayerCutScene()
    {
        animator.PlayKillAnimation();
        brain.EnterKillingState();
        Debug.Log("KILL THEM");
        yield return new WaitForSeconds(3);
    }
}
