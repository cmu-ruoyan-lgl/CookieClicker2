using UnityEngine;
using DG.Tweening; // 引入 DoTween 命名空间
using UnityEngine.UI;

public class FloatingHealthEffect : MonoBehaviour
{
    public RectTransform healthText; // 血量 UI
    public float floatDistance = 50f; // 飘浮距离
    public float fadeDuration = 0.5f; // 持续时间
    public Transform target;

    public void ShowHealth(float amount, Vector3 position)
    {
        // 设置血量的文本内容
        healthText.GetComponent<Text>().text = "+" + amount;
        healthText.DOLocalMoveX(Random.Range(-floatDistance, floatDistance), fadeDuration);
        healthText.DOLocalMoveY(Random.Range(floatDistance, floatDistance * 1.5f), fadeDuration).OnComplete(delegate
        {
            Destroy(healthText.gameObject);
        });
    }
}

