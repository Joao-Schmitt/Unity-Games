using System;

[Serializable]
public class ConfigModel 
{
    public Resolution Resolution { get; set; }
    public LimitFPS LimitFPS { get; set; }
    public bool WindowMode { get; set; }
    public bool VSinc { get; set; }
    public Quality Quality { get; set; }
    public bool Bloom { get; set; }
    public bool AmbientOcclusion { get; set; }
    public bool Reflection { get; set; }
    public float GlobalVolume { get; set; }
    public float MusicVolume { get; set; }
    public float EffectsVolume { get; set; }
    public bool AutoSave { get; set; }
}
[Serializable]
public enum Quality
{
    VeryLow,
    Low,
    Medium,
    High,
    Ultra
}
[Serializable]
public class Resolution
{
    public int Width { get; set; }
    public int Height { get; set; }
}
[Serializable]
public class LimitFPS
{
    public bool Limit { get; set; }
    public int FPS { get; set; }
}