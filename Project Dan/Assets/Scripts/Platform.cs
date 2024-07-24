using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Platform : MonoBehaviour
{
    [SerializeField][Range(0, 360)] float angle;

    Vector3[] points;
    Vector3 dir;
    int current;

    [SerializeField] float speed;
    [SerializeField] float travelDistance;
    [SerializeField] float waitTime;

    [SerializeField] float width;
    [SerializeField] float height;

    float waiting;
    bool running;

    Vector3 floater;
    float TValue;
    int floaterCurrent;

    void Start()
    {
        objects = new List<Transform> ();
        running = true;
        points = new Vector3[2];

        dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

        points[0] = transform.position;
        points[1] = points[0] + dir * travelDistance;

        current = 1;
    }

    void Update()
    {
        if (points == null || points.Length < 0)
        {
            points = new Vector3[2];
        }
        waiting -= Time.deltaTime;

        Move();
    }

    void Move()
    {
        if (waiting > 0) return;
        transform.position = Vector3.MoveTowards(transform.position, points[current], speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, points[current]) < 0.02f)
        {
            waiting = waitTime;
            current = 1 - current;
        }
    }

    List<Transform> objects;
    public void Attach(Transform obj)
    {
        if (objects == null || objects.Count == 0) { objects = new List<Transform>(); }
        objects.Add(obj);
    }

    public void Dettach(Transform obj)
    {
        objects.Remove(obj);
    }

    private void LateUpdate()
    {
        if (waiting <= 0)
        {
            foreach (var item in objects)
            {
                item.position += dir * Mathf.Sign(current - 1) * speed * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!running)
        {
            if (points == null || points.Length < 0)
            {
                points = new Vector3[2];
            }
            points[0] = transform.position;
            points[1] = points[0] + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * travelDistance;
            transform.GetChild(0).localScale = new Vector2(width, height);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(points[0], points[1]);

        Gizmos.DrawWireCube(points[0], new Vector3(width, height));
        Gizmos.DrawWireCube(points[1], new Vector3(width, height));
    }
}
