using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    
    public Vector2Int gridSize = new Vector2Int();
        
    [Header("Grid Info")]
    public Vector2 cellSize = new Vector2();
    public List<Vector2> gridPoints = new List<Vector2>();

    public Vector2 Coordinate(int index)
    {
        return new Vector2(index % gridSize.x, index / gridSize.x);
    }

    public int Index(Vector2 coordinate)
    {
        return (int)(coordinate.x + coordinate.y * gridSize.y);
    }
    
    private void OnDrawGizmos()
    {
        Bounds b = boxCollider2D.bounds;
        
        if(b == null) return;
        
        cellSize = new Vector2(b.size.x / gridSize.x, b.size.y / gridSize.y);
        List<Vector2> currentPoints = new List<Vector2>();

        Vector3 minPosition = b.min + (Vector3)cellSize / 2;
        for (int x= 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 currentPosition = minPosition + new Vector3(cellSize.x * x, cellSize.y * y);
                Color cellColor;
                    
                if (y % 2 == 0)
                {
                    cellColor = x % 2 == 0 ? Color.black : Color.white;
                }
                else
                {
                    cellColor = x % 2 == 1 ? Color.black : Color.white;
                }

                cellColor.a = .5f;
                    
                Gizmos.color = cellColor;
                Gizmos.DrawCube(currentPosition, cellSize);
                currentPoints.Add(currentPosition);
            }
        }
            
        gridPoints = currentPoints;
    }
}
