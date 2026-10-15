using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdCard.Hanel;

public partial class InputAccessCode : Form
{
    public string AccessCode { get; set; } = "";
    public InputAccessCode()
    {
        InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        AccessCode = txtAccessCode.Text.Trim().Replace(" ", "");
        if (AccessCode.Length != 6 && AccessCode.Length != 90)
        {
            MessageBox.Show("Access code không đúng", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        this.DialogResult = DialogResult.OK;
    }
}
