using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrepareSpellsSingleRankCreator : MonoBehaviour 
{
	public Storage storage;
	public GameObject prefab;

	private void OnEnable()
	{
		StartCoroutine(CreateSpellButtons());
	}

	private IEnumerator Clean()
	{
		transform.parent.localPosition = new Vector3(0,300,0);
		while (transform.childCount > 0)
		{
			yield return null;
			GameObject.Destroy(transform.GetChild(0).gameObject);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator CreateSpellButtons()
	{
		yield return StartCoroutine(Clean());
		yield return null;
		int rank = storage.selectedRank;
		CharClassEnum cclass = storage.selectedChar.cclass;
		List<Spell> spells = new List<Spell>();
		if (storage.selectedRankMain)
		{
			// main class spells
			spells.AddRange(storage.spellList.GetSpellsOfClassAndRank(cclass, rank));
		}
		else
		{
			// extra spells (domains / school-spec)
			if (cclass == CharClassEnum.Cleric)
			{
				ClericDomain dom_1 = (ClericDomain)storage.selectedChar.attributes[0];
				ClericDomain dom_2 = (ClericDomain)storage.selectedChar.attributes[1];
				spells.AddRange(storage.spellList.Get2DomainSpellsWithRank(dom_1, dom_2, rank));
			}
			else if (cclass == CharClassEnum.Wizard)
			{
				MagicSchool school = (MagicSchool)storage.selectedChar.attributes[0];
				spells.AddRange(storage.spellList.GetWizardSchoolSpecializationSpells(school, rank));
			}
		}
		CreateSpellButtons(spells);
	}

	private void CreateSpellButtons(List<Spell> list)
	{
		const int startY = 0;
		const int height = 100;
		int numberOfSpells = list.Count;
		for (int i = 0; i < numberOfSpells; ++i)
		{
			GameObject obj = (GameObject)Instantiate(prefab);
			obj.transform.parent = transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = new Vector3(0, startY - (i*height), 0);
			SpellPrepareButton button = obj.GetComponent<SpellPrepareButton>();
			button.spell = list[i];
		}
	}
}
