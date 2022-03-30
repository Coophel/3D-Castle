using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
    public GameState _gameState;
	[SerializeField]
	public SceneControll SceneControll;

	public Dictionary<string, GameObject> ListOfUpgradeUnit = new Dictionary<string, GameObject>();

	/// <summary> Game asset
	private int _playerGold;
	private int _playerWorker;
	private int _playerStage;

	/// <summary> Game asset Get Set
	public int PlayerGold { get { return _playerGold; } set { _playerGold = value; }}
	public int PlayerWorker { get { return _playerWorker; } set { _playerGold = value; }}
	public int PlayerStage { get { return _playerStage; }}

#region Unity Functions
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
		_gameState = GameState.None;
	}

	private void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.6f);
	}
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
}

public enum GameState
{
	None = 0,
	Ready,
	Combat,
	Result,
}