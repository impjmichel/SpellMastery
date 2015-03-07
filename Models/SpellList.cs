using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boomlagoon.JSON;

public class SpellList : MonoBehaviour
{
	private const string cOnlinePath = "http://impdevelopment.eu/SpellMastery/list.data";
	private const string cFilename = "/list.data";
	private string mPath;
	private string mVersion;
	private List<string> mMessage;
	private List<Spell> mSpellList = new List<Spell>();
	
	public List<Spell> spellList
	{
		get { return mSpellList; }
	}
	public string version
	{
		get { return mVersion; }
	}
	public List<string> message
	{
		get { return mMessage; }
	}

	public List<Spell> GetSpellsOfClassAndRank(CharClassEnum cclass, int rank)
	{
		List<Spell> result = mSpellList.FindAll(
			delegate(Spell temp)
			{
				return temp.Contains(cclass, rank);
			});
		return result;
	}

	public List<Spell> GetWizardSchoolSpecializationSpells(MagicSchool school, int rank)
	{
		List<Spell> result = mSpellList.FindAll(
			delegate(Spell temp)
			{
				return temp.Contains(school, CharClassEnum.Wizard, rank);
			});
		return result;
	}

	public List<Spell> Get2DomainSpellsWithRank(ClericDomain domain_1, ClericDomain domain_2, int rank)
	{
		List<Spell> result = GetDomainAndRankSpells(domain_1, rank);
		result.AddRange(GetDomainAndRankSpells(domain_2, rank));
		return result;
	}

	public IEnumerator GetOnlineList()
	{
		WWW www = new WWW(cOnlinePath);
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			string data = www.text;
			if (!string.IsNullOrEmpty(data))
			{
				DeserializeData(data);
			}
		}
		else
		{
			Debug.Log(www.error);
			GetOfflineList();
		}
	}

	private List<Spell> GetDomainSpells(ClericDomain domain)
	{
		List<Spell> result = mSpellList.FindAll(
			delegate(Spell temp)
			{
				return temp.Contains(domain);
			});
		return result;
	}

	private List<Spell> GetDomainAndRankSpells(ClericDomain domain, int rank)
	{
		List<Spell> result = mSpellList.FindAll(
			delegate(Spell temp)
			{
				return temp.Contains(domain, rank);
			});
		return result;
	}

	private void Start()
	{
		mPath = Application.persistentDataPath;
	}

	private void WriteList()
	{
		string data = "{\"spells\":[" + "\n";
		mSpellList.Sort();
		for (int i = 0; i < mSpellList.Count; ++i)
		{
			data += mSpellList[i].Serialize().ToString();
			if (i != mSpellList.Count - 1)
			{ // last items does not want the comma.
				data += ",\n";
			}
		}
		data += "]}";
		string filename = "/list.data";
		using(var stream = File.Create(mPath+filename))
		{
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(data);
			writer.Dispose();
		}
	}

	private void GetOfflineList()
	{
		string data = "";
		using (var stream = File.OpenRead(mPath + cFilename))
		{
			StreamReader reader = new StreamReader(stream);
			data = reader.ReadToEnd();
			reader.Dispose();
		}
		if (!string.IsNullOrEmpty(data))
		{
			DeserializeData(data);
		}
	}

	private void DeserializeData(string data)
	{
		JSONObject obj = JSONObject.Parse(data);
		if (obj.ContainsKey("version"))
		{
			mVersion = obj.GetString("version");
		}
		if (obj.ContainsKey("message"))
		{
			mMessage = new List<string>();
			JSONArray jsonMessage = obj.GetArray("message");
			foreach (var val in jsonMessage)
			{
				mMessage.Add(val.Str);
			}
		}
		JSONArray spells = obj.GetArray("spells");
		foreach (var val in spells)
		{
			Spell spell = new Spell();
			spell.InitiateSpell(val.Obj);
			mSpellList.Add(spell);
		}
		WriteList();
	}
}
