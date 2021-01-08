using NetTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ipCidrCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            //This program loads a list of CIDR ranges, and an IP address or a list of IP addresses.
            //It then checks if the IP address(es) are within one of the CIDR ranges.

            Console.WriteLine("Do you want to check a single IP addresses or a list of IP addresses?");
            Console.WriteLine("Enter y for a single IP or n to input a list of IPs:");
            string mode = Console.ReadLine();

            //read in the list of CIDR ranges
            List<string> cidrRanges = new List<string>();
            Console.WriteLine("Enter a text file (with full path) of the CIDR ranges, with one range per line:");
            cidrRanges = File.ReadAllLines(Console.ReadLine()).ToList();

            //read in the IP address or list of IP addresses to check
            List<string> ipAddresses = new List<string>();
            if (mode == "n")
            {
                Console.WriteLine("Enter a text file (with full path) containing one IP address per line:");
                ipAddresses = File.ReadAllLines(Console.ReadLine()).ToList();
            }
            else
            {
                Console.WriteLine("Enter the IP address:");
                ipAddresses.Add(Console.ReadLine());
            }

            //check each ip address to see if it is in one of the cidr ranges
            List<string> inARange = new List<string>();
            List<string> notInAnyRanges = new List<string>();
            Boolean found = false;
            foreach (string ipString in ipAddresses)
            {
                IPAddress ip = IPAddress.Parse(ipString);
                foreach (string cidrRange in cidrRanges)
                {
                    IPAddressRange range = IPAddressRange.Parse(cidrRange);

                    //if the ip address is in the range, add it to the inARange list
                    if (range.Contains(ip))
                    {
                        inARange.Add(ipString + ";" + cidrRange);
                        found = true;
                        break;
                    }
                }

                //if the ip address wasn't in any of the cidr ranges, add it to the notInAnyRanges list
                if (found == false)
                {
                    notInAnyRanges.Add(ipString);
                }
                else
                {
                    found = false; //reset confirmation bool if a cidr was found
                }
            }

            //print resulting lists
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("IP addresses in a CIDR range:");
            foreach (string ipInfo in inARange)
            {
                string[] ipData = ipInfo.Split(';');
                Console.WriteLine(ipData[0] + " is in " + ipData[1]);
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("IP addresses not in any of the lists:");
            foreach (string ip in notInAnyRanges)
            {
                Console.WriteLine(ip);
            }
        }
    }
}
