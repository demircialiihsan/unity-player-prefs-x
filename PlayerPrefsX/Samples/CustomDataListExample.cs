using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityPlayerPrefsX.Samples
{
    public class CustomDataListExample : MonoBehaviour
    {
        [Serializable]
        class EnemyData
        {
            [SerializeField] int enemyType;
            [SerializeField] Color color;
            [SerializeField] Vector3 position;

            public EnemyData(int enemyType, Color color, Vector3 position)
            {
                this.enemyType = enemyType;
                this.color = color;
                this.position = position;
            }

            public override string ToString()
            {
                // get 'enemyType' and 'position' data in a colored string for the console
                return string.Format("<color=#{0:X2}{1:X2}{2:X2}>Enemy Type: {3}, Position: {4}</color>",
                    (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), enemyType, position);
            }
        }

        List<EnemyData> enemies;

        void Start()
        {
            if (PlayerPrefsX.HasKey("enemyList"))
            {
                enemies = PlayerPrefsX.Get<List<EnemyData>>("enemyList");
            }
            else
            {
                enemies = new List<EnemyData>();
            }
        }

        void OnDestroy()
        {
            PlayerPrefsX.Set("enemyList", enemies);
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 30), "Add Enemy"))
            {
                AddRandomEnemy();
            }
            if (GUI.Button(new Rect(0, 30, 200, 30), "Remove Last Enemy"))
            {
                RemoveLastEnemy();
            }
            if (GUI.Button(new Rect(0, 60, 200, 30), "Print All Enemies"))
            {
                PrintEnemies();
            }
        }

        void AddRandomEnemy()
        {
            var enemyType = Random.Range(1, 6);
            var color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            var position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);

            enemies.Add(new EnemyData(enemyType, color, position));
        }

        void RemoveLastEnemy()
        {
            if (enemies.Count > 0)
                enemies.RemoveAt(enemies.Count - 1);
        }

        void PrintEnemies()
        {
            foreach (var enemy in enemies)
                Debug.Log(enemy);
        }
    }
}