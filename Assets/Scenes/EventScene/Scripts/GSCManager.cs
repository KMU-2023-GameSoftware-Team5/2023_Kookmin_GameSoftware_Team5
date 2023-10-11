using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GSC
{
    [Serializable]
    public struct GSCCallback
    {
        public string name;
        public UnityEvent onCall;
    }

    public class GSCManager : MonoBehaviour
    {
        [SerializeField] GSCScript m_GSCScript;

        [SerializeField] GSCTextbox m_GSCTextbox;
        [SerializeField] GameObject m_buttonPrefab;
        [SerializeField] Transform m_buttonParent;
        [SerializeField] Image m_image;

        [SerializeField] string m_startNode;
        [SerializeField] char m_prefix;

        // Need to be public?
        [SerializeField] List<GSCCallback> m_callbacks;

        List<GSCScriptLine> m_scriptLines;
        IEnumerator m_scriptExecute;

        void OnEnable()
        {
            DestroyAllButtons();
            m_GSCTextbox.Clear();

            m_image.sprite = m_GSCScript.image;

            if (m_scriptLines is null)
            {
                var parser = new GSCParser(m_GSCScript);
                m_scriptLines = parser.Parse(m_prefix);
            }

            if (m_scriptExecute is not null)
                StopCoroutine(m_scriptExecute);

            var controller = new GSCController(
                m_scriptLines,
                m_callbacks,
                m_GSCTextbox,
                CreateButton,
                DestroyAllButtons,
                m_startNode
            );

            var executer = new GSCExecuter(controller);

            m_scriptExecute = executer.Execute();
            StartCoroutine(m_scriptExecute);
        }

        Tuple<Button, TMP_Text> CreateButton()
        {
            GameObject buttonObj = Instantiate(m_buttonPrefab, m_buttonParent);

            return Tuple.Create(
                buttonObj.GetComponent<Button>(),
                buttonObj.GetComponentInChildren<TMP_Text>()
            );
        }

        void DestroyAllButtons()
        {
            foreach (Transform child in m_buttonParent)
                Destroy(child.gameObject);
        }
    }
}
