using UnityEngine;
using System.Collections;

/// <summary>
/// 预览图/缩略图创造器
/// 
/// 2014-09-15
/// design by 顾文光
/// </summary>
public class PreviewCreator : MonoBehaviour {
    public string fileName = "_defaultImg";
    public Camera originSmallCamera;
    RenderTexture myRenderTexture = null;
    public Chequer myChequer;
    public bool isDestroyCam = true;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SaveToPriview()
    {
        if (myChequer != null)
        {
            int rows = myChequer.rowCount;
            int cols = myChequer.columnCount;
            Camera clonedCamera = GameObject.Instantiate(originSmallCamera) as Camera;
            clonedCamera.transform.parent = myChequer.transform;
            clonedCamera.transform.localPosition = new Vector3(
                (float)cols / 2 + 0.5f + myChequer.transOfDataRoot.localPosition.x,
                -1 * ((float)rows / 2 + 0.5f - myChequer.transOfDataRoot.localPosition.y),
                -10);
            clonedCamera.orthographicSize = (float)Mathf.Max(cols, rows) * 5 / 10;

            RenderTexture rt = new RenderTexture(128, 128, 0);
            clonedCamera.targetTexture = rt;
            clonedCamera.Render();

            RenderTexture.active = rt;
            //Rect rect = new Rect(clonedCamera.transform.position.x, clonedCamera.transform.position.y, rt.width, rt.height);
            Rect rect = new Rect(0, 0, rt.width, rt.height);
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
            screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
            screenShot.Apply();

            clonedCamera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.DestroyImmediate(rt);
            if (isDestroyCam)
            {
                GameObject.DestroyImmediate(clonedCamera.gameObject);
            }
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Application.dataPath + GameConstants.DefaultPreviewPath + this.fileName + ".png";//  "/Screenshot.png";
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("截屏了一张照片: {0}", filename));  
        }
    }
}