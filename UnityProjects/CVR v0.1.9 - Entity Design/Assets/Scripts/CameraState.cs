using UnityEngine;
using System.Collections;


public class CameraState : MonoBehaviour {

	public enum CameraView { EditorView, GamePlayView};
	public CameraView StartingState;
	public GameObject EditorViewGO;
	public GameObject GamePlayViewGO;
	public static Camera CurrentCamera;
	public KeyCode EditorKey;
	public KeyCode GameplayKey;
	int viewSwapCount=0;
	CameraView current;
	CameraView past;
    public float fogEndEditor = 100f;
    public float fogStartEditor = 10f;
    public float fogEndGameplay = 100f;
    public float fogStartGameplay = 10f;
	// Use this for initialization
	void Start () {
        RenderSettings.fogStartDistance = fogStartGameplay;
        RenderSettings.fogEndDistance = fogEndGameplay;
		if(StartingState != null)
		current = StartingState;
		else current = CameraView.GamePlayView;
	}
	
	// Update is called once per frame
	void Update () {
		#region Input
		if(Input.GetKeyDown(EditorKey))
		{
            RenderSettings.fogStartDistance = fogStartEditor;
            RenderSettings.fogEndDistance = fogEndEditor;
            GameState.currentState = GameState.GState.EditorMode;
			EditorView();
		}
		if(Input.GetKeyDown(GameplayKey))
		{
            RenderSettings.fogStartDistance = fogStartGameplay;
            RenderSettings.fogEndDistance = fogEndGameplay;
            GameState.currentState = GameState.GState.Active;
			GamePlayView();
		}
		#endregion

//#if DEBUG
		if(viewSwapCount > 1) Debug.Log("<color=Green>CameraState</color> - State was set multiple times in one update. Should only ever be accessed once.",this);
//#endif
		if(current != past)
		{		
			switch (current) {
				case CameraView.EditorView:
				EditorViewGO.SetActive(true);
				CurrentCamera = EditorViewGO.GetComponentInChildren<Camera>();
				GamePlayViewGO.SetActive(false);				                    
				break;

				case CameraView.GamePlayView:
				EditorViewGO.SetActive(false);
				CurrentCamera = GamePlayViewGO.GetComponentInChildren<Camera>();
				GamePlayViewGO.SetActive(true);	
				break;

				default:
				break;
			}
		}
		past = current;	
		viewSwapCount = 0;
	}
	public void EditorView()
	{
		viewSwapCount++;
		current = CameraView.EditorView;
	}
	public void GamePlayView()
	{
		viewSwapCount++;
		current = CameraView.GamePlayView;
	}
}
