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
	/// <summary>
	/// Custom form with project specific properties and methods.
	/// </summary>
	public class ExtendedForm : Form
	{
        private bool inheritTheme = false;

        public bool InheritTheme
        {
            get
            {
                return this.inheritTheme;
            }
            set
            {
                this.inheritTheme = value;
            }
        }

		private ExtendedForm myParentForm;
		/// <summary>
		/// Unique identifying string used to locate specific instances of this form.
		/// </summary>
		/// <returns></returns>
		public GridTheme GridTheme { get; set; }

		/// <summary>
		/// Gets or sets the Grid Theme for the DataGridView controls within the form.
		/// </summary>
		/// <returns></returns>
		public string FormUID { get; set; }

		/// <summary>
		/// Overloads the stock ParentForm property with a read/writable one. And also sets the icon and <seealso cref="GridTheme"/> from the parent form.
		/// </summary>
		/// <returns></returns>
        
		public new ExtendedForm ParentForm
        {
            get
            {
                return myParentForm;
            }

            set
            {
                this.myParentForm = value;
                if (inheritTheme)
                {
                    SetTheme(this.myParentForm);
                }
            }

        }

      

		public ExtendedForm()
		{
		}

		public ExtendedForm(ExtendedForm parentForm)
		{
			this.ParentForm = parentForm;
            SetTheme(parentForm);
		}

        public ExtendedForm(ExtendedForm parentForm, bool inheritTheme = true)
        {

            this.inheritTheme = inheritTheme;
            this.ParentForm = parentForm;
        }


        public virtual bool OKToClose()
		{
			return true;
		}

		/// <summary>
		/// Override and add code to refresh data from the database.
		/// </summary>
		public virtual void RefreshData()
		{
			this.Refresh();
		}

        private void SetTheme(ExtendedForm parentForm)
        {
            Icon = parentForm.Icon;
            GridTheme = parentForm.GridTheme;
        }


    }
}
