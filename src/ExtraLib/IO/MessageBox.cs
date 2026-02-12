namespace san40_u5an40.ExtraLib.IO;

/// <summary>
/// Окно с сообщением
/// </summary>
public static class MessageBox
{
    [DllImport("user32.dll", EntryPoint = "MessageBoxW", CharSet = CharSet.Unicode)]
    private static extern int Show(IntPtr hWnd, string text, string caption, uint type);

    /// <summary>
    /// Метод показа окна с сообщением
    /// </summary>
    /// <param name="text">Текст, выводимый в окне</param>
    /// <param name="caption">Заголовок окна</param>
    /// <param name="type">Настройка параметров окна</param>
    /// <returns>Значение, выбранное пользователем</returns>
    public static int Show(string text, string caption, MessageBoxType type = 0x0000_0000) =>
        Show(IntPtr.Zero, text, caption, (uint)type);
}

/// <summary>
/// Битовое поле, хранящее настройки для окна с сообщением
/// </summary>
[Flags]
public enum MessageBoxType : uint
{
    Window_Ok = 0x0000_0000,
    Window_OkCancel = 0x0000_0001,
    Window_AbortRetryIgnore = 0x0000_0002,
    Window_YesNoCancel = 0x0000_0003,
    Window_YesNo = 0x0000_0004,
    Window_RetryCancel = 0x0000_0005,
    Window_CancelTryContinue = 0x0000_0006,

    Icon_Error = 0x0000_0010,
    Icon_Question = 0x000_00020,
    Icon_Warning = 0x0000_0030,
    Icon_Information = 0x0000_0040,

    DefaultButton_1 = 0x0000_0000,
    DefaultButton_2 = 0x0000_0100,
    DefaultButton_3 = 0x0000_0200,
    DefaultButton_4 = 0x0000_0300,
}

/// <summary>
/// Ввод, указанный пользователем в окне
/// </summary>
public static class MessageBoxResult
{
    public const int Ok = 1;
    public const int Cancel = 2;
    public const int Abort = 3;
    public const int Retry = 4;
    public const int Ignore = 5;
    public const int Yes = 6;
    public const int No = 7;
    public const int Close = 8;
    public const int Help = 9;
    public const int Try = 1;
    public const int Continue = 1;
}