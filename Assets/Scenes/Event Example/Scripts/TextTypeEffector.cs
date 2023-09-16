using System.Collections;
using TMPro;
using UnityEngine;

public class TextTypeEffector : MonoBehaviour
{
	public float typeInterval;

	TMP_Text m_text;

	IEnumerator m_typingCoroutine = null;
	bool m_forceCompleteType = false;

	void Start()
	{
		m_text = GetComponent<TMP_Text>();
		Type(m_text.text);
	}

	void Update()
	{
		// If user clicks mouse0 or screen, force typing to complete.
		m_forceCompleteType = (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
							   Input.GetKeyUp(KeyCode.Mouse0);
	}

	public void Type(string text)
	{
		if (m_typingCoroutine is not null)
			StopCoroutine(m_typingCoroutine);

		m_typingCoroutine = Typing(text);
		StartCoroutine(m_typingCoroutine);
	}

	IEnumerator Typing(string text)
	{
		m_forceCompleteType = false;
		m_text.text = string.Empty;

		foreach (char ch in text)
		{
			if (m_forceCompleteType)
			{
				m_text.text = text;
				yield break;
			}

			yield return new WaitForSeconds(typeInterval);
			m_text.text += ch;
		}
	}
}
