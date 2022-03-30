using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControll : MonoBehaviour
{
    
#region Title_Panel Button
	public void Game_StartButton()
	{
		GameManager.Instance._gameState = GameState.Ready;
		GameManager.Instance.SceneControll.LoadScene("TestScene");
	}

	public void Option_PanelButton()
	{
		PageControll.Instance.TurnPageOff(PageType.Title_Page, PageType.Option_Page);
	}

	public void Back_Option_PanelButton()
	{
		PageControll.Instance.TurnPageOff(PageType.Option_Page, PageType.Title_Page);
	}

	public void Quit_GameButton()
	{
		Application.Quit();
	}
#endregion

#region Game_InterFace_Panel
	public void Game_BulidingMode()
	{
		PageControll.Instance.TurnPageOff(PageType.Game_Interfaces_Page, PageType.Game_Buliding_Page);
	}
#endregion

#region Game_Buliding_Panel
	public void Game_Bulid_To_InterFace()
	{
		PageControll.Instance.TurnPageOff(PageType.Game_Buliding_Page, PageType.Game_Interfaces_Page);
	}
#endregion
}
