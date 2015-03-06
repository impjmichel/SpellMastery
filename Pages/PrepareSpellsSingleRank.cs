using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrepareSpellsSingleRank : SpellBookPage
{
	private void OnEnable()
	{
		SetTitleText();
	}

	private void Update()
	{
		if (storage.selectedRank >= 0)
		{
			UpdateLabels();
		}
	}

	private void SetTitleText()
	{
		string text = "Rank:   " + storage.selectedRank;
		if (!storage.selectedRankMain)
		{
			text += "\n";
			CharClassEnum cclass = storage.selectedChar.cclass;
			if (cclass == CharClassEnum.Wizard)
			{
				text += ((MagicSchool)(storage.selectedChar.attributes[0])).ToString();
				text += " spell";
			}
			else if (cclass == CharClassEnum.Cleric)
			{
				text += ((ClericDomain)(storage.selectedChar.attributes[0])).ToString();
				text += "/" + ((ClericDomain)(storage.selectedChar.attributes[1])).ToString();
				text += " spell";
			}
		}
		UILabel label = gameObject.transform.FindChild("PSSR_Title").GetComponent<UILabel>();
		label.text = text;
	}

	private void UpdateLabels()
	{
		UILabel prepared = gameObject.transform.FindChild("PSSR_prepared").GetComponent<UILabel>();
		UILabel available = gameObject.transform.FindChild("PSSR_available").GetComponent<UILabel>();
		if (storage.selectedRankMain)
		{
			prepared.text = storage.SelectedCharNumbers(false, storage.selectedRank);
			available.text = storage.SelectedCharNumbers(true, storage.selectedRank);
		}
		else
		{
			prepared.text = storage.SelectedCharExtraSpell(false, storage.selectedRank);
			available.text = storage.SelectedCharExtraSpell(true, storage.selectedRank);
		}
	}
}
