using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class TimeLoopManager : MonoBehaviour
{
    [Header("Loop Settings")]
    public float loopDuration = 10f;
    public GameObject ghostPrefab;
    
    [Header("Events")]
    public UnityEvent<int> onLoopStart;
    public UnityEvent<float> onTimeUpdate;
    public UnityEvent onRewind;
    
    private float currentTime;
    private int loopCount = 0;
    private List<GameObject> activeGhosts = new List<GameObject>();
    private List<GhostRecorder> playerRecorders = new List<GhostRecorder>();
    
    void Start()
    {
        playerRecorders.AddRange(FindObjectsOfType<GhostRecorder>());
        StartLoop();
    }
    
    void Update()
    {
        currentTime += Time.deltaTime;
        onTimeUpdate?.Invoke(currentTime / loopDuration);
        
        if (currentTime >= loopDuration)
        {
            RewindTime();
        }
    }
    
    void StartLoop()
    {
        currentTime = 0;
        loopCount++;
        onLoopStart?.Invoke(loopCount);
        
        foreach (var recorder in playerRecorders)
        {
            recorder.StartRecording();
        }
    }
    
    void RewindTime()
    {
        onRewind?.Invoke();
        
        foreach (var recorder in playerRecorders)
        {
            recorder.StopRecording();
            SpawnGhost(recorder);
        }
        
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var proj in projectiles)
        {
            Destroy(proj);
        }
        
        Invoke("StartLoop", 0.5f);
    }
    
    void SpawnGhost(GhostRecorder recorder)
    {
        if (!ghostPrefab || recorder.recordedFrames.Count == 0) return;
        
        GameObject ghost = Instantiate(ghostPrefab, recorder.transform.position, recorder.transform.rotation);
        GhostPlaybackController playback = ghost.GetComponent<GhostPlaybackController>();
        
        if (playback)
        {
            playback.SetRecording(new List<GhostRecorder.FrameData>(recorder.recordedFrames));
            activeGhosts.Add(ghost);
        }
    }
    
    public int GetLoopCount() => loopCount;
    public float GetRemainingTime() => loopDuration - currentTime;
}
