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
    public class DeviceObject : DataMapping
    {


        public DeviceObject()
        {
        }

        public DeviceObject(object data)
        {
            this.MapClassProperties(this, data);
        }

        [DataColumnName(DevicesCols.AssetTag)]
        public string AssetTag { get; set; }

        [DataColumnName(DevicesCols.Description)]
        public string Description { get; set; }

        [DataColumnName(DevicesCols.EQType)]
        public string EquipmentType { get; set; }

        [DataColumnName(DevicesCols.Serial)]
        public string Serial { get; set; }

        [DataColumnName(DevicesCols.Location)]
        public string Location { get; set; }

        [DataColumnName(DevicesCols.CurrentUser)]
        public string CurrentUser { get; set; }

        [DataColumnName(DevicesCols.MunisEmpNum)]
        public string CurrentUserEmpNum { get; set; }

        [DataColumnName(DevicesCols.PurchaseDate)]
        public System.DateTime PurchaseDate { get; set; }

        [DataColumnName(DevicesCols.ReplacementYear)]
        public string ReplaceYear { get; set; }

        [DataColumnName(DevicesCols.OSVersion)]
        public string OSVersion { get; set; }

        [DataColumnName(DevicesCols.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataColumnName(DevicesCols.DeviceUID)]
        public string GUID { get; set; }

        [DataColumnName(DevicesCols.PO)]
        public string PO { get; set; }

        [DataColumnName(DevicesCols.Status)]
        public string Status { get; set; }

        [DataColumnName(DevicesCols.Trackable)]
        public bool IsTrackable { get; set; }

        [DataColumnName(DevicesCols.SibiLinkUID)]
        public string SibiLink { get; set; }

        [DataColumnName(DevicesCols.HostName)]
        public string HostName { get; set; }

        public string FiscalYear { get; set; }
        public string Note { get; set; }

        public DeviceTrackingObject Tracking { get; set; } = new DeviceTrackingObject();
        public DeviceHistoricalObject Historical { get; set; } = new DeviceHistoricalObject();

    }
}
namespace AssetManager
{

    public class RequestObject : DataMapping
    {

        public RequestObject()
        {
            this.GUID = System.Guid.NewGuid().ToString();
        }

        public RequestObject(object data)
        {
            this.MapClassProperties(this, data);
        }

        [DataColumnName(SibiRequestCols.UID)]
        public string GUID { get; set; }

        [DataColumnName(SibiRequestCols.RequestUser)]
        public string RequestUser { get; set; }

        [DataColumnName(SibiRequestCols.Description)]
        public string Description { get; set; }

        [DataColumnName(SibiRequestCols.DateStamp)]
        public System.DateTime DateStamp { get; set; }

        [DataColumnName(SibiRequestCols.NeedBy)]
        public System.DateTime NeedByDate { get; set; }

        [DataColumnName(SibiRequestCols.Status)]
        public string Status { get; set; }

        [DataColumnName(SibiRequestCols.Type)]
        public string RequestType { get; set; }

        [DataColumnName(SibiRequestCols.PO)]
        public string PO { get; set; }

        [DataColumnName(SibiRequestCols.RequisitionNumber)]
        public string RequisitionNumber { get; set; }

        [DataColumnName(SibiRequestCols.ReplaceAsset)]
        public string ReplaceAsset { get; set; }

        [DataColumnName(SibiRequestCols.ReplaceSerial)]
        public string ReplaceSerial { get; set; }

        [DataColumnName(SibiRequestCols.RequestNumber)]
        public string RequestNumber { get; set; }

        [DataColumnName(SibiRequestCols.RTNumber)]
        public string RTNumber { get; set; }

        public DataTable RequestItems;
    }
}
namespace AssetManager
{

    public class DeviceHistoricalObject : DataMapping
    {


        public DeviceHistoricalObject()
        {
        }

        public DeviceHistoricalObject(object data)
        {
            this.MapClassProperties(this, data);
        }

        [DataColumnName(HistoricalDevicesCols.ChangeType)]
        public string ChangeType { get; set; }

        [DataColumnName(HistoricalDevicesCols.HistoryEntryUID)]
        public string GUID { get; set; }

        [DataColumnName(HistoricalDevicesCols.Notes)]
        public string Note { get; set; }

        [DataColumnName(HistoricalDevicesCols.ActionUser)]
        public string ActionUser { get; set; }

        [DataColumnName(HistoricalDevicesCols.ActionDateTime)]
        public System.DateTime ActionDateTime { get; set; }

    }
}
namespace AssetManager
{

    public class DeviceTrackingObject : DataMapping
    {


        public DeviceTrackingObject()
        {
        }

        public DeviceTrackingObject(DataTable data)
        {
            this.MapClassProperties(this, data);
        }

        [DataColumnName(TrackablesCols.CheckoutTime)]
        public System.DateTime CheckoutTime { get; set; }

        [DataColumnName(TrackablesCols.DueBackDate)]
        public System.DateTime DueBackTime { get; set; }

        [DataColumnName(TrackablesCols.CheckinTime)]
        public System.DateTime CheckinTime { get; set; }

        [DataColumnName(TrackablesCols.CheckoutUser)]
        public string CheckoutUser { get; set; }

        [DataColumnName(TrackablesCols.CheckinUser)]
        public string CheckinUser { get; set; }

        [DataColumnName(TrackablesCols.UseLocation)]
        public string UseLocation { get; set; }

        [DataColumnName(TrackablesCols.Notes)]
        public string UseReason { get; set; }

        [DataColumnName(DevicesCols.CheckedOut)]
        public bool IsCheckedOut { get; set; }

        [DataColumnName(TrackablesCols.Notes)]
        public string CheckinNotes { get; set; }

        [DataColumnName(TrackablesCols.DeviceUID)]
        public string DeviceGUID { get; set; }

    }
}
namespace AssetManager
{

    public class AccessGroupObject : DataMapping
    {


        public AccessGroupObject()
        {
        }

        public AccessGroupObject(object data)
        {
            this.MapClassProperties(this, data);
        }

        [DataColumnName(SecurityCols.SecModule)]
        public string AccessModule { get; set; }

        [DataColumnName(SecurityCols.AccessLevel)]
        public int Level { get; set; }

        [DataColumnName(SecurityCols.Description)]
        public string Description { get; set; }

        [DataColumnName(SecurityCols.AvailOffline)]
        public bool AvailableOffline { get; set; }

    }
}
