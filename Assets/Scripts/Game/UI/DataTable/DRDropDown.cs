using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/DR DropDown")]
[RequireComponent(typeof(Image))]
public class DRDropDown : Dropdown
{
    /// <summary>
    /// 在添加Item时，不可用的Index，如果是动态添加的不同列表，可以在打开之前这个里改变值
    /// </summary>
    [SerializeField]
    [HideInInspector]
    private List<int> m_DefaultDisables = new List<int>();
    public List<int> disables
    {
        get { return m_DefaultDisables; }
    }

    /// <summary>
    /// 保存的Toggle
    /// </summary>
    private List<Toggle> m_ItemToggles = new List<Toggle>();

    /// <summary>
    /// 列表打开状态时，可以获取Toggle，可改变Interactable动态修改是否可选
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Toggle GetItemToggle(int index)
    {
        if (index < 0 || index >= m_ItemToggles.Count)
        {
            return null;
        }
        return m_ItemToggles[index];
    }

    public DRDropDown() : base()
    {

    }

    protected override void Awake()
    {
        base.Awake();

        if (template == null)
        {
            Transform tran = transform.Find("Template");
            if (tran != null)
            {
                template = tran as RectTransform;
            }
        }

        if (captionText == null)
        {
            Transform tran = transform.Find("Label");
            if (tran != null)
            {
                captionText = tran.GetComponent<Text>();
            }
        }

        if (itemText == null)
        {
            Transform tran = transform.Find("Template/Viewport/Content/Item/Item Label");
            if (tran != null)
            {
                itemText = tran.GetComponent<Text>();
            }
        }

    }

    /// <summary>
    /// 打开列表时，创建Item的Callback
    /// </summary>
    /// <param name="itemTemplate"></param>
    /// <returns></returns>
    protected override DropdownItem CreateItem(DropdownItem itemTemplate)
    {
        DropdownItem item = base.CreateItem(itemTemplate);

        // 添加Toggle
        m_ItemToggles.Add(item.toggle);

        // 根据Index设置是否可选
        int index = m_ItemToggles.Count - 1;
        for (int i = 0; i < m_DefaultDisables.Count; i++)
        {
            if (index == m_DefaultDisables[i])
            {
                item.toggle.interactable = false;
                break;
            }
        }

        return item;
    }

    /// <summary>
    /// 关闭列表时，销毁Item的Callback
    /// </summary>
    /// <param name="item"></param>
    protected override void DestroyItem(DropdownItem item)
    {
        // 如果包含Toggle，就删除
        if (m_ItemToggles.Contains(item.toggle))
        {
            m_ItemToggles.Remove(item.toggle);
        }

        base.DestroyItem(item);
    }
}