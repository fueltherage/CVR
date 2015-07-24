using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class ColorPaletEditorWindow : EditorWindow {

    static List<ColorCell> colorPalet = new List<ColorCell>();

    public static int CellSize = 50;//size of a Color cell, w=h

    ColorCell currentSelectedCell;
    GUIStyle colorCellStyle; //Style of the palet cells
    Texture2D buttonSelected;
    Texture2D hoverOver;
    Color newColor;
   
    bool init = false;

    [MenuItem("Voxel Painter/Color Palet")]   
    private static void showEditor()
    {
        EditorWindow.GetWindow<ColorPaletEditorWindow>("Color Palet");     


        
    }
    void OnEnable()
    {
        buttonSelected = Resources.Load("activeButton") as Texture2D;
        hoverOver = Resources.Load("ButtonSelected") as Texture2D;
        if (buttonSelected == null) Debug.Log("Didnt load");
    }
    void Init()
    {

        colorCellStyle = new GUIStyle(GUI.skin.button);
        colorCellStyle.margin = new RectOffset(0, 0, 0, 0);
        colorCellStyle.fixedHeight = CellSize;
        colorCellStyle.fixedWidth = CellSize;
        colorCellStyle.border = new RectOffset(0, 0, 0, 0);        
        colorCellStyle.stretchHeight = true;
        colorCellStyle.stretchWidth = true;
        colorCellStyle.normal.background = new Texture2D(0, 0);
        //colorCellStyle.onActive.background = buttonSelected;
        colorCellStyle.active.background = buttonSelected;
        colorCellStyle.hover.background = buttonSelected;
        //colorCellStyle.onActive.background = buttonSelected;

        if (colorPalet.Count == 0)
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    colorPalet.Add(new ColorCell(new Color(0.125f * x, 0.125f * y, 1-0.125f * x, 1)));
        }

        newColor = new Color(0,0,0,1);
        init = true;
    }
    void OnGUI()
    {
        if (!init) Init();
       
    
        int cellsWide = (int)(position.width / CellSize);
        //Draw Color Palet
        for(int i =0; i < colorPalet.Count; i++)
        {            
            colorCellStyle.onNormal.background = colorPalet[i].tex;
            Rect cellRec = new Rect((i * CellSize) % (cellsWide*CellSize), (i / cellsWide) * CellSize, CellSize, CellSize);
            GUI.DrawTexture(cellRec, colorPalet[i].tex);
            if (GUI.Button(cellRec, "", colorCellStyle))
            {
                currentSelectedCell = colorPalet[i];
            }
        }
        float newColY;
        if ((colorPalet.Count * CellSize) % (cellsWide * CellSize) > 0)
        {
           newColY = ((colorPalet.Count / cellsWide)+1) * CellSize;
        }
        else
        {
           newColY = (colorPalet.Count / cellsWide)*CellSize;
        }
        newColY += 10;
        newColor = EditorGUI.ColorField(new Rect(0, newColY , 75, 25), newColor);
        
        if (GUI.Button(new Rect(150, newColY, 120, 25),"Replace Color"))
        {           
            currentSelectedCell.UpdateColor(newColor);
        }

        if (GUI.Button(new Rect(270, newColY, 120, 25), "Add new Color"))
        {
            AddColorToPalet(newColor);
        }
        if (GUI.Button(new Rect(390, newColY, 20, 25), "X"))
        {
            if (currentSelectedCell.tex != null)
            colorPalet.Remove(currentSelectedCell);
        } 
    }
    void AddColorToPalet(Color _color)
    {
        colorPalet.Add(new ColorCell(_color));
    }
	
    struct ColorCell
    {
        public Color color;       
        public Texture2D tex;
        public ColorCell(Color _color)
        {
            color = _color;
            tex = new Texture2D(1,1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
        }
        public void UpdateColor(Color _color)
        {
            color = _color;
            if (tex == null) tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, _color);
            tex.Apply();
        }
    }
}
