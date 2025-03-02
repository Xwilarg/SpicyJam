using UnityEngine;

namespace InflationPotion.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        public float PlayerSpeed;
        public float AttackRange = .75f;
        public float AttackSize = .6f;
    }
}