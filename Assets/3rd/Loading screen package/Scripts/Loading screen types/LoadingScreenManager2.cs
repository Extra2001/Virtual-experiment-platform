using HT.Framework;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class LoadingScreenManager2 : MonoBehaviour
{

    public static LoadingScreenManager2 Instance;

    private Animator _animatorComponent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;
        _animatorComponent = transform.GetComponent<Animator>();
    }

    public void RevealLoadingScreen()
    {
        gameObject.SetActive(true);
        _animatorComponent.SetTrigger("Reveal");
    }

    public void HideLoadingScreen()
    {
        // Call this function, if you want start hiding the loading screen
        _animatorComponent.SetTrigger("Hide");
    }

    public void OnFinishedReveal()
    {
        // TODO: remove it and load your own scene !!
        //transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenRevealed();
    }

    public void OnFinishedHide()
    {
        gameObject.SetActive(false);
        // TODO: remove it and call your functions 
        //transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenHided();
    }

}
