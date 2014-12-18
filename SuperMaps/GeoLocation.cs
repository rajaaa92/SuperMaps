using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace SuperMaps
{
	public class GeoLocation
	{
		public string IP, city;
		public float? lat, lon;

		public GeoLocation()
		{
			GetPublicIP();
			GetLocation();
			GetCity();
		}

		public GeoLocation(float _lon, float _lat)
		{
			GetPublicIP();
			lat = _lat;
			lon = _lon;
			city = GetCity();
		}

		private void GetPublicIP()
		{
			foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces()) {
				if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
					netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
					foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses) {
						if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork) {
							var ipAddress = addrInfo.Address;

							IP = ipAddress.ToString();
						}
					}
				}  
			}
		}

		private void GetLocation() {
			if (IP == null) return;
			string url = "http://api.hostip.info/?ip=" + IP + "&position=true";

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(response.GetResponseStream());
			XmlNodeList elemList = xmlDoc.GetElementsByTagName("Latitude");
			if (elemList.Count > 0)
				lat = float.Parse(elemList[0].InnerXml);
			elemList = xmlDoc.GetElementsByTagName("Longitude");
			if (elemList.Count > 0)
				lon = float.Parse(elemList[0].InnerXml);
		}



		public string GetCity()
		{
			if (lat == null || lon == null)
				return "No city given.";

			string key = "AmRehmDATX104RXriw2iai1dPz4gATdSTspSX302rWVCCdnGmyE4EnoyvJULjchL";
			string url = "http://dev.virtualearth.net/REST/v1/Locations/" + lat + "," + lon + "?o=xml&key=" + key;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(response.GetResponseStream());
			XmlNodeList elemList = xmlDoc.GetElementsByTagName("Name");
			if (elemList.Count > 0)
				return elemList[0].InnerXml;
			else
				return "No city given.";
		}


	}
}

