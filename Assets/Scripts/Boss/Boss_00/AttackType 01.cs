using System;
using System.Collections;
using System.Collections.Generic;
using CraneFSM.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boss_01
{
    public class AttackType_01 : AttackType
    {
        public FloatParameter damage;
        [SerializeField] private AttackRange attackRange;
        
        [Header("Execute Info")] 
        [SerializeField] private GameObject prefab;
        public float delayTime;
        
        // 使用 private 維護內部狀態
        public List<GameObject> cells = new List<GameObject>();
        public List<Vector2> placeCoordinate = new List<Vector2>();
        private Vector2Int gridSize;
        private List<Vector2> gridPoints = new List<Vector2>();

        public override void Execute()
        {
            // 初始化網格數據
            gridSize = attackRange.gridSize;
            gridPoints = attackRange.gridPoints;
            
            ClearCells();
            
            // 計算總格子數與目標安全區域數量 (35% - 75%)
            int totalCells = gridSize.x * gridSize.y;
            int targetSafeCells = Random.Range(totalCells * 7 / 20, totalCells * 15 / 20);
            
            // 0 = 攻擊區域 (生成 Prefab), 1 = 安全區域 (空白)
            int[] currentGrid = new int[totalCells];

            int currentSafeCells = 0;
            int safetyCounter = 0;

            // 嘗試生成安全區域塊
            while (currentSafeCells < targetSafeCells && safetyCounter < 1000)
            {
                safetyCounter++;
                
                // 隨機矩形大小 (15% - 35% 的長寬)
                int w = Random.Range(gridSize.x * 3 / 20, gridSize.x * 7 / 20);
                int h = Random.Range(gridSize.y * 3 / 20, gridSize.y * 7 / 20);

                // 如果加入此矩形會大幅超過目標安全數，則停止
                if (currentSafeCells + (w * h) > targetSafeCells)
                    break;
                
                // 嘗試放置矩形
                if (TryPlaceRectangle(currentGrid, w, h))
                {
                    currentSafeCells += w * h;
                }
            }

            // 根據網格生成特效
            SpawnEffects(currentGrid);

            StartCoroutine(ExecuteAnimation());
        }

        // 嘗試在網格中找到合適位置放置矩形 (嘗試 100 次)
        private bool TryPlaceRectangle(int[] grid, int w, int h)
        {
            for (int i = 0; i < 100; i++)
            {
                // 優化：直接在合法範圍內隨機，無需再檢查邊界
                int startX = Random.Range(0, gridSize.x - w + 1);
                int startY = Random.Range(0, gridSize.y - h + 1);
                Vector2Int pos = new Vector2Int(startX, startY);

                if (CanPlace(grid, pos, w, h))
                {
                    ApplyPlacement(grid, pos, w, h);
                    return true;
                }
            }
            return false;
        }

        // 應用矩形到網格 (將區域標記為 1)
        private void ApplyPlacement(int[] grid, Vector2Int pos, int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int index = (pos.x + x) + gridSize.x * (pos.y + y);
                    grid[index] = 1;
                }
            }
        }
        
        // 檢查是否可以放置 (不重疊且不相鄰其他安全區)
        private bool CanPlace(int[] grid, Vector2Int pos, int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    // 檢查鄰居 (確保安全區域之間有間隔)
                    if (HasOccupiedNeighbor(grid, pos.x + x, pos.y + y))
                        return false;
                }
            }
            return true;
        }

        // 檢查 3x3 範圍內是否有被佔用的格子
        private bool HasOccupiedNeighbor(int[] grid, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int cx = x + i;
                    int cy = y + j;

                    // 邊界檢查
                    if (cx >= 0 && cx < gridSize.x && cy >= 0 && cy < gridSize.y)
                    {
                        // 如果該格不是 0 (即已經被佔用為安全區域)，則返回 true
                        if (grid[cx + gridSize.x * cy] != 0)
                            return true;
                    }
                }
            }
            return false;
        }

        // 生成特效 Prefab
        private void SpawnEffects(int[] grid)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                // 0 代表攻擊區域
                if (grid[i] == 0)
                {
                    if (i < gridPoints.Count) // 安全檢查
                    {
                        GameObject effect = Instantiate(prefab, gridPoints[i], Quaternion.identity);
                        placeCoordinate.Add(attackRange.Coordinate(i));
                        
                        effect.transform.localScale = (Vector3)attackRange.cellSize;
                        cells.Add(effect);
                    }
                }
            }
        }
        
        private void ClearCells()
        {
            foreach (GameObject cell in cells)
            {
                if (cell != null) Destroy(cell);
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
            
            CraneFSM.StateMachine sm = GetComponent<CraneFSM.Core.State>().sm;
            if(sm)
                GetComponent<CraneFSM.Core.State>().sm.SetBool("ultIsRunning", false);
        }

        private void OnDrawGizmos()
        {
            if (placeCoordinate.Count == 0) return;

            foreach (Vector2 pos in placeCoordinate)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(gridPoints[(int)(pos.x + pos.y * gridSize.x)], 
                    new Vector3(attackRange.cellSize.x, attackRange.cellSize.y));
            }
        }
    }
}