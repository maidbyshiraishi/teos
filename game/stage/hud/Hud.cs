using Godot;
using teos.common.stage;
using teos.game.weapon;

namespace teos.game.stage.hud;

/// <summary>
/// ステータス表示
/// </summary>
public partial class Hud : CanvasLayer, IGameNode
{
    [ExportGroup("Start Text")]

    [Export]
    public string Text1 { get; set; }

    [Export]
    public string Text2 { get; set; }

    [Export]
    public string Text3 { get; set; }

    private Label _score;
    private TextureProgressBar _life;
    private Label _remain;
    private Label _bullets;
    private Sprite2D _rotation;

    public override void _Ready()
    {
        _score = GetNode<Label>("Score");
        _life = GetNode<TextureProgressBar>("Life");
        _remain = GetNode<Label>("Remain");
        _bullets = GetNode<Label>("Bullets");
        _rotation = GetNode<Sprite2D>("Rotation");
        AddToGroup(IGameNode.GameNodeGroup);
    }

    public void UpdateScore(int score)
    {
        _score.Text = score.ToString("N0");
    }

    public void UpdateLife(int life)
    {
        _life.Value = life;
    }

    public void UpdateRemain(int remain)
    {
        _remain.Text = remain.ToString();
    }

    public void UpdateWeapon(WeaponRoot weapon)
    {
        if (weapon is not null)
        {
            _bullets.Text = weapon.NumOfBullets.ToString();
            _rotation.Frame = weapon.RotationEnabled ? 0 : 1;
        }
        else
        {
            _bullets.Text = "E";
            _rotation.Frame = 1;
        }
    }

    public void ShowMessage()
    {
        GetNode<Label>("Start/Label_1").Text = Text1;
        GetNode<Label>("Start/Label_2").Text = Text2;
        GetNode<Label>("Start/Label_3").Text = Text3;
        GetNode<Control>("Start").Show();
    }

    public void HideMessage()
    {
        GetNode<Control>("Start").Hide();
    }

    public void SetMessage(string text1, string text2, string text3)
    {
        Text1 = string.IsNullOrWhiteSpace(text1) ? "" : text1;
        Text2 = string.IsNullOrWhiteSpace(text2) ? "" : text2;
        Text3 = string.IsNullOrWhiteSpace(text3) ? "" : text3;
    }
}
