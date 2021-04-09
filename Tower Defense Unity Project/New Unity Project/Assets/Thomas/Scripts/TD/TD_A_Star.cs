using System.Collections;
using System.Collections.Generic;
using Assets.Thomas.Scripts;
using UnityEngine;

public class TD_A_Star : MonoBehaviour
{
    public TD_Map map_;
    private const int COST_MOVE_STRAIGHT = 10;
    private const int COST_MOVE_DIAGONAL = 14;
    private int[,] m_ClosedNodes;
    private int[,] m_OpenNodes;
    private float delta;
    // Start is called before the first frame update
    void Start()
    {
        map_ = this.GetComponent<TD_Map>();
        delta = 0.49f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Resolve()
    {
        ResolveAStar(GetTileByPosition(new Vector3(-3.5f, 0f, -3.5f)), map_.destination_);
    }
    TD_Tile GetTileByPosition(Vector3 argPos)
    {
        for (int y = 0; y < map_.map_size_.x; y++)
        {
            for (int x = 0; x < map_.map_size_.y; x++)
            {
                if (null == map_.map_tiles_[y, x])
                    continue;

                TD_Tile ls_tile = map_.map_tiles_[y, x];
                //check x axis bounds
                if (ls_tile.transform.position.x + delta >= argPos.x &&
                    ls_tile.transform.position.x - delta <= argPos.x)
                {
                    //check z axis bounds
                    if (ls_tile.transform.position.z + delta >= argPos.z &&
                        ls_tile.transform.position.z - delta <= argPos.z)
                    {
                        return ls_tile;
                    }
                }
            }
        }

        return null;
    }
    float GetDistance(TD_Tile arg_tile_a, TD_Tile arg_tile_b)
    {
        float lsDeltaX = Mathf.Abs(arg_tile_a.transform.position.x - arg_tile_b.transform.position.x);
        float lsDeltaY = Mathf.Abs(arg_tile_a.transform.position.z - arg_tile_b.transform.position.z);

        return (lsDeltaX > lsDeltaY) ? CalcDistance(lsDeltaX, lsDeltaY) : CalcDistance(lsDeltaY, lsDeltaX);
    }
    private float CalcDistance(float argX, float argY)
    {
        return COST_MOVE_DIAGONAL * Mathf.Min(argX, argY) + COST_MOVE_STRAIGHT * Mathf.Abs(argX - argY);
    }
    private List<TD_Tile> TracePath(TD_Tile arg_start, TD_Tile arg_dest)
    {
        Debug.Log("Start Trace");
        List<TD_Tile> ls_path = new List<TD_Tile>();
        TD_Tile ls_current = arg_dest;
        ls_path.Add(arg_dest);

        //traverse parents from dest to start
        while (ls_current.Parent != null)
        {
            ls_path.Add(ls_current.Parent);
            ls_current.GetComponentInChildren<MeshRenderer>().material = (Material)(Resources.Load("pathing"));
            ls_current = ls_current.Parent;
        }

        ls_path.Reverse();
        return ls_path;
    }
    void ResolveAStar(Vector3 arg_start_pos, Vector3 arg_dest_pos)
    {
        TD_Tile ls_start = GetTileByPosition(arg_start_pos);
        TD_Tile ls_dest = GetTileByPosition(arg_dest_pos);

        if (null == ls_start || null == ls_dest) return;

        List<TD_Tile> ls_open_items = new List<TD_Tile> { ls_start };
        HashSet<TD_Tile> ls_closed_items = new HashSet<TD_Tile>();

        //initialising costs of path to default val
        for (int i_y = 0; i_y < map_.map_size_.y; i_y++)
        {
            for (int i_x = 0; i_x < map_.map_size_.x; i_x++)
            {
                map_.map_tiles_[i_y, i_x].g = int.MaxValue;
                map_.map_tiles_[i_y, i_x].Parent = null;
            }
        }

        ls_start.g = 0;
        ls_start.h = GetDistance(ls_start, ls_dest);
        ls_start.fCost();

        TD_Tile lsCurrent = ls_open_items[0];

        while (ls_open_items.Count > 0)
        {

            for (int i = 1; i < ls_open_items.Count; i++)
            {
                if (ls_open_items[i].GetFCost() < lsCurrent.GetFCost() || lsCurrent.GetFCost() == ls_open_items[i].GetFCost() && ls_open_items[i].h < lsCurrent.h)
                {
                    lsCurrent = ls_open_items[i];
                }
            }

            ls_open_items.Remove(lsCurrent);
            ls_closed_items.Add(lsCurrent);

            //get closest neighbour based on g cost
            foreach (TD_Tile neighbour in GetNeighbours(lsCurrent))
            {
                if (ls_closed_items.Contains(neighbour))
                    continue;
                if (neighbour.is_blocking_) continue;
                double lsPredictedGCost = lsCurrent.g + GetDistance(lsCurrent, neighbour);
                if (lsPredictedGCost < neighbour.g)
                {
                    neighbour.Parent = lsCurrent;
                    neighbour.g = lsPredictedGCost;
                    neighbour.h = GetDistance(neighbour, ls_dest);
                    neighbour.fCost();

                    //if the neighbour hasnt been added 
                    if (!ls_open_items.Contains(neighbour))
                    {
                        ls_open_items.Add(neighbour);
                    }
                }
            }

            if (lsCurrent == ls_dest)
            {
                TracePath(ls_start, ls_dest);
                return;
            }
        }
    }

    private List<TD_Tile> GetNeighbours(TD_Tile arg_node)
    {
        List<TD_Tile> ls_neighbours = new List<TD_Tile>();
        if (arg_node.IndexX - 1 >= 0)
        {
            //left
            ls_neighbours.Add(map_.map_tiles_[arg_node.IndexY, arg_node.IndexX - 1]);
        }
        if (arg_node.IndexX + 1 < map_.map_size_.x)
        {
            //right
            ls_neighbours.Add(map_.map_tiles_[arg_node.IndexY, arg_node.IndexX + 1]);
        }
        //behind
        if (arg_node.IndexY - 1 >= 0) ls_neighbours.Add(map_.map_tiles_[arg_node.IndexY - 1, arg_node.IndexX]);
        //forward
        if (arg_node.IndexY + 1 < map_.map_size_.y) ls_neighbours.Add(map_.map_tiles_[arg_node.IndexY + 1, arg_node.IndexX]);

        return ls_neighbours;
    }
    void ResolveAStar(TD_Tile arg_start, TD_Tile arg_dest)
    {
        if (null == arg_start || null == arg_dest) return;

        List<TD_Tile> ls_open_items = new List<TD_Tile> { arg_start };
        HashSet<TD_Tile> ls_closed_items = new HashSet<TD_Tile>();

        arg_start.g = 0;
        arg_start.h = GetDistance(arg_start, arg_dest);
        arg_start.fCost();

        TD_Tile ls_current_tile = ls_open_items[0];

        while (ls_open_items.Count > 0)
        {

            if (ls_current_tile == arg_dest)
            {
                TracePath(arg_start, arg_dest);
            }

            ls_current_tile = ls_open_items[0];
            for (int i = 1; i < ls_open_items.Count; i++)
            {
                if (ls_open_items[i].GetFCost() < ls_current_tile.GetFCost())
                {
                    ls_current_tile = ls_open_items[i];
                }
            }

            ls_open_items.Remove(ls_current_tile);
            ls_closed_items.Add(ls_current_tile);

            foreach (TD_Tile neighbour in GetNeighbours(ls_current_tile))
            {
                if (ls_closed_items.Contains(neighbour)) 
                    continue;
                if (neighbour.is_blocking_)
                    continue;
                double lsPredictedGCost = ls_current_tile.g + GetDistance(ls_current_tile, neighbour);
                if (lsPredictedGCost < neighbour.g)
                {
                    neighbour.Parent = ls_current_tile;
                    neighbour.g = lsPredictedGCost;
                    neighbour.h = GetDistance(neighbour, arg_dest);
                    neighbour.fCost();

                    if (!ls_open_items.Contains(neighbour))
                    {
                        //neighbour.GetComponentInChildren<MeshRenderer>().material = (Material)(Resources.Load("pathing"));
                        if(neighbour == arg_dest)
                        {
                            TracePath(arg_start, arg_dest);
                            return;
                        }
                        ls_open_items.Add(neighbour);
                    }
                }
            }
        }
    }
}