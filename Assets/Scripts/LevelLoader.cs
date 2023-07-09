using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public int mapLeft = -7;
    public int mapTop = 4;
    public static int width = 12;
    public static int height = 9;
    private string currentLevel;
    public List<LoadableObject> LoadableObjects = new List<LoadableObject>();
    private Dictionary<string, LoadableObject> mapping;

    private List<List<LoadableObject>> board;
    // Start is called before the first frame update
    void Start()
    {
        foreach (LoadableObject obj in LoadableObjects)
        {
            mapping[obj.loadableName] = obj;
        }
        
        Persistence.instance.levelLoading = true;
        currentLevel = Persistence.instance.GetLevel();
        string[] tokens = currentLevel.Split(',');
        board = new List<List<LoadableObject>>(width);
        int i = 0;
        Dictionary<string, Vector2> later = new Dictionary<string, Vector2>();
        LoadableObject enter = null;
        LoadableObject exit = null;
        LoadableObject dude = null;
        for (int x = 0; x < width; x++)
        {
            board[x] = new List<LoadableObject>(height);
            for (int y = 0; y < height; y++)
            {
                string token = tokens[i];
                if (token.Contains("conditional_wall"))
                {
                    later[token] = new Vector2(x, y);
                    i++;
                    continue;
                }

                Vector3 position = ResolvePosition(x, y);
                GameObject obj = Instantiate(mapping[token].gameObject, position, Quaternion.identity);
                LoadableObject item = obj.GetComponent<LoadableObject>();

                if (token.Contains("dude")) dude = item;

                if (token.Contains("enter")) enter = item;
                if (token.Contains("exit")) exit = item;
                i++;
            }
        }

        if (enter != null && exit != null && dude != null)
        {
            enter.GetComponent<TeleportEnterController>().dude = dude.GetComponent<DudeController>();
            enter.GetComponent<TeleportEnterController>().teleportExit = exit.gameObject;
        }

        foreach (var (token, positionVARIABLE) in later)
        {
            
        }
    }

    private Vector3 ResolvePosition(int x, int y)
    {
        return new Vector3(mapLeft + x, mapTop + y, 5);
    }
}
