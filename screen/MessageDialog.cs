using Godot;
using maid_by_shiraishi.trigger;

namespace maid_by_shiraishi.screen;

/// <summary>
/// メッセージダイアログ
/// </summary>
public partial class MessageDialog : DialogRoot
{
    public override void GetArgument()
    {
        GetDialogArgument("MessageDialog");

        if (m_Argument is null)
        {
            return;
        }

        // 引数はメッセージのみ、あるいはメッセージとESCキーの有効・無効の2つ
        if (m_Argument.Count == 1 && m_Argument[0].VariantType is Variant.Type.String)
        {
            SetMessage(m_Argument[0].AsString());
        }
        else if (m_Argument.Count == 2 && m_Argument[0].VariantType is Variant.Type.String && m_Argument[1].VariantType is Variant.Type.Color)
        {
            SetMessage(m_Argument[0].AsString());
            GetNode<Label>("Message").Modulate = m_Argument[1].AsColor();
        }
        else if (m_Argument.Count == 3 && m_Argument[0].VariantType is Variant.Type.String && m_Argument[1].VariantType is Variant.Type.Color && m_Argument[2].VariantType is Variant.Type.Bool)
        {
            SetMessage(m_Argument[0].AsString());
            GetNode<Label>("Message").Modulate = m_Argument[1].AsColor();

            if (!m_Argument[2].AsBool())
            {
                GetNode<KeyReleaseedTrigger>("EscapeKey").ActionName = null;
            }
        }
    }

    public void SetMessage(string message) => GetNode<Label>("Message").Text = message;

    protected override string GetDefaultFocusNodeName() => "Back";
}
