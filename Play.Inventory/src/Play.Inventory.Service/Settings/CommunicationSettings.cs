using System;

namespace Play.Inventory.Service.Settings
{
    public class CommunicationSettings
    {
        public CommunicationSettingEntry Catalog { get; set; }
    }
    
    public class CommunicationSettingEntry
    {
        public Uri Uri { get; set; }
    }
}