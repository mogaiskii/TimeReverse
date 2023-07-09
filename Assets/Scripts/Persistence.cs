using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static Persistence instance;
    public string gameFinishedScene;
    public List<string> ScenesOrder;
    public List<TextAsset> LevelsOrder;

    public int sceneCursor = 0;
    public int levelCursor = 0;

    private string currentScene;
    private string currentLevel;
    public bool levelLoading = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        NextLevel();
    }


    public string GetScene()
    {
        return currentScene;
    }

    public void NextScene()
    {
        if (ScenesOrder.Count > sceneCursor)
        {
            string scene = ScenesOrder[sceneCursor];
            sceneCursor++;
            currentScene = scene;
        }
        if (gameFinishedScene != null)
        {
            currentScene = gameFinishedScene;
        }
    }

    public void NextLevel()
    {
        if (LevelsOrder.Count > levelCursor)
        {
            TextAsset level = LevelsOrder[levelCursor];
            levelCursor++;
            currentLevel = level.ToString();
        }
        else
        {
            currentLevel = null;
        }
    }

    public string GetLevel()
    {
        if (currentLevel != null) return currentLevel;
        return null;
    }

    public void MoveFurther()
    {
        if (levelLoading)
        {
            NextLevel();
            if (GetLevel() == null)
            {
                NextScene();
            }
        }
        else
        {
            NextScene();
        }
    }
}
