using System.Collections;
using UnityEngine;

public class PageControll : MonoBehaviour
{
    public static PageControll Instance;

	[SerializeField]
	PageType _entryPage;
	[SerializeField]
	Page[] _usingPages;

	private Hashtable _pages;

#region Unity Functions
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			_pages = new Hashtable();
			RegisterAllPages();

			if (_entryPage != PageType.None)
				TurnPageOn(_entryPage);
		}
	}
#endregion

#region Public Functions
	public void TurnPageOn(PageType onPage)
	{
		if (onPage == PageType.None)
			return;

		if (!PageExists(onPage))
			return;

		Page page = GetPage(onPage);

		page.gameObject.SetActive(true);
	}

	public void TurnPageOff(PageType offPage, PageType onPage = PageType.None)
	{
		if (offPage == PageType.None)
			return;

		if (!PageExists(offPage))
			return;

		Page off = GetPage(offPage);

		if (off.gameObject.activeSelf)
		{
			off.gameObject.SetActive(false);
		}

		if (onPage != PageType.None)
		{
			Page on = GetPage(onPage);
			TurnPageOn(onPage);
		}
	}
#endregion

#region Private Functions
	private void RegisterAllPages()
	{
		foreach (var page in _usingPages)
		{
			RegisterPage(page);
		}
	}

	private void RegisterPage(Page page)
	{
		if (PageExists(page._type))
			return;

		_pages.Add(page._type, page);
	}

	private Page GetPage(PageType type)
	{
		if (!PageExists(type))
			return null;

		return (Page)_pages[type];
	}

	private bool PageExists(PageType type)
	{
		return _pages.ContainsKey(type);
	}
#endregion
}
