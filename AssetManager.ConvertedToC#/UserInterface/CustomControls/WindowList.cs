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
using AssetManager.UserInterface.Forms.Attachments;
using AssetManager.UserInterface.Forms.AssetManager;
using AssetManager.UserInterface.Forms.Sibi;
using AssetManager.UserInterface.Forms.GK_Updater;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.CustomControls
{
    public class WindowList : IDisposable
    {

        #region "Fields"

        private Timer withEventsField_RefreshTimer;
        private Timer RefreshTimer
        {
            get { return withEventsField_RefreshTimer; }
            set
            {
                if (withEventsField_RefreshTimer != null)
                {
                    withEventsField_RefreshTimer.Tick -= RefreshTimer_Tick;
                }
                withEventsField_RefreshTimer = value;
                if (withEventsField_RefreshTimer != null)
                {
                    withEventsField_RefreshTimer.Tick += RefreshTimer_Tick;
                }
            }
        }
        private ToolStripDropDownButton DropDownControl = new ToolStripDropDownButton();
        private int intFormCount;

        private ExtendedForm MyParentForm;
        #endregion

        #region "Constructors"

        public WindowList(ExtendedForm parentForm)
        {
            MyParentForm = parentForm;
        }

        #endregion

        #region "Methods"

        public void InsertWindowList(OneClickToolStrip targetToolStrip)
        {
            InitializeDropDownButton(targetToolStrip);
            InitializeTimer();
            RefreshWindowList();
        }

        private void AddParentMenu()
        {
            if (MyParentForm.ParentForm != null)
            {
                ToolStripMenuItem ParentDropDown = NewMenuItem(MyParentForm.ParentForm);
                ParentDropDown.Text = "[Parent] " + ParentDropDown.Text;
                ParentDropDown.ToolTipText = "Parent Form";
                DropDownControl.DropDownItems.Insert(0, ParentDropDown);
                DropDownControl.DropDownItems.Add(new ToolStripSeparator());
            }
        }

        /// <summary>
        /// Recursively build ToolStripItemCollections of Forms and their Children and add them to the ToolStrip. Making sure to add SibiMain to the top of the list.
        /// </summary>
        /// <param name="ParentForm">Form to add to ToolStrip.</param>
        /// <param name="TargetMenuItem">Item to add the Form item to.</param>
        private void BuildWindowList(ExtendedForm parentForm, ToolStripItemCollection targetMenuItem)
        {
            foreach (ExtendedForm frm in Helpers.ChildFormControl.GetChildren(parentForm))
            {
                if (HasChildren(frm))
                {
                    ToolStripMenuItem NewDropDown = NewMenuItem(frm);
                    if (frm is SibiMainForm)
                    {
                        targetMenuItem.Insert(0, NewDropDown);
                    }
                    else
                    {
                        targetMenuItem.Add(NewDropDown);
                    }
                    BuildWindowList(frm, NewDropDown.DropDownItems);
                }
                else
                {
                    if (frm is SibiMainForm)
                    {
                        targetMenuItem.Insert(0, NewMenuItem(frm));
                    }
                    else
                    {
                        targetMenuItem.Add(NewMenuItem(frm));
                    }
                }
            }
        }

        private Form GetFormFromTag(object tag)
        {
            if (tag is Form)
            {
                return (Form)tag;
            }
            return null;
        }

        private string CountText(int count)
        {
            string MainText = "Select Window";
            if (count > 0)
            {
                return MainText + " (" + count + ")";
            }
            else
            {
                return MainText;
            }
        }

        private int FormCount(ExtendedForm parentForm)
        {
            int i = 0;
            foreach (ExtendedForm frm in Helpers.ChildFormControl.GetChildren(parentForm))
            {
                if (!frm.Modal & !object.ReferenceEquals(frm, parentForm))
                {
                    i += FormCount(frm) + 1;
                }
            }
            return i;
        }

        private bool HasChildren(ExtendedForm parentForm)
        {
            var Children = Helpers.ChildFormControl.GetChildren(parentForm);
            if (Children.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (ExtendedForm frm in Children)
                {
                    if (!frm.IsDisposed)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void InitializeDropDownButton(OneClickToolStrip targetToolStrip)
        {
            DropDownControl.Visible = false;
            DropDownControl.Font = targetToolStrip.Font;
            DropDownControl.Text = "Select Window";
            DropDownControl.Image = Properties.Resources.CascadeIcon;
            AddParentMenu();
            targetToolStrip.Items.Insert(targetToolStrip.Items.Count, DropDownControl);
        }

        private void InitializeTimer()
        {
            RefreshTimer = new Timer();
            RefreshTimer.Interval = 500;
            RefreshTimer.Enabled = true;
        }

        private ToolStripMenuItem NewMenuItem(Form frm)
        {
            ToolStripMenuItem newitem = new ToolStripMenuItem();
            newitem.Font = DropDownControl.Font;
            newitem.Text = frm.Text;
            newitem.Image = frm.Icon.ToBitmap();
            newitem.Tag = frm;
            newitem.ToolTipText = "Right-Click to close.";
            newitem.MouseDown += WindowClick;
            return newitem;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshWindowList();
        }

        private void RefreshWindowList()
        {
            var NumOfForms = FormCount(MyParentForm);
            if (MyParentForm.ParentForm == null & NumOfForms < 1)
            {
                DropDownControl.Visible = false;
            }
            else
            {
                DropDownControl.Visible = true;
            }
            if (NumOfForms != intFormCount)
            {
                if (!DropDownControl.DropDown.Focused)
                {
                    DropDownControl.DropDownItems.Clear();
                    AddParentMenu();
                    BuildWindowList(MyParentForm, DropDownControl.DropDownItems);
                    intFormCount = NumOfForms;
                    DropDownControl.Text = CountText(intFormCount);
                }
            }
        }

        private void WindowClick(object sender, MouseEventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            var frm = (ExtendedForm)item.Tag;
            if (e.Button == MouseButtons.Right)
            {
                if ((!object.ReferenceEquals(frm, MyParentForm.ParentForm)))
                {
                    frm.Close();
                    if (frm.Disposing | frm.IsDisposed)
                    {
                        DisposeDropDownItem(item);
                    }
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (!frm.IsDisposed)
                {
                    Helpers.ChildFormControl.ActivateForm(frm);
                }
                else
                {
                    DisposeDropDownItem(item);
                }
            }
        }

        private void DisposeDropDownItem(ToolStripItem item)
        {
            if (DropDownControl.DropDownItems.Count < 1)
            {
                DropDownControl.Visible = false;
                DropDownControl.DropDownItems.Clear();
                item.Dispose();
            }
            else
            {
                item.Visible = false;
                item.Dispose();
                DropDownControl.DropDownItems.Remove(item);
                intFormCount = FormCount(MyParentForm);
                DropDownControl.Text = CountText(intFormCount);
            }
        }

        #endregion

        #region "IDisposable Support"

        // To detect redundant calls
        private bool disposedValue;

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            // TODO: uncomment the following line if Finalize() is overridden above.
            // GC.SuppressFinalize(Me)
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    RefreshTimer.Dispose();
                    DropDownControl.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
            }
            disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        //Protected Overrides Sub Finalize()
        //    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        //    Dispose(False)
        //    MyBase.Finalize()
        //End Sub

        #endregion

    }
}
