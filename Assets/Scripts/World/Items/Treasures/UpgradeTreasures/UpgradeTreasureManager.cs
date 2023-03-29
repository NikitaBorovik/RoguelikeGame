using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTreasureManager : MonoBehaviour
{
    #region Fields
    private List<BaseUpgradeTreasure> upgrades;
    private IUpgradable upgradableEntity;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        upgrades = new List<BaseUpgradeTreasure>();
        upgradableEntity = GetComponent<IUpgradable>();
    }

    private void Update()
    {
        UpdateAll();
    }
    #endregion

    #region Custom Methods
    public void EnableAll() => upgrades.ForEach(u => upgradableEntity.EnableUpgrade(u));

    public void UpdateAll() => upgrades.ForEach(u => upgradableEntity.UpdateUpgrade(u));

    public void DisableAll() => upgrades.ForEach(u => upgradableEntity.DisableUpgrade(u));

    public void AddUpgrade(BaseUpgradeTreasure upgrade)
    {
        upgrades.Add(upgrade);
        upgradableEntity.EnableUpgrade(upgrade);
    }
    #endregion
}
