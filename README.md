# san40_u5an40.ExtraLib
В данном NuGet-пакете представлена библиотека классов, расширяющая стандартные возможности программирования. В ней представлены паттерны, полезные как при функциональном стиле программирования, так и при объектно-ориентированном.

Версионные изменения [в конце](#история-последних-изменений) документа.

## Оглавление:
Полезны в широком круге задач (namespace san40_u5an40.ExtraLib.Broad):
- [Bytes](#bytes)
- [Comparator](#comparator)
- [Counter](#counter)
- [DefaultConstants](#defaultconstants)
- [EmailsParser](#emailsparser)
- [HtmlHelper](#htmlhelper)
- [MessageBox](#messagebox)
- [StringExtension](#stringextension)
- [TimerHelper](#timerhelper)

Паттерновые классы, также полезные в широком круге задач (namespace san40_u5an40.ExtraLib.Broad.Patterns):
- [Readyable](#readyable)
- [Result](#result)
- [Chain and AsyncChain](#chain-and-asyncchain)

Полезны при консольной разработке (namespace san40_u5an40.ExtraLib.ConsoleApp):
- [ConsoleExtension](#consoleextension)

Для специфичных задач (namespace san40_u5an40.ExtraLib.Specific):
- [Reflection](#reflection)
- [StringCrypt](#stringcrypt)

## Bytes
### Назначение
Статический класс для конвертирования байтов.

### Структура
**Статические методы:**
 - `ToSize(Long)` — Переводит полученное количество байт в набор гигабайт, мегабайт, килобайт и байт, представленный структурой `Size`.
 - `ToGb(Long)` — Переводит байты в гигабайты с математическим округлением.
 - `ToMb(Long)` — Переводит байты в мегабайты с математическим округлением.
 - `ToKb(Long)` — Переводит байты в килобайты с математическим округлением.

**Свойства структуры `Size`:**
 - `long GByte` — Количество гигабайт.
 - `long MByte` — Количество мегабайт.
 - `long KByte` — Количество килобайт.
 - `long Byte` — Количество оставшихся байт.

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
Статический класс, который возвращает объект `IComparer` позволяющий сравнивать пользовательские типы по указанному параметру. Полезен при работе с `Array`.

### Структура
**Статические методы:**
 - `GetComparator<TSource, TKey>` — Возвращает созданный объект для сравнения по переданной лямбде. Первым Generic-параметром указывается элемент коллекции, вторым возвращаемый тип данных.

### Примеры кода
```C#
Array.Sort(array, Comparator.GetComparator<User, string>(p => p.Name));
// Сортировка массива объектов "User" по имени
// string - т.к. этим типом представлено свойство Name
```

## Counter
### Назначение
Класс, предназначенный для создания счётчиков.

### Структура
**Свойства:**
- `Value` — Значение счётчика.
- `Name` — Имя счётчика.

**Instance-методы:**
- `Increment` — Инкрементирование счётчика со стандартным или указанным шагом.
- `Decrement` — Декрементирование счётчика со стандартным или указанным шагом.
- `Clone` — Клонирование счётчика.
- `CompareTo` — Сравнение счётчиков.
- `Equals` — Проверка счётчика на равенство (по значениям `Name` и `Value`).
- `GetHashCode` — Переопределён для получения хеш-кода (по значениям `Name` и `Value`).
- `ToString` —  Преобразование счётчика в строковое значение.

**Статические методы:**
- `CreateCounter` — Фабрика счётчиков с указанными значениями.
- `CreateClosuredCounter` — Создание счётчика на основе замыкания.

**Операторы:**
- `+=` — Увеличивает значение счётчика на указанный шаг.
- `-=` — Уменьшает значение счётчика на указанный шаг.
- `++` — Увеличивает значение счётчика на стандартный шаг.
- `--` — Уменьшает значение счётчика на стандартный шаг.
- `+ double` — Увеличивает значение счётчика на указанный шаг.
- `- double` — Уменьшает значение счётчика на указанный шаг.
- `==` — Проверка счётчика на равенство (по значениям `Name` и `Value`).
- `!=` — Проверка счётчика на неравенство (по значениям `Name` и `Value`).
- `>` — Сравнение двух счётчиков с помощью `CompareTo`.
- `>=` — Сравнение двух счётчиков с помощью `CompareTo`.
- `<` — Сравнение двух счётчиков с помощью `CompareTo`.
- `<=` — Сравнение двух счётчиков с помощью `CompareTo`.

### Примеры кода
Со счётчиком на основе замыкания:
```C#
var cnt = Counter.CreateClosuredCounter(10);
for (int i = 0; i < 10; i++)
    Console.Write(cnt() + " ");

// 10 11 12 13 14 15 16 17 18 19
```

С обычным счётчиком:
```C#
Counter counter = new(value: 0, name: "Подпесчеки");
counter += 10;
counter++;
Console.WriteLine(counter); // "Подпесчеки: 11"
```

## DefaultConstants
### Назначение
Дополнительные константы для примитивов.

### Структура
- `string.NotEmpty` — Непустая строка `something`.
- `int.PositiveNumber` — Положительное число `1`.
- `int.Zero` — Содержит ноль (более явная форма `default`).
- `int.NegativeNumber` — Отрицательное число `-1`.

### Примеры кода
```C#
Counter counter = new(value: int.Zero, name: string.NotEmpty);
```

## EmailsParser
### Назначение
Класс для работы с текстом, содержащим email-адреса.

### Структура
**Константы:**
- `EmailRegex` — Регулярное выражение, для поиска в тексте email-адресов.

**Методы:**
- `Replace` — Меняет все email-адреса из текста на указанное значение.
- `Parse` — Получение коллекции email-адресов, хранящихся в тексте.

### Примеры кода
Замена:
```C#
string textForFormatEmails = EmailsParser.Replace(textWithEmails, "{0}");
```

Получение коллекции:
```C#
List<string> emails = EmailsParser.Parse(textWithEmails);
```

## HtmlHelper
### Назначение
Вспомогательный класс, для работы с HTML-текстом.

### Структура
**Константы:**
- `TagRegex` — Регулярное выражение для поиска тегов в тексте

**Методы:**
- `TagsClear` — Очищает строку от тегов.

### Примеры кода
```C#
string html = ...;
string textFromHtmlWithoutTags = HtmlHelper.TagsClear(html);

// Пример вывода:
// Инклюзия\r\nБезопасность
```

## MessageBox
### Назначение
Класс и набор перечислений, предназначенный для работы с окнами уведомлений в проектах, не подразумевающих работу на базе фреймворков графических интерфейсов.

### Структура
**Статический класс `MessageBox`:**
 - `Show` — Метод вывода окна уведомлений. Принимает текст уведомления, заголовок окна и его тип (битовое поле `MessageBoxType`).

**Битовое поле `MessageBoxType`:**
 - `Window` — Типы окон:
     - `Ok` — Только кнопка "Ок".
     - `OkCancel` — Кнопки "Ок" и "Закрыть".
     - `AbortRetryIgnore` — Кнопки "Прервать", "Повторить" и "Пропустить".
     - `YesNoCancel` — Кнопки "Да", "Нет" и "Закрыть".
     - `YesNo` — Кнопки "Да" и "Нет".
     - `RetryCancel` — Кнопки "Повторить" и "Закрыть".
     - `CancelTryContinue` — Кнопки "Закрыть", "Повторить" и "Продолжить".
 - `DefaultButton` — Выбранная кнопка по умолчанию:
     - `1` — Первая кнопка выбрана по умолчанию.
     - `2` — Соответственно.
     - `3` — Соответственно.
     - `4` — Соответственно.
 - `Icon` — Иконка окна уведомления:
     - `Error` — Красный знак ошибки.
     - `Question` — Знак вопроса.
     - `Warning` — Жёлтый знак предупреждения.
     - `Information` — Синий информационный знак.

**Статический класс с результатами окна уведомления `MessageBoxResult`:**
 - `Ok`.
 - `Cancel`.
 - `Abort`.
 - `Retry`.
 - `Ignore`.
 - `Yes`.
 - `No`.
 - `Close`.
 - `Help`.
 - `Try`.
 - `Continue`.

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

## StringExtension
### Назначение
Статический класс с методами расширения для строк.

### Структура
**Методы расширения:**
- `Reduce` — Сокращает строку до указанной длины, возвращая получившееся значение `Message` и остаток длины `Remainder`.
- `ReplaceWhileContain` — Заменяет старое значение на новое, пока в тексте содержится старое значение (удобно для удаления повторяющихся символов).

### Примеры кода
`Reduce`:
```C#
var result = "Очень длинная строка!".Reduce(9);
Console.WriteLine($"Получившаяся строка: {result.Message}; Остаток длины: {result.Remainder}");

// Получившаяся строка: Очень ...; Остаток длины: 0

result = "По-прежнему очень длинная строка!".Reduce(50);
Console.WriteLine($"Получившаяся строка: {result.Message}; Остаток длины: {result.Remainder}");

// Получившаяся строка: По - прежнему очень длинная строка!; Остаток длины: 17
```

`ReplaceWhileContain`:
```C#
string text = "Строка,       где очень много лишних        пробелов";
string textWithoutDoubleSpace = text.ReplaceWhileContain("  ", " ");
```

## TimerHelper
### Назначение
Статический класс предназначенный для использования экспоненциальных таймеров.

### Структура
**Статические методы:**
 - `ExpWait` — Осуществляет ожидание в зависимости от экспоненциального значения таймера, рассчитанного на основе указанной итерации цикла.
 - `ExpWaitAsync` — Осуществляет асинхронное ожидание в зависимости от экспоненциального значения таймера, рассчитанного на основе указанной итерации цикла.

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

## Readyable
### Назначение
Потокобезопасный объект, проверяемый на готовность. Удобен при необходимости отложено инициализировать out-параметры.

### Структура
**Generic-параметр:**
- `TValue` — Тип хранимого значения.

**Свойства:**
- `Name` — Имя объекта.
- `Value` — Значение объекта.
- `State` — Состояние объекта, представленное перечислением `ReadyableState`:
    - `NeverBeReady` — Объект, который никогда не будет готов к использованию.
    - `Waiting` — Объект, ожидающий готовности.
    - `Ready` — Объект, готовый к использованию.
- `IsInitialized` — Логическое значение, отражающее инициализированы ли данные.
- `IsWaiting` — Логическое значение, отражающее находится ли объект в режиме ожидания.
- `IsReady` — Логическое значение, отражающее готов ли объект.
- `IsNeverBeReady` — Логическое значение, отражающее находится ли объект в состоянии, в котором уже никогда не будет готов.

**Методы:**
- `ThrowIfNotInitialized` — Выбрасывает исключение, если значение объекта не инициализировано.
- `ThrowIfNotWaiting` — Выбрасывает исключение, если объект не ожидает значение.
- `ThrowIfNotReady` — Выбрасывает исключение, если объект не находится в состоянии готовности.
- `ToReady` (Только явное использование) — Приводит объект в состояние готовности.
- `ToNeverBeReady` (Только явное использование) — Приводит объект в состояние, в котором он уже не будет готов никогда.

**Ассоциированное с этим классом исключение `ReadyableException`:**
- `Message` — Сообщение об ошибке.
- `Name` — Имя объекта, проверяемого на готовность.
- `State` — Состояние объекта, связанного с ошибкой.

**Соглашение об использовании:**\
Подразумевается, что после создание этого объекта он будет либо инициализирован и приведён в состояние готовности, либо приведён в состояние, когда он никогда не будет готов.\
Если запросить его значение, когда он не находится в состоянии готовности, возникнет исключение. Как и при попытке его привести в состояние готовности, если его значение не было проинициализированно.

### Примеры кода
При успешном выполнении какой-то операции, например:
```C#
// Внутри какого-то метода
readyable.Value = 3;
((IReadyable<int>)readyable).ToReady(); // Явное использование для небольшой защиты от лишнего вмешательства

// Где-нибудь снаружи
int value = readyable.IsReady ? readyable.Value : default;
```

При какой-нибудь ошибке:
```C#
if (somethingBad)
    return ((IReadyable<int>)readyable).ToNeverBeReady();
```

## Result
### Назначение
Класс, хранящий сведенья о результате работы метода.

### Структура
**Статические методы для создания результата:**
- `CreateSuccess` — Создаёт сведенья об успешном результате (Есть интеграция с `Readyable`).
- `CreateFailure` — Создаёт сведенья о невалидном результате (Есть интеграция с `Readyable`).

**Свойства результата:**
- `IsValid` — Валидность результата.
- `Value` — Данные валидного результата.
- `Error` — Данные невалидного результата.

**Generic-параметры:**
- `TSuccess` (первый параметр) — Тип данных, которым будет представлено свойство `Value`.
- `TFailure` (второй параметр) — Тип данных, которым представлено свойство `Error`.

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

### Интеграция с Readyable
Методы `CreateSuccess` и `CreateFailure` имеют перегрузки со вторым `Readyable` параметром, который устанавливает состояние объекта в то или иное значение в зависимости от метода:
- `CreateSuccess` — Инициализирует `Readyable.Value` своим `Result.Value` и устанавливает состояние `Ready`.
- `CreateFailure` — Устанавливает состояние `Readyable` в `NeverBeReady`.

Допустимо передавать в параметр только те объекты, которые находятся в состоянии `Wait`. Иные попытки приведут к исключению `ReadyableException`.

```C#
Readyable<string> readyable = new();
Result<string,string>.CreateSuccess("Какое-то значение!", readyable);

// Теперь readyable.Value == "Какое-то значение!", а readyable.State == ReadyableState.Ready;
```

## Chain and AsyncChain
### Назначение
Классы для создания функциональных цепочек. При их запуске указанные методы начинают до конца исполняться, передавая указанные данные. Если на каком-то этапе возникнет невалидный результат, вернётся соответствующая информация об ошибке.

### Примеры кода
Обычная цепочка:
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

Асинхронная:
```C#
var asyncChainResult = await new AsyncChain<string, int, string>(DOCUMENT_PATH)
    .AddMethod<string, int>(GetNumberFromDocumentAsync)  // Получил string,  вернул Task<Result<int, string>>
    .AddMethod<int, int>(IncrementNumberAsync)           // Получил int,     вернул Task<Result<int, string>>
    .ExecuteAsync();
```

С проектом, реализованным на базе функциональной цепочки, можно ознакомиться [тут](https://github.com/san40-u5an40/ConsoleUtil.ContentsMdGenerator). Также более наглядный пример есть в проекте с тестами.

### Структура
**Создание объекта `Chain` и `AsyncChain`:**
- Generic-параметры:
    1. `TInputData` — Тип входных данных в цепочку.
    2. `TOutputData` — Тип выходных данных из цепочки.
    3. `TError` — Тип возвращаемой на каком-либо этапе ошибки.
- Конструктор, где и указывают начальные данные цепочки.

**Методы цепочки:**
- `AddMethod` — Добавляет указанные делегат в цепочку (есть поддержка `Readyable` out-параметра).
    1. `TInput` — Тип получаемых методом данных.
    2. `TOutput` — Тип возвращаемых методом данных в виде `Result<this, Chain.TError>` или `Task<Result<this, Chain.TError>>` при асинхронных методах.
- `Execute/ExecuteAsync` — Выполняет цепочку и возвращает `Result<Chain.TOutputData, Chain.TError>`.

**Соглашения об использовании:**
- Тип входных данных первого метода цепочки и тип выходных данных последнего метода цепочки должны совпадать с типами начальных и выходных данных самой цепочки (иначе возникнет исключение).
- Методы цепочки должны возвращать `Result<TOutput, TError>`, однако следующий метод принимает параметром только `TOutput`. Информация о валидности и сведенья об ошибке нужны для понимание самой цепочкой корректности выполнения операций.

### Использование out-параметра в AddMethod
С помощью объекта `Readyable` можно получать промежуточные результаты, передаваемых цепочкой данных:
```C#
var incrementResult = new Chain<string, string, string>(START_VALUE)
    .AddMethod<string, int>(ConvertStringIntoInt)
    .AddMethod<int, int>(Increment, out Readyable<int> readyable)    // Сохранит промежуточный результат
    .AddMethod<int, string>(ConvertIntIntoString)
    .Execute();
```

## ConsoleExtension
### Назначение
Статический класс с блоками расширения для консоли.

### Структура
**Статические методы:**
 - `TryReadLine` — Работает также как и схожие методы: записывает в out-параметр результат чтения из консоли, возвращая содержит ли этот результат символы.
 - `WriteColor` — Выводит в консоль текст указанного цвета без `\n`.
 - `WriteColorLine` — Выводит в консоль текст указанного цвета с `\n` на конце.
 - `CleanLine` — Очищает одну строку от текста.

### Примеры кода
`TryReadLine`:
```C#
string result;
while(!Console.TryReadLine(out result))
    Console.WriteLine("Необходимо указать значение, повторите ввод!");
```

`WriteColor`:
```C#
internal static void PrintWelcome(string name)
{
    Console.Write("Добро пожаловать в программу, ");
    Console.WriteColor(name, ConsoleColor.Red);
    Console.Write("!\n");
}
```

`CleanLine`:
```C#
Console.Write("Текст на 3 секунды...");
Thread.Sleep(3000);
Console.CleanLine();
```

## StringCrypt
### Назначение
Статический класс для кодирования и декодирования текста при помощи сдвигового шифрования, с поддержкой переформатирования текста в HEX.

### Структура
**Методы расширения:**
 - `Crypt` — Кодирует текст.
 - `Decrypt` — Декодирует текст.

Первым параметром указывается числовой ключ для сдвига. Вторым необязательным параметром указывается формат кодирования, представленный перечислением `StringCrypter.Type`:
 - `Hex` — Кодирует текст в HEX, и декодирует HEX-формат в обычный текст.
 - `Std` — Кодирует строку только с помощью сдвиговой шифрации.

По умолчанию во втором параметре указан тип: `Std`.

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
**Атрибуты:**
 - `ReflectionAttribute(BindingFlags(optional))` — Атрибут для отметки класса о необходимости осуществления его рефлексивного анализа методом `Reflection.Print()`;

**Статические методы:**
 - `Print()` — Выводит информацию о всех пользовательских типах, имеющих атрибут `ReflectionAttribute` (только этот метод связан с использованием атрибута);
 - `Print(Type, BindingFlags(optional))` — Выводит рефлексивную информацию о переданном в параметр типе согласно указанным флагам.
 - `Print(Assembly, BindingFlags(optional))` — Выводит рефлексивную информацию о переданной в параметр сборке.
 - `Print(String, BindingFlags(optional))` — Выводит рефлексивную информацию о сборке, путь которой передан в параметр метода.
 - `Print(String, String, BindingFlags(optional))` — Выводит рефлексивную информацию о сборке, чей путь указан, и содержащимся в ней типе, название которого указано вторым параметром.

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

Использование по ссылке на тип (поисследуем класс `String`):
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
                         ↓ - экранирование знака "\"
string pathAssembly = "D:\\csharp\\projects\\ExtraLib\\bin\\Debug\\net9.0\\Text.dll";
Reflection.Print(pathAssembly);
```

Использование через указание пути сборки и названия класса:
```C#
                         ↓ - экранирование знака "\"
string pathAssembly = "D:\\csharp\\projects\\ExtraLib\\bin\\Debug\\net9.0\\Std.dll";

Reflection.Print(pathAssembly, "Comparator");
// Или по полному названию
Reflection.Print(pathAssembly, "Std.Comparator");

// Если полученную сборку не удастся открыть, будет выведено сообщение из Exception
// Could not load file or assembly 'D:\csharp\projects\ExtraLib\bin\Debug\net9.0\Std.dll'. Системе не удается найти указанный путь.
```

## История последних изменений
### v3.0.0
- Добавлены тесты для ключевых классов библиотеки!
- Добавлена история последних изменений!
- В `Brad.Patterns` добавлен потокобезопасный объект `Readyable` (с сопутствующими ассоциированными типами), проверяемый на готовность и упрощающий задачи с отложенной инициализацией данных.
- В `Brad.Patterns.Chain` добавлена поддержка out-параметров, возвращающих промежуточные значения цепочки в виде объекта `Readyable`.
- Также появилась `Brad.Patterns.AsyncChain`, унаследовавшая все возможности обычной цепочки, но поддерживающая асинхронные операции. 
- Добавлена потокобезопасная интеграция `Brad.Patterns.Resul` с `Readyable` подробнее в соответствующем [разделе](#result). 
- Класс `Broad.Counter` стал полноценным ООП-счётчиком, старый метод сменил название на `CreateClosuredCounter`.
- Добавлен `Broad.DefaultConstants` с удобными константами для примитивов.
- `Broad.Regexes` был переформирован:
    - `EmailReplace` перемещён в новый класс `EmailsParser`, где присутсвуют также и новые возможности для работы с email-адресами.
    - `TagClear` перемещён в новый класс `HtmlHelper`, где также есть возможность получить строку с регулярным выражением для поиска тегов в тексте.
    - `HrefParse` и `PhoneParse` были удалены, в связи с наличием специализированных пакетов, упрощающих данные задачи.
- В `Broad.StringExtensions` добавлен метод `ReplaceWhileContain`.
- В `ConsoleApp.ConsoleExtensions` добавлен метод `WriteColorLine`.
- Был удалён интерфейс `IValidable` (пока что в нём нет необходимости).
- Изменена структура папок репозитория.
- Изменены якоря в оглавлении.

### v2.0.0
- Добавлен раздел `Broad.Patterns`, хранящий классы, представляющие собой паттерны разработки.
- Класс `Result` перемещён в `Broad.Patterns`.
- Добавлен ковариативный интерфейс `Broad.Patterns.IValidable` для валидируемых объектов.
- Добавлена поддержка классом `Result` интерфейса `IValidable`.
- Добавлена функциональная цепочка `Broad.Patterns.Chain`.

### v1.3.0 - v1.3.1
- Добавлен и исправлен `Broad.Result` — Класс для хранения информации о результате выполнения метода.

### v1.2.0
- Добавлен `Broad.MessageBox` — Класс для создания текстовых окон Windows.

### v1.1.0
- Изменена версия .NET на 10
- Добавлена поддержка блоков расширения для статических методов `Console`.