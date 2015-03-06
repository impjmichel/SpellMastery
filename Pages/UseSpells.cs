using UnityEngine;
using System.Collections;

public class UseSpells : SpellBookPage
{
	public override void OnClick_Back()
	{
		StartCoroutine(PageToLeftPageAnimation());
	}
}
