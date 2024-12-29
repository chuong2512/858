using UnityEngine;
using TMPro;
using DG.Tweening;
using NaughtyAttributes;


namespace GPHive.Game
{
    public class PlayerEconomy : Singleton<PlayerEconomy>
    {
        [SerializeField] private TextMeshProUGUI currencyText;

        public int GetMoney() => PlayerPrefs.GetInt("Player Currency");
        private void SetMoney(int amount) => PlayerPrefs.SetInt("Player Currency", amount);

        [Button]
        private void Add10Coin() => AddMoney(10);
        [Button]
        private void Add100Coin() => AddMoney(100);
        [Button]
        private void Add1000Coin() => AddMoney(1000);

        private void Start()
        {
            currencyText.text = GetMoney().ToString();
        }


        /// <summary>
        /// Returns true if player have enough currency.
        /// </summary>
        /// <param name="spendAmount">Currency amount to spend</param>
        /// <returns></returns>
        public bool SpendMoney(int spendAmount)
        {
            if (GetMoney() < spendAmount) return false;
            else
            {
                var _oldMoney = GetMoney();
                SetMoney(GetMoney() - spendAmount);
                DOTween.To(() => _oldMoney, x => _oldMoney = x, GetMoney(), 1).OnUpdate(() => currencyText.text = _oldMoney.ToString());
                return true;
            }
        }

        public void AddMoney(int amount)
        {
            var _oldMoney = GetMoney();
            SetMoney(GetMoney() + amount);
            DOTween.To(() => _oldMoney, x => _oldMoney = x, GetMoney(), 1).OnUpdate(() => currencyText.text = _oldMoney.ToString());
        }

        public bool CheckEnoughMoney(int amount)
        {
            return GetMoney() >= amount;
        }
    }
}