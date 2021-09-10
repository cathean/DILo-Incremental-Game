using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Button ResourceButton;
    public Image ResourceImage;
    public Text ResourceDescription;
    public Text ResourceUpgradeCost;
    public Text ResourceUnlockCost;

    private ResourceConfig _config;

    private int _level = 1;

    public bool isUnlocked { get; private set; }

    public void SetConfig(ResourceConfig config)
    {
        _config = config;

        //ToString("0") berfungsi untuk membuang angka di belakang koma
        ResourceDescription.text = $"{_config.Name} Lv. {_level}\n+{GetOutput().ToString("0")}";
        ResourceUnlockCost.text = $"Unlock Cost\n{_config.UnlockCost}";
        ResourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";

        SetUnlocked(_config.UnlockCost == 0);
    }

    public double GetOutput()
    {
        return _config.Output * _level;
    }

    public double GetUpgradeCost()
    {
        return _config.UpgradeCost * _level;
    }

    public double GetUnlockCost()
    {
        return _config.UnlockCost;
    }

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();
        if(GameManager.Instance.TotalGold < upgradeCost)
        {
            return;
        }

        GameManager.Instance.AddGold(-upgradeCost);
        _level++;

        ResourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        ResourceDescription.text = $"{_config.Name} Lv.{_level}\n+{GetOutput().ToString("0")}";
    }
    
    private void Start()
    {
        ResourceButton.onClick.AddListener(() =>
        {
            if(isUnlocked)
            {
                UpgradeLevel();
            }
            else
            {
                UnlockResource();
            }
        });
    }

    public void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        if(GameManager.Instance.TotalGold < unlockCost)
        {
            return;
        }

        SetUnlocked(true);
        GameManager.Instance.ShowNextResource();
        AchievementController.instance.UnlockAchievement(AchievementController.AchievementType.UnlockResource, _config.Name);
    }

    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
        ResourceImage.color = isUnlocked ? Color.white : Color.gray;
        ResourceUpgradeCost.gameObject.SetActive(unlocked);
    }
}
