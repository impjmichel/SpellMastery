using UnityEngine;
using System.Collections;

public class RankSelectionButton : MonoBehaviour
{
	public int rank;
	public bool main;

	public void OnCLick()
	{
		Storage storage = GameObject.FindObjectOfType<Storage>();
		if (storage != null)
		{
			storage.selectedRank = rank;
			storage.selectedRankMain = main;
			PrepareSpells prepareSpells = GameObject.FindObjectOfType<PrepareSpells>();
			prepareSpells.OnClick_ToSingleRank();
		}
	}
}
