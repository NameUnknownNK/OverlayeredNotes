using UnityEngine;
using UnityEngine.UI;

//A flexible grid layout that works way better than the built in Unity
//works similar to the built in "Grid Layout Vertical" or "Grid Layout Horizontal"
//could be better, still a WIP
public class GridLayoutUI : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    [ReadOnly]
    [SerializeField]
    private RectOffset alligmentPadding;
    /*[ReadOnly]
    [SerializeField]
    private RectOffset alligmentPadding;*/
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;

    

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        

        if (fitType == FitType.Uniform || fitType == FitType.Width || fitType == FitType.Height)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(rectChildren.Count);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.FixedColumns || fitType == FitType.Width)
        {
            rows = Mathf.CeilToInt(rectChildren.Count / (float)columns);
        }

        if (fitType == FitType.FixedRows || fitType == FitType.Height)
        {
            columns = Mathf.CeilToInt(rectChildren.Count / (float)rows);
        }

        //anti fails
        rows = Mathf.Max(rows, 1);
        columns = Mathf.Max(columns, 1);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        //Calculate cell dimensions
        Vector2 availableSpace;
        availableSpace.x = (parentWidth - (spacing.x * (columns - 1)) - padding.right - padding.left);
        availableSpace.y = (parentHeight - (spacing.y * (rows - 1)) - padding.top - padding.bottom);

        float cellWidth = availableSpace.x / (float)columns;
        float cellHeight = availableSpace.y / (float)rows;

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for(int i = 0; i< rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            float xPos = columnCount * (cellSize.x + spacing.x) + padding.left;
            float yPos = rowCount * (cellSize.y + spacing.y) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);

        }

    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
