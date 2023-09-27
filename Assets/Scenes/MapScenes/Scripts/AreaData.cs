using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Map Area", order = 3)]
public class AreaData : ScriptableObject
{
	[SerializeField] Sprite sprite;
	[SerializeField] string areaName;

	public void SetArea(GameObject areaObject)
	{
		areaObject.GetComponent<Image>().sprite = sprite;
	}

	public void SetIcon(GameObject iconObject)
	{
		iconObject.GetComponentInChildren<Image>().sprite = sprite;
		iconObject.GetComponentInChildren<TMP_Text>().text = areaName;
	}
}
