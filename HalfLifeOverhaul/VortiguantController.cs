using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HalfLifeOverhaul
{
    [RequireComponent(typeof(Animator))]
    public class VortiguantController : MonoBehaviour
    {
        private Animator _animator;
        private GhostController _ghostController;
        private bool _walking = false;
        private VortiguantState _state = VortiguantState.Idle;
        private Material _eyeMaterial;
        private GameObject _lanternSocket;
        
        private void Awake()
        {
            _animator = this.GetRequiredComponent<Animator>();
            _ghostController = this.GetComponentInParent<GhostController>();
            _eyeMaterial = this.GetComponentInChildren<SkinnedMeshRenderer>().materials[4];
            _lanternSocket = this.transform.parent.Find("Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/LanternCarrySocket").gameObject;
            _lanternSocket.transform.parent = transform;
            _lanternSocket.transform.localPosition = new Vector3(0, 28f, 38f);
            _lanternSocket.transform.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }

        enum VortiguantState
        {
            Idle,
            Grab,
            Jibber,
            Jabber,
            Attack,
            Death,
            Run,
            ReachFor,
            LookAround,
            Fear
        }

        private void SetState(VortiguantState state)
        {
            _state = state;
            if (state == VortiguantState.Run)
                _animator.SetBool("Walk", false);
            foreach (VortiguantState s in Enum.GetValues(typeof(VortiguantState)))
            {
                _animator.SetBool(s.ToString(), state == s);
            }
        }

        public void OnChangeAction(GhostAction.Name action)
        {
            MainBehaviour.instance.ModHelper.Console.WriteLine($"ACTION: {action}");
            VortiguantState state;

            switch(action)
            {
                case GhostAction.Name.Grab:
                    state = VortiguantState.Grab;
                    break;
                case GhostAction.Name.Stalk:
                    state = VortiguantState.ReachFor;
                    break;
                case GhostAction.Name.PartyHouse:
                    state = VortiguantState.Jabber;
                    break;
                case GhostAction.Name.IdentifyIntruder:
                    state = VortiguantState.Fear;
                    break;
                case GhostAction.Name.CallForHelp:
                    state = VortiguantState.Jibber;
                    break;
                case GhostAction.Name.Hunt:
                case GhostAction.Name.SearchForIntruder:
                    state = VortiguantState.LookAround;
                    break;
                case GhostAction.Name.Chase:
                    state = VortiguantState.Run;
                    break;
                default:
                    state = VortiguantState.Idle;
                    break;
            }

            SetState(state);
        }

        public void OnDie()
        {
            SetState(VortiguantState.Death);
        }

        public void SetEyeGlow(float glow)
        {
            _eyeMaterial.SetColor("_EmissionColor", new Color(glow, glow, glow));
        }

        private void Update()
        {
            var isMoving = _ghostController.GetSpeed() > 0;
            if(_state != VortiguantState.Run && isMoving != _walking)
            {
                _animator.SetBool("Walk", true);
            }

        }
    }
}
