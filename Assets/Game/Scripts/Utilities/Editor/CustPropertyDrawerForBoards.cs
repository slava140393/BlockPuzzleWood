using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Utilities.Editor
{
	[CustomPropertyDrawer(typeof(ArrayLayoutForBoards))]
	public class CustPropertyDrawerForBoards : PropertyDrawer
	{
		private int _row = 10;
		public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
			EditorGUI.PrefixLabel(position,label);
			Rect newposition = position;
			newposition.y += 18f;
			SerializedProperty data = property.FindPropertyRelative("Rows");
			for(int j=0;j<_row;j++){
				SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("Row");
				newposition.height = 18f;
				if(row.arraySize != _row)
					row.arraySize = _row;
				newposition.width = position.width/_row;
				for(int i=0;i<_row;i++){
					EditorGUI.PropertyField(newposition,row.GetArrayElementAtIndex(i),GUIContent.none);
					newposition.x += newposition.width-18f;
				}

				newposition.x = position.x;
				newposition.y += 18f;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
			return 18f * 8;
		}
	}
}
