using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Code_Slide13_ADO
{
    public partial class frm_DangNhap : Form
    {
        public frm_DangNhap()
        {
            InitializeComponent();
        }
        int dem = 0;
        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);
            string sqlDangNhap = "select count (*) from DANGNHAP where TENDANGNHAP = N'"+txt_TenDangNhap.Text.Trim()+
                "' and MATKHAU = '"+txt_MatKhau.Text+"' ";
            SqlCommand comm = new SqlCommand(sqlDangNhap, conn);
            conn.Open();
            int ketqua = 0;   // Vì tạo try catch nên phải gán ketqua = 0
            try
            {
                ketqua = (int)comm.ExecuteScalar();    //Lỗi khi dùng comm.ExecuteNonQuery() cho Scalar; Đếm = -1 
                if (ketqua >= 1)
                {
                    frm_SinhVien SV = new frm_SinhVien();
                    SV.Show();
                    dem = 0;
                }
                else
                {
                    dem++;
                    MessageBox.Show("Lỗi đăng nhập lần " + dem);
                    if (dem == 3)
                    {
                        MessageBox.Show("Bạn đã nhập sai 3 lần. Thoát chương trình");
                        Application.Exit();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi sql");
            }
            conn.Close();

               
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult DR;
            DR = MessageBox.Show("Bạn có muốn thoát không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(DR == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
