using ClassReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ClassReader.Models
{
	public class Pipeline
	{
		public ISourcePipelineItem SourceBlock { get; set; }
		public IPipelineItem PropagatorBlock { get; set; }
		public ITargetPipelineItem TargetBlock { get; set; }
	}
}
