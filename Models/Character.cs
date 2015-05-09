using Boomlagoon.JSON;
using UnityEngine;
using System.Collections.Generic;

public class Character
{
	private string mName;
	private CharClassEnum mCClass;
	private int mLevel = 1;
	private int mModifier = 0;

	private List<List<KeyValuePair<Spell, bool>>> mMainSpells = new List<List<KeyValuePair<Spell, bool>>>();
	private List<KeyValuePair<Spell, bool>> mExtraSpells = new List<KeyValuePair<Spell, bool>>();

	private List<int> mAttributes = new List<int>(); // domains or specialization

	public string name
	{
		get { return mName; }
		set { mName = value; }
	}

	public CharClassEnum cclass
	{
		get { return mCClass; }
		set { mCClass = value; }
	}

	public int level
	{
		get { return mLevel; }
		set { mLevel = value; }
	}

	public int modifier
	{
		get { return mModifier; }
		set { mModifier = value; }
	}

	public List<List<KeyValuePair<Spell, bool>>> mainSpells
	{
		get { return mMainSpells; }
	}

	public List<KeyValuePair<Spell, bool>> extraSpells
	{
		get { return mExtraSpells; }
	}

	public List<int> attributes
	{
		get { return mAttributes; }
	}

	public Character() { }

	public Character(string name, CharClassEnum charClass, int lvl, int mod, List<int> attributeList)
	{
		mName = name;
		mCClass = charClass;
		mLevel = lvl;
		mModifier = mod;
		mAttributes = attributeList;
	}

	public void Deserialize(JSONObject obj)
	{
		mName = obj.GetString("name");
		mCClass = (CharClassEnum)obj.GetNumber("class");
		mLevel = (int)obj.GetNumber("level");
		mModifier = (int)obj.GetNumber("mod");
		// attributes
		mAttributes.Clear();
		JSONArray tempArray = obj.GetArray("attributes");
		foreach (var val in tempArray)
		{
			mAttributes.Add((int)val.Number);
		}
		// main spells
		mMainSpells.Clear();
		tempArray = obj.GetArray("mainSpells");
		foreach (var array in tempArray)
		{
			List<KeyValuePair<Spell, bool>> dic = new List<KeyValuePair<Spell, bool>>();
			foreach (var pairObj in array.Array)
			{
				Spell spell = new Spell();
				spell.Deserialize(pairObj.Obj.GetObject("spell"));
				bool used = pairObj.Obj.GetBoolean("used");
				dic.Add(new KeyValuePair<Spell, bool>(spell, used));
			}
			mMainSpells.Add(dic);
		}
		// extra spells
		mExtraSpells.Clear();
		tempArray = obj.GetArray("extraSpells");
		foreach (var array in tempArray)
		{
			Spell spell = new Spell();
			spell.Deserialize(array.Obj.GetObject("spell"));
			bool used = array.Obj.GetBoolean("used");
			mExtraSpells.Add(new KeyValuePair<Spell, bool>(spell, used));
		}
	}

	public JSONObject Serialize()
	{
		JSONObject obj = new JSONObject();
		obj.Add("name", mName);
		obj.Add("class", (int)mCClass);
		obj.Add("level", mLevel);
		obj.Add("mod", mModifier);
		// attributes
		JSONArray tempArray = new JSONArray();
		foreach (int i in mAttributes)
		{
			tempArray.Add(i);
		}
		obj.Add("attributes", tempArray);
		// main spells
		tempArray = new JSONArray();
		foreach (List<KeyValuePair<Spell, bool>> list in mMainSpells)
		{
			JSONArray innerArray = new JSONArray();
			foreach (var pair in list)
			{
				JSONObject pairObj = new JSONObject();
				pairObj.Add("spell",pair.Key.Serialize());
				pairObj.Add("used", pair.Value);
				innerArray.Add(pairObj);
			}
			tempArray.Add(innerArray);
		}
		obj.Add("mainSpells", tempArray);
		// extra spells
		tempArray = new JSONArray();
		foreach (KeyValuePair<Spell, bool> pair in mExtraSpells)
		{
			JSONObject pairObj = new JSONObject();
			pairObj.Add("spell", pair.Key.Serialize());
			pairObj.Add("used", pair.Value);
			tempArray.Add(pairObj);
		}
		obj.Add("extraSpells", tempArray);
		return obj;
	}

	public string Info()
	{
		string result = mName + "\n";
		result += ClassInfo();
		return result;
	}

	public string ShortInfo()
	{
		return mName + "\n[" + mCClass + "  " + mLevel + "]";
	}

	public bool HasExtraSpell()
	{
		if (mCClass == CharClassEnum.Wizard)
		{
			MagicSchool spec = ((MagicSchool)(mAttributes[0]));
			if (spec != MagicSchool.none)
			{
				return true;
			}
		}
		else if (mCClass == CharClassEnum.Cleric)
		{
			return true;
		}
		return false;
	}

	public int CastableRanks()
	{
		if (mCClass == CharClassEnum.Wizard || mCClass == CharClassEnum.Cleric || mCClass == CharClassEnum.Druid)
		{
			for (int i = 0; i < 10; ++i)
			{
				if (mLevel < (i * 2) - 1)
				{
					return i;
				}
			}
		}
		else if (mCClass == CharClassEnum.Ranger || mCClass == CharClassEnum.Paladin)
		{
			int adjustment = 1; // needed to keep the arrays at the right size  :'(
			if (mLevel == 4)
			{
				return 1 + adjustment;
			}
			else if (mLevel >= 14)
			{
				return 4 + adjustment;
			}
			return ((mLevel - 2) / 3) + adjustment;
		}
		return 10;
	}

	public string RanksString()
	{
		string result = "";
		int startRank = 0;
		if (mCClass == CharClassEnum.Ranger || mCClass == CharClassEnum.Paladin)
		{ // can't cast rank 0 spells
			startRank = 1;
		}
		for (int i = startRank; i < CastableRanks(); ++i)
		{
			result += i + "\n";
		}
		return result;
	}

	public void ResetSpells()
	{
		ResetMainSpells();
		ResetExtraSpells();
	}

	public void ReadyAllSpells()
	{
		List<List<KeyValuePair<Spell, bool>>> readyList = new List<List<KeyValuePair<Spell, bool>>>();
		foreach (List<KeyValuePair<Spell, bool>> rank in mMainSpells)
		{
			List<KeyValuePair<Spell, bool>> readyRank = new List<KeyValuePair<Spell, bool>>();
			foreach (KeyValuePair<Spell, bool> pair in rank)
			{
				readyRank.Add(new KeyValuePair<Spell, bool>(pair.Key, false));
			}
			readyList.Add(readyRank);
		}
		mMainSpells = readyList;
		if (HasExtraSpell())
		{
			List<KeyValuePair<Spell, bool>> readyExtra = new List<KeyValuePair<Spell, bool>>();
			for (int i = CastableRanks(); i > 0; --i)
			{
				readyExtra.Add(new KeyValuePair<Spell, bool>(new Spell(), true));
			}
			for (int rank = 0; rank < mExtraSpells.Count; ++rank)
			{
				KeyValuePair<Spell, bool> pair = mExtraSpells[rank];
				readyExtra[rank] = new KeyValuePair<Spell, bool>(pair.Key, false);
			}
			mExtraSpells = readyExtra;
		}
	}

	public void PrepareMainSpell(Spell spell, int rank)
	{
		int number = CharacterFile.GetNumberOfSpellsPerDay_Modifier_Level(mModifier, rank);
		if (mCClass == CharClassEnum.Cleric)
		{
			number += CharacterFile.GetNumberOfSpellsPerDay_CLERIC_Level(mLevel, rank);
		}
		else if (mCClass == CharClassEnum.Wizard)
		{
			number += CharacterFile.GetNumberOfSpellsPerDay_WIZARD_Level(mLevel, rank);
		}
		if (mMainSpells[rank].Count >= number)
		{
			int index = -1;
			for (int i = 0; i < mainSpells[rank].Count; ++i)
			{
				if (mainSpells[rank][i].Value)
				{
					index = i;
					break;
				}
			}
			if (index == -1)
			{
				mMainSpells[rank] = new List<KeyValuePair<Spell,bool>>();
			}
			else
			{
				mainSpells[rank].RemoveAt(index);
			}
		}
		mMainSpells[rank].Add(new KeyValuePair<Spell, bool>(spell, false));
	}

	public void PrepareExtraSpell(Spell spell, int rank)
	{
		if (rank >= mExtraSpells.Count)
		{
			while (mExtraSpells.Count <= rank)
			{
				mExtraSpells.Add(new KeyValuePair<Spell, bool>());
			}
		}
		mExtraSpells[rank] = new KeyValuePair<Spell, bool>(spell, false);
	}

	private string ClassInfo()
	{
		string result = mCClass.ToString();
		result += " level: " + mLevel;
		result += "\nAbility Modifier: " + mModifier;
		if (mCClass == CharClassEnum.Wizard)
		{
			result += " (int)";
			result += "\nSpecialization: " + ((MagicSchool)(mAttributes[0])).ToString();
		}
		else
		{
			result += " (wis)";
			if (mCClass == CharClassEnum.Cleric)
			{
				string dom1 = ((ClericDomain)(mAttributes[0])).ToString();
				string dom2 = ((ClericDomain)(mAttributes[1])).ToString();
				result += "\nDomain: " + dom1 + "/" + dom2;
			}
		}
		return result;
	}

	private void ResetMainSpells()
	{
		mMainSpells.Clear();
		int ranks = CastableRanks();
		for (int i = 0; i < ranks; ++i)
		{
			List<KeyValuePair<Spell, bool>> list = new List<KeyValuePair<Spell, bool>>();
			mMainSpells.Add(list);
		}
	}

	private void ResetExtraSpells()
	{
		mExtraSpells = new List<KeyValuePair<Spell,bool>>();
		if (HasExtraSpell())
		{
			for (int i = CastableRanks(); i > 0; --i)
			{
				mExtraSpells.Add(new KeyValuePair<Spell, bool>(new Spell(), true));
			}
		}
	}
}
