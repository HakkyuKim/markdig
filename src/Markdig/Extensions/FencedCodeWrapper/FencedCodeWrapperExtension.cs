using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Extensions.FencedCodeWrapper
{
    public class FencedCodeWrapperExtension : IMarkdownExtension
    {
        private FencedCodeBlockParser _fencedCodeBlockParser;

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Add an additional method to the InfoParser of the default FencedCodeBlockParser
            if (pipeline.BlockParsers.Contains<FencedCodeBlockParser>())
            {
                _fencedCodeBlockParser = pipeline.BlockParsers.Find<FencedCodeBlockParser>();
                _fencedCodeBlockParser.InfoParser += AddAttributesFromParsedInfo;
            }
        }

        private bool AddAttributesFromParsedInfo(BlockProcessor state, ref StringSlice line, IFencedBlock fenced, char openingCharacter)
        {
            // Add the language as an attribute by default
            fenced.GetAttributes().AddClass(_fencedCodeBlockParser.InfoPrefix + fenced.Info);

            // Also add the arguments as an attribute
            fenced.GetAttributes().AddClass(fenced.Arguments);

            // Set Info to null to avoid duplicate parsing
            fenced.Info = null;

            return true;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // Switch the renderer for the Codeblock syntax
            if (renderer.ObjectRenderers.Contains<CodeBlockRenderer>())
            {
                renderer.ObjectRenderers.TryRemove<CodeBlockRenderer>();
                renderer.ObjectRenderers.Add(new FencedCodeWrapperRenderer());
            }
        }
    }
}
