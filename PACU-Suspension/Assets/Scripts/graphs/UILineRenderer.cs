using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    public Vector2Int gridSize;
    public UIGridRenderer grid;

    public List<Vector2> points;
    public float thickness = 10F;

    float width;
    float height;
    float unitwidth;
    float unitheight;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        unitwidth = width / (float)gridSize.x;
        unitheight = height / (float)gridSize.y;
        int count = 0;

        if(points.Count<2)
        {
            return;
        }
        float angle = 0;
        for(int i=0;i<points.Count;i++)
        {
            Vector2 point = points[i];

            if(i<points.Count-1)
            {
                angle = GetAngle(points[i], points[i + 1])+ 45f;
            }
            DrawVerticesforPoint(point, vh,angle);
        }
        for (int i = 0; i < points.Count-1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index+3, index + 2, index);
        }
    }

    void DrawVerticesforPoint(Vector2 point, VertexHelper vh,float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = Quaternion.Euler(0,0,angle)*new Vector3(-0.5f*thickness, 0);
        vertex.position += new Vector3(unitwidth*point.x, unitheight*point.y);
        vh.AddVert(vertex);
        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(0.5f * thickness, 0);
        vertex.position += new Vector3(unitwidth * point.x, unitheight * point.y);
        vh.AddVert(vertex);
    }

    private void Update()
    {
        if(grid!=null)
        {
            if(gridSize!=grid.gridSize)
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
