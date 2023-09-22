using UnityEngine;

[CreateAssetMenu(menuName = "GSCript", order = 2)]
public class GSCScript : ScriptableObject
{
	[TextArea(10, 200)]
	public string script;
}
