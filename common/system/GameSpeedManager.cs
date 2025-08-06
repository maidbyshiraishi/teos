using Godot;
using Godot.Collections;
using teos.common.command;
using teos.common.stage;
using teos.common.trigger;

namespace teos.common.system;

/// <summary>
/// グループ内のノード数に応じてコマンドを実行するトリガー
/// </summary>
public partial class GameSpeedManager : ProcessTriggerRoot
{
    public static readonly string GameSpeedManageGroup = "GameSpeedManageGroup";

    /// <summary>
    /// 監視するグループ
    /// </summary>
    [Export]
    public string GroupName { get; set; } = GameSpeedManageGroup;

    private Array<int> _count = [];
    private Dictionary<string, Node> _container2 = [];
    private Label _label;

    public override void _Ready()
    {
        base._Ready();
        _label = GetNodeOrNull<Label>("CanvasLayer/Value");
        AddToGroup(IGameNode.GameNodeGroup);
        Array<int> temp = [];

        foreach (Node node in GetChildren())
        {
            if (int.TryParse(node.Name, out int value))
            {
                temp.Add(value);
            }
        }

        temp.Sort();
        temp.Reverse();

        foreach (int value in temp)
        {
            string name = value.ToString();
            if (GetNodeOrNull(name) is Node node)
            {
                _count.Add(value);
                _container2.Add(name, node);
            }
        }
    }

    public override void _Process(double delta)
    {
        if (string.IsNullOrWhiteSpace(GroupName))
        {
            return;
        }

        Array<Node> group = GetTree().GetNodesInGroup(GroupName);
        int count = group.Count;

        foreach (int value in _count)
        {
            if (value <= count)
            {
                string name = value.ToString();

                if (_container2.ContainsKey(name))
                {
                    CommandRoot.ExecChildren(_container2[name], Target is null ? this : Target, true);

                    if (_label is not null)
                    {
                        _label.Text = $"{100 * GetNode<ChangeGameSpeedCommand>($"{name}/ChangeGameSpeedCommand").Scale}%";
                    }

                    break;
                }
            }
        }
    }

    public override void _ExitTree()
    {
        Engine.TimeScale = 1.0f;
        base._ExitTree();
    }
}
