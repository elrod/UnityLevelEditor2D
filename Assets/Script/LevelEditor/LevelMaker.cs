using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Loop
{
	public string loopName = "";
	public GameObject[] tiles;
}

public class LevelMaker : MonoBehaviour {

    [Range(1f,2048f)]
    public float width = 32.0f;
    [Range(1f, 2048f)]
    public float height = 32.0f;

    public Color gridColor = Color.green;
	public bool gridVisible = true;

    public GameObject[] tiles;
	public Loop[] loops;
	public Loop[] randomLoops;

    List<GameObject> levelTiles = new List<GameObject>();

    int selectedTile = 0;
	int selectedLoop = 0;
	int selectedRandomLoop = 0;
	int loopIndex = 0;
	bool loop;
	bool randomLoop;

    void OnDrawGizmos()
    {
		if(gameObject.GetComponentsInChildren<Transform>().Length - 1 > levelTiles.Count){
			levelTiles.Clear();
			foreach(Transform child in gameObject.GetComponentsInChildren<Transform>()){
				if(!child.gameObject.Equals(gameObject)){
					levelTiles.Add(child.gameObject);
				}
			}
		}
        DrawGrid();
    }

    void DrawGrid()
    {
        Vector3 pos = Camera.current.transform.position;
		if(gridVisible){
			gridColor.a = 1f;
		}
		else{
			gridColor.a = 0f;
		}
		Gizmos.color = gridColor;

        for (float y = pos.y - 800.0f; y < pos.y + 800.0f; y += this.height)
        {
            Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / this.height) * this.height, 0.0f),
                            new Vector3(1000000.0f, Mathf.Floor(y / this.height) * this.height, 0.0f));
        }
        for (float x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += this.width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / this.width) * this.width, -1000000.0f, 0.0f),
                            new Vector3(Mathf.Floor(x / this.width) * this.width, 1000000.0f, 0.0f));
        }
    }

    public void AddTile(Vector3 position)
    {
        foreach(GameObject tile in levelTiles)
        {
            if(tile.transform.position == position)
            {
                // If we get here, tile is already occupied, ignoring adding request
                //Debug.Log("Tile already occupied, ignoring");
                return;
            }
        }
		if(loop){
			if(loopIndex == loops[selectedLoop].tiles.Length){
				loopIndex = 0;
			}
        	GameObject newTile = Instantiate<GameObject>(loops[selectedLoop].tiles[loopIndex++]) as GameObject;
        	newTile.transform.position = position;
        	newTile.transform.SetParent(transform);
        	levelTiles.Add(newTile);
    
		}
		else if(randomLoop){
			GameObject newTile = Instantiate<GameObject>(randomLoops[selectedRandomLoop].tiles[Random.Range(0, randomLoops[selectedRandomLoop].tiles.Length)]) as GameObject;
			newTile.transform.position = position;
			newTile.transform.SetParent(transform);
			levelTiles.Add(newTile);
		}
		else{
			GameObject newTile = Instantiate<GameObject>(tiles[selectedTile]) as GameObject;
			newTile.transform.position = position;
			newTile.transform.SetParent(transform);
			levelTiles.Add(newTile);
		}
	}

    public void RemoveTileAt(Vector3 position)
    {
        foreach (GameObject tile in levelTiles)
        {
            if (tile.transform.position == position)
            {
                // Found it! Removing tile.
                //Debug.Log("Removing Tile");
                GameObject.DestroyImmediate(tile);
                levelTiles.Remove(tile);
                return;
            }
        }
    }

    public void ResetLevel()
    {
        // Remove all children
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.Equals(gameObject))
            {
                DestroyImmediate(child.gameObject);

            }
        }
        // Clear levelTiles list
        levelTiles.Clear();
    }

    public void SelectTile(int index)
    {
        selectedTile = index;
        // In case of array overflow default to 0
        if(index >= tiles.Length)
            selectedTile = 0;
    }

	public void SelectLoop(int index)
	{
		selectedLoop = index;
		loopIndex = 0;
		// In case of array overflow default to 0
		if(index >= loops.Length)
			selectedLoop = 0;
	}

	public void SelectRandomLoop(int index)
	{
		selectedRandomLoop = index;
		// In case of array overflow default to 0
		if(index >= randomLoops.Length)
			selectedRandomLoop = 0;
	}

	public void EnableRandomLoop(){
		loop = false;
		randomLoop = true;
	}

	public void EnableLoop(){
		randomLoop = false;
		loop = true;
	}

	public void DisableLoop(){
		randomLoop = false;
		loop = false;
	}

}
