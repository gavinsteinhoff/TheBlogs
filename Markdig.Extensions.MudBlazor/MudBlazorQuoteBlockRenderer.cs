using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Extensions.MudBlazor;


public class MudBlazorQuoteBlockRenderer : HtmlObjectRenderer<QuoteBlock>
{
    protected override void Write(HtmlRenderer renderer, QuoteBlock obj)
    {
        renderer.EnsureLine();
        if (renderer.EnableHtmlForBlock)
        {

            if (obj.Count > 0)
            {
                renderer.Write(GenerateQuote());
                renderer.Write(obj[0]);
                renderer.WriteLine("</p>");
                obj.RemoveAt(0);

            }
        }
        renderer.WriteChildren(obj);
        if (renderer.EnableHtmlForBlock)
        {
            renderer.WriteLine("</div>");
        }
        renderer.EnsureLine();
    }

    private string GenerateQuote()
    {
        return @"
<div class=""mud-paper mud-elevation-3 pa-4 my-2"">
    <svg class=""mud-icon-root mud-svg-icon mud-inherit-text mud-icon-size-medium"" focusable=""false""
        viewBox=""0 0 24 24"" aria-hidden=""true"">
        <title>Quote</title>
        <path d=""M0 0h24v24H0z"" fill=""none""></path>
        <path d=""M6 17h3l2-4V7H5v6h3zm8 0h3l2-4V7h-6v6h3z""></path>
    </svg>
    <p class=""mud-typography mud-typography-body1 mud-inherit-text"">
";
    }
}