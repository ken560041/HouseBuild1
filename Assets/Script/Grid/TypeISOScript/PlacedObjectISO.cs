using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Placed", menuName = "Placed/new")]
public class PlacedObjectISO : ScriptableObject
{
    // Start is called before the first frame update
    
    public enum Dir
    {
        Left, Right, Down, Up,
    }

   

    public string nameString;
    public GameObject prefab;
    public GameObject visual;


    public int width;// ngang

    
    public int height;// doc

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Left: return Dir.Up;
            case Dir.Right: return Dir.Down;
            case Dir.Up: return Dir.Right;
            case Dir.Down: return Dir.Left;

        }

    }

    


    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }
    
    public Vector2Int GetRotationOffset(Dir dir)
    {

        switch(dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0,0);
            case Dir.Left: return new Vector2Int(0, width);
            case Dir.Up: return new Vector2Int(width, height);
            case Dir.Right: return new Vector2Int(height, 0);

        }

    }

    /*public List<Vector2Int> GetGridPositionList(Vector3 pivot ,Vector2 offset, Dir dir)
    {
        Vector2Int temp=new Vector2Int((int)(-pivot.x+offset.x), (int)(-pivot.z+offset.y));
        return  GetGridPositionList(temp, dir);
    }*/

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        /*
         Nếu down và up thì mặc định nên không cần thay đổi ngang và dọc
         
         */ 

        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch(dir)
        {

            
            case Dir.Down:
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(i, j));
                    }

                }
                break;
            case Dir.Up: 
                 for(int i = 0; i < height; i++)
                 {
                     for(int j = 0;j<width; j++)
                     {
                         gridPositionList.Add(offset+ new Vector2Int(i, j) );
                     }
                     
                 }
                 break;

            case Dir.Left:
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(i, j));
                    }

                }
                break;
            case Dir.Right:
                for(int i = 0; i < width; i++)
                 {
                     for(int j = 0;j<height; j++)
                     {
                         gridPositionList.Add(offset + new Vector2Int(i, j));
                     }
                     
                 }
                    break;


        }
        return gridPositionList;

    }


    public List<Vector2Int> GetGridPositionFullList(Vector2Int offset)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        // Cạnh trên và dưới
        for (int x = 0; x <= width; x++)
        {
            gridPositionList.Add(offset + new Vector2Int(x, 0));            // cạnh trên
            gridPositionList.Add(offset + new Vector2Int(x, height ));  // cạnh dưới
        }

        // Cạnh trái và phải (bỏ dòng trên và dưới để không bị trùng)
        for (int y = 1; y <= height - 1; y++)
        {
            gridPositionList.Add(offset + new Vector2Int(0, y));           // cạnh trái
            gridPositionList.Add(offset + new Vector2Int(width , y));   // cạnh phải
        }
        return gridPositionList;
    }


}
