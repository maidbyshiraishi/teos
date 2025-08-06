using Godot;
using Godot.Collections;
using teos.common.command;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// 条件式コマンド
/// </summary>
public partial class ConditionalCommand : CommandRoot
{
    /// <summary>
    /// 条件式
    /// </summary>
    [Export]
    public string ConditionalExpression { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        ExecChildren(this, node, Calc());
    }

    protected bool Calc()
    {
        try
        {
            // 変数はGameDataのキーと値を使用する
            GetNode<GameDataManager>("/root/GameDataManager").GetKeysAndValues(out string[] keys, out Array values);
            // 式を評価する
            Expression exp = new();

            if (exp.Parse(ConditionalExpression, keys) is not Error.Ok)
            {
                return false;
            }

            Variant variant = exp.Execute(values, null, false);
            return variant.VariantType is Variant.Type.Bool && variant.AsBool();
        }
        catch
        {
        }

        return false;
    }
}
