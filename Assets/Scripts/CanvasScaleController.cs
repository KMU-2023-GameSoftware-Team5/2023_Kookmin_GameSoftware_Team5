using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CanvasScaleController : MonoBehaviour
{
	public float defaultScreenWidth;
	public float defaultScreenHeight;
	float m_defaultAspectRatio;

	[SerializeField] bool m_enable = true;

	CanvasScaler m_canvasScaler;

	void Start()
	{
		m_canvasScaler = GetComponent<CanvasScaler>();

		if (m_canvasScaler.IsUnityNull())
		{
			Debug.LogError("Can't find canvas scaler!");
			m_enable = false;
		}

		m_defaultAspectRatio = defaultScreenWidth / defaultScreenHeight;

		if (!float.IsFinite(m_defaultAspectRatio))
		{
			Debug.LogError("Screen size value is invalid!");
			m_enable = false;
		}
	}

	void Update()
	{
		if (!m_enable)
			return;

		// Now aspect ratio
		float currentAspectRatio = (float)Screen.width / Screen.height;

		if (m_defaultAspectRatio < currentAspectRatio)
			m_canvasScaler.matchWidthOrHeight = 1; // Match with height
		else
			m_canvasScaler.matchWidthOrHeight = 0; // Match with width
	}
}
