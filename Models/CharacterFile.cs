using UnityEngine;
using Boomlagoon.JSON;
using System.Collections;
using System;

public class CharacterFile
{
	public static int GetTotalNumberOfSpells(Character character, int rank)
	{
		int result = GetNumberOfSpellsPerDay_Modifier_Level(character.modifier, rank);
		if (character.cclass == CharClassEnum.Cleric)
		{
			result += GetNumberOfSpellsPerDay_CLERIC_Level(character.level, rank);
		}
		else if (character.cclass == CharClassEnum.Wizard)
		{
			result += GetNumberOfSpellsPerDay_WIZARD_Level(character.level, rank);
		}
		return result;
	}

	public static int GetNumberOfSpellsPerDay_WIZARD_Level(int level, int rank)
	{
		if (rank > 0)
		{
			int result = level - ((rank - 1) * 2);
			if (result >= 7)
				return 4;
			switch (result)
			{
				case 1: return 1;
				case 2: return 2;
				case 3: return 2;
				case 4: return 3;
				case 5: return 3;
				case 6: return 3;
				default: return 0;
			}
		}
		else
		{
			if (level == 1)
				return 3;
			else
				return 4;
		}
	}
	public static int GetNumberOfSpellsPerDay_CLERIC_Level(int level, int rank)
	{
		if (rank > 0)
		{
			int result = level - ((rank - 1) * 2);
			if (result >= 11)
				return 5;
			switch (result)
			{
				case 1: return 1;
				case 2: return 2;
				case 3: return 2;
				case 4: return 3;
				case 5: return 3;
				case 6: return 3;
				case 7: return 4;
				case 8: return 4;
				case 9: return 4;
				case 10: return 4;
				default: return 0;
			}
		}
		else
		{
			if (level == 1)
				return 3;
			else if (level == 2 || level == 3)
				return 4;
			else if (level >= 4 && level <= 6)
				return 5;
			else
				return 6;
		}
	}
	public static int GetNumberOfSpellsPerDay_Modifier_Level(int mod, int rank)
	{
		if (mod <= 0 || rank <= 0 || mod - rank < 0)
		{
			return 0;
		}
		float result = (float)(mod - rank + 1) / 4f;
		return (int)Math.Ceiling(result);
	}
}

public enum CharClassEnum
{
	Barbarian = 1,
	Bard = 2,
	Cleric = 3,
	Druid = 4,
	Fighter = 5,
	Monk = 6,
	Paladin = 7,
	Ranger = 8,
	Rogue = 9,
	Sorcerer = 10,
	Wizard = 11
}

public enum MagicSchool
{
	NONE = -256,
	Abjuration = 1,
	Conjuration = 2,
	Divination = 3,
	Enchantment = 4,
	Evocation = 5,
	Illusion = 6,
	Necromancy = 7,
	Transmutation = 8,
	Universal = 9
}

public enum ClericDomain
{
	NONE = 0,
	Air = 1,
	Animal = 2,
	Chaos = 3,
	Death = 4,
	Destruction = 5,
	Earth = 6,
	Evil = 7,
	Fire = 8,
	Good = 9,
	Healing = 10,
	Knowledge = 11,
	Law = 12,
	Luck = 13,
	Magic = 14,
	Plant = 15,
	Protection = 16,
	Strength = 17,
	Sun = 18,
	Travel = 19,
	Trickery = 20,
	War = 21,
	Water = 22
}
