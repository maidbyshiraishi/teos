using Godot;
using Godot.Collections;

namespace teos.game.system;

/// <summary>
/// キー設定
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// </summary>
public partial class GameKeyOption : Node
{
    [Export]
    public bool Reverse
    {
        get => _reverse;

        set
        {
            _reverse = value;
            SetStick();
        }
    }

    [Export]
    public double TargetSensitivity { get; set; } = 1.0f;

    [Export]
    public double MouseSensitivity { get; set; } = 1.0f;

    private bool _reverse;
    private static readonly string KeyOptionFilePath = "user://key_options.dat";

    public override void _Ready()
    {
        LoadKeyOptions();
    }

    public void ResetDefaultKeyOptions()
    {
        _reverse = false;
        InputMap.LoadFromProjectSettings();
        TargetSensitivity = 1.0f;
        MouseSensitivity = 1.0f;
    }

    public void LoadKeyOptions()
    {
        ConfigFile keyOptions = new();
        Error e = keyOptions.Load(KeyOptionFilePath);

        if (e is Error.FileNotFound)
        {
            ResetDefaultKeyOptions();
            return;
        }
        else if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{KeyOptionFilePath}を読み込めませんでした。エラーコードは{e}です。ゲームのデフォルト値を使用します。");
            ResetDefaultKeyOptions();
            return;
        }

        Reverse = keyOptions.GetValue("KeyOption", "Reverse", false).AsBool();
        TargetSensitivity = keyOptions.GetValue("KeyOption", "TargetSensitivity", 1.0f).AsDouble();
        MouseSensitivity = keyOptions.GetValue("KeyOption", "MouseSensitivity", 1.0f).AsDouble();
    }


    public void SaveKeyOptions()
    {
        ConfigFile keyOptions = new();
        keyOptions.SetValue("KeyOption", "Reverse", Reverse);
        keyOptions.SetValue("KeyOption", "TargetSensitivity", Mathf.Clamp(TargetSensitivity, 0.1f, 3f));
        keyOptions.SetValue("KeyOption", "MouseSensitivity", Mathf.Clamp(MouseSensitivity, 0.1f, 3f));
        Error e = keyOptions.Save(KeyOptionFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"キー設定の保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
        }
    }

    private void SetStick()
    {
        if (Reverse)
        {
            CopyAction("reverse_game_up", "game_up");
            CopyAction("reverse_game_down", "game_down");
            CopyAction("reverse_game_left", "game_left");
            CopyAction("reverse_game_right", "game_right");
            CopyAction("reverse_game_target_up", "game_target_up");
            CopyAction("reverse_game_target_down", "game_target_down");
            CopyAction("reverse_game_target_left", "game_target_left");
            CopyAction("reverse_game_target_right", "game_target_right");
        }
        else
        {
            CopyAction("normal_game_up", "game_up");
            CopyAction("normal_game_down", "game_down");
            CopyAction("normal_game_left", "game_left");
            CopyAction("normal_game_right", "game_right");
            CopyAction("normal_game_target_up", "game_target_up");
            CopyAction("normal_game_target_down", "game_target_down");
            CopyAction("normal_game_target_left", "game_target_left");
            CopyAction("normal_game_target_right", "game_target_right");
        }
    }

    private void CopyAction(string actionNameFrom, string actionNameTo)
    {
        Array<InputEvent> action = GetAllAction(actionNameFrom);
        InputMap.EraseAction(actionNameTo);
        InputMap.AddAction(actionNameTo);
        AddAllAction(actionNameTo, action);
    }

    private static void AddAllAction(string actionName, Array<InputEvent> actions)
    {
        foreach (InputEvent action in actions)
        {
            InputMap.ActionAddEvent(actionName, action);
        }
    }

    private static Array<InputEvent> GetAllAction(string actionName)
    {
        Array<InputEvent> actions = [];
        Array<InputEvent> actionEvents = InputMap.ActionGetEvents(actionName);

        foreach (InputEvent action in actionEvents)
        {
            actions.Add(action);
        }

        return actions;
    }
}
