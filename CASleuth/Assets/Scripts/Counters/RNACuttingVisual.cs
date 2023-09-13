// StoveCounterVisual.cs÷–
using UnityEngine;

public class RNACuttingVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;
    private const string ON_CUT = "OnCutting";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying;
        animator.SetBool(ON_CUT,showVisual);
    }
}