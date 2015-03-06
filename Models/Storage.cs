using UnityEngine;
using Boomlagoon.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Storage : MonoBehaviour 
{
	private const string charFileName = "/char.data";
	private List<Character> mCharacterLsit = new List<Character>();
	private int mSelectedCharIndex;
	private int mSelectedRank;

	public List<Character> characterList
	{
		get { return mCharacterLsit; }
	}
	public Character selectedChar
	{
		get { return mCharacterLsit[mSelectedCharIndex]; }
	}
	public int selectedCharIndex
	{
		get { return mSelectedCharIndex; }
		set { mSelectedCharIndex = value; }
	}
	public SpellList spellList
	{
		get { return gameObject.GetComponent<SpellList>(); }
	}
	public int selectedRank
	{
		get { return mSelectedRank; }
		set { mSelectedRank = value; }
	}
	public bool selectedRankMain;

	private void Start()
	{
		PlayerPrefs.DeleteAll();
		InitCharacters();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public string SelectedCharNumbers(bool used)
	{
		string result = "";
		for (int rank = 0; rank < selectedChar.mainSpells.Count; ++rank)
		{
			result += SelectedCharNumbers(used, rank);
			if (SelectedCharHasExtraSpell())
			{
				if (selectedChar.cclass != CharClassEnum.Cleric || rank >= 1)
				{
					if (selectedChar.extraSpells.Count <= rank)
					{
						if (used)
						{
							result += "+1";
						}
						else
						{
							result += "+0";
						}
					}
					else
					{
						if (selectedChar.extraSpells[rank].Value == used)
						{
							result += "+1";
						}
						else
						{
							result += "+0";
						}
					}
				}
			}
			result += "\n";
		}
		return result;
	}

	public string SelectedCharNumbers(bool used, int rank)
	{
		string result = "";
		int amount = 0;
		int delta = 1;
		if (used)
		{
			amount = CharacterFile.GetTotalNumberOfSpells(selectedChar, rank);
			delta = -1;
		}
		foreach (KeyValuePair<Spell, bool> pair in selectedChar.mainSpells[rank])
		{
			if (pair.Value == false)
			{
				amount += delta;
			}
		}
		result += amount;
		return result;
	}

	public string SelectedCharInfo()
	{
		return selectedChar.Info();
	}

	public int SelectedCharNumberOfCastableRanks()
	{
		return selectedChar.CastableRanks();
	}

	public bool SelectedCharHasExtraSpell()
	{
		return selectedChar.HasExtraSpell();
	}

	public void PrepareSpell(Spell spell)
	{
		if (selectedRankMain)
		{
			selectedChar.PrepareMainSpell(spell, mSelectedRank);
		}
		else
		{
			selectedChar.PrepareExtraSpell(spell, mSelectedRank);
		}
		WriteCharactersToFile();
	}

	public void ReadyAllSpells()
	{
		selectedChar.ReadyAllSpells();
	}

	public void UseMainSpell(Spell spell, int rank)
	{
		int index = -1;
		List<KeyValuePair<Spell, bool>> list = selectedChar.mainSpells[rank];
		for (int i = 0; i < list.Count; ++i)
		{
			if (list[i].Key.name == spell.name && !list[i].Value)
			{
				index = i;
			}
		}
		list.RemoveAt(index);
		list.Insert(index, new KeyValuePair<Spell, bool>(spell, true));
		WriteCharactersToFile();
	}
	public void UseExtraSpell(Spell spell, int rank)
	{
		Debug.Log("casting spell : " + spell.name + "!");
		selectedChar.extraSpells[rank] = new KeyValuePair<Spell,bool>(spell, true);
		WriteCharactersToFile();
	}

	public void SaveNewCharacter(Character character)
	{
		mCharacterLsit.Add(character);
		WriteCharactersToFile();
	}

	public void ResetCharacterList()
	{
		mCharacterLsit.Clear();
		WriteCharactersToFile();
	}


	private void InitCharacters()
	{
		string data = "";
		try
		{
			using (var stream = File.OpenRead(Application.persistentDataPath + charFileName))
			{
				StreamReader reader = new StreamReader(stream);
				data = reader.ReadToEnd();
			}
		}
		catch (Exception) {}

		if (!string.IsNullOrEmpty(data))
		{
			mCharacterLsit.Clear();
			JSONObject obj = JSONObject.Parse(data);
			JSONArray array = obj.GetArray("characters");
			foreach (var val in array)
			{
				Character character = new Character();
				character.Deserialize(val.Obj);
				mCharacterLsit.Add(character);
			}
		}
	}

	private void WriteCharactersToFile()
	{
		JSONObject data = CharactersJSON();
		using (var stream = File.Create(Application.persistentDataPath + charFileName))
		{
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(data.ToString());
			writer.Dispose();
		}
	}

	private JSONObject CharactersJSON()
	{
		JSONObject obj = new JSONObject();
		JSONArray array = new JSONArray();
		foreach (Character character in mCharacterLsit)
		{
			array.Add(character.Serialize());
		}
		obj.Add("characters", array);
		return obj;
	}
}
