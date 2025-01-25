using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Vector3 movementOffset;

    [SerializeField]
    private float followSpeed;

    [SerializeField]
    private float maxZoom;

    [SerializeField]
    private float minZoom;

    [SerializeField]
    private float zoomLimit;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float maxY;

    private List<PlayerController> activePlayers = new List<PlayerController>();
    public void RegisterPlayer(PlayerController player)
    {
        if (!activePlayers.Contains(player))
        {
            player.RegisterCameraManager(this);
            activePlayers.Add(player);
        }
    }

    public void DeregisterPlayer(PlayerController player)
    {
        if (activePlayers.Contains(player))
        {
            activePlayers.Remove(player);
        }
    }

    private void Update()
    {
        if (activePlayers.Count == 0)
        {
            return;
        }

        MoveCamera();
        ZoomCamera();
    }

    private void ZoomCamera()
    {
        var zoom = Mathf.Lerp(maxZoom, minZoom, GetLargestDistance() / zoomLimit);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoom, Time.deltaTime * zoomSpeed);
    }

    private float GetLargestDistance()
    {
        var bounds = new Bounds(activePlayers[0].GetPossessedPosition(), Vector3.zero);
        foreach (var player in activePlayers)
        {
            bounds.Encapsulate(player.GetPossessedPosition());
        }

        return bounds.size.x;
    }

    private void MoveCamera()
    {
        var centerPosition = GetCenterPosition();
        var offsetPosition = centerPosition + movementOffset;
        var velocity = Vector3.zero;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, offsetPosition, ref velocity, Time.deltaTime * followSpeed);
        var cameraPosition = mainCamera.transform.position;
        if(cameraPosition.y < maxY)
        {
            mainCamera.transform.position = new Vector3(cameraPosition.x, maxY, cameraPosition.z);
        }
    }

    private Vector3 GetCenterPosition()
    {
        if(activePlayers.Count == 1)
        {
            return activePlayers[0].GetPossessedPosition();
        }

        var bounds = new Bounds(activePlayers[0].GetPossessedPosition(), Vector3.zero);
        foreach (var player in activePlayers)
        {
            bounds.Encapsulate(player.GetPossessedPosition());
        }

        return bounds.center;
    }

    internal bool IsWithinBounds(Vector3 predictedPosition)
    {
        var viewport = mainCamera.WorldToViewportPoint(predictedPosition);
        var inCameraFrustum = Is01(viewport.x) && Is01(viewport.y);
        var inFrontOfCamera = viewport.z > 0;

        return inCameraFrustum && inFrontOfCamera;
    }

    public bool Is01(float a)
    {
        return a > 0.05f && a < 0.95f;
    }
    
    public Vector3 GetSpawnLocation()
    {
        var centerPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        var randomOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
        var offsettedPosition = centerPosition - (movementOffset + randomOffset);
        offsettedPosition.z = 0.0f;
        return offsettedPosition;
    }
}
