using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgramManager : MonoBehaviour
{
    public static ProgramManager instance;
    
    public string currentScene;

    public bool hasMovement;
    public float idleOkTime = 2f;
    private float _idleTimer = 2f;
    public bool freeze { get; private set; }
    private float freezeTimer = 4.5f;

    void Awake()
    {
        instance = this;
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        float restart = Input.GetAxis("Restart");
        if (restart != 0)
        {
            RestartLevel();
        }

        if (!hasMovement)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer < 0)
            {
                // TODO: vfx, sfx
            }
        }
        else
        {
            _idleTimer = idleOkTime;
        }

        if (freeze)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer < 0)
            {
                ChangeScene();
            }
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(currentScene);
    }

    void ChangeScene()
    {
        // TODO: Load from persistence
        if (Persistence.instance.GetScene() != null)
        {
            SceneManager.LoadScene(Persistence.instance.GetScene());
        } else if (Persistence.instance.gameFinishedScene != null && Persistence.instance.gameFinishedScene.Length > 0)
        {
            SceneManager.LoadScene(Persistence.instance.gameFinishedScene);
        }
    }
    
    public void LevelIsWon()
    {
        Debug.Log("YAY!");
        freeze = true;
        // TODO: vfx
        Persistence.instance.MoveFurther();
    }
}
