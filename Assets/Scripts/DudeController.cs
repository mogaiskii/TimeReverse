using System.Collections.Generic;
using UnityEngine;

public class DudeController : MonoBehaviour
{
    public GameObject AntiDudePrefab;

    public float speed = 2f;
    public Rigidbody2D rb;
    private Vector2 _moveDirection;

    private bool _tmpBlockRight;

    private List<Vector3> _positionsHistory = new List<Vector3>();
    private Vector3 _lastPosition;
    private bool _shouldWriteHistory = true;

    private bool _antiDudeHasAwaken;

    public AudioSource SoundSource;
    public List<AudioClip> stepsSound = new List<AudioClip>();
    public float soundOffset = 0.15f;
    public float soundEffectiveLength = 0.15f;
    private float soundCooldown = 0;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (ProgramManager.instance.freeze)
        {
            _moveDirection = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        if (_tmpBlockRight && moveX > 0) moveX = 0;
        else _tmpBlockRight = false;

        float moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized * speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_moveDirection.x, _moveDirection.y);

        if (_shouldWriteHistory)
            _positionsHistory.Insert(0, transform.position);

        ProgramManager.instance.hasMovement = transform.position != _lastPosition;
        
        if (soundCooldown > 0)
        {
            soundCooldown -= Time.deltaTime;
        }
        if (_lastPosition != transform.position && soundCooldown <= 0)
        {
            SoundSource.clip = stepsSound[Random.Range(0, stepsSound.Count-1)];
            SoundSource.time = soundOffset;
            SoundSource.Play();
            soundCooldown = soundEffectiveLength;
        }

        _lastPosition = transform.position;
    }

    public void Teleport(Vector3 newPosition)
    {
        List<Vector3> history = new List<Vector3>(_positionsHistory);
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        // TODO: mb vfx
        // TODO: should look to the left after exit
        _tmpBlockRight = true;
        _shouldWriteHistory = false;
        _positionsHistory.Clear();

        CreateAntiDude(history, position);
    }

    private void CreateAntiDude(List<Vector3> positionsHistory, Vector3 position)
    {
        if (!_antiDudeHasAwaken)
        {
            GameObject antiDude = Instantiate(AntiDudePrefab, position, Quaternion.identity);
            antiDude.GetComponent<AntiDudeController>().SetPositions(positionsHistory);
        }
        _antiDudeHasAwaken = true;
    }
}
