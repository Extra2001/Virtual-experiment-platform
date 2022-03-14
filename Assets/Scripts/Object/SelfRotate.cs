/************************************************************************************
    作者：张柯
    描述：旋转物体
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using UnityEngine;
//using RTEditor;
//实现物体自身在三欧拉角接近90倍数时，自旋转到
//保证在无选中操作时方可启用
//欧拉角换算四元数在换回

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
        if (RTGController.Current._targetObject == null)
        {
            if (this.transform.localEulerAngles.x < 30 || this.transform.localEulerAngles.x > 330)
            {
                TarX = 0;
            }
            if (this.transform.localEulerAngles.x < 120 && this.transform.localEulerAngles.x > 60)
            {
                TarX = 90;
            }
            if (this.transform.localEulerAngles.x < 210 && this.transform.localEulerAngles.x > 150)
            {
                TarX = 180;
            }
            if (this.transform.localEulerAngles.x < 300 && this.transform.localEulerAngles.x > 240)
            {
                TarX = 270;
            }

            if (this.transform.localEulerAngles.y < 30 || this.transform.localEulerAngles.y > 330)
            {
                TarY = 0;
            }
            if (this.transform.localEulerAngles.y < 120 && this.transform.localEulerAngles.y > 60)
            {
                TarY = 90;
            }
            if (this.transform.localEulerAngles.y < 210 && this.transform.localEulerAngles.y > 150)
            {
                TarY = 180;
            }
            if (this.transform.localEulerAngles.y < 300 && this.transform.localEulerAngles.y > 240)
            {
                TarY = 270;
            }

            if (this.transform.localEulerAngles.z < 30 || this.transform.localEulerAngles.z > 330)
            {
                TarZ = 0;
            }
            if (this.transform.localEulerAngles.z < 120 && this.transform.localEulerAngles.z > 60)
            {
                TarZ = 90;
            }
            if (this.transform.localEulerAngles.z < 210 && this.transform.localEulerAngles.z > 150)
            {
                TarZ = 180;
            }
            if (this.transform.localEulerAngles.z < 300 && this.transform.localEulerAngles.z > 240)
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
