using UnityEngine;
using System.Collections;

public class NButtonEvent_LevelSelection
: MonoBehaviour {

	LevelInfo levelInfo;
	const string sceneName= "MainGameScene";
    // Use this for initialization
    void Start()
    {
        PreviewItem tempItem = GetComponent<PreviewItem>();
        if (tempItem != null)
        {
            levelInfo = tempItem.info;
        }
    }
	
//	// Update is called once per frame
//	void Update () {
//	
//	}


	public void SelectLevel(LevelInfo levelData)
	{

		Debug.Log(GameConstants.MainSceneName);
	}

	public void OnClick()
	{
		if(null != levelInfo)
		{
			//nuts..
			//create a static leveldata object
			//and load mainGame Scene
			// while

            //MainGame.currentLevelData = levelInfo;
            
            //Application.LoadLevel(GameConstants.MainSceneName);

            MainGame.GoToLevel(levelInfo);
		}
		else
		{
			Debug.LogError("there is no leveldata found");
		}
	}

}
