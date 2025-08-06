using Godot;
using teos.common.trigger;

namespace teos.common.screen;

/// <summary>
/// メッセージダイアログ
/// </summary>
public partial class MessageDialog : DialogRoot
{
    public override void GetArgument()
    {
        GetDialogArgument("MessageDialog");

        // 引数はメッセージのみ、あるいはメッセージとESCキーの有効・無効の2つ
        if (m_Argument is not null && m_Argument.Count == 1 && m_Argument[0].VariantType is Variant.Type.String)
        {
            SetMessage(m_Argument[0].AsString());
        }
        else if (m_Argument is not null && m_Argument.Count == 2 && m_Argument[0].VariantType is Variant.Type.String && m_Argument[1].VariantType is Variant.Type.Bool)
        {
            SetMessage(m_Argument[0].AsString());

            if (!m_Argument[1].AsBool())
            {
                GetNode<KeyReleaseedTrigger>("EscapeKey").ActionName = null;
            }
        }
    }

    public void SetMessage(string message)
    {
        GetNode<Label>("Control/Message").Text = message;
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
