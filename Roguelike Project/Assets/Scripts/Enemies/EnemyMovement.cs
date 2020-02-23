using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovingObject
{
    public Vector3 targetPosition;
    public float stopDistance;
    public bool pathCalculated;
    private List<Node> _path = new List<Node>();

    private class Node
    {
        public int x;
        public int y;
        public float fCost;
        public Node parent;

        public Node(int x, int y, float fCost)
        {
            this.x = x;
            this.y = y;
            this.fCost = fCost;
        }
    }


    protected override void Start()
    {
        GameManager.Instance.enemiesAlive.Add(this);
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        base.Start();
    }

    private void Update()
    {
        if (canMove)
            AttemptMove();
    }


    protected override void Movement()
    {
        Node currentNode = default;
        bool availablePath = false;
        if (pathCalculated == false)
        {
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            Node targetNode = new Node((int)targetPosition.x, (int)targetPosition.y, 0);

            //add start node
            openNodes.Add(new Node((int)transform.position.x, (int)transform.position.y, 0));

            do
            {
                currentNode = GetLowestFCostNode(openNodes);
                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode.x == targetNode.x && currentNode.y == targetNode.y)
                {
                    availablePath = true;
                    break;
                }

                List<Node> currentNodeNeighbours = GetCurrentNodeNeighbours(currentNode);
                foreach (Node neighbour in currentNodeNeighbours)
                {
                    if (closedNodes.Any(node => node.x == neighbour.x && node.y == neighbour.y))
                        continue;

                    if (openNodes.Any(node => node.x == neighbour.x && node.y == neighbour.y) == false)
                    {
                        neighbour.parent = currentNode;
                        openNodes.Add(neighbour);
                    }
                    else
                    {
                        Node nodeInList = openNodes.Find(node => node.x == neighbour.x && node.y == neighbour.y);
                        if (nodeInList.fCost > neighbour.fCost)
                        {
                            nodeInList.fCost = neighbour.fCost;
                            nodeInList.parent = currentNode;
                        }
                    }
                }
            } while (openNodes.Count != 0);

            _path = new List<Node>();
            pathCalculated = true;

            //Create path to follow
            if (availablePath == true)
            {
                Node nodeToFollow = currentNode;
                while (nodeToFollow != null)
                {
                    nodeToFollow = nodeToFollow.parent;
                    _path.Add(nodeToFollow);
                }
            }
        }

        //Only move if an available path exist
        if (_path.Count > 1 && Vector2.Distance(transform.position, targetPosition) >= 1.5f)
        {
            _path.RemoveAt(_path.Count - 1);
            int xDir = _path[_path.Count - 1].x - (int)transform.position.x;
            int yDir = _path[_path.Count - 1].y - (int)transform.position.y;
            Move(xDir, yDir);
        }
        else
        {
            int xDir = (int)targetPosition.x - (int)transform.position.x;
            int yDir = (int)targetPosition.y - (int)transform.position.y;
            _clsSpriteManager.CheckMovement(xDir, yDir);
        }
        
    }

    private Node GetLowestFCostNode(List<Node> nodes)
    {
        float lowestFCost = float.MaxValue;
        Node lowestFCostNode = default;
        foreach (Node node in nodes)
        {
            if (node.fCost < lowestFCost)
            {
                lowestFCostNode = node;
                lowestFCost = node.fCost;
            }
        }
        return lowestFCostNode;
    }

    
    private List<Node> GetCurrentNodeNeighbours(Node node)
    {
        List<Node> currentNodeNeighbours = new List<Node>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (((1 << GameManager.Instance.tilesLayers[node.x + x, node.y + y]) & blockingLayers) == 0)
                {
                    float gCost = Vector2.Distance(new Vector2(node.x + x, node.y + y), transform.position);
                    float hCost = Vector2.Distance(new Vector2(node.x + x, node.y + y), targetPosition);
                    float fCost = gCost + hCost;
                    currentNodeNeighbours.Add(new Node(node.x + x, node.y + y, fCost));
                }
            }
        }
        return currentNodeNeighbours;
    }
}
