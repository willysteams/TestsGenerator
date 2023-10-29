using ClassReader;
using Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks.Dataflow;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tests
{
	public class Tests
	{
		private IPipelineItem _testGenerator;

		[SetUp]
		public void Setup()
		{
			var generator = new TestGeneratorService(5);
			_testGenerator = generator;	
		}

		[Test]
		public async Task TestGeneratorWithEmptyString()
		{
			var item = _testGenerator.GetItem();

			item.Post("");
			item.Complete();
			await item.Completion;
			try
			{
				var result = await item.ReceiveAsync();
			}
			catch (InvalidOperationException ex)
			{
				Assert.Pass(ex.Message);
			}
			Assert.Fail();		
		}

		[Test]
		public async Task TestGeneratorWithStringWithoutClass()
		{
			var item = _testGenerator.GetItem();

			item.Post(" class dsfsdfsd fdsfs s df sd");
			item.Complete();
			await item.Completion;
			try
			{
				var result = await item.ReceiveAsync();
			}
			catch (InvalidOperationException ex)
			{
				Assert.Pass(ex.Message);
			}
			Assert.Fail();
		}

		[Test]
		public async Task TestGeneratorWithFullClass()
		{
			var writeText = File.ReadAllText(@"..\..\..\input\Test1.cs");
			var item = _testGenerator.GetItem();

			item.Post(writeText);
			item.Complete();
			await item.OutputAvailableAsync();		
			var result = item.Receive();

			var classes = CSharpSyntaxTree.ParseText(result).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
				.Where(@class => @class.Modifiers.Any(SyntaxKind.PublicKeyword))
				.Where(@class => !@class.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

			var methods = CSharpSyntaxTree.ParseText(result).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();

			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result);
			Assert.Multiple(() =>
			{
				Assert.That(classes, Has.Length.EqualTo(1));
				Assert.That(methods, Has.Length.EqualTo(3));
			});
		}
		
		[Test]
		public async Task TestGeneratorWithEmptyClass()
		{
			var writeText = File.ReadAllText(@"..\..\..\TestClasses\ClassWithoutMethods.cs");
			var item = _testGenerator.GetItem();

			item.Post(writeText);
			item.Complete();
			await item.OutputAvailableAsync();		
			var result = item.Receive();

			var classes = CSharpSyntaxTree.ParseText(result).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
				.Where(@class => @class.Modifiers.Any(SyntaxKind.PublicKeyword))
				.Where(@class => !@class.Modifiers.Any(SyntaxKind.StaticKeyword)).ToArray();

			var methods = CSharpSyntaxTree.ParseText(result).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();

			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result);
			Assert.Multiple(() =>
			{
				Assert.That(classes, Has.Length.EqualTo(1));
				Assert.That(methods, Is.Empty);
			});
		}
	}
}