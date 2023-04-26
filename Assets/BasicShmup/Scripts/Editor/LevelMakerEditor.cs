using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ShmupCore.Editor
{
    [CustomEditor(typeof(LevelMaker))]
    public class LevelMakerEditor : UnityEditor.Editor
    {
        LevelMaker levelMaker;

        private string setEnemyID = "";
        private int setLaneNumber = 0;

        private string savePath = "/BasicShmup/Resources/LevelData.txt";
        private TextAsset loadData = null;

        #region Editor GUI 
        private float smallSpace = 20;
        private float bigSpace = 50;
        private string playModeTitle = "Editor play mode";
        private bool showPlayMode = false;
        private string setEnemyTitle = "Set enemy part";
        private bool showSetEnemy = false;
        private string saveNLoadTitle = "Save & Load part";
        private bool showSaveNLoad = false;
        private string msgSaveTitle = "Save level data?";
        private string msgSaveContent = "Save to ";
        private string msgSaveConfirm = "Save";
        private string msgSaveCancel = "Cancel";
        private string msgLoadTitle = "Load level data?";
        private string msgLoadContent = "";
        private string msgLoadConfirm = "Load";
        private string msgLoadCancel = "Cancel";
        #endregion

        void OnEnable()
        {
            levelMaker = (LevelMaker)target;
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(bigSpace);
            GUILayout.Label("Editor Part");
            #region  Editor play mode
            GUILayout.Space(smallSpace);
            showPlayMode = EditorGUILayout.Foldout(showPlayMode, playModeTitle);
            if (showPlayMode)
            {
                GUILayout.Label("GameTime: " + levelMaker.GameTime);
                GUILayout.Space(smallSpace);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Play") && Application.isPlaying)
                {
                    levelMaker.PlayLevel();
                }
                if (GUILayout.Button("Stop") && Application.isPlaying)
                {
                    levelMaker.StopLevel();
                }
                if (GUILayout.Button("ReSet") && Application.isPlaying)
                {
                    levelMaker.ReSetLevel();
                }
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region Set enemy part
            GUILayout.Space(smallSpace);
            showSetEnemy = EditorGUILayout.Foldout(showSetEnemy, setEnemyTitle);
            if (showSetEnemy)
            {
                //  Set id
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("EnemyID:");
                setEnemyID = EditorGUILayout.TextField(setEnemyID);
                EditorGUILayout.EndHorizontal();
                //  Set lane
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Set LaneNumber:");
                setLaneNumber = EditorGUILayout.IntField(setLaneNumber);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Spawn Enemy") && Application.isPlaying)
                {
                    //  When set enemy, also create spawn data
                    levelMaker.SetEnemy(setEnemyID, setLaneNumber);
                    var spawnEnemyDatas = levelMaker.EnemySpawnDatas;
                    var spawnEnemyData = new EnemySpawnData(setEnemyID, setLaneNumber, levelMaker.GameTime);
                    spawnEnemyDatas.Add(spawnEnemyData);
                }
            }
            #endregion

            #region Data save and load part
            GUILayout.Space(smallSpace);
            showSaveNLoad = EditorGUILayout.Foldout(showSaveNLoad, saveNLoadTitle);
            if (showSaveNLoad)
            {
                //  Set save path
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Save path and file name:");
                savePath = EditorGUILayout.TextField(savePath);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Save level data"))
                {
                    if (EditorUtility.DisplayDialog(msgSaveTitle, msgSaveContent + savePath, msgSaveConfirm, msgSaveCancel))
                    {
                        var data = new EnemySpawnDatas(levelMaker.EnemySpawnDatas);
                        var toJson = JsonUtility.ToJson(data);
                        TextSaveLoad.Save(toJson, Application.dataPath + savePath);
                    }
                }
                GUILayout.Space(smallSpace);
                //  Load save data
                loadData = (TextAsset)EditorGUILayout.ObjectField(loadData, typeof(TextAsset), true);
                if (GUILayout.Button("Load level data") && loadData!=null)
                {
                    if (EditorUtility.DisplayDialog(msgLoadTitle, msgLoadContent, msgLoadConfirm, msgLoadCancel))
                    {
                        var data = JsonUtility.FromJson<EnemySpawnDatas>(loadData.text);
                        if (data != null)
                        {
                            levelMaker.EnemySpawnDatas = data.EnemySpawnData;
                        }
                    }
                }
            }
            #endregion
        }
    }
}
