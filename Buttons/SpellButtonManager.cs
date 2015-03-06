using UnityEngine;
using System.Collections;

public class SpellButtonManager : MonoBehaviour 
{
	public GameObject prefab;
	public Storage storage;

	private void OnEnable()
	{
		StartCoroutine(Initiate());
	}

	private IEnumerator Initiate()
	{
		while (transform.childCount > 0)
		{
			yield return new WaitForSeconds(1f);
			GameObject.Destroy(transform.GetChild(0).gameObject);
			yield return new WaitForSeconds(1f);
		}

		for (int i = 0; i < 20; ++i)
		{
			GameObject newButton = (GameObject)Instantiate(prefab);
			newButton.transform.parent = transform;
			newButton.transform.localScale = Vector3.one;
			newButton.transform.localPosition = Vector3.zero;
		}
	}
}
