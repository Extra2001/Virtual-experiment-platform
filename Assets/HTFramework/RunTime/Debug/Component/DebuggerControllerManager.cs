using UnityEngine;

namespace HT.Framework
{
    [CustomDebugger(typeof(ControllerManager))]
    internal sealed class DebuggerControllerManager : DebuggerComponentBase
    {
        private ControllerManager _target;

        public override void OnEnable()
        {
            _target = Target as ControllerManager;
        }

        public override void OnDebuggerGUI()
        {
            GUILayout.BeginHorizontal();
            _target.IsEnableBounds = GUILayout.Toggle(_target.IsEnableBounds, "Is Enable Bounds");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Control Mode: ", GUILayout.Width(100));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _target.TheControlMode = (ControlMode)EnumField(_target.TheControlMode);
            GUILayout.EndHorizontal();
        }
    }
}