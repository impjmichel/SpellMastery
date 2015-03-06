using UnityEngine;
using Boomlagoon.JSON;
using System.Collections;

public class CharSelect : SpellBookPage 
{
	public const string DATA = "data";
	public const string ARRAY = "CharacterArray";
	private const int total_height = 640;
	private const int maximum_height = 120;

	private int height = 0;

	public Transform table;
	public GameObject ButtonPrefab;
	public GameObject PanelHome;
	public GameObject CharPane;

	public override void OnClick_Back()
	{
		BackButtonTarget = PanelHome;
		StartCoroutine(PageToLeftPageAnimation());
	}

	public void OnClick_ToCharPane()
	{
		BackButtonTarget = CharPane;
		StartCoroutine(PageToLeftPageAnimation());
	}

	private void OnEnable()
	{
		StartCoroutine(Initiate());
	}

	private IEnumerator Initiate()
	{
		while (table.childCount > 0)
		{
			yield return null;
			GameObject.Destroy(table.GetChild(0).gameObject);
			yield return new WaitForEndOfFrame();
		}
		if (storage.characterList.Count > 0)
		{
			height = total_height / storage.characterList.Count;
			if (height > maximum_height)
			{
				height = maximum_height;
			}
			for (int i = 0; i < storage.characterList.Count; ++i)
			{
				CreateCharacterButton(storage.characterList[i], i);
			}
		}
	}


	private void CreateCharacterButton(Character character, int row)
	{
		GameObject newButton = (GameObject)Instantiate(ButtonPrefab);
		newButton.GetComponent<CharSelectionButton>().x = row;
		newButton.transform.parent = table;
		AnchotObjectToTable(newButton, row);

		UILabel label = newButton.transform.FindChild("B_Label").GetComponent<UILabel>();
		label.text = character.ShortInfo();
	}

	private void AnchotObjectToTable(GameObject obj, int row)
	{
		obj.transform.localScale = Vector3.one;
		UIWidget widget = obj.GetComponent<UIWidget>();
		widget.leftAnchor.target = table;
		widget.rightAnchor.target = table;
		widget.bottomAnchor.target = table;
		widget.topAnchor.target = table;
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
