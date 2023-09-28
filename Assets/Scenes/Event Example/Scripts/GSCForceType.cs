using UnityEngine;

namespace GSC
{
	public class GSCForceType : MonoBehaviour
	{
		GSCText m_textbox;
		bool m_enable = true;

		void Start()
		{
			if (!TryGetComponent(out m_textbox))
			{
				Debug.LogError("(GSC)GSCForceType can't find GSCText");
				m_enable = false;
			}
		}

		void Update()
		{
			if (!m_enable)
				return;

			m_textbox.ForceCompleteType |= Input.GetMouseButtonDown(0) ||
				(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
		}
	}
}
