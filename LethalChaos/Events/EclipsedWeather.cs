using UnityEngine;

namespace LethalChaos;

public class EclipsedWeather : EventBase
{
    public override string Text { get; } = "Eclipsed Moon";
    public override Phase Phase { get; } = Phase.Exploration;

    public override bool IsUseable =>
        base.IsUseable && TimeOfDay.Instance.currentLevelWeather != LevelWeatherType.Eclipsed;

    public override void Activate()
    {
        if (TimeOfDay.Instance.currentLevelWeather != LevelWeatherType.None)
        {
            TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectEnabled = false;
            if (TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectObject != null)
            {
                TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectObject.SetActive(false);
            }
            if (TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectPermanentObject != null)
            {
                TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectPermanentObject.SetActive(false);
            }
        }
        
        RoundManager.Instance.currentLevel.currentWeather = LevelWeatherType.Eclipsed;
        TimeOfDay.Instance.currentLevelWeather = LevelWeatherType.Eclipsed;
        TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectEnabled = true;
        
        if (TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectObject != null)
        {
            TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectObject.SetActive(true);
        }
        
        if (TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectPermanentObject != null)
        {
            TimeOfDay.Instance.effects[(int)TimeOfDay.Instance.currentLevelWeather].effectPermanentObject.SetActive(true);
        }
        
        for (int j = 0; j < RoundManager.Instance.currentLevel.randomWeathers.Length; j++)
        {
            if (RoundManager.Instance.currentLevel.randomWeathers[j].weatherType == RoundManager.Instance.currentLevel.currentWeather)
            {
                TimeOfDay.Instance.currentWeatherVariable = RoundManager.Instance.currentLevel.randomWeathers[j].weatherVariable;
                TimeOfDay.Instance.currentWeatherVariable2 = RoundManager.Instance.currentLevel.randomWeathers[j].weatherVariable2;
            }
        }

        GameObject.Find("EclipseObject").SetActive(true);
        RoundManager.Instance.minOutsideEnemiesToSpawn = TimeOfDay.Instance.currentWeatherVariable; 
        RoundManager.Instance.minEnemiesToSpawn = TimeOfDay.Instance.currentWeatherVariable;

        Utility.SpawnEnemyOutside(typeof(MouthDogAI), false);
        Utility.SpawnEnemyOutside(typeof(MouthDogAI), false);
        Utility.SpawnEnemyOutside(typeof(MouthDogAI), false);
    }
}