using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolitaireSolver
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            BlockConfiguration CameraBlockConfiguration = new BlockConfiguration(new Size(1920, 1080));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmSolitaire(CameraBlockConfiguration));
        }
    }
}
