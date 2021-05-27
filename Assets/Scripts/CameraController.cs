using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player player;
    public Camera camera;
    private Vector3 position;
    public bool isFocused;
    public int CameraSize;
    public GameObject objectInFocus;
    public float size = 5;
    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }
        camera = FindObjectOfType<Camera>();
    }

    private void FollowPlayer()
    {
        size = 5;
        // player.OnEnable();
        if (player.isWithSword && camera.orthographicSize < 4.9)
        {
            position.y += 2;
            camera.orthographicSize += Time.deltaTime * 4;
        }
        else if (player.isWithSword && camera.orthographicSize > 5.3)
        {
            position.y += 2;
            camera.orthographicSize -= Time.deltaTime * 4;
        }
        else if (camera.orthographicSize > 3 && !player.isWithSword)
        {
            position.y += 3.5f;
            camera.orthographicSize -= Time.deltaTime * 4;
        }

        position = player.transform.position;
        position.z = -10f;

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime*2.5f);

    }


    public void FocusOnObject(GameObject gameObject)
    {
        if (camera.orthographicSize < size)
        {
            position.y += 2;
            camera.orthographicSize += Time.deltaTime * 4;
        }
        else if (camera.orthographicSize > size+0.3)
        {
            position.y += 2;
            camera.orthographicSize -= Time.deltaTime * 4;
        }
        player.OnDisable();
        transform.position = Vector3.Lerp(this.transform.position, objectInFocus.transform.position, Time.deltaTime);
        var distance = this.transform.position.magnitude - objectInFocus.transform.position.magnitude;
        if (distance*distance < 0.2)
        {
            Player.Instance.OnEnable();
            isFocused = false;
        }
        position.z = -10f;
    }

    void Update()
    {
        if (isFocused)
            FocusOnObject(objectInFocus);
        else
            FollowPlayer();
    }
}
