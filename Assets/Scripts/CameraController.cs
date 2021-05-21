using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player player;
    private Camera camera;
    private Vector3 position;
    public bool isFocused;
    public GameObject objectInFocus;
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
        if (player.isWithSword && camera.orthographicSize < 5)
        {
            position.y += 2;
            camera.orthographicSize += Time.deltaTime * 4;
        }
        else if (camera.orthographicSize > 3 && !player.isWithSword)
        {
            position.y += 5;
            camera.orthographicSize -= Time.deltaTime * 4;
        }

        position = player.transform.position;
        position.z = -10f;

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);

    }

    IEnumerable FocusTimer(int time)
    {
        yield return new WaitForSeconds(time);
        print("end");
        isFocused = false;
    }

    private void FocusOnObject(GameObject gameObject)
    {
        transform.position = Vector3.Lerp(this.transform.position, objectInFocus.transform.position, Time.deltaTime);
        var distance = this.transform.position.magnitude - objectInFocus.transform.position.magnitude;
        if (distance*distance < 0.2)
            isFocused = false;
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
