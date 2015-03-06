using UnityEngine;
using Boomlagoon.JSON;
using System.Collections;
using System;

public class CharacterFile
{
	public static int GetTotalNumberOfSpells(Character character, int rank)
	{
		int result = 0;
		if (character.cclass == CharClassEnum.Cleric)
		{
			result += GetNumberOfSpellsPerDay_CLERIC_Level(character.level, rank);
		}
		else if (character.cclass == CharClassEnum.Druid)
		{
			result += GetNumberOfSpellsPerDay_DRUID_Level(character.level, rank);
		}
		else if (character.cclass == CharClassEnum.Paladin)
		{
			result += GetNumberOfSpellsPerDay_PALADIN_Level(character.level, rank);
		}
		else if (character.cclass == CharClassEnum.Ranger)
		{
			result += GetNumberOfSpellsPerDay_RANGER_Level(character.level, rank);
		}
		else if (character.cclass == CharClassEnum.Wizard)
		{
			result += GetNumberOfSpellsPerDay_WIZARD_Level(character.level, rank);
		}
		if (result >= 0)
		{
			result += GetNumberOfSpellsPerDay_Modifier_Level(character.modifier, rank);
		}
		return result;
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

	public static int GetNumberOfSpellsPerDay_DRUID_Level(int level, int rank)
	{ // same as Cleric
		return GetNumberOfSpellsPerDay_CLERIC_Level(level, rank);
	}

	public static int GetNumberOfSpellsPerDay_PALADIN_Level(int level, int rank)
	{
		switch (rank)
		{
			case 0: return -1;
			case 1:
				if (level <= 3)
					return -1;
				else if (level == 4 || level == 5)
					return 0;
				else if (level >= 6 && level <= 13)
					return 1;
				else if (level >= 14 && level <= 17)
					return 2;
				else
					return 3;
			case 2:
				if (level <= 7)
					return -1;
				else if (level == 8 || level == 9)
					return 0;
				else if (level >= 10 && level <= 15)
					return 1;
				else if (level >= 16 && level <= 18)
					return 2;
				else
					return 3;
			case 3:
				if (level <= 10)
					return -1;
				else if (level == 11)
					return 0;
				else if (level >= 12 && level <= 16)
					return 1;
				else if (level >= 17 && level <= 18)
					return 2;
				else
					return 3;
			case 4:
				if (level <= 13)
					return -1;
				else if (level == 14)
					return 0;
				else if (level >= 15 && level <= 18)
					return 1;
				else if (level == 19)
					return 2;
				else
					return 3;
		}
		return 0;
	}

	public static int GetNumberOfSpellsPerDay_RANGER_Level(int level, int rank)
	{ // same as Paladin
		return GetNumberOfSpellsPerDay_PALADIN_Level(level, rank);
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
	Barbarian	= 1, 		// does not prepare spells
	Bard		= 2, 		// does not prepare spells
	Cleric		= 3, 	// WIS modifier << implemented
	Druid		= 4, 	// WIS modifier << implemented
	Fighter		= 5,		// does not prepare spells
	Monk		= 6,		// does not prepare spells
	Paladin		= 7,	// WIS modifier << implemented
	Ranger		= 8,	// WIS modifier << implemented
	Rogue		= 9,		// does not prepare spells
	Sorcerer	= 10,		// does not prepare spells
	Wizard		= 11	// INT modifier << implemented
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
