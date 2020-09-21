using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Extensions.FencedCodeWrapper
{
    public class FencedCodeWrapperRenderer : CodeBlockRenderer
    {
        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            string wrapperClassName = null;
            var classList = obj.TryGetAttributes().Classes;

            // If there are more than one class name attributes,
            // wrap the fenced code block with the last class name and
            // remove the name from the list.
            if (classList != null && classList.Count > 1)
            {
                wrapperClassName = classList[classList.Count - 1];
                classList.RemoveAt(classList.Count - 1);
            }

            if (!string.IsNullOrEmpty(wrapperClassName) && renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div class=\"" + wrapperClassName + "\">");
            }

            base.Write(renderer, obj);

            if (!string.IsNullOrEmpty(wrapperClassName) && renderer.EnableHtmlForBlock)
            {
                renderer.Write("</div>");
            }
        }
    }
}
