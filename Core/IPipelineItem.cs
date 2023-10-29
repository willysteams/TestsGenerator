using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ClassReader
{
	public interface IPipelineItem
	{
		IPropagatorBlock<string, string> GetItem();
	}
}
