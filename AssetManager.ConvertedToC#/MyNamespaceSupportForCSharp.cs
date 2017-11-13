using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;

// This file was created by the VB to C# converter (SharpDevelop 4.4.2.9749).
// It contains classes for supporting the VB "My" namespace in C#.
// If the VB application does not use the "My" namespace, or if you removed the usage
// after the conversion to C#, you can delete this file.

namespace AssetManager.My
{
	sealed partial class MyProject
	{
		[ThreadStatic] static MyApplication application;
		
		public static MyApplication Application {
			[DebuggerStepThrough]
			get {
				if (application == null)
					application = new MyApplication();
				return application;
			}
		}
		
		[ThreadStatic] static MyComputer computer;
		
		public static MyComputer Computer {
			[DebuggerStepThrough]
			get {
				if (computer == null)
					computer = new MyComputer();
				return computer;
			}
		}
		
		[ThreadStatic] static User user;
		
		public static User User {
			[DebuggerStepThrough]
			get {
				if (user == null)
					user = new User();
				return user;
			}
		}
		
		[ThreadStatic] static MyForms forms;
		
		public static MyForms Forms {
			[DebuggerStepThrough]
			get {
				if (forms == null)
					forms = new MyForms();
				return forms;
			}
		}
		
		internal sealed class MyForms
		{
			global::AssetManager.UpdateDev UpdateDev_instance;
			bool UpdateDev_isCreating;
			public global::AssetManager.UpdateDev UpdateDev {
				[DebuggerStepThrough] get { return GetForm(ref UpdateDev_instance, ref UpdateDev_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref UpdateDev_instance, value); }
			}
			
			global::AssetManager.NewDeviceForm NewDeviceForm_instance;
			bool NewDeviceForm_isCreating;
			public global::AssetManager.NewDeviceForm NewDeviceForm {
				[DebuggerStepThrough] get { return GetForm(ref NewDeviceForm_instance, ref NewDeviceForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref NewDeviceForm_instance, value); }
			}
			
			global::AssetManager.SibiManageRequestForm SibiManageRequestForm_instance;
			bool SibiManageRequestForm_isCreating;
			public global::AssetManager.SibiManageRequestForm SibiManageRequestForm {
				[DebuggerStepThrough] get { return GetForm(ref SibiManageRequestForm_instance, ref SibiManageRequestForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref SibiManageRequestForm_instance, value); }
			}
			
			global::AssetManager.CopyFilesForm CopyFilesForm_instance;
			bool CopyFilesForm_isCreating;
			public global::AssetManager.CopyFilesForm CopyFilesForm {
				[DebuggerStepThrough] get { return GetForm(ref CopyFilesForm_instance, ref CopyFilesForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref CopyFilesForm_instance, value); }
			}
			
			global::AssetManager.SibiMainForm SibiMainForm_instance;
			bool SibiMainForm_isCreating;
			public global::AssetManager.SibiMainForm SibiMainForm {
				[DebuggerStepThrough] get { return GetForm(ref SibiMainForm_instance, ref SibiMainForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref SibiMainForm_instance, value); }
			}
			
			global::AssetManager.CrypterForm CrypterForm_instance;
			bool CrypterForm_isCreating;
			public global::AssetManager.CrypterForm CrypterForm {
				[DebuggerStepThrough] get { return GetForm(ref CrypterForm_instance, ref CrypterForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref CrypterForm_instance, value); }
			}
			
			global::AssetManager.GKUpdaterForm GKUpdaterForm_instance;
			bool GKUpdaterForm_isCreating;
			public global::AssetManager.GKUpdaterForm GKUpdaterForm {
				[DebuggerStepThrough] get { return GetForm(ref GKUpdaterForm_instance, ref GKUpdaterForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref GKUpdaterForm_instance, value); }
			}
			
			global::AssetManager.UserManagerForm UserManagerForm_instance;
			bool UserManagerForm_isCreating;
			public global::AssetManager.UserManagerForm UserManagerForm {
				[DebuggerStepThrough] get { return GetForm(ref UserManagerForm_instance, ref UserManagerForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref UserManagerForm_instance, value); }
			}
			
			global::AssetManager.AdvancedSearchForm AdvancedSearchForm_instance;
			bool AdvancedSearchForm_isCreating;
			public global::AssetManager.AdvancedSearchForm AdvancedSearchForm {
				[DebuggerStepThrough] get { return GetForm(ref AdvancedSearchForm_instance, ref AdvancedSearchForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref AdvancedSearchForm_instance, value); }
			}
			
			global::AssetManager.PackFileForm PackFileForm_instance;
			bool PackFileForm_isCreating;
			public global::AssetManager.PackFileForm PackFileForm {
				[DebuggerStepThrough] get { return GetForm(ref PackFileForm_instance, ref PackFileForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref PackFileForm_instance, value); }
			}
			
			global::AssetManager.MunisUserForm MunisUserForm_instance;
			bool MunisUserForm_isCreating;
			public global::AssetManager.MunisUserForm MunisUserForm {
				[DebuggerStepThrough] get { return GetForm(ref MunisUserForm_instance, ref MunisUserForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref MunisUserForm_instance, value); }
			}
			
			global::AssetManager.MainForm MainForm_instance;
			bool MainForm_isCreating;
			public global::AssetManager.MainForm MainForm {
				[DebuggerStepThrough] get { return GetForm(ref MainForm_instance, ref MainForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref MainForm_instance, value); }
			}
			
			global::AssetManager.GridForm GridForm_instance;
			bool GridForm_isCreating;
			public global::AssetManager.GridForm GridForm {
				[DebuggerStepThrough] get { return GetForm(ref GridForm_instance, ref GridForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref GridForm_instance, value); }
			}
			
			global::AssetManager.ExtendedForm ExtendedForm_instance;
			bool ExtendedForm_isCreating;
			public global::AssetManager.ExtendedForm ExtendedForm {
				[DebuggerStepThrough] get { return GetForm(ref ExtendedForm_instance, ref ExtendedForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ExtendedForm_instance, value); }
			}
			
			global::AssetManager.ViewDeviceForm ViewDeviceForm_instance;
			bool ViewDeviceForm_isCreating;
			public global::AssetManager.ViewDeviceForm ViewDeviceForm {
				[DebuggerStepThrough] get { return GetForm(ref ViewDeviceForm_instance, ref ViewDeviceForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ViewDeviceForm_instance, value); }
			}
			
			global::AssetManager.SibiNotesForm SibiNotesForm_instance;
			bool SibiNotesForm_isCreating;
			public global::AssetManager.SibiNotesForm SibiNotesForm {
				[DebuggerStepThrough] get { return GetForm(ref SibiNotesForm_instance, ref SibiNotesForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref SibiNotesForm_instance, value); }
			}
			
			global::AssetManager.AttachmentsForm AttachmentsForm_instance;
			bool AttachmentsForm_isCreating;
			public global::AssetManager.AttachmentsForm AttachmentsForm {
				[DebuggerStepThrough] get { return GetForm(ref AttachmentsForm_instance, ref AttachmentsForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref AttachmentsForm_instance, value); }
			}
			
			global::AssetManager.GetCredentialsForm GetCredentialsForm_instance;
			bool GetCredentialsForm_isCreating;
			public global::AssetManager.GetCredentialsForm GetCredentialsForm {
				[DebuggerStepThrough] get { return GetForm(ref GetCredentialsForm_instance, ref GetCredentialsForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref GetCredentialsForm_instance, value); }
			}
			
			global::AssetManager.SibiSelectorForm SibiSelectorForm_instance;
			bool SibiSelectorForm_isCreating;
			public global::AssetManager.SibiSelectorForm SibiSelectorForm {
				[DebuggerStepThrough] get { return GetForm(ref SibiSelectorForm_instance, ref SibiSelectorForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref SibiSelectorForm_instance, value); }
			}
			
			global::AssetManager.SplashScreenForm SplashScreenForm_instance;
			bool SplashScreenForm_isCreating;
			public global::AssetManager.SplashScreenForm SplashScreenForm {
				[DebuggerStepThrough] get { return GetForm(ref SplashScreenForm_instance, ref SplashScreenForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref SplashScreenForm_instance, value); }
			}
			
			global::AssetManager.TrackDeviceForm TrackDeviceForm_instance;
			bool TrackDeviceForm_isCreating;
			public global::AssetManager.TrackDeviceForm TrackDeviceForm {
				[DebuggerStepThrough] get { return GetForm(ref TrackDeviceForm_instance, ref TrackDeviceForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref TrackDeviceForm_instance, value); }
			}
			
			global::AssetManager.ViewHistoryForm ViewHistoryForm_instance;
			bool ViewHistoryForm_isCreating;
			public global::AssetManager.ViewHistoryForm ViewHistoryForm {
				[DebuggerStepThrough] get { return GetForm(ref ViewHistoryForm_instance, ref ViewHistoryForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ViewHistoryForm_instance, value); }
			}
			
			global::AssetManager.ViewTrackingForm ViewTrackingForm_instance;
			bool ViewTrackingForm_isCreating;
			public global::AssetManager.ViewTrackingForm ViewTrackingForm {
				[DebuggerStepThrough] get { return GetForm(ref ViewTrackingForm_instance, ref ViewTrackingForm_isCreating); }
				[DebuggerStepThrough] set { SetForm(ref ViewTrackingForm_instance, value); }
			}
			
			[DebuggerStepThrough]
			static T GetForm<T>(ref T instance, ref bool isCreating) where T : Form, new()
			{
				if (instance == null || instance.IsDisposed) {
					if (isCreating) {
						throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate", new string[0]));
					}
					isCreating = true;
					try {
						instance = new T();
					} catch (System.Reflection.TargetInvocationException ex) {
						throw new InvalidOperationException(Utils.GetResourceString("WinForms_SeeInnerException", new string[] { ex.InnerException.Message }), ex.InnerException);
					} finally {
						isCreating = false;
					}
				}
				return instance;
			}
			
			[DebuggerStepThrough]
			static void SetForm<T>(ref T instance, T value) where T : Form
			{
				if (instance != value) {
					if (value == null) {
						instance.Dispose();
						instance = null;
					} else {
						throw new ArgumentException("Property can only be set to null");
					}
				}
			}
		}
	}
	
	partial class MyApplication : WindowsFormsApplicationBase
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.SetCompatibleTextRenderingDefault(UseCompatibleTextRendering);
			MyProject.Application.Run(args);
		}
	}
	
	partial class MyComputer : Computer
	{
	}
}
