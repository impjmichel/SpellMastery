using UnityEngine;
using System.Collections;

public class UseSpellButton : MonoBehaviour
{
	protected Spell mSpell;
	protected Storage mStorage;
	protected bool mUsed;
	protected int mRank;

	public Spell spell
	{
		get { return mSpell; }
		set
		{
			mSpell = value;
			UILabel label = transform.FindChild("Label").GetComponent<UILabel>();
			label.text = mSpell.ToShortString();
			mStorage = GameObject.FindObjectOfType<Storage>();
		}
	}

	public bool used
	{
		set
		{
			mUsed = value;
			gameObject.GetComponent<UIButton>().enabled = !mUsed;
			if (mUsed)
				gameObject.GetComponent<UIButton>().defaultColor = Color.gray;
			gameObject.GetComponent<UIButton>().UpdateColor(false);
		}
	}

	public int rank
	{
		set { mRank = value; }
	}

	public virtual void OnClick_Use()
	{
		used = true;
		mStorage.UseMainSpell(mSpell, mRank);
	}
}
