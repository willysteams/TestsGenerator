using ClassReader.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Threading.Tasks.Dataflow;

namespace ClassReader.IOServices
{
	public class FileWriter : ITargetPipelineItem
	{
		private readonly int _maxWriteThreads;
		private readonly string _outputDirectory;

		private ActionBlock<string> lastBlock;
		public FileWriter(int maxWriteThreads, string outputDirectory)
		{
			_maxWriteThreads = maxWriteThreads;
			_outputDirectory = outputDirectory;
		}

		public ITargetBlock<string> GetItem()
		{
			var writer = CreateWriteFileBlock(_maxWriteThreads, _outputDirectory);
			lastBlock = writer;
			return writer;
		}

		private ActionBlock<string> CreateWriteFileBlock(int maxFiles, string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var opt = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFiles };
			return new ActionBlock<string>(text => WriteFileAsync(path, text), opt);
		}

		private Task WriteFileAsync(string path, string text)
		{
			var tree = CSharpSyntaxTree.ParseText(text);
			var fileName = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First().Identifier.Text;
			var filePath = Path.Combine(path, fileName + ".cs");
			using var outputFile = new StreamWriter(filePath);
			return outputFile.WriteAsync(text);
		}

		public Task IsFinished()
		{
			return lastBlock.Completion;
		}
	}
}
