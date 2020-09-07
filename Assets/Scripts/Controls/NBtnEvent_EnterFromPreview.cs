using UnityEngine;
using System.Collections;

public class NBtnEvent_EnterFromPreview : MonoBehaviour
{

    public LevelInfo levelData;
    const string sceneName = "MainGameScene";
    //	// Use this for initialization
    //	void Start () {
    //	
    //	}
    //	
    //	// Update is called once per frame
    //	void Update () {
    //	
    //	}


    public void SelectLevel(LevelInfo levelData)
    {

        Debug.Log(GameConstants.MainSceneName);
    }

    void OnClick()
    {
        if (null != levelData)
        {
            //nuts..
            //create a static leveldata object
            //and load mainGame Scene
            // while

            MainGame.currentLevelData = levelData;
            Application.LoadLevel(GameConstants.MainSceneName);
        }
        else
        {
            Debug.LogError("there is no leveldata found");
        }
    }

}
