using HalfLifeOverhaul.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HalfLifeOverhaul.Controllers
{
    [RequireComponent(typeof(Animator))]
    public class GmanController : MonoBehaviour
    {
        private Animator _animator;
        private float _timeUntilRandomIdle;
        private float _idleTimer;

        private void Awake()
        {
            _animator = this.GetRequiredComponent<Animator>();
        }

        public enum GmanState
        {
            LookDown,
            LookAround,
            Brush,
            No,
            Yes,
            Idle2,
            Idle3,
            Idle4
        }
        private GmanState[] _idleAnimations = { GmanState.Idle2, GmanState.Idle3, GmanState.Idle4 };

        public void TriggerAnim(GmanState newState)
        {
            _animator.ResetTrigger(newState.ToString());
            _animator.SetTrigger(newState.ToString());
        }

        private void Update()
        {
            _idleTimer += Time.deltaTime;
            if(_idleTimer > _timeUntilRandomIdle)
            {
                _idleTimer -= _timeUntilRandomIdle;
                TriggerAnim(_idleAnimations[(int)Random.Range(0, _idleAnimations.Length)]);
                _timeUntilRandomIdle = Random.Range(5f, 10f);
            }
        }
    }
}
