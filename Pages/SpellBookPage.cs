using UnityEngine;
using System.Collections;

public class SpellBookPage : MonoBehaviour 
{
	public Transform FrameWork;
	public GameObject Background;
	public Storage storage;
	public GameObject BackButtonTarget;

	public virtual void OnClick_Back()
	{
		StartCoroutine(PageToRightPageAnimation());
	}

	public IEnumerator PageToRightPageAnimation()
	{
		TweenTransform tween = Background.GetComponent<TweenTransform>();
		tween.to = FrameWork.FindChild("Frame_Left");
		tween.from = FrameWork.FindChild("Frame_Right");
		tween.ResetToBeginning();
		tween.PlayForward();
		BackButtonTarget.SetActive(true);
		yield return new WaitForSeconds(tween.duration);
		gameObject.SetActive(false);
	}

	public IEnumerator PageToLeftPageAnimation()
	{
		TweenTransform tween = Background.GetComponent<TweenTransform>();
		tween.to = FrameWork.FindChild("Frame_Right");
		tween.from = FrameWork.FindChild("Frame_Left");
		tween.ResetToBeginning();
		tween.PlayForward();
		BackButtonTarget.SetActive(true);
		yield return new WaitForSeconds(tween.duration);
		gameObject.SetActive(false);
	}
}
