using Godot;

namespace teos.common.screen;

/// <summary>
/// エラーダイアログ
/// </summary>
public partial class ErrorDialog : DialogRoot
{
    /// <summary>
    /// 引数を設定する
    /// </summary>
    /// <param name="argument">引数</param>
    public override void GetArgument()
    {
        GetDialogArgument("ErrorDialog");

        if (m_Argument is not null && m_Argument.Count == 1 && m_Argument[0].VariantType is Variant.Type.String)
        {
            SetMessage(m_Argument[0].AsString());
        }
    }

    /// <summary>
    /// メッセージを設定する
    /// </summary>
    /// <param name="message">エラーメッセージ</param>
    public void SetMessage(string message)
    {
        GetNode<TextEdit>("Control/Message").Text = message;
    }
}
