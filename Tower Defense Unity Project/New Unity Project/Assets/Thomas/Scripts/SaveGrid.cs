using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class SaveGrid : MonoBehaviour
{
    [SerializeField] private TestGridCreation grid_;
    private const KeyCode SAVE_KEY_ = KeyCode.LeftControl;
    private string directory_;
    private string file_name_ = "map_grid.txt";
    private string full_file_path_;
    private string file_contents_to_add_;
    private List<string> tile_attributes_ = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        directory_ = Application.dataPath + "/Maps";
        grid_ = this.GetComponent<TestGridCreation>();
        full_file_path_ = directory_ + "/" + file_name_;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(SAVE_KEY_))
        {
            Save();
        }
    }

    void Save()
    {
        GetTileInformation();
        CheckDirectory();
        //creates or overwrites the existing file
        File.WriteAllText(full_file_path_, file_contents_to_add_);
    }

    void CheckDirectory()
    {
        if(!Directory.Exists(directory_))
            Directory.CreateDirectory(directory_);
    }
    void GetTileInformation()
    {
        for(int i = 0; i < grid_.Tiles.GetLength(0); i++)
        {
            for(int j = 0; j < grid_.Tiles.GetLength(1); j++)
            {
                for(int k = 0; k < grid_.Tiles.GetLength(2); k++)
                {
                    Assets.Thomas.Scripts.GridItem obj = grid_.Tiles[i, j, k];
                    //pushes ASCII representation of tile attributes
                    tile_attributes_.Add(obj.Tile.transform.position.ToString() + ";" + obj.Blocked + "\n");
                }
            }
        }

        //converts contents to a single string
        file_contents_to_add_ = grid_.x.ToString() + ";" +  grid_.z.ToString() + ";" + grid_.y.ToString() + "\n" + string.Concat(tile_attributes_.ToArray());
        
    }
}
