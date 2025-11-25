using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public Tower tower;

    [Header("Upgrade Buttons")]
    public Button fireRateButton;
    public Button rangeButton;
    public Button timeRewardButton;
    public Button damageButton;  

    [Header("Upgrade Texts")]
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI timeRewardText;
    public TextMeshProUGUI damageText; 

    [Header("Upgrade Costs")]
    public int fireRateCost = 5;
    public int rangeCost = 5;
    public int timeRewardCost = 5;
    public int damageCost = 10; 

    private void Start()
    {
        fireRateButton.onClick.AddListener(UpgradeFireRate);
        rangeButton.onClick.AddListener(UpgradeRange);
        timeRewardButton.onClick.AddListener(UpgradeTimeReward);
        damageButton.onClick.AddListener(UpgradeDamage); 
        UpdateUI();
    }

    void UpgradeFireRate()
    {
        if (GameManager.Instance.SpendPoints(fireRateCost))
        {
            tower.fireRate -= 0.2f;
            fireRateCost += 10;
            UpdateUI();
        }
    }

    void UpgradeRange()
    {
        if (GameManager.Instance.SpendPoints(rangeCost))
        {
            tower.range += 0.2f;
            rangeCost += 10;
            UpdateUI();
        }
    }

    void UpgradeTimeReward()
    {
        if (GameManager.Instance.SpendPoints(timeRewardCost))
        {
            GameManager.Instance.timePerKill += 0.25f;
            timeRewardCost += 10;
            UpdateUI();
        }
    }

    void UpgradeDamage() // NEW
    {
        if (GameManager.Instance.SpendPoints(damageCost))
        {
            tower.damage += 0.5f; // Increase tower’s bullet damage
            damageCost += 10;   // Cost rises by 10 each time
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        fireRateText.text = $"Fire Rate ({fireRateCost})";
        rangeText.text = $"Range ({rangeCost})";
        timeRewardText.text = $"Time Reward ({timeRewardCost})";
        damageText.text = $"Damage ({damageCost})"; // NEW
    }
}
