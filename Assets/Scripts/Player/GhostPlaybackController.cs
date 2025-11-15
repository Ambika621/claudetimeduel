using UnityEngine;
using System.Collections.Generic;

public class GhostPlaybackController : MonoBehaviour
{
    [Header("Playback")]
    public float playbackSpeed = 1f;
    
    [Header("Visual")]
    public Material ghostMaterial;
    public TrailRenderer echoTrail;
    public float transparency = 0.5f;
    
    private List<GhostRecorder.FrameData> recording;
    private int currentFrame = 0;
    private float playbackTime = 0;
    private bool isPlaying = false;
    
    private CombatController combat;
    private Rigidbody rb;
    
    void Start()
    {
        combat = GetComponent<CombatController>();
        rb = GetComponent<Rigidbody>();
        
        ApplyGhostMaterial();
        
        if (echoTrail)
        {
            echoTrail.emitting = true;
        }
    }
    
    void Update()
    {
        if (!isPlaying || recording == null || currentFrame >= recording.Count) return;
        
        playbackTime += Time.deltaTime * playbackSpeed;
        
        while (currentFrame < recording.Count && recording[currentFrame].timestamp <= playbackTime)
        {
            PlayFrame(recording[currentFrame]);
            currentFrame++;
        }
        
        if (currentFrame >= recording.Count)
        {
            Destroy(gameObject, 2f);
        }
    }
    
    public void SetRecording(List<GhostRecorder.FrameData> frames)
    {
        recording = frames;
        isPlaying = true;
        playbackTime = 0;
        currentFrame = 0;
    }
    
    void PlayFrame(GhostRecorder.FrameData frame)
    {
        transform.position = frame.position;
        transform.rotation = frame.rotation;
        
        if (rb)
        {
            rb.velocity = frame.velocity;
        }
    }
    
    void ApplyGhostMaterial()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            if (ghostMaterial)
            {
                renderer.material = ghostMaterial;
            }
            
            Color color = renderer.material.color;
            color.a = transparency;
            renderer.material.color = color;
        }
    }
}

