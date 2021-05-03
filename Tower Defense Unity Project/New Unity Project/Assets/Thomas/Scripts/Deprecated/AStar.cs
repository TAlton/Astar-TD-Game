using System.Collections;
using System.Collections.Generic;
using Assets.Thomas.Scripts;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public TestGridCreation m_Grid;
    private const int COST_MOVE_STRAIGHT = 10;
    private const int COST_MOVE_DIAGONAL = 14;
    private int[,] m_ClosedNodes;
    private int[,] m_OpenNodes;
    private float delta;
    // Start is called before the first frame update
    void Start()
    {
        m_Grid = this.GetComponent<TestGridCreation>();
        delta = m_Grid.GridSpacing * 0.49f;
    }

    // Update is called once per frame
    void Update()
    {
        bool lsTest = false;
        if (!lsTest)
        {
            lsTest = !lsTest;
            for(int i = 0; i < 5; i++)
            {
                m_Grid.Tiles[1, i, 0].Blocked = true;
            }
            ResolveAStar(m_Grid.Tiles[0, 0, 0], m_Grid.Tiles[3, 0, 0]);
        }
    }
    private double CalcHeuristic()
    {
        return 1;
    }
    GridItem GetTileByPosition(Vector3 argPos)
    {
        for(int i = 0; i < m_Grid.x; i++)
        {
            for(int j = 0; j < m_Grid.z; j++)
            {
                if (null == m_Grid.Tiles[i, j, 0]) continue;

                GridItem lsGI = m_Grid.Tiles[i, j, 0];
                //make sure the tile is available
                if(!lsGI.Occupied && !lsGI.Blocked)
                {
                    //check x axis bounds
                    if(lsGI.Tile.transform.position.x + delta >= argPos.x &&
                        lsGI.Tile.transform.position.x - delta <= argPos.x)
                    {
                        //check z axis bounds
                        if(lsGI.Tile.transform.position.z + delta >= argPos.z &&
                            lsGI.Tile.transform.position.z - delta <= argPos.z)
                        {
                            return lsGI;
                        }
                    }
                }
            }
        }

        return null;
    }
    float GetDistance(GridItem argGI1, GridItem argGI2)
    {
        float lsDeltaX          = Mathf.Abs(argGI1.Tile.transform.position.x - argGI2.Tile.transform.position.x);
        float lsDeltaY          = Mathf.Abs(argGI1.Tile.transform.position.z - argGI2.Tile.transform.position.z);

        return (lsDeltaX > lsDeltaY) ? CalcDistance(lsDeltaX, lsDeltaY) : CalcDistance(lsDeltaY, lsDeltaX);
    }
    private float CalcDistance(float argX, float argY)
    {
        return COST_MOVE_DIAGONAL * Mathf.Min(argX, argY) + COST_MOVE_STRAIGHT *  Mathf.Abs(argX - argY);
    }
    private List<GridItem> TracePath(GridItem argStart, GridItem argDest)
    {
        Debug.Log("Start Trace");
        List<GridItem> lsPath   = new List<GridItem>();
        GridItem lsCurrent      = argDest;
        lsPath.Add(argDest);

        //traverse parents from dest to start
        while(lsCurrent.Parent != null)
        {
            lsPath.Add(lsCurrent.Parent);
            lsCurrent.Parent.Tile.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Assets/Thomas/Resources/pathing");
            lsCurrent           = lsCurrent.Parent;
        }

        lsPath.Reverse();
        return lsPath;
    }
    void ResolveAStar(Vector3 argStartPos, Vector3 argDestPos)
    {
        GridItem lsStart = GetTileByPosition(argStartPos);
        GridItem lsDest = GetTileByPosition(argDestPos);

        if (null == lsStart || null == lsDest) return;

        List<GridItem> lsOpenItems = new List<GridItem> { lsStart };
        HashSet<GridItem> lsClosedItems = new HashSet<GridItem>();

        //initialising costs of path to default val
        for (int i = 0; i < m_Grid.x; i++)
        {
            for (int j = 0; j < m_Grid.y; j++)
            {
                m_Grid.Tiles[i, 0, j].g = int.MaxValue;
                m_Grid.Tiles[i, 0, j].Parent = null;
            }
        }

        lsStart.g = 0;
        lsStart.h = GetDistance(lsStart, lsDest);
        lsStart.fCost();

        GridItem lsCurrent = lsOpenItems[0];

        while (lsOpenItems.Count > 0)
        {

            for(int i = 1; i < lsOpenItems.Count; i++)
            {
                if (lsOpenItems[i].GetFCost() < lsCurrent.GetFCost() || lsCurrent.GetFCost() == lsOpenItems[i].GetFCost() && lsOpenItems[i].h < lsCurrent.h)
                {
                    lsCurrent = lsOpenItems[i];
                }
            }

            lsOpenItems.Remove(lsCurrent);
            lsClosedItems.Add(lsCurrent);

            //get closest neighbour based on g cost
            foreach(GridItem neighbour in GetNeighbours(lsCurrent))
            {
                if (lsClosedItems.Contains(neighbour)) continue;
                if (neighbour.Blocked || neighbour.Occupied || !neighbour.Active) continue;
                double lsPredictedGCost = lsCurrent.g + GetDistance(lsCurrent, neighbour);
                if (lsPredictedGCost < neighbour.g)
                {
                    neighbour.Parent = lsCurrent;
                    neighbour.g = lsPredictedGCost;
                    neighbour.h = GetDistance(neighbour, lsDest);
                    neighbour.fCost();

                    //if the neighbour hasnt been added 
                    if (!lsOpenItems.Contains(neighbour))
                    {
                        lsOpenItems.Add(neighbour);
                    }
                }
            }

            if(lsCurrent == lsDest)
            {
                TracePath(lsStart, lsDest);
                return;
            }
        }
    }

    private List<GridItem> GetNeighbours(GridItem argNode)
    {
        List<GridItem> lsListNeighbours = new List<GridItem>();
        if (argNode.IndexX - 1 >= 0)
        {
            //left
            lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX - 1, argNode.IndexZ, argNode.IndexY]);
            //left behind
            if (argNode.IndexZ - 1 >= 0) lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX - 1, argNode.IndexZ - 1, argNode.IndexY]);
            //left forward
            if (argNode.IndexZ + 1 < m_Grid.z) lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX - 1, argNode.IndexZ + 1, argNode.IndexY]); 
        }
        if(argNode.IndexX + 1 < m_Grid.x)
        {
            //right
            lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX + 1, argNode.IndexZ, argNode.IndexY]);
            //right behind
            if (argNode.IndexZ - 1 >= 0) lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX + 1, argNode.IndexZ - 1, argNode.IndexY]);
            //right forward
            if (argNode.IndexZ + 1 < m_Grid.z) lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX + 1, argNode.IndexZ + 1, argNode.IndexY]);
        }
        //behind
        if (argNode.IndexZ - 1 >= 0) lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX, argNode.IndexZ - 1, argNode.IndexY]);
        //forward
        if (argNode.IndexZ + 1 < m_Grid.z) lsListNeighbours.Add(m_Grid.Tiles[argNode.IndexX, argNode.IndexZ + 1, argNode.IndexY]);

        return lsListNeighbours;
    }
    void ResolveAStar(GridItem argStart, GridItem argDest)
    {
        if (null == argStart || null == argDest) return;

        List<GridItem> lsOpenItems = new List<GridItem> { argStart };
        HashSet<GridItem> lsClosedItems = new HashSet<GridItem>();

        argStart.g = 0;
        argStart.h = GetDistance(argStart, argDest);
        argStart.fCost();

        GridItem lsCurrent = lsOpenItems[0];

        while (lsOpenItems.Count > 0)
        {

            if (lsCurrent == argDest)
            {
                TracePath(argStart, argDest);
            }

            lsCurrent = lsOpenItems[0];
            for (int i = 1; i < lsOpenItems.Count; i++)
            {
                if (lsOpenItems[i].GetFCost() < lsCurrent.GetFCost())
                {
                    lsCurrent = lsOpenItems[i];
                }
            }

            lsOpenItems.Remove(lsCurrent);
            lsClosedItems.Add(lsCurrent);

            foreach (GridItem neighbour in GetNeighbours(lsCurrent))
            {
                if (lsClosedItems.Contains(neighbour)) continue;
                if (neighbour.Blocked || neighbour.Occupied || !neighbour.Active) continue;
                double lsPredictedGCost = lsCurrent.g + GetDistance(lsCurrent, neighbour);
                if (lsPredictedGCost < neighbour.g)
                {
                    neighbour.Parent = lsCurrent;
                    neighbour.g = lsPredictedGCost;
                    neighbour.h = GetDistance(neighbour, argDest);
                    neighbour.fCost();

                    if (!lsOpenItems.Contains(neighbour))
                    {
                        lsOpenItems.Add(neighbour);
                    }
                }
            }
        }

        //nopath
    }
}