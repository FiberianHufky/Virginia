using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Management.Deployment;
using Windows.ApplicationModel;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!                                                                  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                                                                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

namespace CleansingsD1
{


    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!packinfo class CONTAINER FOR ALL INFO                            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    public class packinfo
    {


        public Package pack { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string DisplayName { get; set; }
        public string LogoPath { get; set; }
        public string uapval { get; set; }
        public Uri UriLogoPath { get; set; }
        public bool SafeToPurge { get; set; }
        public string PackageFamilyName { get; set; }

    }


    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@packinfo class CONTAINER FOR ALL INFO                            @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


    public class prepforpurge
    {


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!SHLOADINDIRECTSTRING LIBRARY                                      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@SHLOADINDIRECTSTRING LIBRARY                                      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GET ALL PACKAGES FROM USER     RETURN LIST                        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        public static List<packinfo> EfPMfU()
        {

            List<packinfo> AllPackages = new List<packinfo>();

            PackageManager pm = new PackageManager();

            System.Security.Principal.SecurityIdentifier uId = System.Security.Principal.WindowsIdentity.GetCurrent().User; 

            IEnumerable<Package> rawPackages = pm.FindPackagesForUser(uId.Value);

            string SD = Environment.GetEnvironmentVariable("SystemDrive");

            DirectoryInfo dirInfo = new DirectoryInfo(SD + "/Program Files/WindowsApps");

            foreach (Package package in rawPackages)
            {

                if (dirInfo.GetDirectories(package.Id.FullName).Count() > 0)
                {

                    AllPackages.Add(new packinfo()
                    {

                        pack = package,
                        FullName = package.Id.FullName,
                        Name = package.Id.Name,
                        FullPath = (SD + "/Program Files/WindowsApps/" + package.Id.FullName), 
                        PackageFamilyName = package.Id.FamilyName,
                        SafeToPurge = true

                    });

                }
                else
                {

                    AllPackages.Add(new packinfo()
                    {

                        pack = package,
                        FullName = package.Id.FullName,
                        Name = package.Id.Name,
                        PackageFamilyName = package.Id.FamilyName,
                        FullPath = "in spaaaaaaaaaaaaaaace or just SystemApps pick whatever",
                        SafeToPurge = false

                    });

                }

            }

            return AllPackages;

        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@GET ALL PACKAGES FROM USER     RETURN LIST                        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GET DATA FROM "appxmanifest.xml" RETURN LIST                      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        public static List<packinfo> EfXML(List<packinfo> AllPackages)
        {

            XmlDocument xml = new XmlDocument();

            foreach (packinfo package in AllPackages)
            {

                if (package.SafeToPurge == true)
                {

                    xml.Load(package.FullPath + "/AppxManifest.xml");

                    XmlNodeList nodeListfUV = xml.GetElementsByTagName("Package");

                    foreach (XmlNode nodefUV in nodeListfUV)
                    {

                        XmlAttributeCollection actUV = nodefUV.Attributes;

                        for (int i = 0; i < actUV.Count; i++)
                        {

                            if (actUV[i].Name == "xmlns:m2")
                            {

                                package.uapval = "m2:";
                                break;

                            }
                            else if (actUV[i].Name == "xmlns:uap")
                            {

                                package.uapval = "uap:";
                                break;

                            }
                            else
                            {

                                package.uapval = "none";

                            }

                        }

                    }

                    GLPfPXML(xml, package);

                    XmlNodeList nodeList = xml.GetElementsByTagName("DisplayName");

                    foreach (XmlNode node in nodeList)
                    {

                        if (!node.InnerText.Contains("ms-resource:"))
                        {

                            package.DisplayName = node.InnerText;

                        }
                        else
                        {

                            GDNfSHL(package, xml);

                        }

                    }

                }

            }

            return AllPackages;

        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@GET DATA FROM "appxmanifest.xml" RETURN LIST                      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GENERATE DISPLAYNAME FROM ms:resource URI  VOID                   !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        static void GDNfSHL(packinfo package, XmlDocument xml)
        {

            XmlNodeList nodeList = xml.GetElementsByTagName(package.uapval + "VisualElements");
            foreach (XmlNode node in nodeList)
            {

                XmlAttributeCollection atCol = node.Attributes;

                for (int i = 0; i < atCol.Count; i++)
                {

                    if (atCol[i].Name == "DisplayName")
                    {

                        if (atCol[i].Value.Contains("ms-resource:"))
                        {

                            string[] dunbar =
                            {

                                atCol[i].Value,
                                atCol[i].Value.Insert(12, "Resources"),
                                atCol[i].Value.Insert(12, "/Resources"),
                                atCol[i].Value.Insert(12, "Resources/"),
                                atCol[i].Value.Insert(12, "/Resources/")

                            };

                            for (int z = 4; z >= 0; z--)
                            {

                                string fs = string.Format("@{{{0}? {1}}}", package.FullName, dunbar[z]);
                                var outBuf = new StringBuilder(256);
                                SHLoadIndirectString(fs, outBuf, outBuf.Capacity, IntPtr.Zero);
                                if (!String.IsNullOrEmpty(outBuf.ToString()))
                                {

                                    package.DisplayName = outBuf.ToString();

                                    break;

                                }

                            }

                        }

                    }

                }

            }

        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@GENERATE DISPLAYNAME FROM ms:resource URI  VOID                   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GENERATE LOGOPATH       VOID                                      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        static void GLPfPXML(XmlDocument xml, packinfo package)
        {

            XmlNodeList nodeList = xml.GetElementsByTagName(package.uapval + "VisualElements");

            if (nodeList.Count > 0)
            {

                foreach (XmlNode node in nodeList)
                {

                    XmlAttributeCollection walters = node.Attributes;
                    for (int z = 0; z < walters.Count; z++)
                    {

                        if (walters[z].Name == "Square150x150Logo" || walters[z].Name == "Square310x310Logo" || walters[z].Name == "Square44x44Logo" || walters[z].Name == "Square71x71Logo")
                        {

                            CtMLPF(walters[z].Value, package);
                            break;

                        }

                    }

                }

            }
            else
            {

                XmlNodeList nodeListfNone = xml.GetElementsByTagName("Logo");

                foreach (XmlNode nodefNone in nodeListfNone)
                {

                    CtMLPF(nodefNone.InnerText, package);

                }

            }

        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@GENERATE LOGOPATH       VOID                                      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!PART OF LOGOPATH GENERATOR NOTHING SPECIAL KEEP SCROLLING         !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        static void CtMLPF(string lp, packinfo package)
        {

            if (!String.IsNullOrEmpty(lp))
            {

                string[] path = lp.Split('\\');
                string[] logo = path[path.Length - 1].Split('.');
                string dirPath = package.FullPath;

                for (int i = 0; i < path.Length - 1; i++)
                {

                    dirPath += '\\' + path[i];

                }

                string[] all = Directory.GetFiles(dirPath, "*" + logo[0] + "*");

                for (int z = 0; z < all.Length; z++)
                {

                    if (!all[z].Contains("contrast"))
                    {

                        package.LogoPath = all[z];
                        break;

                    }
                    else
                    {

                        package.LogoPath = all[0];

                    }

                }

            }

        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@PART OF LOGOPATH GENERATOR NOTHING SPACIAL KEEP SCROLLING         @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


    }


    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!NECRO CLASS TO REMOVE AND RESURRECT PACKETS                      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    public class necro
    {


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!BURY METHOD TO REMOVE PACKET AND SAVE DATA FOR LAZARUS            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        public static void bury(packinfo package)
        {


            string SD = Environment.GetEnvironmentVariable("SystemDrive");
            string execName = Environment.UserName;

            DirectoryInfo dirInfo = new DirectoryInfo(SD + "/Users/" + execName);


            if(dirInfo.GetDirectories("VirgGraveyard").Count() == 0)
            {


                dirInfo.CreateSubdirectory("VirgGraveyard");

                
            }

            File.AppendAllText(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt", "");
            string[] prep = File.ReadAllLines(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt");
            bool repeat = false;


            foreach(string p in prep)
            {


                if(p.Contains(package.FullName))
                {


                    repeat = true;
                    break;


                }


            }


            if(repeat == false)
            {


                File.AppendAllText(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt", package.PackageFamilyName + "@" + package.DisplayName + "@" + package.FullName + Environment.NewLine);


            }


            PackageManager exec = new PackageManager();

            exec.RemovePackageAsync(package.FullName);

            System.Threading.Thread.Sleep(1000);


        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@BURY METHOD TO REMOVE PACKET AND SAVE DATA FOR LAZARUS            @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!LAZARUS METHOD TO RESTORE PACKETS                                 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
        
        public static List<packinfo> lazarus()
        {


            string SD = Environment.GetEnvironmentVariable("SystemDrive");
            string execName = Environment.UserName;

            List<packinfo> AllPackages = new List<packinfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(SD + "/Users/" + execName + "/VirgGraveyard/");


            if(dirInfo.GetFiles("VirgGraveyard.txt").Count() > 0)
            {


                string[] prep = File.ReadAllLines(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt");


                foreach (string p in prep)
                {


                    string[] a = p.Split('@');

                    AllPackages.Add(new packinfo() { PackageFamilyName = a[0], DisplayName = a[1], FullName = a[2] });


                }


            }


            return AllPackages;


        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@LAZARUS METHOD TO RESTORE PACKETS                                 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!FINALLAZARUS METHOD RESTORE PACKAGES DELETES RECORD FROM TXT      !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        public static void finalLazarus(string fullName, string familyName)
        {


            string SD = Environment.GetEnvironmentVariable("SystemDrive");
            string execName = Environment.UserName;

            List<packinfo> AllPackages = new List<packinfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(SD + "/Users/" + execName + "/VirgGraveyard/");

            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?PFN=" + familyName));


            if (dirInfo.GetFiles("VirgGraveyard.txt").Count() > 0)
            {


                string[] prep = File.ReadAllLines(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt");

                File.Delete(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt");

                foreach (string p in prep)
                {


                    if(!p.Contains(fullName))
                    {


                        File.AppendAllText(SD + "/Users/" + execName + "/VirgGraveyard/" + "VirgGraveyard.txt", p + Environment.NewLine);


                    }


                }


            }


        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@FINALLAZARUS METHOD RESTORE PACKAGES DELETES RECORD FROM TXT      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


    }


    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@NECRO CLASS TO REMOVE AND RESURRECT PACKETS                       @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


}