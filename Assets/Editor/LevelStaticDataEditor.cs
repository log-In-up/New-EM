using Assets.Scripts.Logic;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.StaticData;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelStaticData = (LevelStaticData)target;

            if (GUILayout.Button("Collect Data"))
            {
                levelStaticData.EnemySpawnData =
                    FindObjectsOfType<SpawnMarker>()
                    .Select(idComponent => new EnemySpawnData(
                        idComponent.GetComponent<UniqueId>().Id,
                        idComponent.EnemyType,
                        idComponent.transform.position,
                        idComponent.transform.rotation))
                    .ToList();

                levelStaticData.LevelName = SceneManager.GetActiveScene().name;

                if (GameObject.FindGameObjectsWithTag(levelStaticData.PlayerSpawnPointTag).Length > 0)
                {
                    Transform playerSpawnPoint = GameObject.FindWithTag(levelStaticData.PlayerSpawnPointTag).transform;

                    levelStaticData.InitialPlayerPosition = playerSpawnPoint.position;
                    levelStaticData.InitialPlayerRotation = playerSpawnPoint.rotation;
                }
            }

            EditorUtility.SetDirty(target);
        }
    }
}