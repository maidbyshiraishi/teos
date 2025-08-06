using Godot;

namespace teos.common.system.game_button_controller;

/// <summary>
/// ゲームの入力の連打、押しっぱなしを検出する
/// </summary>
public partial class GameButtonController : Node
{
    [Export]
    public string Action { get; set; }

    private GameButtonStateMachine _a;
    private GameButtonStateMachine _b;

    public override void _Ready()
    {
        _a = GetNode<GameButtonStateMachine>("game_a");
        _b = GetNode<GameButtonStateMachine>("game_b");
    }

    public GameButtonState GetButtonState(string action)
    {
        return action switch
        {
            "game_a" => _a.GetButtonState(),
            "game_b" => _b.GetButtonState(),
            _ => GameButtonState.Null
        };
    }

    public void Reset()
    {
        _a.ResetStateMachine();
        _b.ResetStateMachine();
    }
}
