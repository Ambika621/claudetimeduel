using UnityEngine;
using System.Collections.Generic;

public class GhostRecorder : MonoBehaviour
{
    [System.Serializable]
    public class FrameData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public string action;
        public float timestamp;
    }
    
    public List<FrameData> recordedFrames = new List<FrameData>();
    private bool isRecording = false;
    private Rigidbody rb;
    private float recordTime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        if (isRecording)
        {
            RecordFrame();
        }
    }
    
    public void StartRecording()
    {
        isRecording = true;
        recordedFrames.Clear();
        recordTime = 0;
    }
    
    public void StopRecording()
    {
        isRecording = false;
    }
    
    void RecordFrame()
    {
        FrameData frame = new FrameData
        {
            position = transform.position,
            rotation = transform.rotation,
            velocity = rb ? rb.velocity : Vector3.zero,
            timestamp = recordTime
        };
        
        recordedFrames.Add(frame);
        recordTime += Time.fixedDeltaTime;
    }
    
    public void RecordAction(string actionType)
    {
        if (isRecording && recordedFrames.Count > 0)
        {
            recordedFrames[recordedFrames.Count - 1].action = actionType;
        }
    }
}

