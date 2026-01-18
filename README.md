# san40_u5an40.ExtraLib
В данном NuGet-пакете представлена библиотека классов, которая была сформирована в ходе моего обучения разработке на .NET/C#. Какие-то классы могут быть полезны в широком круге задач, какие-то специфичны для консольной разработки или же конкретной задачи приложения.

## Оглавление:
Полезны в широком круге задач (namespace san40_u5an40.ExtraLib.Broad):
- [Bytes](https://github.com/san40-u5an40/NuGet.ExtraLib/tree/main?tab=readme-ov-file#bytes)
- [Comparator](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#comparator)
- [Counter](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#counter)
- [MessageBox](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#messagebox)
- [Regexes](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#regexes)
- [StringExtension](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#stringextension)
- [TimerHelper](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#timerhelper)

Паттерновые классы, также полезные в широком круге задач (namespace san40_u5an40.ExtraLib.Broad.Patterns):
- [Result](https://github.com/san40-u5an40/NuGet.ExtraLib/tree/main?tab=readme-ov-file#result)
- [Chain](https://github.com/san40-u5an40/NuGet.ExtraLib/tree/main?tab=readme-ov-file#chain)
- [IValidable](https://github.com/san40-u5an40/NuGet.ExtraLib/tree/main?tab=readme-ov-file#ivalidable)

Полезны при консольной разработке (namespace san40_u5an40.ExtraLib.ConsoleApp):
- [ConsoleExtension](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#consoleextension)

Для специфичных задач (namespace san40_u5an40.ExtraLib.Specific):
- [Reflection](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#reflection)
- [StringCrypt](https://github.com/san40-u5an40/NuGet.ExtraLib?tab=readme-ov-file#stringcrypt)

## Bytes
### Назначение
Статический класс для конвертирования байтов.

### Структура
Статические методы:
 - ToSize(Long) — Переводит полученное количество байт в набор гигабайт, мегабайт, килобайт и байт, представленный структурой Size.
 - ToGb(Long) — Переводит байты в гигабайты с математическим округлением.
 - ToMb(Long) — Переводит байты в мегабайты с математическим округлением.
 - ToKb(Long) — Переводит байты в килобайты с математическим округлением.

Свойства структуры Size:
 - long GByte — Количество гигабайт.
 - long MByte — Количество мегабайт.
 - long KByte — Количество килобайт.
 - long Byte — Количество оставшихся байт.

### Примеры кода
```C#
var drives = DriveInfo.GetDrives();

foreach (var drive in drives)
{
    var str = new StringBuilder()
        .AppendLine("Имя диска: " + drive.Name)
        .AppendLine("Метка диска: " + drive.VolumeLabel)
        .AppendLine("Общий размер: " + Bytes.ToSize(drive.TotalSize));

    Console.WriteLine(str);
}

// Вывод:
// 
// Имя диска: C:\
// Метка диска:
// Общий размер: 100 Гбайт, 207 Мбайт, 1008 Кбайт 0 байт
// 
// Имя диска: D:\
// Метка диска: Data
// Общий размер: 171 Гбайт, 115 Мбайт, 1020 Кбайт 0 байт
```

## Comparator
### Назначение
Статический класс, который возвращает объект IComparer позволяющий сравнивать пользовательские типы по указанному параметру.

### Структура
Статические методы:
 - GetComparator\<TSource, TKey> — Возвращает созданный объект для сравнения по переданной лямбде. Первым Generic-параметром указывается элемент коллекции, вторым возвращаемый тип данных.

### Примеры кода
```C#
Array.Sort(array, Comparator.GetComparator<User, string>(p => p.Name));
// Сортировка массива объектов "User" по имени
// string - т.к. этим типом представлено свойство Name
```

## Counter
### Назначение
Статический класс, предназначенный для создания счётчика на основе замыкания.

### Структура
Статические методы:
 - GetCounter — Метод создания счётчика на основе замыкания, параметром принимает начальное значение счётчика.

### Примеры кода
```C#
var cnt = Counter.GetCounter(10);
for (int i = 0; i < 10; i++)
    Console.Write(cnt() + " ");

// 10 11 12 13 14 15 16 17 18 19
```

## MessageBox
### Назначение
Класс и набор перечислений, предназначенный для работы с окнами уведомлений в проектах, не подразумевающих работу на базе фреймворков графических интерфейсов.

### Структура
Статический класс `MessageBox`:
 - Show — Метод вывода окна уведомлений. Принимает текст уведомления, заголовок окна и его тип (битовое поле `MessageBoxType`).

Битовое поле MessageBoxType:
 - Window — Типы окон:
     - Ok — Только кнопка "Ок".
     - OkCancel — Кнопки "Ок" и "Закрыть".
     - AbortRetryIgnore — Кнопки "Прервать", "Повторить" и "Пропустить".
     - YesNoCancel — Кнопки "Да", "Нет" и "Закрыть".
     - YesNo — Кнопки "Да" и "Нет".
     - RetryCancel — Кнопки "Повторить" и "Закрыть".
     - CancelTryContinue — Кнопки "Закрыть", "Повторить" и "Продолжить".
 - DefaultButton — Выбранная кнопка по умолчанию:
     - 1 — Первая кнопка выбрана по умолчанию.
     - 2 — Соответственно.
     - 3 — Соответственно.
     - 4 — Соответственно.
 - Icon — Иконка окна уведомления:
     - Error — Красный знак ошибки.
     - Question — Знак вопроса.
     - Warning — Жёлтый знак предупреждения.
     - Information — Синий информационный знак.

Статический класс с результатами окна уведомления `MessageBoxResult`:
 - Ok.
 - Cancel.
 - Abort.
 - Retry.
 - Ignore.
 - Yes.
 - No.
 - Close.
 - Help.
 - Try.
 - Continue.

### Примеры кода
```C#
int result = MessageBox.Show(
    "Продолжить работу программы?",
    "Окно уведомлений",
    MessageBoxType.Window_YesNoCancel | MessageBoxType.DefaultButton_2 | MessageBoxType.Icon_Question);

if (result == MessageBoxResult.Yes)
    Console.WriteLine("Я каменщик, работаем дальше...");
else if (result == MessageBoxResult.No)
    Console.WriteLine("User'а ответ!");
else
    return;
```

## Regexes
### Назначение
Статический класс с возможностью различного преобразования текста при помощи регулярных выражений.

### Структура
Статические методы:
 - EmailReplace — Заменяет все email-адреса из текста на указанное выражение.
 - HrefParse — Ищет в тексте все ссылки из атрибута `href` и текст этих ссылок. Возвращает коллекцию MatchCollection с группами `href` и `text`.
 - TagClear — Удаляет из текста найденные теги.
 - PhoneParse — Извлекает из строки с указанным номером число в формате `long`, также удаляя начальные "+7" или "8".

Все методы используют регулярные выражения с флагом `RegexOptions.Compiled`, что замедляет сборку, но увеличивает скорость работы приложения.

### Примеры кода
EmailReplace
```C#
string text = "По всем вопросам обращайтесь на почту: alexandr.dev2011@gmail.com";

string newText = Regexes.EmailReplace(text, "{email}");
// По всем вопросам обращайтесь на почту: {email}
```

HrefParse
```C#
string html = ...;

foreach (Match match in Regexes.HrefParse(html))
	Console.WriteLine($"Href: {match.Groups["href"].Value, -30} Text: {match.Groups["text"].Value}");  

// Пример вывода:
// Href: /science/seminar/inclusion     Text: Инклюзия
// Href: /science/seminar/security      Text: Безопасность
```

TagClear
```C#
string html = ...;
string newHtml = Regexes.TagClear(html);

// Пример вывода:
// Инклюзия Безопасность
```

PhoneParse
```C#
var phones = new string[]
{
    "88005553535",
    "+78005553535",
    "+7 (800) 555-35-35",
    "7 800 555 35 35",
    "8 (800) 555 35-35"
};

foreach(var phone in phones)
{
    long numbers = Regexes.PhoneParse(phone);
    Console.WriteLine($"{numbers:+7 (###) ###-##-##}");  // Все в одном формате
}

// Вывод:
// +7 (800) 555 - 35 - 35
// +7 (800) 555 - 35 - 35
// +7 (800) 555 - 35 - 35
// +7 (800) 555 - 35 - 35
// +7 (800) 555 - 35 - 35
```

## StringExtension
### Назначение
Статический класс с методами расширения для строк.

### Структура
Методы расширения:
 - Reduce — Сокращает строку до указанной длины, возвращая получившееся значение (Message) и остаток длины (Remainder).

### Примеры кода
```C#
var result = "Очень длинная строка!".Reduce(9);
Console.WriteLine($"Получившаяся строка: {result.Message}; Остаток длины: {result.Remainder}");

// Получившаяся строка: Очень ...; Остаток длины: 0

result = "По-прежнему очень длинная строка!".Reduce(50);
Console.WriteLine($"Получившаяся строка: {result.Message}; Остаток длины: {result.Remainder}");

// Получившаяся строка: По - прежнему очень длинная строка!; Остаток длины: 17
```

## TimerHelper
### Назначение
Статический класс предназначенный для использования экспоненциальных таймеров.

### Структура
Статические методы:
 - ExpWait — Осуществляет ожидание в зависимости от экспоненциального значения таймера, рассчитанного на основе указанной итерации цикла.
 - ExpWaitAsync — Осуществляет асинхронное ожидание в зависимости от экспоненциального значения таймера, рассчитанного на основе указанной итерации цикла.

### Примеры кода
Без необязательных параметров:
```C#
for(int i = 0; i < int.MaxValue; i++)
{
    Console.Write($"\rИтерация: {i}");
    TimerHelper.ExpWait(i);
    // В зависимости от итерации блокирует поток на разное количество времени
}
```

С указанием начального значения таймера и шага:
```C#
for(int i = 0; i < int.MaxValue; i++)
{
    Console.Write($"\rИтерация: {i}");
    TimerHelper.ExpWait(i, 6.5, 0.1);
}
```

## Result
### Назначение
Класс, хранящий сведенья о результате работы метода.

### Структура
Статические методы для создания результата:
- CreateSuccess — Создаёт сведенья об успешном результате.
- CreateFailure — Создаёт сведенья о невалидном результате.

Свойства результата:
- IsValid — Валидность результата.
- Value — Данные валидного результата.
- Error — Данные невалидного результата.

Generic-параметры:
- TSuccess (первый параметр) — Тип данных, которым будет представлено свойство Value.
- TFailure (второй параметр) — Тип данных, которым представлено свойство Error.

### Примеры кода
Стандартное использование:
```C#
using san40_u5an40.ExtraLib.Broad;

var success = Result<string, string>.CreateSuccess("Успешный успех!");
var failure = Result<string, string>.CreateFailure("Потеря потерь!");

string successMessage = success.IsValid ? success.Value : success.Error;
string failureMessage = failure.IsValid ? failure.Value : failure.Error;

Console.WriteLine(successMessage + '\n' + failureMessage);
```

При частом использовании одинакового набора Generic-параметров можно использовать псевдонимы:
```C#
using StringResult = san40_u5an40.ExtraLib.Broad.Result<string, string>;
var success = StringResult.CreateSuccess("Успешный успех!");
```

И конечно же допустимо использовать различные пользовательские типы в качестве Generic-параметров:
```C#
using san40_u5an40.ExtraLib.Broad;

var success = Result<SuccessInfo, FailureInfo>.CreateSuccess(new SuccessInfo("Вася", 25));
string message =
    success.IsValid ?
    success.Value.Name + ' ' + success.Value.Age :
    success.Error.Message;
Console.WriteLine(message);

record SuccessInfo(string Name, int Age);
record FailureInfo(string Message, int ErrorCode);
```

## Chain
### Назначение
Класс для создания функциональной цепочки. При её запуске указанные методы начинают до конца исполняться, передавая указанные данные. Если на каком-то этапе возникнет невалидный результат, вернётся соответствующая информация об ошибке.

### Примеры кода
```C#
var chainResult = new Chain<string[], object, string>(args)   // string[] - Тип входных данных, object - Выходных, string - Ошибка, args - Сами входные данные
    .AddMethod<string[], ArgumentsInfo>(HandleArguments)      // Получил string[],      вернул Result<ArgumentsInfo, string>
    .AddMethod<ArgumentsInfo, ContentInfo>(GetContents)       // Получил ArgumentsInfo, вернул Result<ContentInfo, string>
    .AddMethod<ContentInfo, WriterInfo>(GetOutputFileInfo)    // Получил ContentInfo,   вернул Result<WriterInfo, string>
    .AddMethod<WriterInfo, object>(WriteContents)             // Получил WriterInfo,    вернул Result<object, string>
    .Execute();                                               // Возвращает Result<object, string> (в данном случае object - заглушка)

if (!chainResult.IsValid)
{
    Console.WriteLine(chainResult.Error);
    return;
}

Console.WriteLine("Файл с оглавлением успешно создан!");
```
С проектом, реализованным на базе функциональной цепочки, можно ознакомиться [тут](https://github.com/san40-u5an40/ConsoleUtil.ContentsMdGenerator).

### Структура
Создание объекта Chain:
- Generic-параметры:
    1. TInputData — Тип входных данных в цепочку.
    2. TOutputData — Тип выходных данных из цепочки.
    3. TError — Тип возвращаемой на каком-либо этапе ошибки.
- Конструктор, где и указывают начальные данные цепочки.

Методы цепочки:
- AddMethod — Добавляет указанные делегат в цепочку.
    1. TInput — Тип получаемых методом данных.
    2. TOutput — Тип возвращаемых методом данных в виде Result<this, Chain.TError>.
- Execute — Выполняет цепочку и возвращает Result<Chain.TOutputData, Chain.TError>.

Важные уточнения:
- Тип входных данных первого метода цепочки и тип выходных данных последнего метода цепочки должны совпадать с типами начальных и выходных данных самой цепочки.
- Методы цепочки должны возвращать Result<TOutput, TError>, однако следующий метод принимает параметром только TOutput. Информация о валидности и сведенья об ошибке нужны для понимание самой цепочкой корректности выполнения операций.
- Все специально заложенные исключения проявят себя на этапе компиляции. Но, конечно же, никто не исключает непредусмотренных ошибок...

## IValidable
### Назначение
Ковариативный обобщённый интерфейс для объектов, проверяемых на валидность.

### Структура
Свойства:
- IsValid — Логическое значение, отражающее валиден ли результат.
- Value — Значение валидного результата.
- Error — Значение невалидного результата.

Generic-параметры:
- TSuccess — Тип Value.
- TFailure — Тип Error.

## ConsoleExtension
### Назначение
Статический класс с блоками расширения для консоли.

### Структура
Статические методы:
 - TryReadLine — Работает также как и схожие методы: записывает в out-параметр результат чтения из консоли, возвращая содержит ли этот результат символы.
 - WriteColor — Выводит в консоль текст указанного цвета.
 - CleanLine — Очищает одну строку от текста.

### Примеры кода
TryReadLine
```C#
string result;
while(!Console.TryReadLine(out result))
    Console.WriteLine("Необходимо указать значение, повторите ввод!");
```

WriteColor
```C#
internal static void PrintWelcome(string name)
{
    Console.Write("Добро пожаловать в программу, ");
    Console.WriteColor(name, ConsoleColor.Red);
    Console.Write("!\n");
}
```

CleanLine
```C#
Console.Write("Текст на 3 секунды...");
Thread.Sleep(3000);
Console.CleanLine();
```

## StringCrypt
### Назначение
Статический класс для кодирования и декодирования текста при помощи сдвигового шифрования, с поддержкой переформатирования текста в HEX.

### Структура
Методы расширения:
 - Crypt — Кодирует текст.
 - Decrypt — Декодирует текст.

Первым параметром указывается числовой ключ для сдвига. Вторым необязательным параметром указывается формат кодирования, представленный перечислением StringCrypter.Type:
 - Hex — Кодирует текст в HEX, и декодирует HEX-формат в обычный текст.
 - Std — Кодирует строку только с помощью сдвиговой шифрации.

По умолчанию во втором параметре указан тип: Std.

### Примеры кода
Со стандартным форматированием:
```C#
string text = "Simple string";

string crypted = text.Crypt(-10);
// I_cfb[▬ijh_d]

string decrypted = crypted.Decrypt(10);
// Simple string
```

С форматированием в HEX:
```C#
string text = "Simple string";

string crypted = text.Crypt(-10, StringCrypter.Type.Hex);
// 49 5F 63 66 62 5B 16 69 6A 68 5F 64 5D

string decrypted = crypted.Decrypt(10, StringCrypter.Type.Hex);
// Simple string
```

## Reflection
### Назначение
Статический класс, предназначенный для исследования пользовательских типов.

### Структура
Атрибуты:
 - ReflectionAttribute(BindingFlags(optional)) — Атрибут для отметки класса о необходимости осуществления его рефлексивного анализа методом Reflection.Print();

Статические методы:
 - Print() — Выводит информацию о всех пользовательских типах, имеющих атрибут ReflectionAttribute (только этот метод связан с использованием атрибута);
 - Print(Type, BindingFlags(optional)) — Выводит рефлексивную информацию о переданном в параметр типе согласно указанным флагам.
 - Print(Assembly, BindingFlags(optional)) — Выводит рефлексивную информацию о переданной в параметр сборке.
 - Print(String, BindingFlags(optional)) — Выводит рефлексивную информацию о сборке, путь которой передан в параметр метода.
 - Print(String, String, BindingFlags(optional)) — Выводит рефлексивную информацию о сборке, чей путь указан, и содержащимся в ней типе, название которого указано вторым параметром.

### Примеры кода
Использование через атрибут:
```C#
// Main:
Reflection.Print();

// Тестовый класс для анализа
[Reflection]
public class ClassTest
{
    private static int count = 0;
    private string? name;
    private Func<int>? getNumber;

    public string? Name => name;

    public ClassTest(string name) { }

    public ClassTest() { }

    public event Func<int> GetNumber
    {
        add
        {
            if (value == null)
                return;

            getNumber += value;
        }
        remove
        {
            if (value == null || getNumber == null)
                return;

            if (!getNumber.GetInvocationList().Contains(value))
                return;

            getNumber -= value;
        }
    }

    internal void Print(string mess, string mess2) {  }
}

// Вывод:
// Состав сборки "SandBox.dll":
//
//   Класс "ClassTest":
//   |
//   |  Поля:
//   |  |
//   |  |  Имя: name                 | Тип: System.String                            | Атрибуты: Private
//   |  |  Имя: getNumber            | Тип: System.Func`1[System.Int32]              | Атрибуты: Private
//   |  |  Имя: count                | Тип: System.Int32                             | Атрибуты: Private, Static
//   |  |
//   |  Общее количество: 3
//   |
//   |  Методы:
//   |  |
//   |  |  Имя: get_Name
//   |  |     Атрибуты: Public, HideBySig, SpecialName
//   |  |     Возвращаемый тип: System.String
//   |  |
//   |  |  Имя: add_GetNumber
//   |  |     Атрибуты: Public, HideBySig, SpecialName
//   |  |     Возвращаемый тип: Void
//   |  |     Параметры: (System.Func`1[System.Int32]) value
//   |  |
//   |  |  Имя: remove_GetNumber
//   |  |     Атрибуты: Public, HideBySig, SpecialName
//   |  |     Возвращаемый тип: Void
//   |  |     Параметры: (System.Func`1[System.Int32]) value
//   |  |
//   |  |  Имя: Print
//   |  |     Атрибуты: Assembly, HideBySig
//   |  |     Возвращаемый тип: Void
//   |  |     Параметры: (System.String) mess, (System.String) mess2
//   |  |
//   |  Общее количество: 4
//   |
//   |  Конструкторы:
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.String) name
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |
//   |  Общее количество: 2
//   |
//   Конец класса
//
// Конец сборки
```

Использование по ссылке на тип (поисследуем класс String):
```C#
// Main:
Reflection.Print(typeof(System.String));

// Вывод:
// Состав сборки "System.Private.CoreLib.dll":
//
//   Класс "String":
//   |
//   |  Поля:
//   |  |
//   |  |  Имя: _stringLength        | Тип: System.Int32                             | Атрибуты: Private, InitOnly, NotSerialized
//   |  |  Имя: _firstChar           | Тип: System.Char                              | Атрибуты: Private, NotSerialized
//   |  |  Имя: Empty                | Тип: System.String                            | Атрибуты: Public, Static, InitOnly
//   |  |
//   |  Общее количество: 3
//   |
//   |  Методы:
//   |  |
//   |  |  Имя: FastAllocateString
//   |  |     Атрибуты: Assembly, Static, HideBySig
//   |  |     Возвращаемый тип: System.String
//   |  |     Параметры: (System.Int32) length
//   |  |

// Тут очень много методов, можете при желании поисследовать сами

//   |  |
//   |  |  Имя: LastIndexOf
//   |  |     Атрибуты: Public, HideBySig
//   |  |     Возвращаемый тип: Int32
//   |  |     Параметры: (System.String) value, (System.Int32) startIndex, (System.Int32) count, (System.StringComparison) comparisonType
//   |  |
//   |  Общее количество: 267
//   |
//   |  Конструкторы:
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.Char[]) value
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.Char[]) value, (System.Int32) startIndex, (System.Int32) length
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.Char*) value
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.Char*) value, (System.Int32) startIndex, (System.Int32) length
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.SByte*) value
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.SByte*) value, (System.Int32) startIndex, (System.Int32) length
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.SByte*) value, (System.Int32) startIndex, (System.Int32) length, (System.Text.Encoding) enc
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.Char) c, (System.Int32) count
//   |  |
//   |  |  Имя: .ctor
//   |  |     Атрибуты: Public, HideBySig, SpecialName, RTSpecialName
//   |  |     Параметры: (System.ReadOnlySpan`1[System.Char]) value
//   |  |
//   |  Общее количество: 9
//   |
//   Конец класса
//
// Конец сборки
```

Использование по ссылке на сборку:
```C#
Reflection.Print(Assembly.GetAssembly(typeof(Regex)));
// Выведет все классы, входящие в состав "System.Text.RegularExpressions.dll"
```

Использование через указание пути сборки:
```C#
                         ↓ - экранизация знака "\"
string pathAssembly = "D:\\csharp\\projects\\ExtraLib\\bin\\Debug\\net9.0\\Text.dll";
Reflection.Print(pathAssembly);
```

Использование через указание пути сборки и названия класса:
```C#
                         ↓ - экранизация знака "\"
string pathAssembly = "D:\\csharp\\projects\\ExtraLib\\bin\\Debug\\net9.0\\Std.dll";

Reflection.Print(pathAssembly, "Comparator");
// Или по полному названию
Reflection.Print(pathAssembly, "Std.Comparator");

// Если полученную сборку не удастся открыть, будет выведено сообщение из Exception
// Could not load file or assembly 'D:\csharp\projects\ExtraLib\bin\Debug\net9.0\Std.dll'. Системе не удается найти указанный путь.
```