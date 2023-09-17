using System.Collections;
using TMPro;
using UnityEngine;

public class TextTypeEffector : MonoBehaviour
{
	public float m_typeInterval;
	public TMP_Text m_sourceTextbox;
	public TMP_Text m_destinationTextbox;

	IEnumerator m_typingCoroutine = null;

	string m_prevText = string.Empty;
	bool m_forceCompleteType = false;

	void Update()
	{
		// If user clicks mouse0 or screen, force typing to complete.
		m_forceCompleteType = (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
							   Input.GetKeyUp(KeyCode.Mouse0);

		if (m_prevText != m_sourceTextbox.text)
		{
			m_prevText = m_sourceTextbox.text;

			if (m_typingCoroutine is not null)
				StopCoroutine(m_typingCoroutine);

			m_typingCoroutine = StartTyping();
			StartCoroutine(m_typingCoroutine);
		}
	}

	IEnumerator StartTyping()
	{
		string text = m_sourceTextbox.text;

		m_destinationTextbox.text = string.Empty;
		m_forceCompleteType = false;

		foreach (char ch in text)
		{
			if (m_forceCompleteType)
			{
				m_destinationTextbox.text = text;
				yield break;
			}

			yield return new WaitForSeconds(m_typeInterval);
			m_destinationTextbox.text += ch;
		}
	}
}
