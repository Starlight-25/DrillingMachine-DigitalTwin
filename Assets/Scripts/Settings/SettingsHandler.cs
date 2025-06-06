using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public Settings Settings;
    private string SettingsPath;
    private List<ISettingsUpdater> SettingsUpdaters;
    
    
    
    
    
    private void Start()
    {
        SettingsPath = Path.Combine(Application.persistentDataPath, "settings.json");
        if (!File.Exists(SettingsPath)) CreateSettingsData();
        LoadSettingsData();
        SettingsUpdaters = new List<ISettingsUpdater>();
    }



    

    private void CreateSettingsData()
    {
        string settingsData = Resources.Load<TextAsset>("settingsModel").text;
        File.WriteAllText(SettingsPath, settingsData);
    }

    
    
    

    public void LoadSettingsData() =>
        Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));





    public void SaveSettingsData() =>
        File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(Settings, Formatting.Indented));





    public void Add(ISettingsUpdater settingsUpdater) => SettingsUpdaters.Add(settingsUpdater);




    public void ApplySettings()
    {
        foreach (ISettingsUpdater settingsUpdater in SettingsUpdaters)
            settingsUpdater.UpdateFromSettings();
        SaveSettingsData();
    }
}