using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCoctail", menuName = "_drinkData/Coctail")]
public class CoctailRecipeSO : ScriptableObject
{
    public Sprite Image;
    public string Name;
    public string Description;
    public List<DrinkSO> Recipe;
    public int BonusScore;
    public int BonusTime;

    public bool StomachHasRecipe(List<DrinkSO> stomach)
    {
        if (Recipe.Count != stomach.Count)
        {
            return false;
        }
        for (int i = 0; i < Recipe.Count; i++)
        {
            if (Recipe[i] != stomach[i])
            {
                return false;
            }
        }
        return true;
    }

}
