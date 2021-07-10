/************************************************************************************
    作者：张柯
    描述：旋转物体
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using UnityEngine;
using RTEditor;

public class SelfRotate : HTBehaviour
{
    GameObject RTF;
    Tweener tw;
    float TarX;
    float TarY;
    float TarZ;
    bool onTW = false;
    private Vector3 GetInpectorEulers(Transform mTransform)
    {
        Vector3 angle = mTransform.eulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;

        if (Vector3.Dot(mTransform.up, Vector3.up) >= 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = angle.x - 360f;
            }
        }
        if (Vector3.Dot(mTransform.up, Vector3.up) < 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180 - angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180 - angle.x;
            }
        }

        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }

        if (angle.z > 180)
        {
            z = angle.z - 360f;
        }
        Vector3 vector3 = new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
        //Debug.Log(" Inspector Euler:  " + Mathf.Round(x) + " , " + Mathf.Round(y) + " , " + Mathf.Round(z));
        return vector3;
    }
    // Start is called before the first frame update
    void Start()
    {
        onTW = false;
        RTF = GameObject.Find("(Singleton)RTEditor.EditorObjectSelection");
    }
    void LiveImageVanish()
    {
        onTW = false;
    }
    // Update is called once per frame
    void Update()
    {
        tw.OnComplete(LiveImageVanish);
        TarX = this.transform.localEulerAngles.x;
        TarY = this.transform.localEulerAngles.y;
        TarZ = this.transform.localEulerAngles.z;
        if (RTF.GetComponent<EditorObjectSelection>().Skode_Press() == 0)
        {
            if (this.transform.localEulerAngles.x < 15 || this.transform.localEulerAngles.x > 375)
            {
                TarX = 0;
            }
            if (this.transform.localEulerAngles.x < 105 && this.transform.localEulerAngles.x > 75)
            {
                TarX = 90;
            }
            if (this.transform.localEulerAngles.x < 195 && this.transform.localEulerAngles.x > 165)
            {
                TarX = 180;
            }
            if (this.transform.localEulerAngles.x < 285 && this.transform.localEulerAngles.x > 255)
            {
                TarX = 270;
            }

            if (this.transform.localEulerAngles.y < 15 || this.transform.localEulerAngles.y > 375)
            {
                TarY = 0;
            }
            if (this.transform.localEulerAngles.y < 105 && this.transform.localEulerAngles.y > 75)
            {
                TarY = 90;
            }
            if (this.transform.localEulerAngles.y < 195 && this.transform.localEulerAngles.y > 165)
            {
                TarY = 180;
            }
            if (this.transform.localEulerAngles.y < 285 && this.transform.localEulerAngles.y > 255)
            {
                TarY = 270;
            }

            if (this.transform.localEulerAngles.z < 15 || this.transform.localEulerAngles.z > 375)
            {
                TarZ = 0;
            }
            if (this.transform.localEulerAngles.z < 105 && this.transform.localEulerAngles.z > 75)
            {
                TarZ = 90;
            }
            if (this.transform.localEulerAngles.z < 195 && this.transform.localEulerAngles.z > 165)
            {
                TarZ = 180;
            }
            if (this.transform.localEulerAngles.z < 285 && this.transform.localEulerAngles.z > 255)
            {
                TarZ = 270;
            }
            if (!onTW)
            {
                tw = this.transform.DOLocalRotate(new Vector3(TarX, TarY, TarZ), 2f).SetEase(Ease.OutExpo);
                onTW = true;
            }
        }
    }
}
