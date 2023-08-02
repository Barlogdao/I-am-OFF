using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization.Scripts;

public class RecipeComboVisual : MonoBehaviour
{

    [SerializeField] private Image _coctailImage;
    [SerializeField] private TextMeshProUGUI _coctailName, _bonusScore, _bonusTime;
    [SerializeField] private Transform _comboblock;
    [SerializeField] Image _imagePref;
    [SerializeField] GameObject _lock;
    [SerializeField] Image _lockedImage;
    private CoctailRecipeSO _coctail;
    public void Init(CoctailRecipeSO cocktail)
    {
        _coctail = cocktail;
        _coctailImage.sprite = cocktail.Image;
        _coctailName.text = LocalizationManager.Localize($"Cocktail.{cocktail.Name}");
        _bonusScore.text = cocktail.BonusScore.ToString();
        _bonusTime.text = cocktail.BonusTime.ToString();
        foreach(var drink in cocktail.Recipe)
        {
            Instantiate(_imagePref,_comboblock).sprite = drink.Image;
            
        }
        _lock.SetActive(true);
        for (int i = 0; i < _coctail.CoinCost; i++)
        {
            Instantiate(_lockedImage, _lock.transform);
        }
        //if (SaveProvider.Instace.SaveData.UnlockedRecipies.Contains(coctail.Name))
        //{
        //    _lock.SetActive(false);
        //}

        if (SaveProvider.Instace.SaveData.RecipeIsUnlocked(cocktail))
        {
            _lock.SetActive(false);
        }
    }

    public void UnlockRecipe()
    {
        if(!SaveProvider.Instace.SaveData.RecipeIsUnlocked(_coctail)
            && SaveProvider.Instace.SaveData.PlayerCoins >= _coctail.CoinCost)
        {
            _lock.SetActive(false);
            SaveProvider.Instace.SaveData.UnlockRecipe(_coctail);
            SaveProvider.Instace.SpendCoins(_coctail.CoinCost);
        }
    }
}