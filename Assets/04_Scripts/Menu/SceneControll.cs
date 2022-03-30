using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControll : MonoBehaviour
{
    public static SceneControll Instance;

	[SerializeField]
	private CanvasGroup _canvasGroup;
	[SerializeField]
	private Image _progressBar;

	private string _loadSceneName;

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
	}
#endregion

#region Public Functions
	public void LoadScene(string sceneName)
	{
		gameObject.SetActive(true);
		SceneManager.sceneLoaded += OnSceneLoaded;
		_loadSceneName = sceneName;
		StartCoroutine(LoadSceneProcess());
	}
#endregion

#region Private Functions
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (arg0.name == _loadSceneName)
		{
			StartCoroutine(Fade(false));
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}

	private IEnumerator Fade(bool isFadeIn)
	{
		float timer = 0f;
		while (timer <= 1f)
		{
			yield return null;
			timer += Time.unscaledDeltaTime * 3f;
			_canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
		}

		if (!isFadeIn)
		{
			gameObject.SetActive(false);
		}
	}

	private IEnumerator LoadSceneProcess()
	{
		_progressBar.fillAmount = 0f;
		yield return StartCoroutine(Fade(true));

		Debug.Log("Loading scene name : " + _loadSceneName);
		AsyncOperation ao = SceneManager.LoadSceneAsync(_loadSceneName);
		ao.allowSceneActivation = false;

		float timer = 0f;
		while (!ao.isDone)
		{
			yield return null;

			if (ao.progress < 0.9f)
			{
				_progressBar.fillAmount = ao.progress;
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				_progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
				if (_progressBar.fillAmount >= 1f)
				{
					ao.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
#endregion
}
