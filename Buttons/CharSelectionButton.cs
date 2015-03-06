using UnityEngine;
using System.Collections;

public class CharSelectionButton : MonoBehaviour 
{
	public int x;

	public void OnCLick()
	{
		Storage storage = GameObject.FindObjectOfType<Storage>();
		if (storage != null)
		{
			storage.selectedCharIndex = x;
			CharSelect charSelect = GameObject.FindObjectOfType<CharSelect>();
			charSelect.OnClick_ToCharPane();
		}
	}
}
