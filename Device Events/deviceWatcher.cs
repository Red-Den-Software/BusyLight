using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Busy_Light.Device_Events
{
    public class DeviceWatcher
    {
        public event EventHandler DeviceInserted;
        public event EventHandler DeviceRemoved;

        private ManagementEventWatcher insertWatcher;
        private ManagementEventWatcher removeWatcher;

        public DeviceWatcher()
        {
            initWatchers();
        }

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            DeviceInserted?.Invoke(this, EventArgs.Empty);
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            DeviceRemoved?.Invoke(this, EventArgs.Empty);
        }

        private void initWatchers()
        {
            var insertQuery = new WqlEventQuery(
                "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_SerialPort'");

            insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += DeviceInsertedEvent;
            insertWatcher.Start();

            var removeQuery = new WqlEventQuery(
                "SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_SerialPort'");

            removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += DeviceRemovedEvent;
            removeWatcher.Start();
        }
        public void Stop()
        {
            insertWatcher?.Stop();
            insertWatcher?.Dispose();

            removeWatcher?.Stop();
            removeWatcher?.Dispose();
        }
    }
}
