using UnityEngine;
using System.Collections;

public static class GameState {
    public enum GState { EditorMode, Active };
    public static GState currentState = GState.Active;

    public static bool gamePaused = false;
    public static bool ControllerEnabled = false;
	public static bool OculusEnabled = false;

	public delegate void OculusModeEventHandler(bool enabled);
	public static event OculusModeEventHandler OculusModeUpdater;

	public static void SwitchOculusMode(bool enabled)
	{
		if(OculusModeUpdater != null)
		{
			OculusModeUpdater(enabled);
		}
	}

	public static int VirusCount=0;
	public static int BloodCellCount =0;


}
