using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCatcher : MonoBehaviour
{
    #region Variables

    Camera mcamera = null;
    TransparentWindow transparentWindow = null;
    bool drawmode = false;//can draw lines?
    Dictionary<int, Stack<LineRenderer>> lines;//all the lines drawn
    List<Vector2Int> lastcolor = new List<Vector2Int>(); //undo management
    Color[] loadedColors;
    int color = 0;//actual selected color index
    float brushSize = 1;
    bool badStart = true;//was the click to start drawing a new line in the UI?

        #region SetUp

    [SerializeField]
    Material lineMaterial = null;

    [Header("Drawmode indicators")]
    [SerializeField]
    Image[] indicators = null;

    [Header("Color based Indicators")]
    [SerializeField]
    Image[] colorIndicators = null;

    [Header("Pallette")]
    [SerializeField]
    Transform pallete = null;

        #endregion

    #endregion

    #region PrivateMethods

        #region MonoBehaviourCallBacks

    void Start()
    {
        mcamera = GetComponent<Camera>();
        transparentWindow = new TransparentWindow();
        LoadColors();
    }

    private void Update()
    {
        Ray ray = mcamera.ScreenPointToRay(Input.mousePosition);
        bool notlanded = Physics2D.Raycast(ray.origin, ray.direction).collider == null;
        if (!drawmode)
        {
            transparentWindow.SetClickThrough(notlanded);
        }
        else
        {
            DrawLine(notlanded);
        }
    }

        #endregion

        #region Init

    /*Loads the colors from the buttons from varialbe 'pallete'
     * and initializes the 'lines' dictionary*/
    void LoadColors()
    {
        loadedColors = new Color[pallete.childCount];
        lines = new Dictionary<int, Stack<LineRenderer>>(loadedColors.Length);
        for (int i = 0; i < loadedColors.Length; i++)
        {
            loadedColors[i] = pallete.GetChild(i).GetComponent<Image>().color;
            lines[i] = new Stack<LineRenderer>();
        }
    }

        #endregion

        #region Utils

    //Returns mouse position in world space
    private Vector3 MouseToWorld()
    {
        Vector3 mouse = new Vector3();
        mouse.x = Input.mousePosition.x;
        mouse.y = mcamera.pixelHeight - Input.mousePosition.y;
        mouse.z = 10;
        return mcamera.ScreenToWorldPoint(mouse);
    }

        #endregion

        #region UIManagement

    private void DrawModeIndicatorsUpdate()
    {
        foreach (Image b in indicators)
        {
            b.color = drawmode ? Color.green : Color.red;
        }
    }

    private void ColorIndicatorsUpdate()
    {
        foreach (Image b in colorIndicators)
        {
            b.color = loadedColors[color];
        }
    }

        #endregion

        #region DrawLines

    /*Checks mouse input and:
     * if 'canStart' and its a click, draws a new line
     * if its a hold and a line its being drawn, continues the line
     * if its a release and a line its being drawn, finishes the line*/
    void DrawLine(bool canStart)
    {
        LineRenderer actual;
        if (Input.GetMouseButtonDown(0))
        {
            badStart = !canStart;
            if (canStart)
            {
                actual = NewLine();
                AddPoint(actual);
                if (!lines.ContainsKey(color)) lines[color] = new Stack<LineRenderer>();
                lines[color].Push(actual);
            }
        }
        if (Input.GetMouseButton(0) && !badStart)
        {
            actual = lines[color].Peek();
            actual.positionCount++;
            AddPoint(actual);
        }
        if (Input.GetMouseButtonUp(0) && !badStart)
        {
            lines[color].Peek().Simplify(0.0001f);
        }
    }

    //Creates a new line with the current color and size and returns it
    private LineRenderer NewLine()
    {
        LineRenderer actual = (new GameObject()).AddComponent<LineRenderer>();
        AddLatestColor(color);
        actual.material = lineMaterial;
        actual.endColor = loadedColors[color];
        actual.startColor = loadedColors[color];
        actual.startWidth = 0.1f * brushSize;
        actual.endWidth = 0.1f * brushSize;
        actual.transform.localScale = new Vector3(1, -1, 1);
        actual.useWorldSpace = false;
        actual.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        actual.receiveShadows = false;
        actual.numCornerVertices = 90;

        actual.positionCount = 1;
        return actual;
    }

    //Adds a new point at the mouse position to the line 'lr'
    private void AddPoint(LineRenderer lr)
    {
        lr.SetPosition(lr.positionCount - 1, MouseToWorld());
    }

        #endregion

        #region UndoManagement

    /*if totally, removes all entries of color 'c' from the 'lastcolor' list
     * else, removes only the last one of them*/
    private void RemoveColor(int c, bool totally)
    {
        for (int i = lastcolor.Count - 1; i > 0; i--)
        {
            if (lastcolor[i].x == c)
            {
                if (totally)
                {
                    lastcolor.RemoveAt(i);
                }
                else
                {
                    if (lastcolor[i].y == 1) lastcolor.RemoveAt(i);
                    else
                    {
                        lastcolor[i] -= Vector2Int.up;
                    }
                    return;
                }
            }
        }
    }

    //Adds 'c' at the end of 'lastcolor' list
    private void AddLatestColor(int c)
    {
        if (lastcolor.Count > 0)
        {
            Vector2 lastpos = lastcolor[lastcolor.Count - 1];
            if (c == lastpos.x)
            {
                lastcolor[lastcolor.Count - 1] += Vector2Int.up;
            }
            else
            {
                lastcolor.Add(new Vector2Int(c, 1));
            }
        }
        else
        {
            lastcolor.Add(new Vector2Int(c, 1));
        }
    }

        #endregion

    #endregion

    #region PublicMethods

        #region Setters

    public void SetColor(int x)
    {
        if (x != color)
        {
            color = x;
            ColorIndicatorsUpdate();
        }
    }

    public void SetBushSize(float x)
    {
        brushSize = x;
    }

        #endregion

        #region Funcionalities

    //Turns on/off drawmode and click trhough, updates UI
    public void DrawModeSwitch()
    {
        drawmode = !drawmode;
        DrawModeIndicatorsUpdate();
        transparentWindow.SetClickThrough(!drawmode);
    }

    /*if general, deletes last line drawn
     * else, deletes last line drawn of the current selected color*/
    public void Undo(bool general)
    {
        if (lastcolor.Count == 0) return;
        int targetcolor = general ? lastcolor[lastcolor.Count - 1].x : color;
        RemoveColor(targetcolor, false);

        if (lines.ContainsKey(targetcolor) && lines[targetcolor].Count != 0) Destroy(lines[targetcolor].Pop().gameObject);
    }

    /*if general, deletes all lines
     * else, deletes all lines of the current selected color*/
    public void Clear(bool general)
    {
        if (lastcolor.Count == 0) return;
        if (general)
        {
            foreach (int key in lines.Keys)
            {
                while (lines[key].Count > 0)
                {
                    Destroy(lines[key].Pop().gameObject);
                }
            }
            lastcolor.Clear();
        }
        else
        {
            if (lines.ContainsKey(color))
            {
                Stack<LineRenderer> target = lines[color];
                while (target.Count > 0)
                {
                    Destroy(target.Pop().gameObject);
                }
                RemoveColor(color, true);
            }
        }
    }

    public void CloseApp()
    {
        Application.Quit();
    }

        #endregion

    #endregion

}
