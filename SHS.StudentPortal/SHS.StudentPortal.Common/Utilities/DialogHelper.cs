using Radzen;

namespace SHS.StudentPortal.Common.Utilities;

public class DialogHelper
{
    public static ConfirmOptions DefaultConfirmOptions => new ConfirmOptions()
    {
        CloseDialogOnEsc = false,
        CloseDialogOnOverlayClick = false,
        ShowClose = false,
        Draggable = false,
        OkButtonText = "Ok",
        CancelButtonText = "Cancel"
    };

    public static DialogOptions DefaultWideDialogOptions => new DialogOptions()
    {
        CloseDialogOnEsc = false,
        CloseDialogOnOverlayClick = false,
        ShowClose = false,
        Draggable = false,
        Height = "90%",
        Width = "90%"
    };

    public static DialogOptions DefaultMediumDialogOptions => new DialogOptions()
    {
        CloseDialogOnEsc = false,
        CloseDialogOnOverlayClick = false,
        ShowClose = false,
        Draggable = false,
        Height = "auto",
        Width = "90%"
    };

    public static DialogOptions DefaultSmallDialogOptions => new DialogOptions()
    {
        CloseDialogOnEsc = false,
        CloseDialogOnOverlayClick = false,
        ShowClose = false,
        Draggable = false,
        Height = "auto",
        Width = "300px"
    };
}
