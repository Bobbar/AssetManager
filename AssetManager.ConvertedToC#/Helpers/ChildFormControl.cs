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

namespace AssetManager.Helpers
{
    public static class ChildFormControl
    {

        public static void ActivateForm(ExtendedForm form)
        {
            if (!form.IsDisposed)
            {
                form.Show();
                form.Activate();
                form.WindowState = FormWindowState.Normal;
            }
        }

        public static bool AttachmentsIsOpen(ExtendedForm parentForm)
        {
            foreach (ExtendedForm frm in GetChildren(parentForm))
            {
                if (frm is AttachmentsForm & object.ReferenceEquals(frm.ParentForm, parentForm))
                {
                    ActivateForm(frm);
                    return true;
                }
            }
            return false;
        }

        public static void CloseChildren(ExtendedForm parentForm)
        {
            var Children = GetChildren(parentForm);
            if (Children.Count > 0)
            {
                foreach (ExtendedForm child in Children)
                {
                    child.Dispose();
                }
            }
            Children.Clear();
        }

        public static List<ExtendedForm> GetChildren(ExtendedForm parentForm)
        {
            return Application.OpenForms.OfType<ExtendedForm>().ToList().FindAll(f => object.ReferenceEquals(f.ParentForm, parentForm) & !f.IsDisposed);
        }

        public static void LookupDevice(ExtendedForm parentForm, DeviceObject device)
        {
            if (device.GUID != null)
            {
                if (!FormIsOpenByUID(typeof(ViewDeviceForm), device.GUID))
                {
                    ViewDeviceForm NewView = new ViewDeviceForm(parentForm, device.GUID);
                }
            }
            else
            {
                OtherFunctions.Message("Device not found.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", parentForm);
            }
        }

        public static void MinimizeChildren(ExtendedForm parentForm)
        {
            foreach (ExtendedForm child in GetChildren(parentForm))
            {
                child.WindowState = FormWindowState.Minimized;
            }
        }

        public static void RestoreChildren(ExtendedForm parentForm)
        {
            foreach (ExtendedForm child in GetChildren(parentForm))
            {
                child.WindowState = FormWindowState.Normal;
            }
        }

        public static bool SibiIsOpen()
        {
            if (Application.OpenForms.OfType<SibiMainForm>().Any())
            {
                return true;
            }
            return false;
        }

        public static ExtendedForm GetChildOfType(ExtendedForm parentForm, Type childType)
        {
            return GetChildren(parentForm).Find(f => f.GetType() == childType);
        }

        public static bool FormTypeIsOpen(Type formType)
        {
            foreach (ExtendedForm frm in Application.OpenForms)
            {
                if (frm.GetType() == formType)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Find a form by its type and returns it if found.
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        public static ExtendedForm FindFormByType(Type formType)
        {
            foreach (ExtendedForm frm in Application.OpenForms)
            {
                if (frm.GetType() == formType)
                    return frm;
            }
            return null;
        }

        public static bool FormIsOpenByUID(Type formType, string UID)
        {
            foreach (ExtendedForm frm in Application.OpenForms)
            {
                if (frm.GetType() == formType && frm.FormUID == UID)
                {
                    ActivateForm(frm);
                    return true;
                }
            }
            return false;
        }

        public static bool OKToCloseChildren(ExtendedForm parentForm)
        {
            bool CanClose = true;
            var frms = GetChildren(parentForm).ToArray();
            for (int i = 0; i <= frms.Length - 1; i++)
            {
                if (!frms[i].OKToClose())
                    CanClose = false;
            }
            return CanClose;
        }

        /// <summary>
        /// Returns the current instance of <see cref="GKUpdaterForm"/>. If one does not exists, creates new instance and returns it.
        /// </summary>
        /// <returns></returns>
        public static GKUpdaterForm GKUpdaterInstance()
        {
            GKUpdaterForm currentGKUpdInstance;
            //Check for current instance.
            if (!FormTypeIsOpen(typeof(GKUpdaterForm)))
            {
                //If no current instance, create a new one and return it.
                currentGKUpdInstance = new GKUpdaterForm();
                return currentGKUpdInstance;
            }
            else
            {
                //If an instance is found, return the current instance.
                currentGKUpdInstance = (GKUpdaterForm)FindFormByType(typeof(GKUpdaterForm));
                if (currentGKUpdInstance != null)
                {
                    return currentGKUpdInstance;
                }
            }
            return null;
        }


    }
}
