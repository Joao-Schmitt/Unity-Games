using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuInicial, menuOptions, rawImage;
    public AudioSource selectSound;
    public string newGameScene;
    private Animator animatorRawImage;

    public TMP_Dropdown resolution, quality;
    public TMP_InputField textFPS;
    public Toggle limitFPS, windowMode, vSinc, bloom, occlusion, reflection, autoSave;
    public Slider globalVol, musicsVol, effectVol;

    void Start()
    {
        ApplyConfigs();

        rawImage.SetActive(false);
        menuOptions.SetActive(false);
        animatorRawImage = rawImage.GetComponent<Animator>();
    }

    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown)
        {
            selectSound.Play();
            videoPlayer.Play();
            animatorRawImage.SetTrigger("fadeIn");
            rawImage.SetActive(true);
            menuInicial.SetActive(true);
        }
    }

    public void Options()
    {
        menuInicial.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void Salvar()
    {
        SaveConfigs();
        RetornarMenuInicial();
    }

    public void RetornarMenuInicial()
    {
        menuInicial.SetActive(true);
        menuOptions.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void ApplyConfigs()
    {
        var configs = LoadConfigs();

        if(configs == null)
            return;

        //Aplica a resolucao e modo janela
        Screen.SetResolution(configs.Resolution.Width, configs.Resolution.Height, !configs.WindowMode);

        //Aplica o preset de qualidade
        QualitySettings.SetQualityLevel((int)configs.Quality);

        //Aplica limite de FPS
        Application.targetFrameRate = configs.LimitFPS.Limit ? configs.LimitFPS.FPS : -1;

        //Ativar o vsinc
        QualitySettings.vSyncCount = configs.VSinc? 1 : 0;

        //Aplica o volume
        SceneConfigs.effectsVolume = configs.EffectsVolume;
        SceneConfigs.globalVolume = configs.GlobalVolume;
        SceneConfigs.musicVolume = configs.MusicVolume;
        SceneConfigs.autoSave = configs.AutoSave;

        //Aplica configs de pós-processamento
        SceneConfigs.bloom = configs.Bloom;
        SceneConfigs.reflections = configs.Reflection;
        SceneConfigs.occlusion = configs.AmbientOcclusion;
    }

    private ConfigModel LoadConfigs()
    {
        try
        {
            var fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MenuAula/ConfigData.save";
            if (!File.Exists(fileDirectory))
                return null;

            var binaryFormatter = new BinaryFormatter();
            var file = File.OpenRead(fileDirectory);

            var configs = (ConfigModel)binaryFormatter.Deserialize(file);
            file.Close();

            if (configs != null)
            {
                var option = resolution.options.Where(x => x.text == $"{configs.Resolution.Width}x{configs.Resolution.Height}").FirstOrDefault();
                resolution.value = resolution.options.IndexOf(option);
                quality.value = (int)configs.Quality;
                textFPS.text = configs.LimitFPS.FPS.ToString();
                limitFPS.isOn = configs.LimitFPS.Limit;
                windowMode.isOn = configs.WindowMode;
                vSinc.isOn = configs.VSinc;
                bloom.isOn = configs.Bloom;
                occlusion.isOn = configs.AmbientOcclusion;
                reflection.isOn = configs.Reflection;
                autoSave.isOn = configs.AutoSave;
                globalVol.value = configs.GlobalVolume;
                effectVol.value = configs.EffectsVolume;
                musicsVol.value = configs.MusicVolume;
            }

            return configs;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void SaveConfigs()
    {
        try
        {
            var resolutionModel = new Resolution();

            switch (resolution.value)
            {
                case 0:
                    resolutionModel.Width = 800;
                    resolutionModel.Height = 600;
                    break;
                case 1:
                    resolutionModel.Width = 1280;
                    resolutionModel.Height = 720;
                    break;
                case 2:
                    resolutionModel.Width = 1920;
                    resolutionModel.Height = 1080;
                    break;
                case 3:
                    resolutionModel.Width = 2560;
                    resolutionModel.Height = 1440;
                    break;
                case 4:
                    resolutionModel.Width = 3840;
                    resolutionModel.Height = 2160;
                    break;
            }

            var configs = new ConfigModel()
            {
                AmbientOcclusion = occlusion.isOn,
                AutoSave = autoSave.isOn,
                Bloom = bloom.isOn,
                VSinc = vSinc.isOn,
                WindowMode = windowMode.isOn,
                GlobalVolume = globalVol.value,
                EffectsVolume = effectVol.value,
                MusicVolume = musicsVol.value,
                Reflection = reflection.isOn,
                LimitFPS = new LimitFPS()
                {
                    FPS = int.Parse(textFPS.text),
                    Limit = limitFPS.isOn
                },
                Quality = (Quality)quality.value,
                Resolution = resolutionModel
            };

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MenuAula/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var binaryFormatter = new BinaryFormatter();
            var file = File.Create(path + "ConfigData.save");

            binaryFormatter.Serialize(file, configs);
            file.Close();

            ApplyConfigs();
        }
        catch (Exception ex)
        {
            return;
        }
    }
}
