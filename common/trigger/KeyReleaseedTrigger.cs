using Godot;
using teos.common.command;
using teos.common.stage;

namespace teos.common.trigger;

/// <summary>
/// キーの開放でコマンドを実行するトリガー
/// </summary>
public partial class KeyReleaseedTrigger : Node
{
    /// <summary>
    /// コマンドを実行するアクション名
    /// </summary>
    [Export]
    public string ActionName { get; set; }

    [Export]
    public Node Target { get; set; }

    private bool _enabled = true;

    public override void _Ready()
    {
        AddToGroup(StageRoot.ProcessGroup);
    }

    public override void _Process(double delta)
    {
        if (_enabled && !string.IsNullOrEmpty(ActionName) && Input.IsActionJustReleased(ActionName))
        {
            CommandRoot.ExecChildren(this, Target, true);
            _enabled = false;
        }
        else if (!_enabled && !Input.IsActionPressed(ActionName))
        {
            // 例えば、ポーズ画面をPキーで閉じると再度ポーズ画面が開くのを回避する
            _enabled = true;
        }
    }
}
