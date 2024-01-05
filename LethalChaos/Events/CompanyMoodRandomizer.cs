namespace LethalChaos;

public class CompanyMoodRandomizer : EventBase
{
    public override string Text { get; } = "Buy Rate randomizer";
    public override Phase Phase { get; } = Phase.Company;

    public override void Activate()
    {
        StartOfRound.Instance.companyBuyingRate *= UnityEngine.Random.Range(0.9f, 1.5f);
    }
}