using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid<TGridObject> 
{
    // Kiểu dữ liệu được định nghĩa lại 
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanger;

    public class OnGridObjectChangedEventArgs: EventArgs
    {
        public int x;
        public int y;
    }



    private int  width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray;
    private Vector3 originPosition;
    public Grid(int width, int height, float size, Vector3 originPosition, Func<Grid<TGridObject>,int ,int, TGridObject> createGridObject) { 
    
        this.width = width;
        this.height = height;

        this.cellSize = size;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];
        

        for(int i = 0; i < gridArray.GetLength(0); i++)
        {
            for(int j=0;j< gridArray.GetLength(1); j++)
            {
                setBuilding(i,j);
                gridArray[i,j]= createGridObject(this, i,j);
            }
        }


        Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);


    }
    

    private void setBuilding(int x, int y) { 

       // Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1), Color.red, 100f);
       // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y ), Color.red, 100f);

    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanger?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = z });
    }



    public void SetGridObject(int x, int y, TGridObject value)
    {
        if(x>=0 && y>=0 && x<width && y< height)
        {
            gridArray[x,y] = value;
            if (OnGridObjectChanger != null)
            {
                OnGridObjectChanger(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            }
        }
    }

    public void SetGridObject(Vector3 worldPosition,TGridObject value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0.3f, y) * cellSize + originPosition;
    }


    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);

    }
    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        int x = gridPosition.x;
        int z = gridPosition.y;

        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWidth()
    {
        return height;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public TGridObject GetGridObject(int x,int y)
    {
        if (x >= 0 && y >= 0 &&x<width && y<height ) { 
        
            return gridArray[x,y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;

        GetXZ(worldPosition,out x,out y);
        return GetGridObject(x,y);
    }


}
