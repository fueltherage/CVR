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
 
	// Use this for initialization
	void Start () {
        
		if(StartingState != null)
		{
			current = StartingState;	
		}
		else
		{
			current = CameraView.GamePlayView;
		}
		if(current == CameraView.GamePlayView)setGamePlayView();
		else setEditorView();

	}
	
	// Update is called once per frame
	void Update () {
		#region Input
		if(Input.GetKeyDown(EditorKey))
		{           
            GameState.currentState = GameState.GState.EditorMode;
			EditorView();
		}
		if(Input.GetKeyDown(GameplayKey))
		{            
            GameState.currentState = GameState.GState.Active;
			GamePlayView();
		}
		#endregion

//#if DEBUG
//		if(viewSwapCount > 1) Debug.Log("<color=Green>CameraState</color> - State was set multiple times in one update. Should only ever be accessed once.",this);
////#endif
//		if(current != past)
//		{		
//			switch (current) {
//				case CameraView.EditorView:
//				setEditorView();			                    
//				break;
//
//				case CameraView.GamePlayView:
//				setGamePlayView();
//				break;
//
//				default:
//				break;
//			}
//		}
//		past = current;	
//		viewSwapCount = 0;
	}
	void setEditorView()
	{
		EditorViewGO.SetActive(true);
		CurrentCamera = EditorViewGO.GetComponentInChildren<Camera>();
		GamePlayViewGO.SetActive(false);	
	}
	void setGamePlayView()
	{
		GamePlayViewGO.SetActive(true);	
		CurrentCamera = GamePlayViewGO.GetComponentInChildren<Camera>();
		EditorViewGO.SetActive(false);

	}
	public void EditorView()
	{
		viewSwapCount++;
		current = CameraView.EditorView;
		setEditorView();

	}
	public void GamePlayView()
	{
		viewSwapCount++;
		current = CameraView.GamePlayView;
		setGamePlayView();
	}
}
