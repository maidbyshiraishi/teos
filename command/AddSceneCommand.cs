using Godot;
using teos.stage;
using teos.system;

namespace teos.command;

/// <summary>
/// シーンを生成するコマンド
/// </summary>
public partial class AddSceneCommand : CommandRoot
{
    [Signal]
    public delegate void SceneAddedEventHandler(Node node, string parentNodePath);

    [Signal]
    public delegate void SceneToNodeAddedEventHandler(Node node, Node parentNode);

    /// <summary>
    /// 生成するシーン
    /// </summary>
    [Export]
    public PackedScene Scene { get; set; }

    /// <summary>
    /// 生成するノードの親ノード
    /// </summary>
    [Export]
    public Node ParentNode { get; set; }

    /// <summary>
    /// 生成するノードの位置の基準ノード
    /// </summary>
    [Export]
    public Node2D PositionNode { get; set; }

    private GameStageRoot _gameStageRoot;

    public override void _Ready()
    {
        _gameStageRoot = GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot();
        SceneAdded += _gameStageRoot.AddScene;
        SceneToNodeAdded += _gameStageRoot.AddSceneToNode;
    }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        AddScene();
    }

    public void AddScene()
    {
        if (Scene is not null && Scene.Instantiate() is Node scene)
        {
            if (scene is Node2D node2d && PositionNode is not null)
            {
                node2d.GlobalPosition = PositionNode.GlobalPosition;
            }

            _ = ParentNode is null
                ? EmitSignal(SignalName.SceneToNodeAdded, [scene, this])
                : EmitSignal(SignalName.SceneToNodeAdded, [scene, ParentNode]);
        }
    }
}
