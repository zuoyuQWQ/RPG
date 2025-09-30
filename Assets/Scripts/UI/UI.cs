using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;


    public UI_ItemTooltip itemToolTip;
    public UI_StatTooltip statTooltip;
    public UI_CraftWindow craftWindow;
    
    void Awake()
    {
        // 在Awake中进行组件检查，如果没有手动赋值则尝试自动查找
        if (itemToolTip == null)
        {
            itemToolTip = FindObjectOfType<UI_ItemTooltip>();
            if (itemToolTip == null)
                Debug.LogWarning("UI_ItemTooltip not found! Please assign it in the inspector or ensure it exists in the scene.");
        }
        
        if (statTooltip == null)
        {
            statTooltip = FindObjectOfType<UI_StatTooltip>();
            if (statTooltip == null)
                Debug.LogWarning("UI_StatTooltip not found! Please assign it in the inspector or ensure it exists in the scene.");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(null);
        
        // 安全检查，避免空引用异常
        if (itemToolTip != null)
            itemToolTip.gameObject.SetActive(false);
        
        if (statTooltip != null)
            statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWithKeyTo(characterUI);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchTo(null);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            SwitchWithKeyTo(craftUI);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(optionUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false) ;
            return;
        }
        SwitchTo(_menu);
    }
}
