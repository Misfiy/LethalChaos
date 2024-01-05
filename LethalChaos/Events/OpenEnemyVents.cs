namespace LethalChaos;

public class OpenEnemyVents : EventBase
{
    public override string Text { get; } = "Open Vents";
    public override Phase Phase { get; } = Phase.Exploration;
    public override void Activate()
    {
        foreach (EnemyVent enemyVent in RoundManager.Instance.allEnemyVents.Where(v => !v.occupied))
        {
            enemyVent.enemyType = RoundManager.Instance.currentLevel.Enemies.GetRandom().enemyType;
            enemyVent.enemyTypeIndex =
                RoundManager.Instance.currentLevel.Enemies.FindIndex(e => e.enemyType == enemyVent.enemyType);
            enemyVent.occupied = true;
            RoundManager.Instance.SpawnEnemyFromVent(enemyVent);
        }
    }
}