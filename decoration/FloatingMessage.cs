using Godot;
using teos.system;

namespace teos.decoration;

/// <summary>
/// フローティングメッセージ
/// </summary>
public partial class FloatingMessage : Node2D
{
    [Export]
    public string Text { get; set; } = "text";

    [Export]
    public Color Color { get; set; } = Colors.White;

    [Export]
    public string SeName { get; set; }

    public override void _Ready()
    {
        GetNode<Label>("Label").Text = Text;
        GetNode<Label>("Label").SelfModulate = Color;
        GetNode<SePlayer>("/root/SePlayer").Play(SeName);

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        AnimationPlayer player = GetNode<AnimationPlayer>("AnimationPlayer");
        _ = player.Connect(AnimationMixer.SignalName.AnimationFinished, new(this, MethodName.AnimationFinished));
        player.Play("floating_message");
    }

    public void AnimationFinished(StringName animName)
    {
        QueueFree();
    }
}
