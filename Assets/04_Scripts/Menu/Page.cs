using System.Collections;
using UnityEngine;

public class Page : MonoBehaviour
{
    public PageType _type;
}

public enum PageType
{
// Menu Pages
	None = 0,
	Title_Page,
	Option_Page,
//	InGame Pages
	Game_Interfaces_Page,
	Game_Buliding_Page,
	Game_Unit_Merge_Page,
	Game_Over_Page
}