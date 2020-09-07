using UnityEngine;
using System.Collections;

public class MainGameLoader : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

        if (null != MainGame.currentLevelData)
        {
            MainGame.SetOperationState(OperationState.Pen);
            MainGame.ClearAllColorClueNumbers();
            LoadChequer();
        }
    }

    //	// Update is called once per frame
    //	void Update () {
    //	
    //	}


    void LoadChequer()
    {
        LevelInfo info = MainGame.currentLevelData;
        if (null != info.mainChequerObject && null != info.uiRoot)
        {

            GameObject clonedObject = GameObject.Instantiate(info.mainChequerObject) as GameObject;
            clonedObject.transform.position = Vector2.zero;


            GameObject clonedUIRoot = GameObject.Instantiate(info.uiRoot) as GameObject;
            clonedUIRoot.transform.position = Vector2.zero;

            AnswerText answerText = clonedUIRoot.GetComponentInChildren<AnswerText>();
            Chequer chequer = clonedObject.GetComponentInChildren<Chequer>();
            if (answerText != null)
            {
                chequer.transOfAnswerText = answerText.transform.parent;
            }

        }
        Camera.main.orthographicSize = info.mainCameraSize;

    }
}
