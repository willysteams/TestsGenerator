using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassReader.Interfaces
{
	internal interface IPipeline
	{
		void Run(string input);
		Task IsFinished();
	}
}
