using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "雷击效果", menuName = "Data/Item Effect/Thunder Strike")]

public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab,_enemyPosition.position,Quaternion.identity);

        Destroy(newThunderStrike, .5f);
    }
}
