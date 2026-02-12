namespace ExtraLib.Tests.Data;

public static class HtmlHelperTests
{
    [Test]
    public static void TagsClear_FromHtml_ReturnClearedText()
    {
        string html = @"
<div class=""skill-card card-csharp"">
  <h3>База .NET/C#</h3>
  <ul>
    <li>ООП</li>
    <li>
      Generic-типы, в частности ковариативность и контрвариативность
    </li>
    <li>Коллекции</li>
  </ul>
</div>";
        string expected = "База .NET/C#\r\nООП\r\nGeneric-типы, в частности ковариативность и контрвариативность\r\nКоллекции";

        string actual = HtmlHelper.TagsClear(html);

        Assert.That(actual, Is.EqualTo(expected));
    }
}