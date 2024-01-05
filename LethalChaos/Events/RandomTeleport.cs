using System.Collections;
using GameNetcodeStuff;
using UnityEngine;

namespace LethalChaos;

public class RandomTeleport : EventBase
{
    public override string Text { get; } = "Random Teleport";
    public override Phase Phase { get; } = Phase.Exploration;
    public override bool IsUseable => GameNetworkManager.Instance.localPlayerController.isInsideFactory;

    public override void Activate()
    {
        foreach (GameObject playerObject in StartOfRound.Instance.allPlayerObjects)
        {
            PlayerControllerB controller = playerObject.GetComponent<PlayerControllerB>();

            if (controller == null || controller.isPlayerDead) continue;

            RoundManager.Instance.StartCoroutine(DoTeleport(controller));
        }
        
        // PlayerControllerB playerController = GameNetworkManager.Instance.localPlayerController;
        //
        // playerController.averageVelocity = 0f;
        // playerController.velocityLastFrame = Vector3.zero;
        //
        // playerController.TeleportPlayer(RoundManager.Instance.allEnemyVents.ToList().GetRandom().transform.position);
        // playerController.isInElevator = false;
        // playerController.isInHangarShipRoom = false;
        // playerController.isInsideFactory = true;
        //
        // for (int i = 0; i < playerController.ItemSlots.Length; i++)
        // {
        //     if (playerController.ItemSlots[i] != null)
        //     {
        //         playerController.ItemSlots[i].isInFactory = true;
        //     }
        // }
    }

    private IEnumerator DoTeleport(PlayerControllerB controller)
    {
        controller.beamOutBuildupParticle.Play();

        yield return new WaitForSeconds(3.5f);
        
        controller.beamOutBuildupParticle.Stop();
        
        PlayerControllerB playerController = GameNetworkManager.Instance.localPlayerController;
        
        playerController.averageVelocity = 0f;
        playerController.velocityLastFrame = Vector3.zero;
        
        playerController.TeleportPlayer(RoundManager.Instance.allEnemyVents.ToList().GetRandom().transform.position);
        playerController.isInElevator = false;
        playerController.isInHangarShipRoom = false;
        playerController.isInsideFactory = true;
        
        for (int i = 0; i < playerController.ItemSlots.Length; i++)
        {
            if (playerController.ItemSlots[i] != null)
            {
                playerController.ItemSlots[i].isInFactory = true;
            }
        }
        
        // yield return new WaitForSeconds(1);
    }
}