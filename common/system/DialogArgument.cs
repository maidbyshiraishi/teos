using Godot;
using Godot.Collections;

namespace teos.common.system;

/// <summary>
/// ダイアログ引数制御
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// </summary>
public partial class DialogArgument : Node
{
    private readonly Dictionary<string, Array<Variant>> dictinary = [];

    public void SetArgument(string key, Array<Variant> argument)
    {
        if (string.IsNullOrWhiteSpace(key) || argument == null || dictinary.ContainsKey(key))
        {
            return;
        }

        dictinary.Add(key, argument);
    }

    public Array<Variant> GetArgument(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || !dictinary.TryGetValue(key, out Array<Variant> argument))
        {
            return [];
        }

        _ = dictinary.Remove(key);
        return argument;
    }
}
