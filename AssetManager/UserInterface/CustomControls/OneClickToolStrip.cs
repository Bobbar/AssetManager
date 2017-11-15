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
namespace AssetManager.UserInterface.CustomControls
{
	/// <summary>
	/// Modified Toolstrip that responds to clicks as soon as the parent form is activated. As opposed to requiring two clicks (one to activate, another to focus).
	/// </summary>
	public partial class OneClickToolStrip
	{

		public OneClickToolStrip() : base()
		{
			// This call is required by the designer.
			InitializeComponent();

			// Add any initialization after the InitializeComponent() call.

		}

	}
}
