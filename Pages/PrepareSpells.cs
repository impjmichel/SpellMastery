using UnityEngine;
using System.Collections;

public class PrepareSpells : SpellBookPage
{
	private const int total_height = 640;
	private const int maximum_height = 120;

	private int height = 0;

	public Transform table;
	public GameObject ButtonPrefab;
	public GameObject CharPane;
	public GameObject SingleRankPreparation;

	public override void OnClick_Back()
	{
		BackButtonTarget = CharPane;
		StartCoroutine(PageToLeftPageAnimation());
	}

	public void OnClick_ToSingleRank()
	{
		BackButtonTarget = SingleRankPreparation;
		StartCoroutine(PageToLeftPageAnimation());
	}

	private void OnEnable()
	{
		StartCoroutine(Initiate());
	}

	private IEnumerator Initiate()
	{
		yield return StartCoroutine(Clean());

		int number = storage.SelectedCharNumberOfCastableRanks();
		height = total_height / number;
		if (height > maximum_height)
		{
			height = maximum_height;
		}
		for (int i = 0; i < number; ++i)
		{
			CreateRankButton(i);
		}
	}

	private IEnumerator Clean()
	{
		int index = 0;
		Transform child;
		while (table.childCount > 2)
		{
			yield return null;
			child = table.GetChild(index);
			if (child.gameObject.name == "MainButtonGrid" || child.gameObject.name == "ExtraButtonGrid")
			{
				index++;
			}
			else
			{
				GameObject.Destroy(table.GetChild(index).gameObject);
			}
			yield return new WaitForEndOfFrame();
		}
		child = table.FindChild("MainButtonGrid");
		while (child.childCount > 0)
		{
			yield return null;
			GameObject.Destroy(child.GetChild(0).gameObject);
			yield return new WaitForEndOfFrame();
		}
		child = table.FindChild("ExtraButtonGrid");
		while (child.childCount > 0)
		{
			yield return null;
			GameObject.Destroy(child.GetChild(0).gameObject);
			yield return new WaitForEndOfFrame();
		}
	}

	private void CreateRankButton(int row)
	{
		bool extraSpell = storage.SelectedCharHasExtraSpell();
		GameObject mainButton = (GameObject)Instantiate(ButtonPrefab);
		mainButton.GetComponent<RankSelectionButton>().rank = row;
		mainButton.GetComponent<RankSelectionButton>().main = true;
		if (extraSpell)
		{
			Transform target = table.FindChild("MainButtonGrid");
			mainButton.transform.parent = target;
			AnchorObjectToTable(target, mainButton, row);

			CreateExtraRankButton(row);
		}
		else
		{
			mainButton.transform.parent = table;
			AnchorObjectToTable(table, mainButton, row);
		}
		UILabel label = mainButton.transform.FindChild("B_Label").GetComponent<UILabel>();
		label.text = "Rank:   " + row;
	}

	private void CreateExtraRankButton(int row)
	{
		if (row == 0)
		{
			if (storage.selectedChar.cclass == CharClassEnum.Cleric)
			{ // clerics don't get an extra spell for rank 0.
				return;
			}
		}
		Transform target = table.FindChild("ExtraButtonGrid");
		GameObject extraButton = (GameObject)Instantiate(ButtonPrefab);
		extraButton.GetComponent<RankSelectionButton>().rank = row;
		extraButton.GetComponent<RankSelectionButton>().main = false;
		extraButton.transform.parent = target;
		UILabel label = extraButton.transform.FindChild("B_Label").GetComponent<UILabel>();
		label.text = "+1";
		AnchorObjectToTable(target, extraButton, row);
	}

	private void AnchorObjectToTable(Transform target, GameObject obj, int row)
	{
		obj.transform.localScale = Vector3.one;
		UIWidget widget = obj.GetComponent<UIWidget>();
		widget.leftAnchor.target = target;
		widget.rightAnchor.target = target;
		widget.bottomAnchor.target = target;
		widget.topAnchor.target = target;
		widget.leftAnchor.relative = 0;
		widget.leftAnchor.absolute = 0;
		widget.rightAnchor.relative = 1;
		widget.rightAnchor.absolute = 0;
		widget.bottomAnchor.relative = 1;
		widget.bottomAnchor.absolute = (row + 1) * -height;
		widget.topAnchor.relative = 1;
		widget.topAnchor.absolute = row * -height;
		widget.Update();
	}
}
