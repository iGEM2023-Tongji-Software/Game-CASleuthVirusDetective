// StoveCounterVisual.cs÷–
using UnityEngine;

public class GRNAVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject particlesGameObject;

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
        particlesGameObject.SetActive(showVisual);
    }
}