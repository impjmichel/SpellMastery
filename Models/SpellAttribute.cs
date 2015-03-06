using UnityEngine;
using System.Collections;

public class SpellAttribute
{
	private int mRank;
	private Attribute mAttribute;
	private int mAttributeValue;

	public int rank
	{
		get { return mRank; }
		set { mRank = value; }
	}

	public Attribute attribute
	{
		get { return mAttribute; }
		set { mAttribute = value; }
	}

	public int attributeValue
	{
		get { return mAttributeValue; }
		set { mAttributeValue = value; }
	}
}

public enum Attribute
{
	Class = 1,
	Domain = 2
}
