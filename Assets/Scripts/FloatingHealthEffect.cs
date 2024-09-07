using UnityEngine;
using DG.Tweening; // ���� DoTween �����ռ�
using UnityEngine.UI;

public class FloatingHealthEffect : MonoBehaviour
{
    public RectTransform healthText; // Ѫ�� UI
    public float floatDistance = 50f; // Ʈ������
    public float fadeDuration = 0.5f; // ����ʱ��
    public Transform target;

    public void ShowHealth(float amount, Vector3 position)
    {
        // ����Ѫ�����ı�����
        healthText.GetComponent<Text>().text = "+" + amount;
        healthText.DOLocalMoveX(Random.Range(-floatDistance, floatDistance), fadeDuration);
        healthText.DOLocalMoveY(Random.Range(floatDistance, floatDistance * 1.5f), fadeDuration).OnComplete(delegate
        {
            Destroy(healthText.gameObject);
        });
    }
}

