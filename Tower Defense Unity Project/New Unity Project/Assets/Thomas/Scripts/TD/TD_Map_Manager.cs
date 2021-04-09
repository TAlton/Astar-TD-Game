using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class TD_Map_Manager : MonoBehaviour
{
    public const KeyCode SAVE_KEY_ = KeyCode.LeftControl;
    private const string BREAK_CHAR_ = ";";
    private TD_Map map_;
    private string directory_;
    private string file_name_ = "map_grid.txt";
    private string full_file_path_;
    private string file_contents_to_add_;
    private List<string> tile_attributes_ = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Awake()
    {
        directory_ = Application.dataPath + "/Maps";
        full_file_path_ = directory_ + "/" + file_name_;
        map_ = this.GetComponent<TD_Map>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        GetTileInformation();
        CheckDirectory();
        //creates or overwrites the existing file
        File.WriteAllText(full_file_path_, file_contents_to_add_);
    }
    //returns all the tiles to update
    public List<Vector3Int> Open()
    {
        //im so sorry you have to read this
        /*
         * should have used json but the object structure was not designed with that in mind
         * would have to create a class and set vars to write as system.serializable
         * this would sequire a restructure
         */
        List<Vector3Int> ls_list_attrs = new List<Vector3Int>();
        //string[] ls_map_text = File.ReadAllLines(full_file_path_);
        using (StreamReader ls_sr = new StreamReader(full_file_path_))
        {
            string ls_line;

            //hack to get map size from first line
            bool ls_first_line = true;
            int ls_break_char_index = -1;
            int ls_current_index = 0;

            while ((ls_line = ls_sr.ReadLine()) != null)
            {
               ls_break_char_index = ls_line.IndexOf(BREAK_CHAR_);
               ls_current_index = 0;

                //handling first line for map size
                if (ls_first_line)
                {
                    int ls_x = int.Parse(ls_line.Substring(ls_current_index, ls_break_char_index));
                    ls_current_index = ls_break_char_index + 1;
                    ls_break_char_index = ls_line.IndexOf(BREAK_CHAR_, ls_current_index);
                    int ls_y = int.Parse(ls_line.Substring(ls_current_index, ls_break_char_index - ls_current_index));
                    ls_list_attrs.Add(new Vector3Int(ls_x, ls_y, 0));
                    ls_first_line = false;
                    continue;
                }

                int ls_index_y = int.Parse(ls_line.Substring(ls_current_index, ls_break_char_index));
                //save current index
                ls_current_index = ls_break_char_index + 1;
                //update new break char location
                ls_break_char_index = ls_line.IndexOf(BREAK_CHAR_, ls_current_index);

                int ls_index_x = int.Parse(ls_line.Substring(ls_current_index, ls_break_char_index - ls_current_index));

                ls_current_index = ls_break_char_index + 1;
                ls_break_char_index = ls_line.IndexOf(BREAK_CHAR_, ls_current_index);

                int ls_content_id = int.Parse(ls_line.Substring(ls_current_index, ls_break_char_index - ls_current_index));


                ls_list_attrs.Add(new Vector3Int(ls_index_x, ls_index_y, ls_content_id));
            }
        }
        return ls_list_attrs;
    }
    private void CheckDirectory()
    {
        if (!Directory.Exists(directory_))
            Directory.CreateDirectory(directory_);
    }
    private void GetTileInformation()
    {
        for(int i_y = 0; i_y < map_.map_size_.y; i_y++)
        {
            for(int i_x = 0; i_x < map_.map_size_.x; i_x++)
            {
                TD_Tile ls_tile = map_.map_tiles_[i_y, i_x];
                //only save the tiles that aent empty
                if (TileContentType.EMPTY == ls_tile.Content.Type) 
                    continue;
                //saves the index of the tile and the type of content the tile holds
                tile_attributes_.Add(i_y + BREAK_CHAR_ + i_x + BREAK_CHAR_ + (int)(ls_tile.content_.Type) + BREAK_CHAR_ +  "\n");
            }
        }

        //add in map size to init then the non empty tiles
        file_contents_to_add_ = map_.map_size_.x + BREAK_CHAR_ + map_.map_size_.y + BREAK_CHAR_ + "\n" + string.Concat(tile_attributes_.ToArray());

    }
}
