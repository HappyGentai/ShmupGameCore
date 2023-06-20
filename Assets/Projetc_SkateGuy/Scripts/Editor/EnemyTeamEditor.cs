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
        List<Enemy> summonEnemys = new List<Enemy>();

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
            enemyTeam.OnMemberCreate.AddListener((Enemy enemy) =>
            {
                summonEnemys.Add(enemy);
            });
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
            GUILayout.Space(bigSpace);
            GUILayout.Label("Test Part(Play mode only)");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Summon") && Application.isPlaying)
            {
                StopSummon();
                enemyTeam.SummonMember();
            }
            if (GUILayout.Button("StopSummon") && Application.isPlaying)
            {
                StopSummon();
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
                var enemySpawnHelper = enemy.gameObject.AddComponent<EnemySpawnHelper>();
                enemySpawnHelper.m_TargetObject = enemy;
                enemySpawnHelper.m_DelayTime = memberData.DelaySpawnTime;
                enemySpawnHelper.m_LogicData = memberData.LogicData;
                enemySpawnHelper.SetLogicData(enemySpawnHelper.m_LogicData);
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
                            var spawnHelper = enemy.gameObject.GetComponent<EnemySpawnHelper>();
                            string logicData = "";
                            float delayTime = 0;
                            if (spawnHelper != null)
                            {
                                logicData = spawnHelper.GetLogicData();
                                delayTime = spawnHelper.m_DelayTime;
                            }
                            var newMemberData = new EnemyTeamMemberData(enemyPrefab, setPos, delayTime, logicData);
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

        private void StopSummon()
        {
            var summonEnemyCount = summonEnemys.Count;
            for (int index = 0; index < summonEnemyCount; ++index)
            {
                var enemy = summonEnemys[index];
                enemy.Recycle();
            }
            summonEnemys.Clear();
            enemyTeam.StopSummon();
        }
    }
}
