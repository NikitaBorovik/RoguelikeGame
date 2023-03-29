using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField]
    private Image weaponPicture;

    public void SetWeaponPicture(Sprite weaponSprite)
    {
        weaponPicture.sprite = weaponSprite;
    }
}
