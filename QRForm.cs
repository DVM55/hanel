using RAR.IdCard.Sdk.Reader;
using RAR.IdCard.Sdk.Reader.HN212;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdCard.Hanel
{
    public partial class QRForm : Form
    {
        private VnHn212Reader _reader;
        public QRForm(VnHn212Reader reader)
        {
            _reader = reader;
            InitializeComponent();
        }

        private void QRForm_Load(object sender, EventArgs e)
        {
        }

        private void QRForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
