using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWB_OptionPackageInstaller
{
    public partial class Form1 : Form
    {
        #region Properties
        private string pathOfSWB;

        public string PathOfSWB
        {
            get { return pathOfSWB; }
            set { pathOfSWB = value; }
        }

        private string pathOfOptionPackages;

        public string PathOfOptionPackages
        {
            get { return pathOfOptionPackages; }
            set { pathOfOptionPackages = value; }
        }
        #endregion


        #region Variables
        public DirectoryInfo tempDir; 
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void tbPathOfSWB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pathOfSWB = tbPathOfSWB.Text.Trim();
                CheckSWBPath();
            }

        }

        private void tbPathOfOptionpackages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pathOfOptionPackages= tbPathOfOptionpackages.Text.Trim();
                CheckOptionPackagePath();
            }
        }
        public bool CheckSWBPath()
        {
            if (!Directory.Exists(PathOfSWB))
            {
                MessageBox.Show("The given path of the SWB does not exists!");

                return false;
            }
            else
                return true;
        }
        public bool CheckOptionPackagePath()
        {
            if (!Directory.Exists(PathOfOptionPackages))
            {
                MessageBox.Show("The given path of the optionpackages does not exists!");

                return false;
            }
            else
                return true;
        }
        private void btStart_Click(object sender, EventArgs e)
        {
            if (!CheckOptionPackagePath())
            {
                CheckSWBPath();
            }
            PathOfOptionPackages = tbPathOfOptionpackages.Text.Trim();
            PathOfSWB = tbPathOfSWB.Text.Trim();
            InstallingProcess();
            
        }

        private void InstallingProcess()
        {
            foreach (FileInfo optionPackage in new DirectoryInfo(PathOfOptionPackages).GetFiles("*.zip"))
            {
                String ZipPath = optionPackage.FullName;
                tempDir = Directory.CreateDirectory(string.Format("{0}{1}temp", PathOfSWB, System.IO.Path.DirectorySeparatorChar));
                String extractPath = tempDir.FullName;

                ZipFile.ExtractToDirectory(ZipPath, extractPath);

                DirectoryInfo destinationDirectory =
                    Directory.CreateDirectory(string.Format("{0}{1}{2}{1}{3}", PathOfSWB, System.IO.Path.DirectorySeparatorChar, "features", optionPackage.Name));

                ZipFile.ExtractToDirectory(string.Format(extractPath, Properties.Settings.Default.ContentZipFileName), destinationDirectory.FullName);

                /*process of installation
                 * unzip each zip file and copy the folders(plugins,features) content to the same named folder under the swb directory
                 * create a new directory to the features folder (with the same name as the optionpackage
                 * unzip content.jar file content to this newly created folder
                 * start swb    (optional)
                 */
            }
            Directory.Delete(tempDir.FullName, true);

            MessageBox.Show("Installation of optionpackages is successfull! \r\n Start SWB then check the installed packages!");
        }
    }
}
