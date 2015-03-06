using UnityEngine;
using System.Collections;

public class SpellPrepareButton : MonoBehaviour
{
	private Spell mSpell;
	private Storage mStorage;

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

	public void OnClick_Prepare()
	{
		mStorage.PrepareSpell(mSpell);
	}
}
