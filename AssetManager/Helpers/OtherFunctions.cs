using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using MyDialogLib;
namespace AssetManager
{

    static class OtherFunctions
    {
        public static Stopwatch stpw = new Stopwatch();

        private static int intTimerHits = 0;
        public static void StartTimer()
        {
            stpw.Stop();
            stpw.Reset();
            stpw.Start();
        }

        public static string StopTimer()
        {
            stpw.Stop();
            intTimerHits += 1;
            string Results = intTimerHits + "  Stopwatch: MS:" + stpw.ElapsedMilliseconds + " Ticks: " + stpw.ElapsedTicks;
            Debug.Print(Results);
            return Results;
        }

        public static string ElapTime()
        {
            string results = intTimerHits + "  Elapsed: MS:" + stpw.ElapsedMilliseconds + " Ticks: " + stpw.ElapsedTicks;
            Debug.Print(results);
            return results;
        }

        public static void EndProgram()
        {
            GlobalSwitches.ProgramEnding = true;
            Logging.Logger("Ending Program...");
            PurgeTempDir();
        }

        public static void PurgeTempDir()
        {
            try
            {
                Directory.Delete(Paths.DownloadPath, true);
            }
            catch
            {
            }
        }

        public static void AdjustComboBoxWidth(object sender, EventArgs e)
        {
            var senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth = (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;
            int newWidth = 0;
            foreach (string s in ((ComboBox)sender).Items)
            {
                newWidth = Convert.ToInt32(g.MeasureString(s, font).Width) + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }

        public static string NotePreview(string Note, int CharLimit = 50)
        {
            if (!string.IsNullOrEmpty(Note))
            {
                if (Note.Length > CharLimit)
                {
                    return Note.Substring(0, CharLimit) + "...";
                }
                else
                {
                    return Note;
                }
            }
            else
            {
                return "";
            }
        }

        public static DialogResult Message(string Prompt, int Buttons = (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, string Title = null, Form ParentFrm = null)
        {
            SetWaitCursor(false, ParentFrm);
            AdvancedDialog NewMessage = new AdvancedDialog(ParentFrm);
            return NewMessage.DialogMessage(Prompt, Buttons, Title, ParentFrm);
        }
        
        public static bool OKToEnd()
        {
            if (GlobalSwitches.BuildingCache)
            {
                Message("Still building DB Cache. Please wait and try again.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Critical Function Running");
                return false;
            }

            var GKUpdInstance = Helpers.ChildFormControl.GKUpdaterInstance();
            if (GKUpdInstance.Visible && !GKUpdInstance.OKToClose())
                return false;
            return true;

        }

        public delegate void SetWaitCursorVoidDelegate(bool waiting, Form parentForm = null);
        public static void SetWaitCursor(bool Waiting, Form parentForm = null)
        {
            if (parentForm == null)
            {
                Application.UseWaitCursor = Waiting;
                Application.DoEvents();
            }
            else
            {
                if (parentForm.InvokeRequired)
                {
                    SetWaitCursorVoidDelegate d = new SetWaitCursorVoidDelegate(SetWaitCursor);
                    parentForm.Invoke(d, new object[] { Waiting, parentForm });
                }
                else
                {
                    if (Waiting)
                    {
                        parentForm.Cursor = Cursors.AppStarting;
                    }
                    else
                    {
                        parentForm.Cursor = Cursors.Default;
                    }
                    parentForm.Update();
                }
            }
        }

        public static string RTFToPlainText(string rtfText)
        {
            try
            {
                if (rtfText.StartsWith("{\\rtf"))
                {
                    using (RichTextBox rtBox = new RichTextBox())
                    {
                        rtBox.Rtf = rtfText;
                        return rtBox.Text;
                    }
                }
                else
                {
                    return rtfText;
                }
            }
            catch (ArgumentException ex)
            {
                //If we get an argument error, that means the text is not RTF so we return the plain text.
                return rtfText;
            }
        }

        public static void SetRichTextBox(RichTextBox richTextBox, string text)
        {
            if (text.StartsWith("{\\rtf"))
            {
                richTextBox.Rtf = text;
            }
            else
            {
                richTextBox.Text = text;
            }
        }

    }
}
