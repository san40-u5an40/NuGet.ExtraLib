namespace san40_u5an40.ExtraLib.IO;

/// <summary>
/// Окно с сообщением
/// </summary>
[SupportedOSPlatform("Windows")]
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

    /// <summary>
    /// Метод показа окна с сообщением
    /// </summary>
    /// <param name="text">Текст, выводимый в окне</param>
    /// <param name="caption">Заголовок окна</param>
    /// <param name="options">Настройка параметров окна</param>
    /// <returns>Значение, выбранное пользователем</returns>
    public static int Show(string text, string caption, MessageBoxOptions options) =>
        Show(IntPtr.Zero, text, caption, (uint)options.WindowType | (uint)options.IconType | (uint)options.DefaultButton);
}

/// <summary>
/// Битовое поле, хранящее настройки для окна с сообщением
/// </summary>
[SupportedOSPlatform("Windows")]
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
/// Опции окна уведомлений
/// </summary>
/// <param name="WindowType">Тип окна уведомлений</param>
/// <param name="IconType">Тип иконки</param>
/// <param name="DefaultButton">Изначально выбранная кнопка</param>
[SupportedOSPlatform("Windows")]
public record MessageBoxOptions(
    WindowType WindowType = WindowType.Ok,
    IconType IconType = IconType.None,
    DefaultButton DefaultButton = DefaultButton.Button1
);

/// <summary>
/// Тип окна
/// </summary>
[SupportedOSPlatform("Windows")]
public enum WindowType : uint
{
    Ok = 0x0000_0000,
    OkCancel = 0x0000_0001,
    AbortRetryIgnore = 0x0000_0002,
    YesNoCancel = 0x0000_0003,
    YesNo = 0x0000_0004,
    RetryCancel = 0x0000_0005,
    CancelTryContinue = 0x0000_0006,
}

/// <summary>
/// Тип иконки у окна
/// </summary>
[SupportedOSPlatform("Windows")]
public enum IconType : uint
{
    None = 0x0000_0000,
    Error = 0x0000_0010,
    Question = 0x000_00020,
    Warning = 0x0000_0030,
    Information = 0x0000_0040,
}

/// <summary>
/// Изначально выбранная кнопка у окна
/// </summary>
[SupportedOSPlatform("Windows")]
public enum DefaultButton : uint
{
    Button1 = 0x0000_0000,
    Button2 = 0x0000_0100,
    Button3 = 0x0000_0200,
    Button4 = 0x0000_0300,
}

/// <summary>
/// Ввод, указанный пользователем в окне
/// </summary>
[SupportedOSPlatform("Windows")]
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
