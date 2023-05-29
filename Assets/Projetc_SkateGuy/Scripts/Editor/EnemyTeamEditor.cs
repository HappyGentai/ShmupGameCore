using System.Collections.Generic;
using UnityEngine;
using SkateGuy.GameElements;
using SkateGuy.GameElements.EnemyGroup;
using UnityEditor;

namespace SkateGuy.Editor
{
    [CustomEditor(typeof(EnemyTeam))]
    public class EnemyTeamEditor : UnityEditor.Editor
    {
        EnemyTeam enemyTeam;

        #region Editor GUI 
        private float bigSpace = 50;
        private string msgSaveTitle = "Save";
        private string msgSaveContent = "Overwrite current data?";
        private string msgSaveConfirm = "Confirm";
        private string msgSaveCancel = "Cancel";
        #endregion

        private void OnEnable()
        {

            enemyTeam = (EnemyTeam)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(bigSpace);
            GUILayout.Label("Editor Part");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load"))
            {
                LoadEnemyTeam();
            }
            if (GUILayout.Button("Save"))
            {
                SaveTeam();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void LoadEnemyTeam()
        {
            var teamDatas = enemyTeam.MemberDatas;
            var memberCount = teamDatas.Length;
            for (int index = 0; index < memberCount; ++index)
            {
                var memberData = teamDatas[index];
                var go = PrefabUtility.InstantiatePrefab(memberData.EnemyPrefab);
                var enemy = (Enemy)go;
                enemy.transform.SetParent(enemyTeam.transform);
                enemy.transform.localPosition = memberData.SetPosition;
            }
        }

        private void SaveTeam()
        {
            if (EditorUtility.DisplayDialog(msgSaveTitle, msgSaveContent, msgSaveConfirm, msgSaveCancel))
            {
                var childCount = enemyTeam.transform.childCount;
                List<EnemyTeamMemberData> newData = new List<EnemyTeamMemberData>();
                for (int index = 0; index < childCount; ++index)
                {
                    var child = enemyTeam.transform.GetChild(index);
                    
                    if (PrefabUtility.IsPartOfAnyPrefab(child))
                    {
                        var enemy = child.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            var enemyPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(enemy);
                            var setPos = enemy.transform.localPosition;
                            var newMemberData = new EnemyTeamMemberData(enemyPrefab, setPos);
                            newData.Add(newMemberData);
                        }
                    }
                }

                enemyTeam.MemberDatas = newData.ToArray();
                for (int index = childCount - 1; index >= 0; --index)
                {
                    var child = enemyTeam.transform.GetChild(index);
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
