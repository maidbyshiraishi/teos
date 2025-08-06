using Godot;

namespace teos.common.command;

/// <summary>
/// システムのファイルマネージャで指定フォルダを開くコマンド
/// </summary>
public partial class OpenFileManagerCommand : CommandRoot
{
    /// <summary>
    /// 開くフォルダ
    /// </summary>
    [Export]
    public string Path { get; set; } = "user://";

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        string path = ProjectSettings.GlobalizePath(Path);

        if (DirAccess.DirExistsAbsolute(path))
        {
            _ = OS.ShellShowInFileManager(path);
        }
    }
}
