using UnityEngine;
using UnityEngine.UI;

public class WeaponIconUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Color normalColor = Color.gray;
    [SerializeField] Color selectedColor = Color.white;

    public void Setup(Sprite icon)
    {
        iconImage.sprite = icon;
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        iconImage.color = selected ? selectedColor : normalColor;
        transform.localScale = selected ? Vector3.one * 1.2f : Vector3.one;
    }
}
