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
        Debug.Log("已隐藏2");
        // Call this function, if you want start hiding the loading screen
        _animatorComponent.SetTrigger("Hide");
        Invoke(nameof(OnFinishedHide), 0.5f);
    }

    public void OnFinishedReveal()
    {
        // TODO: remove it and load your own scene !!
        //transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenRevealed();
    }

    public void OnFinishedHide()
    {
        Debug.Log("已隐藏3");
        GetComponent<Canvas>().enabled = false;
        // TODO: remove it and call your functions 
        //transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenHided();
    }

}
