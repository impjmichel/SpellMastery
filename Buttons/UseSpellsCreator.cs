using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UseSpellsCreator : MonoBehaviour
{
	private const int height = -80;
	public Storage storage;
	public GameObject buttonPrefab;
	public GameObject extraButtonPrefab;
	public GameObject labelPrefab;

	private void OnEnable()
	{
		StartCoroutine(CreateSpellButtons());
	}

	private IEnumerator Clean()
	{
		transform.parent.localPosition = new Vector3(0, 300, 0);
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
		int rank = 0;
		//creating everything, per rank
		foreach (List<KeyValuePair<Spell, bool>> spells in storage.selectedChar.mainSpells)
		{
			CreateLabel(rank);
			CreateSpellButtons(spells, rank);
			if (storage.SelectedCharHasExtraSpell())
			{
				if (storage.selectedChar.extraSpells.Count > rank)
				{
					KeyValuePair<Spell, bool> pair = storage.selectedChar.extraSpells[rank];
					if (!string.IsNullOrEmpty(pair.Key.name))
					{
						CreateExtraSpellButton(pair, rank);
					}
				}
			}
			++rank;
		}
	}

	private void CreateLabel(int rank)
	{
		string text = "Rank  " + rank;
		GameObject obj = (GameObject)Instantiate(labelPrefab);
		obj.transform.parent = transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(0, (transform.childCount * height), 0);
		obj.GetComponent<UILabel>().text = text;
	}

	private void CreateSpellButtons(List<KeyValuePair<Spell, bool>> list, int rank)
	{
		foreach (KeyValuePair<Spell, bool> pair in list)
		{
			GameObject obj = (GameObject)Instantiate(buttonPrefab);
			obj.transform.parent = transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = new Vector3(0, (transform.childCount * height), 0);
			UseSpellButton button = obj.GetComponent<UseSpellButton>();
			button.spell = pair.Key;
			button.used = pair.Value;
			button.rank = rank;
		}
	}

	private void CreateExtraSpellButton(KeyValuePair<Spell, bool> pair, int rank)
	{
		if (pair.Key != null)
		{
			GameObject obj = (GameObject)Instantiate(extraButtonPrefab);
			obj.transform.parent = transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = new Vector3(0, (transform.childCount * height), 0);
			UseExtraSpellButton button = obj.GetComponent<UseExtraSpellButton>();
			button.spell = pair.Key;
			button.used = pair.Value;
			button.rank = rank;
		}
	}
}
