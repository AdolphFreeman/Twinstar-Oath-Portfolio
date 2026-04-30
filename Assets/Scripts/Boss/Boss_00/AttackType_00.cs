using System;
using System.Collections;
using System.Collections.Generic;
using CraneFSM.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Boss_001
{
    public class AttackType_00 : AttackType
    {
        public AttackRange attackRange;
        public FloatParameter damage;

        [Header("Execute Info")] 
        public GameObject prefab;
        public float delayTime;

        [Header("Execute Data")] 
        private List<GameObject> _inRangePlayers = new List<GameObject>();
        public List<GameObject> cells = new List<GameObject>();
        public List<Vector2Int> placeCoordinate = new  List<Vector2Int>();

        public override void Execute()
        {
            Vector2Int gridSize = attackRange.gridSize;
            List<Vector2> gridPoints = attackRange.gridPoints;
            
            ClearCells();

            int randomXNum = UnityEngine.Random.Range(gridSize.x * 2 / 10, gridSize.x * 6 / 10);
            int randomYNum = UnityEngine.Random.Range(gridSize.y * 2 / 10, gridSize.y * 6 / 10);

            // 使用 HashSet 獲取不重複的隨機索引，提升效能
            HashSet<int> xIndexes = new HashSet<int>();
            while (xIndexes.Count < randomXNum)
            {
                xIndexes.Add(UnityEngine.Random.Range(0, gridSize.x));
            }

            HashSet<int> yIndexes = new HashSet<int>();
            while (yIndexes.Count < randomYNum)
            {
                yIndexes.Add(UnityEngine.Random.Range(0, gridSize.y));
            }

            // 使用 HashSet 避免在十字交加處重複生成物件
            HashSet<Vector2Int> uniqueCoordinates = new HashSet<Vector2Int>();
            
            foreach (int xIndex in xIndexes)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    uniqueCoordinates.Add(new Vector2Int(xIndex, y));
                }
            }

            foreach (int yIndex in yIndexes)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    uniqueCoordinates.Add(new Vector2Int(x, yIndex));
                }
            }

            foreach (Vector2Int coordinate in uniqueCoordinates)
            {
                placeCoordinate.Add(coordinate);
                
                GameObject effect = Instantiate(prefab, gridPoints[coordinate.x + gridSize.x * coordinate.y], Quaternion.identity);
                effect.transform.localScale = attackRange.cellSize;
                
                cells.Add(effect);
            }

            StartCoroutine(ExecuteAnimation());
        }

        void ClearCells()
        {
            foreach (GameObject cell in cells)
            {
                if (cell != null)
                {
                    Destroy(cell);
                }
            }
            
            cells.Clear();
            placeCoordinate.Clear();
        }

        void ApplyDamage()
        {
            List<GameObject> hitPlayers = new List<GameObject>();
            LayerMask mask = LayerMask.GetMask("Player");

            foreach (Vector2 coordinate in placeCoordinate)
            {
                int index = (int)(coordinate.x + attackRange.gridSize.x * coordinate.y);
                Vector2 center = attackRange.gridPoints[index];
                Vector2 size = attackRange.cellSize;
                
                Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0,  mask);

                foreach (Collider2D hit in hits)
                {
                    if(!hitPlayers.Contains(hit.gameObject))
                        hitPlayers.Add(hit.gameObject);
                }
            }
            
            print($"Hit Player: {hitPlayers.Count}");
            foreach (GameObject player in hitPlayers)
            {
                PlayerCore core = player.GetComponent<PlayerCore>();
                core.GetDamage(damage.value);
            }
        }
        
        IEnumerator ExecuteAnimation()
        {
            yield return new WaitForSeconds(delayTime);
            ApplyDamage();
            ClearCells();
            
            var state = GetComponent<CraneFSM.Core.State>();
            if (state != null && state.sm != null)
                state.sm.SetBool("ultIsRunning", false);
        }
        
        private void OnDrawGizmos()
        {
            if (placeCoordinate.Count == 0 || attackRange == null || attackRange.gridPoints == null || attackRange.gridPoints.Count == 0) return;

            Gizmos.color = Color.red;
            foreach (Vector2Int pos in placeCoordinate)
            {
                int index = pos.x + attackRange.gridSize.x * pos.y;
                if (index >= 0 && index < attackRange.gridPoints.Count)
                {
                    Gizmos.DrawCube(attackRange.gridPoints[index], 
                        new Vector3(attackRange.cellSize.x, attackRange.cellSize.y, 1f));
                }
            }
        }
    }
}