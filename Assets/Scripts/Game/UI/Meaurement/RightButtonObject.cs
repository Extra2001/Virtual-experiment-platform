/************************************************************************************
    作者：荆煦添
    描述：右键单击被测物体处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine;

public class RightButtonObject : HTBehaviour
{
    public ObjectValue objectValue;
    public int index;

    private void Update()
    {
        if (Main.m_Procedure.CurrentProcedure is OnChairProcedure && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var item in gameObject.transform.GetComponentsInChildren<Collider>(true))
                    if (hit.collider.gameObject.GetInstanceID().Equals(item.gameObject.GetInstanceID()))
                        {
                            Main.m_UI.OpenTemporaryUI<ObjectInfoUILogic>(objectValue);
                            break;
                        }
            }
        }

        objectValue.childrenPostition[index] = transform.localPosition.GetMyVector();
        objectValue.childrenRotation[index] = transform.localRotation.GetMyVector();
    }
}
