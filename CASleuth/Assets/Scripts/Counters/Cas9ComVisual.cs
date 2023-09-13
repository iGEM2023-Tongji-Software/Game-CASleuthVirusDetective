// ContainerCounterVisual.cs÷–
using System;
using UnityEngine;

public class Cas9ComVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    private const string ON_CUT = "OnCutting";

    [SerializeField] private MixCounter containerCounter;
    [SerializeField] private GameObject particlesGameObject;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnObjectPlaced += ContainerCounter_OnPlayerGrabbedObject;
        containerCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }


    private void StoveCounter_OnStateChanged(object sender,MixCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == MixCounter.State.Frying;
        animator.SetBool(ON_CUT, showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
