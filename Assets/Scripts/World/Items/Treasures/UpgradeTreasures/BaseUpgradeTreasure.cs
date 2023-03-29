using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgradeTreasure : ScriptableObject
{
    public bool IsEnabled { get; private set; } = false;
    public Sprite sprite;

    #region Default Visitor Methods
    public void Enable(IUpgradable upgradable)
    {
        Debug.LogWarning($"There's no method-visitor taking: {upgradable.GetType().Name} param. ");
    }
    public void UpdateUpgrade(IUpgradable upgradable)
    {
        Debug.LogWarning($"There's no method-visitor taking: {upgradable.GetType().Name} param. ");
    }

    public void Disable(IUpgradable upgradable)
    {
        Debug.LogWarning($"There's no method-visitor taking: {upgradable.GetType().Name} param. ");
    }
    #endregion

    #region Template Methods
    public void Enable(Player upgradable)
    {
        if (IsEnabled) return;
        IsEnabled = true;
        Upgrade(upgradable);
    }
    public void UpdateUpgrade(Player upgradable)
    {
        if (!IsEnabled) return;
        UpdateIfEnabled(upgradable);
    }

    public void Disable(Player upgradable)
    {
        if (!IsEnabled) return;
        IsEnabled = false;
        Degrade(upgradable);
    }
    #endregion

    #region Abstract Methods
    protected abstract void Upgrade(Player upgradable);

    protected abstract void UpdateIfEnabled(Player upgradable);

    protected abstract void Degrade(Player upgradable);
    #endregion
}
