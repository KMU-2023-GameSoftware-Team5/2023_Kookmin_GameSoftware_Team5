using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScaleController : MonoBehaviour
{
	public float defaultScreenWidth;
	public float defaultScreenHeight;
	float m_defaultAspectRatio;

	CanvasScaler m_canvasScaler;
	bool m_enabled = true;

	void Start()
	{
		m_canvasScaler = GetComponent<CanvasScaler>();

		if (m_canvasScaler.IsUnityNull())
		{
			Debug.LogError("Can't find canvas scaler!");
			m_enabled = false;
		}

		m_defaultAspectRatio = defaultScreenWidth / defaultScreenHeight;

		if (!float.IsFinite(m_defaultAspectRatio))
		{
			Debug.LogError("Screen size value is invalid!");
			m_enabled = false;
		}
	}

	void Update()
	{
		if (!m_enabled)
			return;

		// Now aspect ratio
		float currentAspectRatio = (float)Screen.width / Screen.height;

		if (m_defaultAspectRatio < currentAspectRatio)
			m_canvasScaler.matchWidthOrHeight = 1; // Match with height
		else
			m_canvasScaler.matchWidthOrHeight = 0; // Match with width
	}
}
