using ClassReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ClassReader.IOServices
{
	public class FileReader : ISourcePipelineItem
	{
		private readonly int _maxReadThreads;
		private TransformManyBlock<string, string> startBlock;

		public FileReader(int maxReadThreads)
		{
			_maxReadThreads = maxReadThreads;
		}

		public ISourceBlock<string> GetItem()
		{
			var directoryReader = CreateReadDirectoryBlock();
			var reader = CreateReadFileBlock(_maxReadThreads);

			startBlock = directoryReader;

			var opt = new DataflowLinkOptions { PropagateCompletion = true };
			directoryReader.LinkTo(reader, opt);
			return reader;
		}

		private TransformManyBlock<string, string> CreateReadDirectoryBlock()
		{
			return new TransformManyBlock<string, string>(ReadDirectory);
		}

		private string[] ReadDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				throw new ArgumentException("Directory doesn't exist");
			}

			return Directory.EnumerateFiles(path).ToArray();
		}

		private TransformBlock<string, string> CreateReadFileBlock(int maxFiles)
		{
			var opt = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFiles };
			return new TransformBlock<string, string>(ReadFileAsync, opt);
		}

		private async Task<string> ReadFileAsync(string path)
		{
			if (!File.Exists(path))
			{
				throw new ArgumentException("File doesn't exist");
			}

			var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096,
				FileOptions.Asynchronous);
			using var reader = new StreamReader(fs, Encoding.UTF8);
			return await reader.ReadToEndAsync();
		}

		public void StartPipeline(string input)
		{
			startBlock.Post(input); 
			startBlock.Complete();
		}
	}
}
