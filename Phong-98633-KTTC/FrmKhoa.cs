using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phong_98633_KTTC
{
    public partial class FrmKhoa : Form
    {
        public int namhientai = DateTime.Now.Year;
        public FrmKhoa()
        {
            InitializeComponent();
            loadDgvKhoa();
        }

        private void FrmKhoa_Load(object sender, EventArgs e)
        {
            
            nmpNam.Value = namhientai; 
        }
        
        private void loadDgvKhoa()
        {
            dgvKhoa.DataSource = Database.Query("SELECT * FROM Khoa");
            btnsua.Enabled = btnxoa.Enabled = (dgvKhoa.Rows.Count > 0);
        }

        private bool CheckForm()
        {
            bool flag = true;
            erpBatLoi.Clear();
            if(txtTenKhoa.Text.Trim() == "")
            {
                erpBatLoi.SetError(txtTenKhoa, "Không được để trống chỗn này nhé!");
                flag = false;       
            }
            if(nmpNam.Value > nmpNam.Value)
            {
                erpBatLoi.SetError(nmpNam, "Khoa này thành lập trong tương lai à?");
            }
            return flag;
        }
        private void btnthem_Click(object sender, EventArgs e)
        {
            if (CheckForm() == false) return;
            bool flag = true;

            string check = "SELECT * FROM Khoa WHERE TenKhoa=N'" + txtTenKhoa.Text + "'";
            if (Database.Query(check).Rows.Count > 0) {
                erpBatLoi.SetError(txtTenKhoa, "Trùng tên khoa!");
                flag = false;
            }
            if(nmpNam.Value > DateTime.Now.Year)
            {
                erpBatLoi.SetError(nmpNam, "Khoa này thành lập trong tương lai à?");
                return;
            }
            if (flag == false) return;

            check = "INSERT Khoa VALUES(N'" + txtTenKhoa.Text + "', " + nmpNam.Value.ToString() + ")";
            Database.Execute(check);
            loadDgvKhoa();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if(CheckForm() == false) return;
            string maKhoa = dgvKhoa.CurrentRow.Cells[0].Value.ToString();

            string sql = "SELECT * FROM Khoa WHERE MaKhoa != " + maKhoa +
                " AND TenKhoa=N'" + txtTenKhoa.Text + "'";
            if (nmpNam.Value > DateTime.Now.Year)
            {
                erpBatLoi.SetError(nmpNam, "Khoa này thành lập trong tương lai à?");
                return;
            }

            Database.Execute("UPDATE Khoa SET NamThanhLap='" + nmpNam.Value.ToString() + 
                "' WHERE TenKhoa=N'" + txtTenKhoa.Text + "'");
            loadDgvKhoa();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa ko?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            string maKhoa = dgvKhoa.CurrentRow.Cells[0].Value.ToString();

            Database.Execute("DELETE Khoa WHERE MaKhoa = " + maKhoa);
            loadDgvKhoa();
        }

        private void dgvKhoa_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtTenKhoa.Text = dgvKhoa.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (dgvKhoa.Rows[e.RowIndex].Cells[2].Value != null)
            {
                // Chuyển đổi giá trị sang decimal
                decimal namThanhLap;
                if (decimal.TryParse(dgvKhoa.Rows[e.RowIndex].Cells[2].Value.ToString(), out namThanhLap))
                {
                    nmpNam.Value = namThanhLap; // Gán giá trị vào NumericUpDown
                }
                else
                {
                    nmpNam.Value = namhientai; // Hoặc gán một giá trị mặc định nếu không thể chuyển đổi
                }
            }
        }
    }
}
