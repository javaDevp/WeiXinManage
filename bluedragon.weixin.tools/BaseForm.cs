using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace bluedragon.multipaltform.sol.ui
{
    public partial class BaseForm : XtraForm
    {
        private DefaultLookAndFeel defaultLook = new DefaultLookAndFeel();

        public BaseForm()
        {
            InitializeComponent();
        }

        protected void ApplySkin(string skinName)
        {
            defaultLook.LookAndFeel.SkinName = skinName;
        }
    }
}
