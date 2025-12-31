using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class Pathfinder
{
    public static Vector2Int[] FindPath(Vector2Int start, Vector2Int end, Vector2Int[] blockedArray, int n)
    {
        int width = BoardManager.Instance.GetMaxWidth();
        int height = BoardManager.Instance.GetInitBoardHeight();
        bool[,] visited = new bool[width, height];
        Vector2Int[,] parent = new Vector2Int[width, height];
        Vector2Int[] queue = new Vector2Int[width * height]; 

        int blockLength = n;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                visited[i, j] = false;
            }
        }
        for (int i = 0; i < blockLength; i++)
        {
            Vector2Int blockPos = blockedArray[i];
            visited[blockPos.x, blockPos.y] = true;
        }
        visited[start.x,start.y] = true;

        int index = 0;
        int indexToAdd = 1;
        queue[0] = start;
        while (index < indexToAdd)
        {
            if (queue[index] == end) break;

            int x = queue[index].x;
            int y = queue[index].y;
            
            


            if (x-1 > 0 && !visited[x-1, y])
            {
                visited[x-1, y] = true;
                queue[indexToAdd++] = new Vector2Int(x-1, y);
                parent[x-1, y] = queue[index];
            }
            if (x+1 < width && !visited[x+1, y])
            {
                visited[x+1, y] = true;
                queue[indexToAdd++] = new Vector2Int(x+1, y);
                parent[x+1, y] = queue[index];
            }
            if ( y-1 > 0  && !visited[x, y-1])
            {
                visited[x, y-1] = true;
                queue[indexToAdd++] = new Vector2Int(x, y-1);
                parent[x, y-1] = queue[index];
            }
            if (y + 1 < height && !visited[x, y+1])
            {
                visited[x, y+1] = true;
                queue[indexToAdd++] = new Vector2Int(x, y+1);
                parent[x, y+1] = queue[index];
            }

            index++;
        }
        Vector2Int[] path = new Vector2Int[width * height];
        int pathIndex = 0;
        Vector2Int currentVector = end;

        while (pathIndex < 100)
        {
            path[pathIndex++] = currentVector;
            currentVector = parent[currentVector.x, currentVector.y];
            if (currentVector == start) break;
        }

        Vector2Int[] realPath = new Vector2Int[pathIndex];
        for (int i=0; i < pathIndex; i++)
        {
            realPath[i] = path[i];
        }

        return realPath;
    }
}

