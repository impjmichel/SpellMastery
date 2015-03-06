using UnityEngine;
using System.Collections;

public class CharCreate : SpellBookPage 
{
	private Character mCharacter = new Character();

	public UIInput nameInput;

	public GameObject WizardSpecSelecter;
	public GameObject ClericDomain_1;
	public GameObject ClericDomain_2;

	public Character character
	{
		get { return mCharacter; }
	}

	public override void OnClick_Back()
	{
		StartCoroutine(PageToLeftPageAnimation());
	}

	public void OnChange_CharacterName()
	{
		nameInput.value = nameInput.value.Trim();
		if (!nameInput.isSelected)
		{
			ResetNameValue();
		}
	}

	public void ScrollViewChildCentered(string labelText)
	{
		labelText = labelText.Trim();
		ResetNameValue();
		if (labelText.StartsWith("+"))
		{
			// modifier
			mCharacter.modifier = int.Parse(labelText);
		}
		else if (labelText.StartsWith(">"))
		{
			// wizard spec
			WizardSpecHandler(labelText.Substring(1));
		}
		else if (labelText.StartsWith("1:"))
		{
			// cleric domain 1
			DomainHandler(1, labelText.Substring(2).Trim());
		}
		else if (labelText.StartsWith("2:"))
		{
			// cleric domain 2
			DomainHandler(2, labelText.Substring(2).Trim());
		}
		else
		{
			if (labelText.Length <=2)
			{
				// level
				mCharacter.level = int.Parse(labelText);
			}
			else
			{
				// class
				ClassHandler(labelText);
			}
		}
	}

	public void OnClick_Save()
	{
		mCharacter.name = nameInput.value;
		mCharacter.ResetSpells();
		storage.SaveNewCharacter(mCharacter);
		Reset();
		OnClick_Back();
	}

	private void OnEnable()
	{
		mCharacter = new Character();
		Reset();
	}

	private void Reset()
	{
		// TODO!
	}

	private void ResetNameValue()
	{
		if (string.IsNullOrEmpty(nameInput.value))
		{
			nameInput.value = "enter name here";
		}
	}

	private void ClassHandler(string text)
	{
		// reset active gameobject that might not need to be active.
		WizardSpecSelecter.SetActive(false);
		ClericDomain_1.SetActive(false);
		ClericDomain_2.SetActive(false);

		if (text == CharClassEnum.Cleric.ToString())
		{
			ClericDomain_1.SetActive(true);
			ClericDomain_2.SetActive(true);
			mCharacter.cclass = CharClassEnum.Cleric;
		}
		else if (text == CharClassEnum.Wizard.ToString())
		{
			WizardSpecSelecter.SetActive(true);
			mCharacter.cclass = CharClassEnum.Wizard;
		}
	}

	private void WizardSpecHandler(string text)
	{
		if (mCharacter.attributes.Count != 1)
		{
			mCharacter.attributes.Clear();
			mCharacter.attributes.Add(0);
		}
		foreach (MagicSchool school in MagicSchool.GetValues(typeof(MagicSchool)))
		{
			if (school.ToString() == text)
			{
				mCharacter.attributes[0] = (int)school;
				break;
			}
			mCharacter.attributes[0] = (int)MagicSchool.NONE;
		}
	}

	private void DomainHandler(int domain, string text)
	{
		if (domain == 1 || domain == 2)
		{
			if (mCharacter.attributes.Count != 2)
			{
				mCharacter.attributes.Clear();
				mCharacter.attributes.Add(0);
				mCharacter.attributes.Add(0);
			}
			foreach (ClericDomain dom in ClericDomain.GetValues(typeof(ClericDomain)))
			{
				if (dom.ToString() == text)
				{
					mCharacter.attributes[domain - 1] = (int)dom;
				}
			}
		}
	}
}
