using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class Spell
{
	private string mName;
	private MagicSchool mSchool;
	private List<SpellAttribute> mClasses = new List<SpellAttribute>();
	private List<SpellAttribute> mDomains = new List<SpellAttribute>();


	public string name
	{
		get { return mName; }
	}
	public MagicSchool school
	{
		get { return mSchool; }
	}
	public List<SpellAttribute> classes
	{
		get { return mClasses; }
	}
	public List<SpellAttribute> domains
	{
		get { return mDomains; }
	}


	public void InitiateSpell(JSONObject spellObject)
	{
		mName = spellObject.GetString("name");
		mSchool = (MagicSchool)spellObject.GetNumber("school");
		JSONArray classes = spellObject.GetArray("classes");
		foreach(var val in classes)
		{
			SpellAttribute attribute = new SpellAttribute();
			attribute.attribute = Attribute.Class;
			attribute.attributeValue = (int)val.Obj.GetNumber("class");
			attribute.rank = (int)val.Obj.GetNumber("rank");
			mClasses.Add(attribute);
		}
		JSONArray domains = spellObject.GetArray("domains");
		foreach (var val in domains)
		{
			SpellAttribute attribute = new SpellAttribute();
			attribute.attribute = Attribute.Domain;
			attribute.attributeValue = (int)val.Obj.GetNumber("domain");
			attribute.rank = (int)val.Obj.GetNumber("rank");
			mDomains.Add(attribute);
		}
	}

	public bool Contains(ClericDomain domain)
	{
		foreach(SpellAttribute attribute in mDomains)
		{
			if (attribute.attributeValue == (int)domain)
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(ClericDomain domain, int rank)
	{
		foreach (SpellAttribute attribute in mDomains)
		{
			if (attribute.attributeValue == (int)domain && attribute.rank == rank)
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(CharClassEnum charClass)
	{
		foreach (SpellAttribute attribute in mClasses)
		{
			if (attribute.attributeValue == (int)charClass)
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(CharClassEnum charClass, int rank)
	{
		foreach (SpellAttribute attribute in mClasses)
		{
			if (attribute.attributeValue == (int)charClass && attribute.rank == rank)
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(MagicSchool school)
	{
		return mSchool == school;
	}

	public bool Contains(MagicSchool school, CharClassEnum cclass, int rank)
	{
		foreach (SpellAttribute attribute in mClasses)
		{
			if (attribute.attributeValue == (int)cclass && attribute.rank == rank)
			{
				return mSchool == school;
			}
		}
		return false;
	}

	public override string ToString()
	{
		string result = mName;
		result += " (" + mSchool.ToString() + ") ";
		return result;
	}

	public string ToShortString()
	{
		string result = "(" + mSchool.ToString().Substring(0,3) + ") ";
		result += mName;
		return result;
	}

	public void Deserialize(JSONObject obj)
	{
		mName = obj.GetString("name");
		mSchool = (MagicSchool)obj.GetNumber("school");
		mClasses = new List<SpellAttribute>();
		foreach(var value in obj.GetArray("classes"))
		{
			SpellAttribute tempAttribute = new SpellAttribute();
			tempAttribute.attribute = Attribute.Class;
			tempAttribute.attributeValue = (int)value.Obj.GetNumber("class");
			tempAttribute.rank = (int)value.Obj.GetNumber("rank");
			mClasses.Add(tempAttribute);
		}
		mDomains = new List<SpellAttribute>();
		foreach (var value in obj.GetArray("domains"))
		{
			SpellAttribute tempAttribute = new SpellAttribute();
			tempAttribute.attribute = Attribute.Domain;
			tempAttribute.attributeValue = (int)value.Obj.GetNumber("domain");
			tempAttribute.rank = (int)value.Obj.GetNumber("rank");
			mClasses.Add(tempAttribute);
		}
	}

	public JSONObject Serialize()
	{
		JSONObject obj = new JSONObject();
		obj.Add("name", new JSONValue(mName));
		obj.Add("school", new JSONValue((int)mSchool));
		JSONArray tempClasses = new JSONArray();
		foreach(SpellAttribute attribute in mClasses)
		{
			JSONObject temp = new JSONObject();
			temp.Add("class", attribute.attributeValue);
			temp.Add("rank", attribute.rank);
			tempClasses.Add(new JSONValue(temp));
		}
		obj.Add("classes", tempClasses);
		JSONArray tempDomains = new JSONArray();
		foreach (SpellAttribute attribute in mDomains)
		{
			JSONObject temp = new JSONObject();
			temp.Add("domain", attribute.attributeValue);
			temp.Add("rank", attribute.rank);
			tempDomains.Add(new JSONValue(temp));
		}
		obj.Add("domains", tempDomains);
		return obj;
	}
}
