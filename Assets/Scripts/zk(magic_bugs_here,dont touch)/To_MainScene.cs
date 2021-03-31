using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class To_MainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Tomainscene);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Tomainscene()
    {
        var AniInstance = GameObject.Find("Canvas").GetComponent<FadeAnimate>();
        var tmp = GameObject.Find("Canvas").GetComponentsInChildren<RawImage>(true);
        if (tmp.Length != 0 && AniInstance != null)
        {
            var Transition = tmp[tmp.Length - 1].gameObject;
            Transition.SetActive(true);
            AniInstance.Show(Transition, 400);
            Invoke("Delay", (float)0.5);
        }
        else
        {
            Delay();
        }
    }

    void Delay()
    {
        SceneManager.LoadScene("MainScene");
    }
}
