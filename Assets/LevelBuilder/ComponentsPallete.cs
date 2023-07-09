using System;
using System.Collections.Generic;
using UnityEngine;


public class ComponentsPallete : MonoBehaviour
{
    public static ComponentsPallete instance;
    
    public List<GameObject> Prefabs;
    public GameObject FloorPrefab;

    public float mapLeft = -4f;
    public float mapUp = -4;
    public int mapWidth = 12;
    public int mapHeight = 9;

    public float prefabsLeft = -12;
    public float prefabsUp = 5;
    private PalleteChoice pickedObject;

    public List<List<PalleteChoice>> board;  // [x][y]

    public WireSetInstrument wireSetInstrument;
    public OnOffInstrument onOffInstrument;

    public void Start()
    {
        instance = this;

        board = new List<List<PalleteChoice>>();
        for (int x = 0; x < mapWidth; x++)
        {
            board.Add(new List<PalleteChoice>());
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 position = new Vector3(mapLeft + x, mapUp + y, 10);
                Instantiate(FloorPrefab, position, Quaternion.identity);
                board[x].Add(null);
            }
        }

        for (int x = 0; x < Prefabs.Count; x++)
        {
            Vector3 position = new Vector3(prefabsLeft + x * 1.1f, prefabsUp, 3);
            Instantiate(Prefabs[x], position, Quaternion.identity);
        }
    }

    private string GenerateLevelCode()
    {
        string outp = "";
        foreach (List<PalleteChoice> row in board)
        {
            foreach (PalleteChoice item in row)
            {
                if (item == null)
                {
                    outp = outp + "null,";
                }
                else
                {
                    outp = outp + item.ToLevelStr() + ",";
                }
            }
        }
        return outp;
    }
    
    public void SaveToClip()
    {
        string levelCode = GenerateLevelCode();
        Debug.Log(levelCode);
        GUIUtility.systemCopyBuffer = levelCode;
    }

    public Vector3 MouseCorrectedPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePosition.x - 0.5f, mousePosition.y - 0.5f, mousePosition.z);
    }

    public (int, int) MouseBoardPosition()
    {
        Vector3 mousePosition = MouseCorrectedPosition();

        int boardX = (int)(Mathf.Ceil(mousePosition.x) - mapLeft);
        int boardY = (int)(Mathf.Ceil(mousePosition.y) - mapUp);
        return (boardX, boardY);
    }

    public bool MouseOnBoard()
    {
        Vector3 mousePosition = MouseCorrectedPosition();

        bool onBoard = mousePosition.x > mapLeft-1 && mousePosition.x < mapLeft-1 + mapWidth &&
                       mousePosition.y > mapUp-1 && mousePosition.y < mapUp-1 + mapHeight;
        return onBoard;
    }

    public void Update()
    {

        Vector3 mousePosition = MouseCorrectedPosition();
        bool onBoard = MouseOnBoard();

        if (Input.GetAxis("Restart") != 0)
        {
            CleanupPicked();
        }
        
        if (pickedObject != null)
        {
            UpdatePickedItem(onBoard, mousePosition);
        } else if (wireSetInstrument == null && onOffInstrument == null && Input.GetMouseButtonDown(1) && onBoard)
        {
            var (boardX, boardY) = MouseBoardPosition();
            PalleteChoice current = board[boardX][boardY];
            if (current != null) Destroy(current.gameObject);
            board[boardX][boardY] = null;
        } else if (wireSetInstrument != null)
        {
            wireSetInstrument.transform.position = new Vector3(mousePosition.x, mousePosition.y, 1);
            if (Input.GetMouseButtonDown(0) && onBoard)
            {
                var (boardX, boardY) = MouseBoardPosition();
                PalleteChoice selectedItem = board[boardX][boardY];
                if (selectedItem != null)
                {
                    wireSetInstrument.connectItem(selectedItem);
                }
            }
        } else if (onOffInstrument != null)
        {
            onOffInstrument.transform.position = new Vector3(mousePosition.x, mousePosition.y, 1);
            if (Input.GetMouseButtonDown(0) && onBoard)
            {
                var (boardX, boardY) = MouseBoardPosition();
                PalleteChoice selectedItem = board[boardX][boardY];
                if (selectedItem != null)
                {
                    onOffInstrument.SwitchItem(selectedItem);
                }
            }
        }
    }

    void UpdatePickedItem(bool onBoard, Vector3 mousePosition)
    {
        
        if (onBoard)
        {
            pickedObject.transform.position = new Vector3(
                Mathf.Ceil(mousePosition.x), Mathf.Ceil(mousePosition.y), 1
            );
        }
        else
        {
            pickedObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 1);
        }

        if (Input.GetMouseButtonDown(0) && onBoard)
        {
            // create and set to board
            Vector3 newPosition =
                new Vector3(pickedObject.transform.position.x, pickedObject.transform.position.y, 5);
            GameObject newObject = Instantiate(pickedObject.gameObject, newPosition, Quaternion.identity);
            PalleteChoice newItem = newObject.GetComponent<PalleteChoice>();
            newItem.SetState(PalleteItemState.ON_BOARD);

            var (boardX, boardY) = MouseBoardPosition();
            newItem.boardCoords = new Vector2(boardX, boardY);
            PalleteChoice current = board[boardX][boardY];
            if (current != null) Destroy(current.gameObject);
            board[boardX][boardY] = newItem;

            // TODO: check constraints
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(pickedObject.gameObject);
            pickedObject = null;
        }
    }

    void CleanupPicked()
    {
        if (pickedObject != null)
        {
            Destroy(pickedObject.gameObject);
            pickedObject = null;
        }

        if (wireSetInstrument != null)
        {
            Destroy(wireSetInstrument.gameObject);
            wireSetInstrument = null;
        }

        if (onOffInstrument != null)
        {
            
            Destroy(onOffInstrument.gameObject);
            onOffInstrument = null;
        }
    }
    
    public void SetPickedObject(PalleteChoice picked)
    {
        CleanupPicked();
        pickedObject = picked;
    }

    public void SetWireInstrument(WireSetInstrument instrument)
    {
        CleanupPicked();
        wireSetInstrument = instrument;
    }

    public void SetOnOffInstrument(OnOffInstrument instrument)
    {
        CleanupPicked();
        onOffInstrument = instrument;
    }
}
