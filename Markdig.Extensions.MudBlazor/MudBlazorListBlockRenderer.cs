using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Extensions.MudBlazor;


public class MudBlazorListBlockRenderer : HtmlObjectRenderer<ListBlock>
{
    protected override void Write(HtmlRenderer renderer, ListBlock obj)
    {
        renderer.EnsureLine();

        if (renderer.EnableHtmlForBlock)
        {
            if (obj.Parent is MarkdownDocument)
            {
                renderer.Write($"<div class=\"mud-paper mud-elevation-3 pa-4 my-2\">");
                renderer.Write($"<div class=\"mud-list mud-list-padding\">");
            }
        }

        foreach (var item in obj)
        {
            var listItem = (ListItemBlock)item;

            renderer.EnsureLine();

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write($"<div tabindex=\"0\" class=\"mud-list-item mud-list-item-gutters\">");
                renderer.Write($"<div class=\"mud-list-item-text\">");
            }

            ContainerBlock children = listItem;

            var nested = children.Count > 1;

            if (children.Count > 0)
                renderer.Write(children[0]);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("</div>");
                renderer.Write("</div>");

                if (nested)
                {
                    renderer.Write($"<div class=\"mud-collapse-container mud-collapse-entered\" style=\"height:auto;\">");
                    renderer.Write("<div class=\"mud-collapse-wrapper\">");
                    renderer.Write("<div class=\"mud-collapse-wrapper-inner\">");
                    renderer.Write("<div class=\"mud-list mud-nested-list\">");
                }
            }
            if (children.Count > 0)
                listItem.Remove(children[0]);

            renderer.WriteChildren(listItem);

            if (renderer.EnableHtmlForBlock)
            {
                if (nested)
                {
                    renderer.Write("</div>");
                    renderer.Write("</div>");
                    renderer.Write("</div>");
                    renderer.Write("</div>");
                }
            }

            renderer.EnsureLine();
        }

        if (renderer.EnableHtmlForBlock)
        {
            if (obj.Parent is MarkdownDocument)
            {
                renderer.Write("</div>");
                renderer.Write("</div>");
            }
        }

        renderer.EnsureLine();
    }
}