using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskSoundHandler
{
    AudioSource _taskSoundSource;
    Dictionary<TaskSoundType, AudioClip> _taskSoundMap = new Dictionary<TaskSoundType, AudioClip>();
    public bool IsPlaying => _taskSoundSource.isPlaying;

    public void Initialize(TaskSoundHandlerData data)
    {
        _taskSoundSource = new GameObject("Task Sound Source").AddComponent<AudioSource>();
        _taskSoundSource.playOnAwake = false;
        _taskSoundSource.loop = false;
        _taskSoundSource.transform.SetParent(data.TaskSoundSourceParentTransform);

        foreach(TaskSoundEntry entry in data.TaskSoundEntryList)
            _taskSoundMap.Add(entry.TaskSoundType, entry.TaskSoundClip);
    }

    public void StopTaskSound() => _taskSoundSource.Stop();
    public void StartTaskSound(TaskSoundType taskSoundType)
    {
        if(_taskSoundSource.isPlaying) return;

        _taskSoundSource.clip = _taskSoundMap[taskSoundType];
        _taskSoundSource.Play();
    }

    public void SetVolume(float volume) => _taskSoundSource.volume = volume;
}

[Serializable]
public class TaskSoundEntry
{
   public TaskSoundType TaskSoundType;
   public AudioClip TaskSoundClip; 
}

[Serializable]
public class TaskSoundHandlerData
{
    public Transform TaskSoundSourceParentTransform;
    public List<TaskSoundEntry> TaskSoundEntryList = new();
}