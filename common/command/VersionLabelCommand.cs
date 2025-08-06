using Godot;

namespace teos.common.command;

/// <summary>
/// ラベルの文字にバージョンを表示するコマンド
/// </summary>
public partial class VersionLabelCommand : CommandRoot
{
    /// <summary>
    /// バージョンを表示するラベル
    /// </summary>
    [Export]
    public Label VersionLabel { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || VersionLabel is null)
        {
            return;
        }

        VersionLabel.Text = ProjectSettings.GetSetting("application/config/version").AsString();
    }
}
