using System.Threading.Tasks;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    private Animator _animatorComponent;

    private void Start()
    {
        _animatorComponent = transform.GetComponent<Animator>();
        UIAPI.Instance.LoadingScreenManager = this;
        HideLoadingScreen();
    }

    public void RevealLoadingScreen()
    {
        GetComponent<Canvas>().enabled = true;
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
        GetComponent<Canvas>().enabled = false;
        // TODO: remove it and call your functions 
        //transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenHided();
    }

}
