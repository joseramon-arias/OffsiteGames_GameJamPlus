using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIBasic : EnemyAI
{
    [Space]
    [Title("Broadcasting on")]
    [SerializeField] private ApplyCardEffectEventChannelSO _eventApplyCardEffect;
    [SerializeField] private ApplyAbsorbEffectEventChannelSO _eventApplyAbsorbEffect;

    [Title("Combat")]
    [SerializeField] private RuntimeSetCardData _enemyDeck;
    [SerializeField] private VariablePlayerCombat _variablePlayer;

    public UnityEvent OnAttack;

    public override void PerformActions()
    {
        CardData cardToPlay = _enemyDeck.GetRandomItem();
        if (cardToPlay.AppliesToSelf)
            _eventApplyCardEffect.RaiseEvent(cardToPlay, this);
        else if (cardToPlay.AppliesAbsorb)
        {
            _eventApplyAbsorbEffect.RaiseEvent(cardToPlay, this, _variablePlayer.Value);
            OnAttack.Invoke();
        }
        else
        {
            _eventApplyCardEffect.RaiseEvent(cardToPlay, _variablePlayer.Value);
            OnAttack.Invoke();
        }
    }
}
