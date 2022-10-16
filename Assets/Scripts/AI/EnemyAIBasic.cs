using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBasic : EnemyAI
{
    [SerializeField] private RuntimeSetCardData _enemyDeck;
    [SerializeField] private VariablePlayerCombat _variablePlayer;

    [Title("Broadcasting on")]
    [SerializeField] private ApplyCardEffectEventChannelSO _eventApplyCardEffect;

    public override void PerformActions()
    {
        CardData cardToPlay = _enemyDeck.GetRandomItem();
        if (cardToPlay.AppliesToSelf)
            _eventApplyCardEffect.RaiseEvent(cardToPlay, this);
        else
            _eventApplyCardEffect.RaiseEvent(cardToPlay, _variablePlayer.Value);
        Debug.Log("Action performed!");
    }
}
