using Android.App;
using Android.Net;
using Android.Net.Wifi;
using System;

namespace DisertationProject.Controllers
{
    /// <summary>
    /// Network controller class
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// Conectivity manager
        /// </summary>
        private ConnectivityManager _connectivityManager;

        private static NetworkController _instance;

        public static NetworkController Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new NetworkController();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Wifi manager
        /// </summary>
        private WifiManager _wifiManager;

        /// <summary>
        /// Wifi lock used so that music stream oever wifi even when phone is locked
        /// </summary>
        private WifiManager.WifiLock _wifiLock;

       
        /// <summary>
        /// Check is connected to the network
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        //public bool IsConnected
        //{
        //    get
        //    {
        //        if (_connectivityManager.GetNetworkInfo(ConnectivityType.Wifi).IsConnected ||
        //            _connectivityManager.GetNetworkInfo(ConnectivityType.Mobile).IsConnected)
        //            return true;
        //        else
        //            return false;
        //    }
        //}

        public bool IsConnected
        {
            get
            {
                try
                {
                    var activeConnection = _connectivityManager.ActiveNetworkInfo;
                    return ((activeConnection != null) && activeConnection.IsConnected);
                }
                catch (Exception e)
                {
                    //TODO log error
                    return false;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private NetworkController()
        {
            _wifiManager = (WifiManager)Application.Context.GetSystemService(Android.Content.Context.WifiService);
            _connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Android.Content.Context.ConnectivityService);
            _wifiLock = _wifiManager.CreateWifiLock(WifiMode.Full, "Wifi lock");
        }

        /// <summary>
        /// Lock the wifi so we can still stream under lock screen
        /// </summary>
        public void AquireWifiLock()
        {
            if (!_wifiLock.IsHeld)
                _wifiLock.Acquire();
        }

        /// <summary>
        /// This will release the wifi lock if it is no longer needed
        /// </summary>
        public void ReleaseWifiLock()
        {
            if (_wifiLock.IsHeld)
                _wifiLock.Release();
        }
    }
}