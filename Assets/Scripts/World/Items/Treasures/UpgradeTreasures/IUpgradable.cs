using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    void EnableUpgrade(BaseUpgradeTreasure upgrade);
    void UpdateUpgrade(BaseUpgradeTreasure upgrade);
    void DisableUpgrade(BaseUpgradeTreasure upgrade);
}
