using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

[Flags]
public enum WallState
{
    LEFT = 1,
    RIGHT = 2,
    UP = 4,
    DOWN = 8,

    VISITED = 128
}

public struct WallPosition
{
    public int X;
    public int Y;
}

public struct Neighbor
{
    public WallPosition neighborPosition;
    public WallState sharedWall;
}


public static class MazeGenerator
{

    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }

    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        var rng = new System.Random();
        var positionStack = new Stack<WallPosition>();
        var randomPosition = new WallPosition { X = rng.Next(0, width), Y = rng.Next(0, height) };

        maze[randomPosition.X, randomPosition.Y] |= WallState.VISITED;
        positionStack.Push(randomPosition);

        while(positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var currentNeighbors = GetUnvisitedNeighbors(current, maze, width, height);

            if(currentNeighbors.Count > 0)
            {
                positionStack.Push(current);

                var randNeighborIndex = rng.Next(0, currentNeighbors.Count);
                var randomNeighbor = currentNeighbors[randNeighborIndex];

                var nPosition = randomNeighbor.neighborPosition;
                maze[current.X, current.Y] &= ~randomNeighbor.sharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbor.sharedWall);

                maze[nPosition.X, nPosition.Y] |= WallState.VISITED;

                positionStack.Push(nPosition);
            }
        }
        return maze;
    }

    private static List<Neighbor> GetUnvisitedNeighbors(WallPosition p, WallState[,] maze, int width, int height)
    {
        List<Neighbor> neighbors = new List<Neighbor>();
        if (p.X > 0)
        {
            if (!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED))
            {
                neighbors.Add(new Neighbor { neighborPosition = new WallPosition { X = p.X - 1, Y = p.Y }, sharedWall = WallState.LEFT });
            }
        }

        if (p.X < width - 1)
        {
            if (!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED))
            {
                neighbors.Add(new Neighbor { neighborPosition = new WallPosition { X = p.X + 1, Y = p.Y }, sharedWall = WallState.RIGHT });
            }
        }

        if (p.Y > 0)
        {
            if (!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED))
            {
                neighbors.Add(new Neighbor { neighborPosition = new WallPosition { X = p.X, Y = p.Y - 1 }, sharedWall = WallState.DOWN });
            }
        }

        if (p.Y < height - 1)
        {
            if (!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED))
            {
                neighbors.Add(new Neighbor { neighborPosition = new WallPosition { X = p.X, Y = p.Y + 1 }, sharedWall = WallState.UP });
            }
        }

        return neighbors;
    }
    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initialState = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = initialState;
            }
        }
        return ApplyRecursiveBacktracker(maze, width, height);
    }
}
