using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
namespace AssetManager
{
	public class NoPingException : Exception
	{

		public NoPingException()
		{
		}

		public NoPingException(string message) : base(message)
		{
		}

		public NoPingException(string message, Exception inner) : base(message, inner)
		{
		}

	}
}
