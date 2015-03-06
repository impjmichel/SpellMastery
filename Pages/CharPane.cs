using UnityEngine;
using System.Collections;

public class CharPane : SpellBookPage 
{
	public GameObject CharSelect;
	public GameObject PrepareSpells;
	public GameObject CastSpells;

	public override void OnClick_Back()
	{
		BackButtonTarget = CharSelect;
		StartCoroutine(PageToRightPageAnimation());
	}

	public void OnClick_PrepareSpells()
	{
		BackButtonTarget = PrepareSpells;
		StartCoroutine(PageToRightPageAnimation());
	}

	public void OnClick_ReadyAllSpells()
	{
		storage.ReadyAllSpells();
		UpdateLabels();
	}

	public void OnClick_CastSpells()
	{
		BackButtonTarget = CastSpells;
		StartCoroutine(PageToRightPageAnimation());
	}

	private void OnEnable()
	{
		UILabel label = gameObject.transform.FindChild("CP_Title").GetComponent<UILabel>();
		label.text = storage.SelectedCharInfo();
		UpdateRankLabel();
		UpdateLabels();
	}

	private void UpdateRankLabel()
	{
		UILabel label = gameObject.transform.FindChild("CP_Label_Ranks").GetComponent<UILabel>();
		label.text = storage.SelectedCharRanksString();
	}

	private void UpdateLabels()
	{
		UILabel label = gameObject.transform.FindChild("CP_Label_Prepared").GetComponent<UILabel>();
		label.text = storage.SelectedCharNumbers(false);
		label = gameObject.transform.FindChild("CP_Label_Used").GetComponent<UILabel>();
		label.text = storage.SelectedCharNumbers(true);
	}
}
