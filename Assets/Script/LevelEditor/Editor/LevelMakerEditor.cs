using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelMaker))]
public class LevelMakerEditor : Editor {

    LevelMaker grid;

    public void OnEnable()
    {
        grid = (LevelMaker)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // SEPARATOR
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("SINGLE TILE");
        // Customizing the inspector a little bit
        GUILayout.BeginHorizontal();
        for (int i = 0; i < grid.tiles.Length; i++)
        {
            GameObject tilePrefab = grid.tiles[i];
            // We want two buttons per line
            if(i % 2 == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            if (GUILayout.Button(tilePrefab.name))
            {
                grid.SelectTile(i);
				grid.DisableLoop();
            }
        }
        GUILayout.EndHorizontal();
        // SEPARATOR
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("SEQUENTIAL LOOPS");
		GUILayout.BeginHorizontal();
		for(int j = 0; j < grid.loops.Length; j++){
			if(j % 2 == 0 && j != 0)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			if(GUILayout.Button(grid.loops[j].loopName == "" ? "Loop "+ j : grid.loops[j].loopName)){
				grid.SelectLoop(j);
				grid.EnableLoop();
			}
		}
		GUILayout.EndHorizontal();
		// SEPARATOR
		GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("RANDOM LOOPS");
		GUILayout.BeginHorizontal();
		for(int z = 0; z < grid.randomLoops.Length; z++){
			if(z % 2 == 0 && z != 0)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			if(GUILayout.Button(grid.randomLoops[z].loopName == "" ? "Loop "+ z : grid.randomLoops[z].loopName)){
				grid.SelectRandomLoop(z);
				grid.EnableRandomLoop();
			}
		}
		GUILayout.EndHorizontal();
		// SEPARATOR
		GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
		GUILayout.Label("WARNING AREA!!!");
        if (GUILayout.Button("Rebuild Level"))
        {
            grid.RebuildLevel();
        }
        if (GUILayout.Button("Reset Level"))
        {
            grid.ResetLevel();
        }
    }

    void OnSceneGUI()
    {
        HandleInput();
    }

    /* From what I found on the internet, looks like we need to intercept events and disable their default action in order           *
     * to capture mouse in scene view... seems to work fine, but won't allow us to select objects in the scene if the LevelEditor    *
     * empty is selected, we will need to change focus on hierarchy, to select other things :)                                       */
    void HandleInput()
    {
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = controlID;
                //Debug.Log("MOUSE DOWN");
                e.Use();
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                //Debug.Log("MOUSE UP");
                OnMouseUpHandler(e.mousePosition, e.button);
                e.Use();
                break;
            case EventType.MouseDrag:
                GUIUtility.hotControl = controlID;
                //Debug.Log("MOUSE DRAG");
                OnMouseDragHandler(e.mousePosition, e.button);
                e.Use();
                break;
        }
    }

    void OnMouseUpHandler(Vector2 mpos, int button)
    {
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(mpos.x, -mpos.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;
        // The spawn position should be snapped to the grid
        Vector3 snappedPosition = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f, 
            Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f, 
            0.0f);
        // Add element: left click, remove element, right click
        if (button == 0)
        {
            grid.AddTile(snappedPosition);
        }
        else if(button == 1)
        {
            grid.RemoveTileAt(snappedPosition);
        }
    }

    // Support mouse drag
    void OnMouseDragHandler(Vector2 mpos, int button)
    {
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(mpos.x, -mpos.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;
        // The spawn position should be snapped to the grid
        Vector3 snappedPosition = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
            Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f,
            0.0f);
        // Add element: left click, remove element, right click
        if (button == 0)
        {
            grid.AddTile(snappedPosition);
        }
		else if(button == 1)
        {
            grid.RemoveTileAt(snappedPosition);
        }
    }
}
