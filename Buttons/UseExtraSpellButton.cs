using UnityEngine;
using System.Collections;

public class UseExtraSpellButton : UseSpellButton
{
	public override void OnClick_Use()
	{
		used = true;
		mStorage.UseExtraSpell(mSpell, mRank);
	}
}
