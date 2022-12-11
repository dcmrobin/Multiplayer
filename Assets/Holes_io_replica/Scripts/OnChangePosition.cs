using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2dCollider;
    public PolygonCollider2D ground2dCollider;
    public MeshCollider GeneratedMeshCollider;
    public Collider GroundCollider;
    public float initialScale = 0.2f;
    public float holeSpeed = 1;
    Vector3 targetPos;
    Mesh GeneratedMesh;

    public IEnumerator ScaleHole()
    {
        Vector3 StartScale = transform.localScale;
        Vector3 EndScale = StartScale * 2;

        float t = 0;
        while (t <= 0.4f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(StartScale, EndScale, t);
            yield return null;
        }
    }

    private void Start()
    {
        GameObject[] AllGOs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in AllGOs)
        {
            if (go.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GeneratedMeshCollider, true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, GroundCollider, true);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, false);

        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, GroundCollider, false);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, true);
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2dCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2dCollider.transform.localScale = transform.localScale * initialScale;
            MakeHole2d();
            Make3dMeshCollider();
        }
    }

    private void MakeHole2d()
    {
        Vector2[] PointPositions = hole2dCollider.GetPath(0);

        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2dCollider.transform.TransformPoint(PointPositions[i]);
        }

        ground2dCollider.pathCount = 2;
        ground2dCollider.SetPath(1, PointPositions);
    }

    private void Make3dMeshCollider()
    {
        if (GeneratedMesh != null) Destroy(GeneratedMesh);
        GeneratedMesh = ground2dCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position += Vector3.left * holeSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position += Vector3.right * holeSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.position += Vector3.forward * holeSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.position += Vector3.back * holeSpeed * Time.deltaTime;
        }

        if (holeSpeed <= 0.5f)
        {
            holeSpeed = 0.5f;
        }
    }
}
