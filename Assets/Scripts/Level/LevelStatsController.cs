using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatsController
{
    private FloatStatistic levelTime;

    public LevelStatsController()
    {
        levelTime = new FloatStatistic("Level Time", 0f);
    }

    public void UpdateTime(float timeDelta)
    {
        this.levelTime.AddToValue(timeDelta);
    }

    public FloatStatistic LevelTime
    {
        get
        {
            return levelTime;
        }
    }
}
