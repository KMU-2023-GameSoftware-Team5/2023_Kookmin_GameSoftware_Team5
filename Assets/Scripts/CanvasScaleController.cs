using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CanvasScaleController : MonoBehaviour
{
	public float screenWidth = 1920;
	public float screenHeight = 1080;

	[SerializeField] bool m_enable = true;

	CanvasScaler m_canvasScaler;

	void Start()
	{
		if (!TryGetComponent(out m_canvasScaler))
		{
			Debug.LogError("Can't find canvas scaler");
			m_enable = false;
		}
	}

	void Update()
	{
		if (!m_enable)
			return;

		float nowAspectRatio = screenWidth / screenHeight;

		if (!float.IsFinite(nowAspectRatio))
		{
			Debug.LogError("Screen size value is invalid");
			return;
		}

		// Now aspect ratio
		float currentAspectRatio = (float)Screen.width / Screen.height;

		if (nowAspectRatio < currentAspectRatio)
			m_canvasScaler.matchWidthOrHeight = 1; // Match with height
		else
			m_canvasScaler.matchWidthOrHeight = 0; // Match with width
	}
}
