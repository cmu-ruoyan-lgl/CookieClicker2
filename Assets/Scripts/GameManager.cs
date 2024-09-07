using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //点击的次数
    int count;
    //每次点击加的数量
    internal int speed = 1;
    //滑动条最大值
    public int maxEnergyValue = 10;
    //滑动条当前的值
    float currentEnergyValue;
    //滑动条每秒减的数值
    public float reduceValue = 1;
    //点击加的倍数
    internal int multiple = 1;

    internal float autoClickerTime = 0;
    //是否购买了加倍
    internal bool isUsed;
    // 购买的商店道具编号
    internal int isUsedID = 1;
    //加倍使用时长
    public float usedTime = 10;
    //加倍使用时长计时器
    float usedTimer;
    //各个区间和所加的倍数
    public float maxArea = 0.25f;
    public int addSpeed = 1;
    
    public float maxArea2 = 0.5f;
    public int addSpeed2 = 2;

    public int addSpeed3 = 4;

    public Color color;
    public Color color2;
    public Color color3;

    private void Update()
    {
        //如果开始使用加倍，就开始倒计时
        if (isUsed)
        {
            if (usedTimer < usedTime)
            {
                usedTimer += Time.deltaTime;
                autoClickerTime += Time.deltaTime;
                if(autoClickerTime > 1) {
                    autoClickerTime -= 1;
                    UIManager.Instance.Click();
                }
            }
            else
            {
                isUsed = false;
                usedTimer = 0;
                multiple = 1;
                UIManager.Instance.RefreshSpeed(speed * multiple);
            }
        }
        //如果滑动条的值大于0，开始计时是不是超过设置的秒数没点击了
        if (currentEnergyValue > 0)
        {
            ChangEnergy(-Time.deltaTime * reduceValue);
            UIManager.Instance.RefreshPrecent(GetEnergyPrecent());
        }
        else
        {
            currentEnergyValue = 0;
            UIManager.Instance.RefreshPrecent(0);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 改变滑动条的值，并刷新倍数显示
    /// </summary>
    /// <param name="val"></param>
    public void ChangEnergy(float val)
    {
        currentEnergyValue += val;
        if (currentEnergyValue > maxEnergyValue)
            currentEnergyValue = maxEnergyValue;
        if (currentEnergyValue < 0)
            currentEnergyValue = 0;

        GetAddSpeed();
        UIManager.Instance.RefreshSpeed(speed* multiple);
    }

    public int GetAddSpeed()
    {
        if (GetEnergyPrecent() < maxArea && GetEnergyPrecent() >= 0)
        {
            speed = addSpeed;
            UIManager.Instance.ChangeColor(color);
        }
        else if (GetEnergyPrecent() >= maxArea && GetEnergyPrecent() < maxArea2)
        {
            speed = addSpeed2;
            UIManager.Instance.ChangeColor(color2);
        }
        else if (GetEnergyPrecent() >= maxArea2)
        {
            speed = addSpeed3;
            UIManager.Instance.ChangeColor(color3);
        }
        return speed;
    }

    /// <summary>
    /// 获得滑动条比例
    /// </summary>
    /// <returns></returns>
    public float GetEnergyPrecent()
    {
        return currentEnergyValue / maxEnergyValue;
    }

    /// <summary>
    /// 增加点击次数
    /// </summary>
    public void AddCount()
    {
        count += GetSingleAdd();
    }

    public void SubCount(int cost)
    {
        count -= cost;
    }

    public int GetSingleAdd()
    {
        return speed * multiple;
    }

    /// <summary>
    /// 获得点击次数
    /// </summary>
    /// <returns></returns>
    public int GetCount()
    {
        return count;
    }
}
