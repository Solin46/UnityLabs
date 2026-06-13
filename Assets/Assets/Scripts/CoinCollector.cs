using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private int coinsCount = 0; // переменная хранения монет

    public int CoinsCount => coinsCount; // передаёт значение приватной переменной другим скриптам

    // метод проверки, что столкновение произошло именно с монеткой (тег Coin)
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }
    }

    private void CollectCoin(GameObject coinObject)
    {
        coinsCount++; // увеличение счетчика накопленных монет
        
        Debug.Log($"<color=yellow>Монетка собрана!</color> Всего в кармане: {coinsCount}"); // консольная проверка

        Destroy(coinObject); // удаление объекта монетки со сцены
    }
}