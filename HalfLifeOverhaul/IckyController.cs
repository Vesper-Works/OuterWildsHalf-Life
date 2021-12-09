using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HalfLifeOverhaul
{
    [RequireComponent(typeof(Animator))]
    public class IckyController : MonoBehaviour
    {
        private AnglerfishController _anglerfishController;
        private Animator _animator;

        private void Awake()
        {
            _anglerfishController = transform.parent.GetRequiredComponent<AnglerfishController>();
            _anglerfishController.OnChangeAnglerState += OnChangeAnglerState;
            _animator = this.GetRequiredComponent<Animator>();
            _animator.SetBool("Lurking", true);
        }

        private void OnDestroy()
        {
            _anglerfishController.OnChangeAnglerState -= OnChangeAnglerState;
        }

        private void OnChangeAnglerState(AnglerfishController.AnglerState state)
        {
            _animator.SetBool("Lurking", state == AnglerfishController.AnglerState.Lurking);
            _animator.SetBool("Investigating", state == AnglerfishController.AnglerState.Investigating);
            _animator.SetBool("Chasing", state == AnglerfishController.AnglerState.Chasing);
            _animator.SetBool("Consuming", state == AnglerfishController.AnglerState.Consuming);
            _animator.SetBool("Stunned", state == AnglerfishController.AnglerState.Stunned);
        }
    }
}
