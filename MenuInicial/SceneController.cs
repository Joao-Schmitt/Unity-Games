using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SceneController : MonoBehaviour
{
    private AudioSource[] audioSources;
    private PostProcessVolume[] postProcessVolumes;

    void Start()
    {
        //Audio
        audioSources = GameObject.FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audioSources)
        {
            if(audio.gameObject.tag == "music")
                audio.volume = SceneConfigs.musicVolume / 100;

            if(audio.gameObject.tag == "effect")
                audio.volume = SceneConfigs.effectsVolume / 100;

            audio.volume = Mathf.Clamp(audio.volume, 0, SceneConfigs.globalVolume / 100);
        }


        //Post Process
        postProcessVolumes = GameObject.FindObjectsOfType<PostProcessVolume>();

        foreach (PostProcessVolume volume in postProcessVolumes)
        {
            if (volume.profile.TryGetSettings(out Bloom bloom))
                bloom.active = SceneConfigs.bloom;

            if (volume.profile.TryGetSettings(out AmbientOcclusion occlusion))
                occlusion.active = SceneConfigs.occlusion;

            if (volume.profile.TryGetSettings(out ScreenSpaceReflections reflections))
                reflections.active = SceneConfigs.reflections;
        }
    }
}
