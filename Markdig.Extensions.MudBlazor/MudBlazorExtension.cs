using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.MudBlazor;

public static class SyntaxHighlightingExtensions
{
    public static MarkdownPipelineBuilder UseMudBlazorExtension(this MarkdownPipelineBuilder pipeline)
    {
        pipeline.Extensions.Add(new MudBlazorExtension());
        return pipeline;
    }
}

public class MudBlazorExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.TryFind<CodeBlockRenderer>(out var codeBlockRenderer);
            if (codeBlockRenderer is null)
            {
                codeBlockRenderer = new CodeBlockRenderer();
            }
            codeBlockRenderer.OutputAttributesOnPre = true;

            htmlRenderer.ObjectRenderers.Add(new MudBlazorTableRenderer());

            if (htmlRenderer.ObjectRenderers.Contains<HtmlTableRenderer>())
                htmlRenderer.ObjectRenderers.TryRemove<HtmlTableRenderer>();

            htmlRenderer.ObjectRenderers.Add(new MudBlazorTableRenderer());

            if (htmlRenderer.ObjectRenderers.Contains<ListRenderer>())
                htmlRenderer.ObjectRenderers.TryRemove<ListRenderer>();

            htmlRenderer.ObjectRenderers.Add(new MudBlazorListBlockRenderer());

            if (htmlRenderer.ObjectRenderers.Contains<QuoteBlockRenderer>())
                htmlRenderer.ObjectRenderers.TryRemove<QuoteBlockRenderer>();

            htmlRenderer.ObjectRenderers.Add(new MudBlazorQuoteBlockRenderer());
        }
    }

    private static void PipelineOnDocumentProcessed(MarkdownDocument document)
    {
        foreach (var node in document.Descendants())
        {
            switch (node)
            {
                case ParagraphBlock block:
                    {
                        block.GetAttributes().AddClass("mud-typography mud-typography-body1 mud-inherit-text");
                        break;
                    }
                case HeadingBlock block:
                    {
                        var mudblazorLevel = block.Level + 3;
                        mudblazorLevel = mudblazorLevel > 6 ? 6 : mudblazorLevel;
                        block.GetAttributes().AddClass($"mud-typography mud-typography-h{mudblazorLevel} mud-inherit-text my-4");
                        break;
                    }
                case LinkInline block:
                    {
                        if (block.IsImage)
                        {
                            block.GetAttributes().AddClass("mud-image object-contain object-center mud-elevation-25 rounded-lg fluid");
                            //block.GetAttributes().AddProperty("style", "width:100%");
                        }
                        else
                        {
                            block.GetAttributes().AddClass("mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1");
                        }
                        break;
                    }
                case ThematicBreakBlock block:
                    {
                        block.GetAttributes().AddClass("mud-divider");
                        break;
                    }
                case CodeBlock block:
                    {
                        block.GetAttributes().AddClass("mud-paper mud-elevation-3 pa-4 my-1");
                        break;
                    }
            }
        }
    }
}