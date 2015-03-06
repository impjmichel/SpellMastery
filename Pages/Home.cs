using UnityEngine;
using System.Collections;

public class Home : SpellBookPage
{
	public GameObject PanelCharSelect;
	public GameObject PanelCharCreate;

	public void OnClick_CharSelect()
	{
		BackButtonTarget = PanelCharSelect;
		StartCoroutine(PageToRightPageAnimation());
	}

	public void OnClick_CharCreate()
	{
		BackButtonTarget = PanelCharCreate;
		StartCoroutine(PageToRightPageAnimation());
	}

	public void OnClick_Reset()
	{
		PlayerPrefs.DeleteAll();
		storage.ResetCharacterList();
	}
}
