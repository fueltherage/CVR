using UnityEngine;
using System.Collections;

public static class GameState {
    public enum GState {EditorMode, Active};
    public static GState currentState = GState.EditorMode;

    public static bool gamePaused = false;
}
