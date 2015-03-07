using UnityEngine;
using Boomlagoon.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Storage : MonoBehaviour 
{
	/// <summary>
	/// The number for a version control system thing. Any change results in a "memory wipe".
	/// </summary>
	private const string cVersion = "1.0.6"; 
	private const string charFileName = "/char.data";

	private List<Character> mCharacterLsit = new List<Character>();
	private int mSelectedCharIndex;
	private int mSelectedRank;

	public GameObject Popup;

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
		StartCoroutine(VersionControl());
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
		int startRank = 0;
		if (selectedChar.cclass == CharClassEnum.Paladin || selectedChar.cclass == CharClassEnum.Ranger)
		{
			startRank = 1;
		}
		for (int rank = startRank; rank < selectedChar.mainSpells.Count; ++rank)
		{
			result += SelectedCharNumbers(used, rank);
			if (SelectedCharHasExtraSpell())
			{
				if (selectedChar.cclass != CharClassEnum.Cleric || rank >= 1)
				{
					result += "+" + SelectedCharExtraSpell(used, rank);
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

	public string SelectedCharExtraSpell(bool used, int rank)
	{
		string result = "";
		if (SelectedCharHasExtraSpell())
		{
			if (selectedChar.extraSpells.Count <= rank)
			{
				if (used)
				{
					result += "1";
				}
				else
				{
					result += "0";
				}
			}
			else
			{
				if (selectedChar.extraSpells[rank].Value == used)
				{
					result += "1";
				}
				else
				{
					result += "0";
				}
			}
			
		}
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

	public string SelectedCharRanksString()
	{
		return selectedChar.RanksString();
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
		selectedChar.extraSpells[rank] = new KeyValuePair<Spell,bool>(spell, true);
		WriteCharactersToFile();
	}

	public void SaveNewCharacter(Character character)
	{
		mCharacterLsit.Add(character);
		WriteCharactersToFile();
	}

	public void ResetEverything(bool quit)
	{
		string[] files = Directory.GetFiles(Application.persistentDataPath);
		foreach (string file in files)
		{
			File.Delete(file);
		}
		PlayerPrefs.DeleteAll();
		mCharacterLsit.Clear();
		WriteCharactersToFile();
		if (quit)
		{
			Application.Quit();
		}
		else
		{
			StartCoroutine(spellList.GetOnlineList());
		}
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

	

	public void OnClick_PopupExit()
	{
		ResetEverything(true);
	}

	private IEnumerator VersionControl()
	{
		yield return StartCoroutine(spellList.GetOnlineList());
		if (!string.IsNullOrEmpty(spellList.version))
		{
			OnlineVersionControl(spellList.version);
		}
	}

	private void OnlineVersionControl(string version)
	{
		if (cVersion != version)
		{
			//popup text
			string text = "[b]SpellMastery\nv" + version + " released![/b]";
			// show popup
			UILabel label = Popup.transform.FindChild("Label").GetComponent<UILabel>();
			label.text = text;
			Popup.SetActive(true);
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
