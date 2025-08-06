using Godot;

namespace teos.common.system.game_button_controller;

/// <summary>
/// ゲームの入力の連打、押しっぱなしを検出する
/// </summary>
public partial class GameButtonStateMachine : Node
{
    private string _action;
    protected AnimationTree m_AnimationTree;
    protected AnimationNodeStateMachinePlayback m_StateMachine;

    public override void _Ready()
    {
        m_AnimationTree = GetNode<AnimationTree>("AnimationTree");
        m_StateMachine = (AnimationNodeStateMachinePlayback)m_AnimationTree.Get("parameters/playback");
        ClearStateMachine();
        _action = Name;
    }

    public override void _Process(double delta)
    {
        bool press = Input.IsActionPressed(_action);
        m_AnimationTree.Set("parameters/conditions/press", press);
        m_AnimationTree.Set("parameters/conditions/release", !press);
    }

    public GameButtonState GetButtonState()
    {
        return (string)m_StateMachine.GetCurrentNode() switch
        {
            "release" => GameButtonState.Release,
            "long_press" => GameButtonState.LongPress,
            "repeat" => GameButtonState.Repeat,
            "press" or "press_release" => GameButtonState.Unstable,
            _ => GameButtonState.Null,
        };
    }

    private void ClearStateMachine()
    {
        m_AnimationTree.Set("parameters/conditions/press", false);
        m_AnimationTree.Set("parameters/conditions/release", false);
    }

    public void ResetStateMachine()
    {
        ClearStateMachine();
        m_StateMachine.Start("Start");
    }
}
