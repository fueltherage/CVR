using UnityEngine;
using System.Collections;

public static class GameGod {

    public static float PercentInfected=0.0f;
    public static float HostHealth=100.0f;

    public static void Reset()
    {
        PercentInfected = 0.0f;
        HostHealth = 100.0f;
    }

}
