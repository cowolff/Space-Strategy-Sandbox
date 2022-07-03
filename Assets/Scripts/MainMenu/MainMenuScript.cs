using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    private UIDocument m_UIDocument;

    VisualElement mods;
    VisualElement factions;
    Button startGame;
    Button quitGame;

    public StyleSheet styleSheet;

    string selectedMod;
    string displayMod;
    string selectedFaction;

    string[] mod_list;

    // Start is called before the first frame update
    void Start()
    {
        var rootElement = m_UIDocument.rootVisualElement;

        rootElement.styleSheets.Add(styleSheet);

        this.mods = rootElement.Q<VisualElement>("ModList");
        this.factions = rootElement.Q<VisualElement>("FactionList");
        this.startGame = rootElement.Q<Button>("StartButton");

        this.mod_list = Directory.GetDirectories("Config");
        // this.mod_list = AssetDatabase.GetS.ubFolders("Assets/Config");

        foreach(string mod in mod_list){
            Button new_button = new Button(){ text = mod.Split("\\")[mod.Split("\\").Length - 1] };
            new_button.clickable.clicked += () => {
                    this.selectedMod = new_button.text;
            };
            new_button.AddToClassList("ModSelector");
            this.mods.Add(new_button);
        }

        this.startGame.clickable.clicked += this.OnStartClicked;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.displayMod != this.selectedMod){
            this.factions.Clear();
            string path = "Config/" + this.selectedMod + "/Planets.json";
            using (StreamReader r = new StreamReader(path)){
                string json = r.ReadToEnd();
                ListPlanets planets = JsonUtility.FromJson<ListPlanets>(json);

                List<string> faction_list = new List<string>();
                foreach(PlanetJSON planet in planets.planets){
                    faction_list.Add(planet.faction);
                }
                var distinctFactions = faction_list.Distinct();
                foreach(string faction in distinctFactions){
                    Button new_button = new Button(){ text = faction };
                    new_button.clickable.clicked += () => {
                        this.selectedFaction = faction;
                    };
                    new_button.AddToClassList("ModSelector");
                    this.factions.Add(new_button);
                }
                this.selectedFaction = faction_list[0];
                this.displayMod = this.selectedMod;
            }
        }
    }

    private void OnStartClicked(){
        StaticInformation.faction = this.selectedFaction;
        StaticInformation.path = "Config/" + this.selectedMod + "/";
        SceneManager.LoadScene(1);
    }

    private void OnExitClicked(){
        Application.Quit();
    }
}
