namespace san40_u5an40.ExtraLib.Reflection;

/// <summary>
/// Атрибут для отметки пользовательского типа, требующего рефлексивный анализ
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ReflectionAttribute(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly) : Attribute
{
    /// <summary>
    /// Флаги для рефлексивного поиска
    /// </summary>
    public BindingFlags Flags => flags;
}

/// <summary>
/// Класс для вывода рефлексивной информации об отмеченных пользовательского типа
/// </summary>
public static class Reflection
{
    // Визуальная иерархия информации
    private const string ZERO_LEVEL = "|   ";
    private const string FIRST_LEVEL = "|   |  ";
    private const string SECOND_LEVEL = "|   |  |  ";
    private const string THIRD_LEVEL = "|   |  |     ";

    // Разделитель информации о fields
    private const string SEPARATOR = " | ";

    /// <summary>
    /// Выводит рефлексивную информацию о всех пользовательских типах, отмеченных атрибутом <c>[Reflection]</c>, в данном решении
    /// </summary>
    public static void Print()
    {
        // Установка цвета вывода
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        // Получение всех сборок решения
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Перебор всех сборок
        foreach (var assembly in assemblies)
        {
            // Фильтрация пользовательских типов, не имеющих атрибут рефлексии
            var types = GetTypesWithAttribute(assembly);

            // Если в сборке таких пользовательских типов нет, то продолжение перебора сборок
            if (types.Length == 0)
                continue;

            // Если такие пользовательские типы есть, то вывод информации о них и о сборке, которая их содержит
            Console.WriteLine("\nThe composition of the assembly \"" + assembly.GetName().Name + ".dll\":");
            Console.WriteLine(ZERO_LEVEL);

            // Локальная функция, выводящая информацию о типах, содержащих атрибут
            PrintTypesInfo(types);

            // Завершение блока информации о сборке
            Console.WriteLine("End of assembly");
        }

        // Сброс цвета
        Console.ResetColor();

        // Локальная функция выбора типов, имеющих необходимый атрибут
        static Type[] GetTypesWithAttribute(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(p => p.GetCustomAttribute<ReflectionAttribute>() != null)
                .ToArray();
        }

        // Локальная функция вывода информации о разных типах
        static void PrintTypesInfo(Type[] types)
        {
            foreach (var type in types)
            {
                var flags = GetAttributeFlags(type);
                PrintTypeInfo(type, flags);
            }
        }

        // Локальная функция получения флагов из свойства атрибута
        static BindingFlags GetAttributeFlags(Type type)
        {
            ReflectionAttribute? statisticAtr = type.GetCustomAttribute<ReflectionAttribute>();
            return statisticAtr!.Flags;
        }
    }

    // Вывод информации об одном пользовательском типе
    private static void PrintTypeInfo(Type type, BindingFlags flags)
    {
        // Получение полей, методов и конструкторов типа
        var fields = type.GetFields(flags);
        var methods = type.GetMethods(flags);
        var constructors = type.GetConstructors(flags);

        // Подсчёт количества полей, методов и конструкторов
        int cntFields = fields.Length;
        int cntMethods = methods.Length;
        int cntConstructors = constructors.Length;

        // Вывод заголовка пользовательского типа
        Console.WriteLine(ZERO_LEVEL + "Type: \"" + type.Name + "\":");
        Console.WriteLine(FIRST_LEVEL);

        // Если в пользовательском типе есть поля, вывод информации о них
        if (cntFields > 0)
            PrintFieldsInfo(fields, cntFields);

        // Если в пользовательском типе есть методы, вывод информации о них
        if (cntMethods > 0)
            PrintMethodsInfo(methods, cntMethods);

        // Если в пользовательском типе есть конструкторы, вывод информации о них
        if (cntConstructors > 0)
            PrintConstructorsInfo(constructors, cntConstructors);

        // Завершение блока информации о пользовательском типе
        Console.WriteLine(ZERO_LEVEL + "End of type");
        Console.WriteLine(ZERO_LEVEL);
    }

    // Вывод информации о полях пользовательского типа
    private static void PrintFieldsInfo(FieldInfo[] fields, int cntFields)
    {
        Console.WriteLine(FIRST_LEVEL + "Fields:");
        Console.WriteLine(SECOND_LEVEL);

        foreach (var field in fields)
        {
            var fieldInfo = new StringBuilder()
                .Append(SECOND_LEVEL)
                .Append("Name: ")
                .Append(field.Name.PadRight(20))
                .Append(SEPARATOR)
                .Append("Type: ")
                .Append(field.FieldType.ToString().PadRight(40))
                .Append(SEPARATOR)
                .Append("Attributes: ")
                .Append(field.Attributes);
            Console.WriteLine(fieldInfo);
        }

        Console.WriteLine(SECOND_LEVEL);
        Console.WriteLine(FIRST_LEVEL + "Total number: " + cntFields);
        Console.WriteLine(FIRST_LEVEL);
    }

    // Вывод информации о методах пользовательского типа
    private static void PrintMethodsInfo(MethodInfo[] methods, int cntMethods)
    {
        Console.WriteLine(FIRST_LEVEL + "Methods:");
        Console.WriteLine(SECOND_LEVEL);

        foreach (var method in methods)
        {
            Console.WriteLine(SECOND_LEVEL + "Name: " + method.Name);
            Console.WriteLine(THIRD_LEVEL + "Attributes: " + method.Attributes);
            Console.WriteLine(THIRD_LEVEL + "Return type: " + method.ReturnParameter);

            if (method.GetParameters().Length > 0)
            {
                string[] paramsInfo = method
                    .GetParameters()
                    .Select(p => "(" + p.ParameterType + ") " + p.Name)
                    .ToArray();

                Console.WriteLine(THIRD_LEVEL + "Parameters: " + string.Join(", ", paramsInfo));
            }

            Console.WriteLine(SECOND_LEVEL);
        }

        Console.WriteLine(FIRST_LEVEL + "Total number: " + cntMethods);
        Console.WriteLine(FIRST_LEVEL);
    }

    // Вывод информации о конструкторах пользовательского типа
    private static void PrintConstructorsInfo(ConstructorInfo[] constructors, int cntConstructors)
    {
        Console.WriteLine(FIRST_LEVEL + "Constructors:");
        Console.WriteLine(SECOND_LEVEL);

        foreach (var constructor in constructors)
        {
            Console.WriteLine(SECOND_LEVEL + "Name: " + constructor.Name);
            Console.WriteLine(THIRD_LEVEL + "Attributes: " + constructor.Attributes);

            if (constructor.GetParameters().Length > 0)
            {
                string[] paramsInfo = constructor
                    .GetParameters()
                    .Select(p => "(" + p.ParameterType + ") " + p.Name)
                    .ToArray();

                Console.WriteLine(THIRD_LEVEL + "Parameters: " + string.Join(", ", paramsInfo));
            }

            Console.WriteLine(SECOND_LEVEL);
        }

        Console.WriteLine(FIRST_LEVEL + "Total number: " + cntConstructors);
        Console.WriteLine(FIRST_LEVEL);
    }
    
    // Вывод информации о сборке и её пользовательск(ом/их) тип(е/ах)
    private static void PrintInfo(Type? type, Assembly? assembly, BindingFlags flags)
    {
        // Определение типа выводимой информации
        var typePrintInfo = (type, assembly) switch
        {
            (null, null) => TypePrintInfo.Zero,
            (not null, null) => TypePrintInfo.OnlyType,
            (null, not null) => TypePrintInfo.OnlyAssembly,
            (not null, not null) => TypePrintInfo.FullInfo
        };

        // Проверка на null
        if (typePrintInfo == TypePrintInfo.Zero)
            return;

        // Получение сборки по типу (если она не была указана)
        if (typePrintInfo == TypePrintInfo.OnlyType)
            assembly = type!.Assembly;
        
        // Получение имени сборки
        string? assemblyName = assembly!.GetName()!.Name;

        // Установка цвета вывода
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        // Вывод заголовка сборки
        Console.WriteLine("\nThe composition of the assembly \"" + assemblyName + ".dll\":");
        Console.WriteLine(ZERO_LEVEL);

        // Вывод информации об одном типе (либо если он указан вместе со сборкой, либо если он указан один)
        if (typePrintInfo == TypePrintInfo.OnlyType || typePrintInfo == TypePrintInfo.FullInfo)
            PrintTypeInfo(type!, flags);
        // Если же указана только сборка, то вывод информации о всех её типах
        else
        {
            foreach (var type_ in assembly!.GetTypes())
                PrintTypeInfo(type_!, flags);
        }

        // Завершение блока информации о сборке и её тип(е/ах)
        Console.WriteLine("End of assembly");

        // Сброс цвета
        Console.ResetColor();
    }

    // Перечисление для определения типа вывода
    private enum TypePrintInfo
    {
        Zero = 0,
        OnlyType = 1,
        OnlyAssembly = 2,
        FullInfo = 3
    }

    /// <summary>
    /// Выводит рефлексивную информацию о переданном в параметр типе
    /// </summary>
    /// <param name="type">Тип для рефлексивного анализа</param>
    /// /// <param name="flags">Флаги получения полей, методов и конструкторов</param>
    public static void Print(Type? type, BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    {
        // Проверка параметра на null
        if (type == null)
            return;

        // Вывод информации об одном типе
        PrintInfo(type, null, flags);
    }

    /// <summary>
    /// Выводит рефлексивную информацию о всех типах внутри переданной сборки
    /// </summary>
    /// <param name="assembly">Ссылка на сборку</param>
    /// <param name="flags">Флаги получения полей, методов и конструкторов</param>
    public static void Print(Assembly? assembly, BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    {
        // Проверка на наличие сборки для анализа
        if (assembly == null)
            return;

        // Вывод информации о всех типах в сборке
        PrintInfo(null, assembly, flags);
    }

    /// <summary>
    /// Выводит рефлексивную информацию о всех типах в сборке, путь которой указан в параметре
    /// </summary>
    /// <param name="path">Путь до сборки</param>
    /// <param name="flags">Флаги получения полей, методов и конструкторов</param>
    public static void Print(string? path, BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    {
        // Проверка на наличие переданного пути
        if (string.IsNullOrEmpty(path))
            return;

        // Если получилось открыть сборку, то выводим информацию о ней, если не получилось, то вывод ошибки
        if (TryOpenAssembly(path, out string? error, out Assembly? assembly))
            PrintInfo(null, assembly, flags);
        else
            Console.WriteLine(error);
    }

    // Попытка открыть сборку по указанному пути
    private static bool TryOpenAssembly(string path, out string? errorInfo, out Assembly? assembly)
    {
        try
        {
            // Попытка открытия сборки
            assembly = Assembly.LoadFrom(path);

            // Если сборка открылась корректно
            errorInfo = null;
            return true;
        }
        catch (Exception ex)
        {
            (assembly, errorInfo) = (null, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Выводит рефлексивную информацию о пользовательском типе, который содержится в сборке по указанному пути
    /// </summary>
    /// <param name="path">Путь до сборки</param>
    /// <param name="typeName">Название пользовательского типа</param>
    /// <param name="flags">Флаги получения полей, методов и конструкторов</param>
    public static void Print(string? path, string? typeName, BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    {
        // Проверка на наличие переданного пути и названия тип
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(typeName))
            return;

        // Если получилось открыть сборку и получить тип из указанного имени, то выводим информацию об этом типе
        if (TryOpenAssembly(path, out string? error, out Assembly? assembly) && TryGetTypeFromAssembly(assembly!, typeName, out Type? type))
            PrintInfo(type, assembly, flags);
        else
            Console.WriteLine(error);
    }

    // Попытка получить тип по его названию из указанной сборки
    private static bool TryGetTypeFromAssembly(Assembly assembly, string typeName, out Type? type)
    {
        // Попытка получить тип из сборки не меняя имя
        if ((type = assembly.GetType(typeName, false)) == null)
        {
            // Если у typeName отсутствует namespace, добавляем вручную и пробуем снова получить тип по имени
            typeName = assembly.GetName().Name + '.' + typeName;
            if ((type = assembly.GetType(typeName, false)) == null)
                return false;
        }

        // При успехе
        return true;
    }
}