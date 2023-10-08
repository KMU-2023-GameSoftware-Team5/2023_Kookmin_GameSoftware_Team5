using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace GSC
{
	[CustomEditor(typeof(GSCTextbox))]
	[CanEditMultipleObjects]
	public class GSCTextboxEditor : TMP_EditorPanelUI
	{
		SerializedProperty m_typeInterval;

		protected override void OnEnable()
		{
			m_typeInterval = serializedObject.FindProperty("m_typeInterval");

			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Rect rect = EditorGUILayout.GetControlRect(false, 22);
			GUI.Label(rect, new GUIContent("<b>GSC Textbox Settings</b>"), TMP_UIStyleManager.sectionHeader);

			EditorGUILayout.PropertyField(m_typeInterval);
			serializedObject.ApplyModifiedProperties();

			base.OnInspectorGUI();
		}
	}
}
