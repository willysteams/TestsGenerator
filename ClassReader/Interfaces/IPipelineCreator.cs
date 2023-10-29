using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassReader.Interfaces
{
	public interface IPipelineCreator
	{
		void AddSourseItem(ISourcePipelineItem pipelineItem);
		void AddItem(IPipelineItem pipelineItem);
		void AddTargetItem(ITargetPipelineItem pipelineItem);
	}
}
