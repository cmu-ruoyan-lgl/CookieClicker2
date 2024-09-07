using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text countTxt;
    public Text speedTxt;
    public float duration = 2f; // 次数增加动画持续时间
    int startValue;// 起始值
    int endValue; // 结束值
    public Slider slider;
    public Text sliderPrecent;
    //外部容器，向右滑动的
    public RectTransform container;
    //记录鼠标初始位置
    private Vector3 _mouseStartPos;
    //是否开始拖拽
    private bool _isDragging;
    public Image fill;
    public GameObject pre;
    public Transform parent;
    bool isStore;

    internal int[] storeItemCost = {-1, 600, 2000};
    // internal int[] storeItemLast = {-1, 3600, 10800};
    internal int[] storeItemLast = {-1, 3, 10};

    public Button buyBtn1, buyBtn2;

    public GameObject shadow1, shadow2;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 购买道具
    /// </summary>
    /// 
    public void BuyItem(int itemID)
    {
        Button buyBtn;
        GameObject shadow;
        if(itemID == 1) {
            buyBtn = buyBtn1;
            shadow = shadow1;
        }else if (itemID == 2) {
            buyBtn = buyBtn2;
            shadow = shadow2;
        }else {
            Debug.Log("买错东西");
            return;
        }
        if(GameManager.Instance.GetCount() >= storeItemCost[itemID]) {
            GameManager.Instance.SubCount(storeItemCost[itemID]);
        }else {
            Debug.Log("你钱不够");
            return;
        }
        buyBtn.interactable = false;
        shadow.SetActive(true);
        
        StartCoroutine(DelayedAction(storeItemLast[itemID] + 0.25f, buyBtn, shadow));
        // Invoke(() => RefreshStoreItem(buyBtn, shadow), storeItemLast[itemID] + 0.25f);
        // Invoke("RefreshStoreItem", storeItemLast[itemID] + 0.25f, buyBtn, shadow);
        
        GameManager.Instance.multiple = 2;
        GameManager.Instance.autoClickerTime = 0;
        RefreshSpeed(GameManager.Instance.speed * GameManager.Instance.multiple);
        GameManager.Instance.isUsed = true;
    }

    IEnumerator DelayedAction(float delay, Button buyBtn, GameObject shadow)
    {
        yield return new WaitForSeconds(delay);
        RefreshStoreItem(buyBtn, shadow);
    }

    public void RefreshStoreItem(Button buyBtn, GameObject shadow)
    {
        Debug.Log("刷新商店");
        if(buyBtn != null)
            buyBtn.interactable = true;
        shadow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 鼠标左键按下
        {
            _mouseStartPos = Input.mousePosition;
            _isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))  // 鼠标左键释放
        {
            _isDragging = false;
            DetectSwipe();
        }
    }

    /// <summary>
    /// 滑动屏幕
    /// </summary>
    void DetectSwipe()
    {
        Vector3 mouseEndPos = Input.mousePosition;
        float swipeDistanceX = mouseEndPos.x - _mouseStartPos.x;

        if (swipeDistanceX > 50) // 设置一个阈值来检测滑动
        {
            container.DOAnchorPosX(0, 0.3f);
            isStore = false;
        }
        else if (swipeDistanceX < -50) // 设置一个阈值来检测滑动
        {
            container.DOAnchorPosX(-900, 0.3f);
            isStore = true;
        }
    }

    public void Store()
    {
        isStore = !isStore;
        if(isStore)
            container.DOAnchorPosX(-900, 0.3f);
        else
            container.DOAnchorPosX(0, 0.3f);
    }

    /// <summary>
    /// 滑动条显示进度和百分比
    /// </summary>
    /// <param name="val"></param>
    public void RefreshPrecent(float val)
    {
        slider.value = val;
        int speed = GameManager.Instance.GetAddSpeed();
        sliderPrecent.text = (slider.value * 100).ToString("f0")+"% - 增加的倍数："+speed;
    }

    public void ChangeColor(Color color)
    {
        fill.color = color;
    }

    /// <summary>
    /// 刷新次数显示
    /// </summary>
    /// <param name="count"></param>
    public void RefreshCount(int count)
    {
        endValue = count;
        StartCoroutine(AnimateNumber(startValue, endValue, duration));
        //countTxt.text = "次数：" + count;
    }

    /// <summary>
    /// 刷新倍速显示
    /// </summary>
    /// <param name="speed"></param>
    public void RefreshSpeed(int speed)
    {
        speedTxt.text = "倍速：" + speed;
    }

    /// <summary>
    /// 次数增加的动画
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator AnimateNumber(int start, int end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentage = elapsed / duration;
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(start, end, percentage));
            countTxt.text = "次数：" + currentValue; // 或者 text.text = currentValue.ToString(); 如果你用的是普通的 UI Text
            yield return null;
        }
        countTxt.text = "次数：" + end; // 确保最终值显示正确
        startValue = end;
    }

    /// <summary>
    /// 点击饼干
    /// </summary>
    public void Click()
    {
        GameManager.Instance.AddCount();
        RefreshCount(GameManager.Instance.GetCount());
        GameManager.Instance.ChangEnergy(1);
        RefreshPrecent(GameManager.Instance.GetEnergyPrecent());
        GameObject go = Instantiate(pre);
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.GetComponent<FloatingHealthEffect>().ShowHealth(GameManager.Instance.GetSingleAdd(), parent.position);
    }
}
