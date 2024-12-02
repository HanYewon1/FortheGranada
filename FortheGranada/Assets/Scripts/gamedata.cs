using UnityEngine;
using UnityEngine.SceneManagement;

public class gamedata : MonoBehaviour
{
    Scene current_scene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_scene = SceneManager.GetActiveScene();
        if (current_scene.name != "MainMenuScene")
        {
            GameSave();
        }
        
    }

    public void GameSave()
    {
        PlayerPrefs.SetInt("Stage", GameManager.Instance.stage);
        PlayerPrefs.SetInt("Diff", GameManager.Instance.diff);
        PlayerPrefs.SetInt("Health", GameManager.Instance.health);
        PlayerPrefs.SetInt("MaxHealth", GameManager.Instance.maxHealth);
        PlayerPrefs.SetFloat("Speed", GameManager.Instance.speed);
        PlayerPrefs.SetInt("StealthTime", GameManager.Instance.stealthTime);
        PlayerPrefs.SetInt("HealthItem", GameManager.Instance.health_item);
        PlayerPrefs.SetInt("StealthItem", GameManager.Instance.stealth_item);
        PlayerPrefs.SetInt("SpeedItem", GameManager.Instance.speed_item);
        PlayerPrefs.SetInt("HasteItem", GameManager.Instance.haste_item);
        PlayerPrefs.SetInt("PreviewItem", GameManager.Instance.preview_item);
        PlayerPrefs.SetInt("RessurectionItem", GameManager.Instance.ressurection_item);
        PlayerPrefs.Save();
        Debug.Log("save");
    }

    public void GameLoad()
    {   
        int stage = PlayerPrefs.GetInt("Stage");
        string stage_scene = "";
        
        if(stage <= 3)
        {
            stage_scene = "Stage_" + stage;
        }
        else
        {
            stage_scene = "Stage_Boss";
        }

        Scene scene = SceneManager.GetSceneByName(stage_scene);
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        GameManager.Instance.is_running = true;
        GameManager.Instance.stage = stage;
        GameManager.Instance.diff = PlayerPrefs.GetInt("Diff");
        GameManager.Instance.maxHealth = PlayerPrefs.GetInt("MaxHealth");
        GameManager.Instance.health = PlayerPrefs.GetInt("Health");
        GameManager.Instance.speed = PlayerPrefs.GetFloat("Speed");
        GameManager.Instance.originspeed = 4;
        SceneManager.LoadScene(stage_scene);

        GameManager.Instance.stealthTime = PlayerPrefs.GetInt("StealthTime");
        GameManager.Instance.health_item = PlayerPrefs.GetInt("HealthItem");
        GameManager.Instance.stealth_item = PlayerPrefs.GetInt("StealthItem");
        GameManager.Instance.speed_item = PlayerPrefs.GetInt("SpeedItem");
        GameManager.Instance.haste_item = PlayerPrefs.GetInt("HasteItem");
        GameManager.Instance.preview_item = PlayerPrefs.GetInt("PreviewItem");
        GameManager.Instance.ressurection_item = PlayerPrefs.GetInt("RessurectionItem");

        GameManager.Instance.updatehealth();
        GameManager.Instance.SetItemIcon();
        
        Debug.Log("load");
    }
}
