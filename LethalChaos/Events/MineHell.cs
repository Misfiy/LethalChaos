using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LethalChaos;

public class MineHell : EventBase
{
    public override string Text { get; } = "Mine Hell";
    public override Phase Phase { get; } = Phase.MoonLanding;
    public override void Activate()
    {
        for (int k = 0; k < RoundManager.Instance.shipSpawnPathPoints.Length; k++)
        {
            SpawnMines(RoundManager.Instance.shipSpawnPathPoints[k].position);
        }
    }

    private void SpawnMines(Vector3 original, int amount = 10, bool spawnMore = true)
    {
        for (int a = 0; a < amount; a++)
        {
            Vector3 pos =
                RoundManager.Instance.GetRandomNavMeshPositionInBoxPredictable(
                    original, 35, randomSeed: new(UnityEngine.Random.Range(1, 1000)), layerMask: -5);
            
            GameObject landmine = Object.Instantiate(Variables.Landmine, pos, Quaternion.identity);
            landmine.GetComponent<NetworkObject>().Spawn(true);
            
            if (spawnMore)
                SpawnMines(pos, 2, false);
        }
    }
}