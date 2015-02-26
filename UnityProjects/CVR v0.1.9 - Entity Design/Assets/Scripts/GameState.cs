using UnityEngine;
using System.Collections;

public static class GameState {
    public enum GState {EditorMode, Active};
    public static GState currentState = GState.Active;

    public static bool gamePaused = false;
}
