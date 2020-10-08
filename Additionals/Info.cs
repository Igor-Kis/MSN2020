﻿using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Additionals
{
    public sealed class Info
    {
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            byte[] bytes = GetHash(inputString);
            return GetHashString(bytes);
        }

        public static string GetHashString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte b in bytes)
            {
                if (i != 0 && i % 2 == 0)
                    sb.Append("-");
                sb.Append(b.ToString("X2"));
                i++;
            }

            return sb.ToString();
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            string result = "";
            foreach (var item in bytes)
            {
                result += ((result == "") ? "" : ",") + item.ToString();
            }
            return result;
        }
        
        #region Win32_
        //ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);
        //Win32_1394Controller
        //Win32_1394ControllerDevice
        //Win32_Account
        //Win32_AccountSID
        //Win32_ACE
        //Win32_ActionCheck
        //Win32_AllocatedResource
        //Win32_ApplicationCommandLine
        //Win32_ApplicationService
        //Win32_AssociatedBattery
        //Win32_AssociatedProcessorMemory
        //Win32_BaseBoard
        //Win32_BaseService
        //Win32_Battery
        //Win32_Binary
        //Win32_BindImageAction
        //Win32_BIOS
        //Win32_BootConfiguration
        //Win32_Bus
        //Win32_CacheMemory
        //Win32_CDROMDrive
        //Win32_CheckCheck
        //Win32_CIMLogicalDeviceCIMDataFile
        //Win32_ClassicCOMApplicationClasses
        //Win32_ClassicCOMClass
        //Win32_ClassicCOMClassSetting
        //Win32_ClassicCOMClassSettings
        //Win32_ClassInfoAction
        //Win32_ClientApplicationSetting
        //Win32_CodecFile
        //Win32_COMApplication
        //Win32_COMApplicationClasses
        //Win32_COMApplicationSettings
        //Win32_COMClass
        //Win32_ComClassAutoEmulator
        //Win32_ComClassEmulator
        //Win32_CommandLineAccess
        //Win32_ComponentCategory
        //Win32_ComputerSystem
        //Win32_ComputerSystemProcessor
        //Win32_ComputerSystemProduct
        //Win32_COMSetting
        //Win32_Condition
        //Win32_CreateFolderAction
        //Win32_CurrentProbe
        //Win32_DCOMApplication
        //Win32_DCOMApplicationAccessAllowedSetting
        //Win32_DCOMApplicationLaunchAllowedSetting
        //Win32_DCOMApplicationSetting
        //Win32_DependentService
        //Win32_Desktop
        //Win32_DesktopMonitor
        //Win32_DeviceBus
        //Win32_DeviceMemoryAddress
        //Win32_DeviceSettings
        //Win32_Directory
        //Win32_DirectorySpecification
        //Win32_DiskDrive
        //Win32_DiskDriveToDiskPartition
        //Win32_DiskPartition
        //Win32_DisplayConfiguration
        //Win32_DisplayControllerConfiguration
        //Win32_DMAChannel
        //Win32_DriverVXD
        //Win32_DuplicateFileAction
        //Win32_Environment
        //Win32_EnvironmentSpecification
        //Win32_ExtensionInfoAction
        //Win32_Fan
        //Win32_FileSpecification
        //Win32_FloppyController
        //Win32_FloppyDrive
        //Win32_FontInfoAction
        //Win32_Group
        //Win32_GroupUser
        //Win32_HeatPipe
        //Win32_IDEController
        //Win32_IDEControllerDevice
        //Win32_ImplementedCategory
        //Win32_InfraredDevice
        //Win32_IniFileSpecification
        //Win32_InstalledSoftwareElement
        //Win32_IRQResource
        //Win32_Keyboard
        //Win32_LaunchCondition
        //Win32_LoadOrderGroup
        //Win32_LoadOrderGroupServiceDependencies
        //Win32_LoadOrderGroupServiceMembers
        //Win32_LogicalDisk
        //Win32_LogicalDiskRootDirectory
        //Win32_LogicalDiskToPartition
        //Win32_LogicalFileAccess
        //Win32_LogicalFileAuditing
        //Win32_LogicalFileGroup
        //Win32_LogicalFileOwner
        //Win32_LogicalFileSecuritySetting
        //Win32_LogicalMemoryConfiguration
        //Win32_LogicalProgramGroup
        //Win32_LogicalProgramGroupDirectory
        //Win32_LogicalProgramGroupItem
        //Win32_LogicalProgramGroupItemDataFile
        //Win32_LogicalShareAccess
        //Win32_LogicalShareAuditing
        //Win32_LogicalShareSecuritySetting
        //Win32_ManagedSystemElementResource
        //Win32_MemoryArray
        //Win32_MemoryArrayLocation
        //Win32_MemoryDevice
        //Win32_MemoryDeviceArray
        //Win32_MemoryDeviceLocation
        //Win32_MethodParameterClass
        //Win32_MIMEInfoAction
        //Win32_MotherboardDevice
        //Win32_MoveFileAction
        //Win32_MSIResource
        //Win32_NetworkAdapter
        //Win32_NetworkAdapterConfiguration
        //Win32_NetworkAdapterSetting
        //Win32_NetworkClient
        //Win32_NetworkConnection
        //Win32_NetworkLoginProfile
        //Win32_NetworkProtocol
        //Win32_NTEventlogFile
        //Win32_NTLogEvent
        //Win32_NTLogEventComputer
        //Win32_NTLogEventLog
        //Win32_NTLogEventUser
        //Win32_ODBCAttribute
        //Win32_ODBCDataSourceAttribute
        //Win32_ODBCDataSourceSpecification
        //Win32_ODBCDriverAttribute
        //Win32_ODBCDriverSoftwareElement
        //Win32_ODBCDriverSpecification
        //Win32_ODBCSourceAttribute
        //Win32_ODBCTranslatorSpecification
        //Win32_OnBoardDevice
        //Win32_OperatingSystem
        //Win32_OperatingSystemQFE
        //Win32_OSRecoveryConfiguration
        //Win32_PageFile
        //Win32_PageFileElementSetting
        //Win32_PageFileSetting
        //Win32_PageFileUsage
        //Win32_ParallelPort
        //Win32_Patch
        //Win32_PatchFile
        //Win32_PatchPackage
        //Win32_PCMCIAController
        //Win32_Perf
        //Win32_PerfRawData
        //Win32_PerfRawData_ASP_ActiveServerPages
        //Win32_PerfRawData_ASPNET_114322_ASPNETAppsv114322
        //Win32_PerfRawData_ASPNET_114322_ASPNETv114322
        //Win32_PerfRawData_ASPNET_ASPNET
        //Win32_PerfRawData_ASPNET_ASPNETApplications
        //Win32_PerfRawData_IAS_IASAccountingClients
        //Win32_PerfRawData_IAS_IASAccountingServer
        //Win32_PerfRawData_IAS_IASAuthenticationClients
        //Win32_PerfRawData_IAS_IASAuthenticationServer
        //Win32_PerfRawData_InetInfo_InternetInformationServicesGlobal
        //Win32_PerfRawData_MSDTC_DistributedTransactionCoordinator
        //Win32_PerfRawData_MSFTPSVC_FTPService
        //Win32_PerfRawData_MSSQLSERVER_SQLServerAccessMethods
        //Win32_PerfRawData_MSSQLSERVER_SQLServerBackupDevice
        //Win32_PerfRawData_MSSQLSERVER_SQLServerBufferManager
        //Win32_PerfRawData_MSSQLSERVER_SQLServerBufferPartition
        //Win32_PerfRawData_MSSQLSERVER_SQLServerCacheManager
        //Win32_PerfRawData_MSSQLSERVER_SQLServerDatabases
        //Win32_PerfRawData_MSSQLSERVER_SQLServerGeneralStatistics
        //Win32_PerfRawData_MSSQLSERVER_SQLServerLatches
        //Win32_PerfRawData_MSSQLSERVER_SQLServerLocks
        //Win32_PerfRawData_MSSQLSERVER_SQLServerMemoryManager
        //Win32_PerfRawData_MSSQLSERVER_SQLServerReplicationAgents
        //Win32_PerfRawData_MSSQLSERVER_SQLServerReplicationDist
        //Win32_PerfRawData_MSSQLSERVER_SQLServerReplicationLogreader
        //Win32_PerfRawData_MSSQLSERVER_SQLServerReplicationMerge
        //Win32_PerfRawData_MSSQLSERVER_SQLServerReplicationSnapshot
        //Win32_PerfRawData_MSSQLSERVER_SQLServerSQLStatistics
        //Win32_PerfRawData_MSSQLSERVER_SQLServerUserSettable
        //Win32_PerfRawData_NETFramework_NETCLRExceptions
        //Win32_PerfRawData_NETFramework_NETCLRInterop
        //Win32_PerfRawData_NETFramework_NETCLRJit
        //Win32_PerfRawData_NETFramework_NETCLRLoading
        //Win32_PerfRawData_NETFramework_NETCLRLocksAndThreads
        //Win32_PerfRawData_NETFramework_NETCLRMemory
        //Win32_PerfRawData_NETFramework_NETCLRRemoting
        //Win32_PerfRawData_NETFramework_NETCLRSecurity
        //Win32_PerfRawData_Outlook_Outlook
        //Win32_PerfRawData_PerfDisk_PhysicalDisk
        //Win32_PerfRawData_PerfNet_Browser
        //Win32_PerfRawData_PerfNet_Redirector
        //Win32_PerfRawData_PerfNet_Server
        //Win32_PerfRawData_PerfNet_ServerWorkQueues
        //Win32_PerfRawData_PerfOS_Cache
        //Win32_PerfRawData_PerfOS_Memory
        //Win32_PerfRawData_PerfOS_Objects
        //Win32_PerfRawData_PerfOS_PagingFile
        //Win32_PerfRawData_PerfOS_Processor
        //Win32_PerfRawData_PerfOS_System
        //Win32_PerfRawData_PerfProc_FullImage_Costly
        //Win32_PerfRawData_PerfProc_Image_Costly
        //Win32_PerfRawData_PerfProc_JobObject
        //Win32_PerfRawData_PerfProc_JobObjectDetails
        //Win32_PerfRawData_PerfProc_Process
        //Win32_PerfRawData_PerfProc_ProcessAddressSpace_Costly
        //Win32_PerfRawData_PerfProc_Thread
        //Win32_PerfRawData_PerfProc_ThreadDetails_Costly
        //Win32_PerfRawData_RemoteAccess_RASPort
        //Win32_PerfRawData_RemoteAccess_RASTotal
        //Win32_PerfRawData_RSVP_ACSPerRSVPService
        //Win32_PerfRawData_Spooler_PrintQueue
        //Win32_PerfRawData_TapiSrv_Telephony
        //Win32_PerfRawData_Tcpip_ICMP
        //Win32_PerfRawData_Tcpip_IP
        //Win32_PerfRawData_Tcpip_NBTConnection
        //Win32_PerfRawData_Tcpip_NetworkInterface
        //Win32_PerfRawData_Tcpip_TCP
        //Win32_PerfRawData_Tcpip_UDP
        //Win32_PerfRawData_W3SVC_WebService
        //Win32_PhysicalMemory
        //Win32_PhysicalMemoryArray
        //Win32_PhysicalMemoryLocation
        //Win32_PNPAllocatedResource
        //Win32_PnPDevice
        //Win32_PnPEntity
        //Win32_PointingDevice
        //Win32_PortableBattery
        //Win32_PortConnector
        //Win32_PortResource
        //Win32_POTSModem
        //Win32_POTSModemToSerialPort
        //Win32_PowerManagementEvent
        //Win32_Printer
        //Win32_PrinterConfiguration
        //Win32_PrinterController
        //Win32_PrinterDriverDll
        //Win32_PrinterSetting
        //Win32_PrinterShare
        //Win32_PrintJob
        //Win32_PrivilegesStatus
        //Win32_Process
        //Win32_Processor
        //Win32_ProcessStartup
        //Win32_Product
        //Win32_ProductCheck
        //Win32_ProductResource
        //Win32_ProductSoftwareFeatures
        //Win32_ProgIDSpecification
        //Win32_ProgramGroup
        //Win32_ProgramGroupContents
        //Win32_ProgramGroupOrItem
        //Win32_Property
        //Win32_ProtocolBinding
        //Win32_PublishComponentAction
        //Win32_QuickFixEngineering
        //Win32_Refrigeration
        //Win32_Registry
        //Win32_RegistryAction
        //Win32_RemoveFileAction
        //Win32_RemoveIniAction
        //Win32_ReserveCost
        //Win32_ScheduledJob
        //Win32_SCSIController
        //Win32_SCSIControllerDevice
        //Win32_SecurityDescriptor
        //Win32_SecuritySetting
        //Win32_SecuritySettingAccess
        //Win32_SecuritySettingAuditing
        //Win32_SecuritySettingGroup
        //Win32_SecuritySettingOfLogicalFile
        //Win32_SecuritySettingOfLogicalShare
        //Win32_SecuritySettingOfObject
        //Win32_SecuritySettingOwner
        //Win32_SelfRegModuleAction
        //Win32_SerialPort
        //Win32_SerialPortConfiguration
        //Win32_SerialPortSetting
        //Win32_Service
        //Win32_ServiceControl
        //Win32_ServiceSpecification
        //Win32_ServiceSpecificationService
        //Win32_SettingCheck
        //Win32_Share
        //Win32_ShareToDirectory
        //Win32_ShortcutAction
        //Win32_ShortcutFile
        //Win32_ShortcutSAP
        //Win32_SID
        //Win32_SMBIOSMemory
        //Win32_SoftwareElement
        //Win32_SoftwareElementAction
        //Win32_SoftwareElementCheck
        //Win32_SoftwareElementCondition
        //Win32_SoftwareElementResource
        //Win32_SoftwareFeature
        //Win32_SoftwareFeatureAction
        //Win32_SoftwareFeatureCheck
        //Win32_SoftwareFeatureParent
        //Win32_SoftwareFeatureSoftwareElements
        //Win32_SoundDevice
        //Win32_StartupCommand
        //Win32_SubDirectory
        //Win32_SystemAccount
        //Win32_SystemBIOS
        //Win32_SystemBootConfiguration
        //Win32_SystemDesktop
        //Win32_SystemDevices
        //Win32_SystemDriver
        //Win32_SystemDriverPNPEntity
        //Win32_SystemEnclosure
        //Win32_SystemLoadOrderGroups
        //Win32_SystemLogicalMemoryConfiguration
        //Win32_SystemMemoryResource
        //Win32_SystemNetworkConnections
        //Win32_SystemOperatingSystem
        //Win32_SystemPartitions
        //Win32_SystemProcesses
        //Win32_SystemProgramGroups
        //Win32_SystemResources
        //Win32_SystemServices
        //Win32_SystemSetting
        //Win32_SystemSlot
        //Win32_SystemSystemDriver
        //Win32_SystemTimeZone
        //Win32_SystemUsers
        //Win32_TapeDrive
        //Win32_TemperatureProbe
        //Win32_Thread
        //Win32_TimeZone
        //Win32_Trustee
        //Win32_TypeLibraryAction
        //Win32_UninterruptiblePowerSupply
        //Win32_USBController
        //Win32_USBControllerDevice
        //Win32_UserAccount
        //Win32_UserDesktop
        //Win32_VideoConfiguration
        //Win32_VideoController
        //Win32_VideoSettings
        //Win32_VoltageProbe
        //Win32_WMIElementSetting
        //Win32_WMISetting
        #endregion

        public string processorIds = "";
        public string processorsCount = "";
        public string motherBoard = "";
        public string memoryIds = "";

        public string windowsId = "";

        //public string macAdresses = "";
        public string hdds = "";

        public string machineName = "";
        public string userName = "";
        public string userDomainName = "";

        public Info()
        {
            try
            {
                processorIds = GetProcessorsIds();
            }
            catch { processorIds = "error retriving processors Ids"; }
            try
            {
                processorsCount = Environment.ProcessorCount.ToString();
            }
            catch { processorsCount = "error retriving processors count"; }
            try
            {
                motherBoard = GetMotherboard();
            }
            catch { motherBoard = "error retriving motherBoard info"; }
            try
            {
                memoryIds = GetMemoryIds();
            }
            catch { memoryIds = "error retriving memory Ids"; }
            try
            {
                windowsId = GetWindowsID();
            }
            catch { windowsId = "error retriving windowsId"; }

            //macAdresses = GetMacAdresses();
            try
            {
                hdds = GetHDDs();
            }
            catch { hdds = "error retriving hdds info"; }
            try
            {
                machineName = Environment.MachineName;
            }
            catch { machineName = "error retriving machineName"; }
            try
            {
                userName = Environment.UserName;
            }
            catch { userName = "error retriving userName"; }
            try
            {
                userDomainName = Environment.UserDomainName;
            }
            catch { userDomainName = "error retriving userDomainName"; }
        }

        private string GetMemoryIds()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectCollection oCollection = searcher.Get();
            foreach (ManagementObject oManagementObject in oCollection)
            {
                result = string.Format("{0}{1}{2}", result, ((result == "") ? "" : ";"), (string)oManagementObject["SerialNumber"]);
            }
            return (result);
        }

        private static string GetProcessorsIds()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            ManagementObjectCollection oCollection = searcher.Get();
            foreach (ManagementObject oManagementObject in oCollection)
            {
                result = string.Format("{0}{1}{2}", result, ((result == "") ? "" : ";"), (string)oManagementObject["ProcessorId"]);
            }
            return (result);
        }

        private string GetMotherboard()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Product, SerialNumber FROM Win32_BaseBoard");
            foreach (ManagementObject obj in searcher.Get())
            {
                foreach (PropertyData data in obj.Properties)
                    result = string.Format("{0}{1}{2}", result, ((result == "") ? "" : ";"), data.Value);
            }
            return result;
        }

        private string GetHDDs()
        {
            string result = "";

            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject hdd in searcher.Get())
            {
                if (((string)hdd["MediaType"]).Contains("Fixed hard disk"))
                {
                    result += string.Format("{0}{1}{2}::{3}", result, ((result == "") ? "" : ";"), (hdd["Model"]), (hdd["SerialNumber"]).ToString().Trim());
                }
            }

            //var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            //foreach (ManagementObject hdd in searcher.Get())
            //{
            //    result += ((result == "") ? "" : Environment.NewLine) + (hdd["Model"]);
            //}
            //var searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            //foreach (ManagementObject hdd in searcher2.Get())
            //{
            //    if (hdd["SerialNumber"] != null)
            //        result += ((result == "") ? "" : Environment.NewLine) + (hdd["SerialNumber"]).ToString().Trim();
            //}
            return result;
        }

        private string GetMacAdresses()
        {
            string result = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    result = string.Format("{0}{1}{2}", result, ((result == "") ? "" : ";"), nic.GetPhysicalAddress().ToString());
                }
            }
            return result;
        }

        private string GetWindowsID()
        {
            string result = "";
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (var os in searcher.Get())
            {
                result = string.Format("{0}{1}{2}", result, ((result == "") ? "" : ";"), (os["SerialNumber"]));
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PIds:");
            sb.Append(processorIds);
            sb.Append(";;PCount:");
            sb.Append(processorsCount);
            sb.Append(";;MB:");
            sb.Append(motherBoard);
            sb.Append(";;Mem:");
            sb.Append(memoryIds);
            sb.Append(";;WinId:");
            sb.Append(windowsId);
            sb.Append(";;HDDs:");
            sb.Append(hdds);
            sb.Append(";;Machine:");
            sb.Append(machineName);
            sb.Append(";;User:");
            sb.Append(userName);
            sb.Append(";;Domain:");
            sb.Append(userDomainName);

            return sb.ToString();
        }
    }
}