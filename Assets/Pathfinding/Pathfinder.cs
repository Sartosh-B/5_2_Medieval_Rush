using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;


    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null)
        {
            grid = gridManager.Grid;
        }

        
    }

    void Start()
    {
        //setting the start and end coordinates for the pathfinding algoritm
        startNode = gridManager.Grid[startCoordinates];  //grabing start node from grid manager (which contains all nodes on map starting from (x,y) = (0,0)) using Vector2Int startCoordinates which is declared in editor Object pool gameobject
        destinationNode = gridManager.Grid[destinationCoordinates]; //same as above but with destination coordinate
        BreadthFirstSearch();
        BuildPath();
    }

    
    void ExploreNeighbors() //function that find neighbors for node
    {
        List<Node> neighbors = new List<Node>();        //declaring list of neighbors

        //loop to fill the list, the loop is using list of directions
        foreach (Vector2Int direction in directions)    
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction; 

            if (grid.ContainsKey(neighborCoords)) 
            {
                neighbors.Add(grid[neighborCoords]);

                //TODO: Remove afer testing
                //grid[neighborCoords].isExpored = true;
                //grid[currentSearchNode.coordinates].isPath = true;
            }
            foreach (Node neighbor in neighbors)
            {
                if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
                {
                    neighbor.connectedTo = currentSearchNode;
                    reached.Add(neighbor.coordinates, neighbor);
                    frontier.Enqueue(neighbor);
                }
            }

        }
    }

    void BreadthFirstSearch()
    {
        bool isRunning = true;

        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

        while(frontier.Count > 0 && isRunning == true)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExpored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() //function that returns List of nodes that showing a shortest path
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }
}
