using UnityEngine;
using UnityEngine.Events;

namespace GameMap
{
	[CreateAssetMenu(menuName = "Map Area", order = 3)]
	public class AreaData : ScriptableObject
	{
		public string areaName;
		public Sprite sprite;
		public UnityEvent onClick;
	}
}
