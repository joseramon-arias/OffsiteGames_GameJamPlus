using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private RuntimeSetEnemyAI _currentEnemiesInCombat;
    [SerializeField] private VariableEnemyAI _currentEnemyOnHover;

    [Title("Listening on")]
    [SerializeField] private VoidEventChannelSO _eventPrepareEnemyTurn;
    [SerializeField] private VoidEventChannelSO _eventStartEnemyTurn;
    [SerializeField] private CardEventChannelSO _eventCardSelected;
    [SerializeField] private CardEventChannelSO _eventCardDeselected;
    [SerializeField] private CardEventChannelSO _eventCardEffectHasBeenActivated;

    [Title("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _eventEndEnemyTurn;
    [SerializeField] private ApplyCardEffectEventChannelSO _eventApplyCardEffect;

    private void Awake()
    {
        _eventPrepareEnemyTurn.OnEventRaised += PrepareEnemyTurn;
        _eventStartEnemyTurn.OnEventRaised += RunEnemyTurn;
        _eventCardSelected.OnEventRaised += CardHasBeenSelected;
        _eventCardDeselected.OnEventRaised += CardHasBeenDeselected;
        _eventCardEffectHasBeenActivated.OnEventRaised += CardEffectHasBeenActivated;
    }

    private void PrepareEnemyTurn()
    {
        foreach (EnemyAI ai in _currentEnemiesInCombat.Items)
        {
            if (ai.TryGetComponent(out Health health))
            {
                health.ApplySapplingDamage();
            }
        }
        Debug.Log("Enemies prepared!");
    }

    private void RunEnemyTurn()
    {
        foreach(EnemyAI ai in _currentEnemiesInCombat.Items)
        {
            ai.MaybePerformActions();
        }
        Debug.Log("All Actions performed!");

        EndEnemyTurn();
    }

    private void EndEnemyTurn()
    {
        _eventEndEnemyTurn.RaiseEvent();

        foreach (EnemyAI ai in _currentEnemiesInCombat.Items)
        {
            if (ai.TryGetComponent(out Health health))
            {
                health.MaybeSubtractExposedPoint();
                health.MaybeSubtractProtectedPoints();
            }
        }
    }

    private void CardHasBeenSelected(Card card)
    {
        foreach (EnemyAI ai in _currentEnemiesInCombat.Items)
        {
            ai.OnHoverIsActive = true;
        }
    }

    private void CardHasBeenDeselected(Card card)
    {
        foreach (EnemyAI ai in _currentEnemiesInCombat.Items)
        {
            ai.OnHoverIsActive = false;
        }
    }

    private void CardEffectHasBeenActivated(Card card)
    {
        if (!card.Data.TargetsAll)
            _eventApplyCardEffect.RaiseEvent(card.Data, _currentEnemyOnHover.Value);
        else
        {
            foreach (EnemyAI ai in _currentEnemiesInCombat.Items)
            {
                _eventApplyCardEffect.RaiseEvent(card.Data, ai);
            }
        }
    }
}
