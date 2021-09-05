using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;
public class Button : MonoBehaviour
{
    public GameManager GM;
    public Transform Camera;
    public void Press()
    {
        if (GM.Win)
        {
            SceneManager.LoadScene(0);
        }
        else if (GM.Freshness <= 0)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            GM.ShowText();
            StartCoroutine(CameraMove());
        }
     
    }
    IEnumerator CameraMove()
    {
        Vector3 LastPos = new Vector3(0, 0);
        if (gameObject.name == "Down")
        {
            GM.Freshness += GM.DownMove;
            GM.buttonID = 1;
            LastPos = Camera.position + new Vector3(0, -15, -10);
        }
        else if (gameObject.name == "Left")
        { 
            GM.Freshness += GM.LeftMove;
            GM.buttonID = 2;
            LastPos = Camera.position + new Vector3(-15, 0, -10);
        }
        else if (gameObject.name == "Right")
        {
            GM.buttonID = 3;
            LastPos = Camera.position + new Vector3(15, -0, -10);
        }
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Camera.localPosition = Vector3.MoveTowards(Camera.position, LastPos, Time.deltaTime * 30);
            if (Camera.localPosition == LastPos)
            {
                if (Camera.localPosition.y <= -15)
                {
                    Camera.localPosition = new Vector3(0, 15, -10);

                }
                else if (Camera.localPosition.x <= -15)
                {
                    Camera.localPosition = new Vector3(15, 0, -10);
                }
                else if (Camera.localPosition.x >= 15)
                {
                    Camera.localPosition = new Vector3(-15, 0, -10);
                }
                else
                {
                    for (int z = 0; z < 3; z++)
                    {
                        GM.button[z].SetActive(false);

                    }
                    GM.setButton();
                    StopAllCoroutines();
                    yield break;
                }
                LastPos = new Vector3(0, 0, -10);
            }
        }
    }
}

public class JsonHelper<T>
{

    public static string GetJsonObject(string jsonString, string handle)
    {
        string pattern = "\"" + handle + "\"\\s*:\\s*\\{";

        Regex regx = new Regex(pattern);

        Match match = regx.Match(jsonString);

        if (match.Success)
        {
            int bracketCount = 1;
            int i;
            int startOfObj = match.Index + match.Length;
            for (i = startOfObj; bracketCount > 0; i++)
            {
                if (jsonString[i] == '{') bracketCount++;
                else if (jsonString[i] == '}') bracketCount--;
            }
            return "{" + jsonString.Substring(startOfObj, i - startOfObj);
        }

        //no match, return null
        return null;
    }

    public void JSONsave(string path, T CardJson)
    {
        var JSON = JsonUtility.ToJson(CardJson, true);
        File.WriteAllText(path, JSON);
    }
    public T JSONload(string path)
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(path));
    }

}