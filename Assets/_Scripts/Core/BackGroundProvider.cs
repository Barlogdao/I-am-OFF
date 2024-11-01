using System;
using System.Linq;
using UnityEngine;

public class BackGroundProvider : MonoBehaviour
{
    [SerializeField] private BackgroundSheet _backgroundSheet;

    public static Action BackgroundChanged;
    public static BackGroundProvider Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public BackgroundSO[] GetAllBackgrounds()
    {
        return _backgroundSheet.Backgrounds
            .OrderBy(x => x.EarnType)
            .ThenBy(y => y.CoinCost)
            .ThenBy(z => z.ID)
            .ToArray();
    }

    public Sprite GetBackground(int id)
    {
        return _backgroundSheet.Backgrounds.First(x => x.ID == id).Image;
    }
}
