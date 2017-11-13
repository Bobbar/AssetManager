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
	public partial class SibiNotesForm
	{

		private RequestObject MyRequest;
		public RequestObject Request {
			get { return MyRequest; }
		}

		public SibiNotesForm(ExtendedForm parentForm, RequestObject request)
		{
			InitializeComponent();
			this.ParentForm = parentForm;
			MyRequest = request;
			ShowDialog(parentForm);
		}

		public SibiNotesForm(ExtendedForm parentForm, string noteUID)
		{
			InitializeComponent();
			this.ParentForm = parentForm;
			FormUID = noteUID;
			ViewNote(noteUID);
		}

		private void ClearAll()
		{
			rtbNotes.Clear();
		}

		private void ViewNote(string noteUID)
		{
			try {
				cmdOK.Visible = false;
				rtbNotes.Clear();
				string NoteText = GlobalInstances.AssetFunc.GetSqlValue(SibiNotesCols.TableName, SibiNotesCols.NoteUID, noteUID, SibiNotesCols.Note);
				string NoteTimeStamp = GlobalInstances.AssetFunc.GetSqlValue(SibiNotesCols.TableName, SibiNotesCols.NoteUID, noteUID, SibiNotesCols.DateStamp);
				this.Text += " - " + NoteTimeStamp;
				OtherFunctions.SetRichTextBox(ref rtbNotes, NoteText);
				rtbNotes.ReadOnly = true;
				rtbNotes.BackColor = Color.White;
				Show();
				Activate();
			} catch (Exception ex) {
				ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
			}
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Abort;
			this.Dispose();
		}

		private void rtbNotes_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			Process.Start(e.LinkText);
		}

	}
}
