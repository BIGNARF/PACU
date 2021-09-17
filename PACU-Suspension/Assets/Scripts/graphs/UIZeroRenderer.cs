using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIZeroRenderer : Graphic
{
    public Vector2Int gridSize;
    public UIGridRenderer grid;

    public float thickness = 10F;

    float width;
    float height;
    float unitwidth;
    float unitheight;

    public float negativeoffset;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        unitwidth = width / (float)gridSize.x;
        unitheight = height / (float)gridSize.y;
        // int count = 0;

        Vector2 point = new Vector2(0,negativeoffset);
            DrawVerticesforPoint(point, vh);
        point = new Vector2(width,negativeoffset);
        DrawVerticesforPoint(point, vh);

        vh.AddTriangle(0, 1, 3);
            vh.AddTriangle( 3, 2, 0);
    }

    void DrawVerticesforPoint(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = new Vector3(0, -0.5f * thickness);
        vertex.position += new Vector3(point.x, point.y);
        vh.AddVert(vertex);
        vertex.position = new Vector3(0,0.5f * thickness);
        vertex.position += new Vector3(point.x, point.y);
        vh.AddVert(vertex);
    }

    private void Update()
    {
        if (grid != null)
        {
            if (gridSize != grid.gridSize)
            {
                gridSize = grid.gridSize;
                SetVerticesDirty();
            }
        }
    }

    public float GetAngle(Vector2 me, Vector2 Target)
    {
        return (float)(Mathf.Atan2(Target.y - me.y, Target.x - me.x) * (180 / Mathf.PI));
    }
}
