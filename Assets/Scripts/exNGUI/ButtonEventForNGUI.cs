using UnityEngine;
using System.Collections;

public class ButtonEventForNGUI : MonoBehaviour {

	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void QuitGame()
	{
		Debug.Log("bbb");
		Application.Quit ();
	}


	public void BackMainMenu()
	{
        MainGame.BackMainMenu();
	}

    public void ResumeGame()
    {
        MainGame.ResumeGame();
    }

    public void PauseGame()
    {
        MainGame.PauseGame();
    }

    public void Restart()
    {
        MainGame.RestartGame();
    }

    public void GoNext()
    {
        MainGame.GoNextGame();
    }
}
