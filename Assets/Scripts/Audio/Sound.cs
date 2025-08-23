using UnityEngine;

[System.Serializable]
public class Sound
{
    public string audioName;
    public AudioClip clip;
}
[System.Serializable]
public class SoundItem
{
    public TrashMadeOf audioName;
    public AudioClip[] clip;
}
