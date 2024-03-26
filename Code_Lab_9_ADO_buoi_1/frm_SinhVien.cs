using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Code_Lab_9_ADO_buoi_1;
using Microsoft.VisualBasic;

namespace Code_Slide13_ADO
{
    public partial class frm_SinhVien : Form
    {
        //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
        //SqlConnection conn1;                                     // Dùng để sử dụng hàm Mở kết nối và Đóng kết nối 
        public frm_SinhVien()
        {
            //conn1 = new SqlConnection(chuoiketnoi);           // Dùng để sử dụng hàm Mở kết nối và Đóng kết nối 
            InitializeComponent();
        }       
       /* public void Open_Conn()
        {          
            if (conn1.State == ConnectionState.Closed)
                conn1.Open();
        }
        public void Close_Conn()
        {
            if (conn1.State == ConnectionState.Open)
                conn1.Close();
        }*/
        private void btn_Them_Click(object sender, EventArgs e)
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn1 = new SqlConnection(chuoiketnoi);
            //string sql = "insert into SINHVIEN values('003',N'Phát','Cát Thành')";  // Thêm thụ động
            string sql = "insert into SINHVIEN values('" + txt_MaSV.Text + "',N'" + txt_HoTen.Text + 
                "',Convert(Datetime, '" + dateTimePicker1.Value + "', 103), '" + cb_Khoa.SelectedValue +
                "', '" + txt_Hinh.Text + "', '" + txt_Nam.Text + "', N'"+richTextBox1.Text+"', '"
                +lb_TinhThanh.SelectedValue+"')";
            //string sql = "insert into SINHVIEN values('" + txt_MaSV.Text + "',N'" + txt_HoTen.Text + "', Convert(datetime, '" + dateTimePicker1.Text + "', 103), N'" + cb_Khoa.SelectedValue + "', '" + txt_Hinh.Text + "','" + txt_Nam.Text + "')";
            //Phải dùng SelectedValue cho cb_Khoa bởi vì để DataGridView lấy giá trị của MÃ KHOA (.SelectedValue) chứ không phải giá trị của TÊN KHOA (.Text)
            SqlCommand comm = new SqlCommand(sql, conn1); 
            conn1.Open();
            // Open_Conn();     // Dùng để sử dụng hàm Mở kết nối và Đóng kết nối 
     // Hàm try catch phải bỏ vào giữa conn.Open và conn.Close thì khi nhập bị trùng mã, ta nhập lại mã đúng thì mới dùng đc
            try
            {              
                pictureBox1.Image.Save(duongdan + txt_Hinh.Text); // Hình ảnh đặt trong try catch// Thêm ký tự TextBox hình ảnh chứ ko nên để trống. Hoặc để mặc định trong ô TextBox Hình Ảnh là jpg_png_jpeg 
                //pictureBox1.Image.Save(@"E:\Giang day\GiangDay_Winform CS_414_SL_Khuong_20170516\CS414 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\HINHANH\" + txt_Hinh.Text);
                int ketqua = comm.ExecuteNonQuery();
                if (ketqua >= 1) MessageBox.Show("Thêm thành công");
                else MessageBox.Show("Thêm thất bại");
            }
            catch (Exception ex) { MessageBox.Show("Lỗi catch, Trùng mã, lỗi SQL, thiếu Ghi chú hoặc phải nhập textbox hình ảnh"); }          // Nếu để SqlException thì textbox hình ảnh sẽ bị lỗi 
          //  finally 
          //  { 
                conn1.Close();
                //Close_Conn();              // Dùng để sử dụng hàm Mở kết nối và Đóng kết nối 
           // }
            LoadData();
        }

        private void btn_Dem_Click(object sender, EventArgs e)
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn2 = new SqlConnection(chuoiketnoi);
            conn2.Open();
            string sqldem = "select count (*) from SINHVIEN";
            SqlCommand comm = new SqlCommand(sqldem, conn2);
            int ketqua = (int)comm.ExecuteScalar();
            conn2.Close();
            txt_Dem.Text = ketqua.ToString();
        }

        private void frm_SinhVien_Load(object sender, EventArgs e)
        {
            int x = DateTime.Now.Year - dateTimePicker1.Value.Year + 1;
            txt_Nam.Text = x.ToString();
            //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //SqlConnection conn3 = new SqlConnection(chuoiketnoi);
            //string sqlLoad = "select * from SINHVIEN";
            //SqlDataAdapter da = new SqlDataAdapter(sqlLoad, conn3);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //dataGridView1.DataSource = dt;

            Load_cb_Khoa();  // Load bảng Khoa trước rồi mới Load DataGridview sau, bở vì khi chạy ctrinh là máy chạy chỗ combobox trước
            LoadReader_cb_Khoa();  // vì Reader chỉ đọc dữ liêu nên sau khi load danh sách các khoa lên Combobox, lúc này bắt
                                     // sự kiện cho từng item trong Combobox không đc vì chưa có chọn Value (Reader chỉ đọc 1 cột)
            Load_lb_ThanhPho();
            LoadData();
            //LoadReader();
        }
        public void Load_cb_Khoa()
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn3 = new SqlConnection(chuoiketnoi);
            string sqlCB = "select * from KHOA";
            SqlDataAdapter daCB = new SqlDataAdapter(sqlCB, conn3);
            DataTable dtCB = new DataTable();
            daCB.Fill(dtCB);
            cb_Khoa.DataSource = dtCB;
            cb_Khoa.DisplayMember = "TENKHOA";
            cb_Khoa.ValueMember = "MAKHOA";
        }
        public void LoadReader_cb_Khoa()
        {
            //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //SqlConnection connRD_cb = new SqlConnection(chuoiketnoi);
            //string sqlCB = "select * from KHOA";
            //SqlCommand commReader = new SqlCommand(sqlCB, connRD_cb);

            //connRD_cb.Open();
            //SqlDataReader daRD_cb = commReader.ExecuteReader();
            //while (daRD_cb.Read())
            //{
            //    cb_Khoa.Items.Add(daRD_cb["MAKHOA"]);
            //}
            ////connRD_cb.Close();
            //daRD_cb.Close();
        }
        public void Load_lb_ThanhPho()
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn3 = new SqlConnection(chuoiketnoi);

            string sqlLB = "select * from TINHTHANH";
            SqlDataAdapter daLB = new SqlDataAdapter(sqlLB, conn3);
            DataTable dtLB = new DataTable();
            daLB.Fill(dtLB);
            lb_TinhThanh.DataSource = dtLB;
            lb_TinhThanh.DisplayMember = "TENTT";
            lb_TinhThanh.ValueMember = "MATT";
        }
        public void LoadData()
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn3 = new SqlConnection(chuoiketnoi);
            String sqlLoad = "select MSSV, HOTEN , NGAYNHAPHOC, MAKHOA, HINHANH, " +
                "NAMTHU, GHICHU, MATT," +
                " HocPhi_1_Nam, [NAMTHU]*[HocPhi_1_Nam] as HocPhiDaNop from SINHVIEN";
            //String sqlLoad = "select * from SINHVIEN";                          // Load Form
            SqlDataAdapter da = new SqlDataAdapter(sqlLoad, conn3);

            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //DataSet ds = new DataSet();
            //da.Fill(ds, "SINHVIEN");
            //dataGridView1.DataSource = ds;
            //dataGridView1.DataMember = "SINHVIEN";

            //int x = DateTime.Now.Year - dateTimePicker1.Value.Year + 1;
            //txt_Nam.Text = x.ToString();
        }
        public void LoadReader()   // Load Gridview theo SqlDataReader
        {
            //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //SqlConnection connRD = new SqlConnection(chuoiketnoi);
            //string sqlLoad = "select * from SINHVIEN";

            //connRD.Open();
            //SqlCommand commRD = new SqlCommand(sqlLoad, connRD);
            //SqlDataReader SDR = commRD.ExecuteReader();
            //DataTable dtRD = new DataTable();
            //dtRD.Load(SDR);
            //dataGridView1.DataSource = dtRD;
            //connRD.Close();
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);
            conn.Open();
            DialogResult DR = MessageBox.Show("Bạn có muốn xóa không","Thông báo", MessageBoxButtons.YesNo);
            if(DR == DialogResult.Yes)
            try
            {
                string sql = "delete from SINHVIEN where MSSV = '" + txt_MaSV.Text + "'";
                File.Delete(duongdan + txt_Hinh.Text);  // Nên để trên SqlCommand, để nếu Xóa mà ko có tên hình thì sẽ báo lỗi.
                                                        //Nếu để dưới thì sau khi xóa thì trong Folder HINHANH vẫn còn ảnh ??
                //Nếu File ko hiện ra chữ màu xanh thì do chưa có thư viện using System.IO;
                SqlCommand comm = new SqlCommand(sql, conn);
                int ketqua = comm.ExecuteNonQuery();
                if (ketqua >= 1)
                MessageBox.Show("Xóa thành công");
                else MessageBox.Show("Xóa thất bại, không có mã ...."); 
            }
            catch(Exception ex)
            {
                MessageBox.Show(" (Catch) Xóa thất bại, Chưa có tên hình hoặc lỗi SQL....");
            }
            conn.Close();           
            LoadData();          
        }

        private void btn_CapNhat_Click(object sender, EventArgs e)
        {
            //int x = DateTime.Now.Year - dateTimePicker1.Value.Year + 1;
            //txt_Nam.Text = x.ToString();
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);         
            conn.Open();
            try
            {   
                string sqlCapNhat = "update SINHVIEN set HOTEN = N'" + txt_HoTen.Text + "', NGAYNHAPHOC = Convert(Datetime, '" + dateTimePicker1.Value + "', 103), MAKHOA = '" + cb_Khoa.SelectedValue
                    + "', HINHANH = N'" + txt_Hinh.Text + "', NAMTHU = '" + txt_Nam.Text + "', GHICHU = N'"+
                    richTextBox1.Text+"', MATT = '"+lb_TinhThanh.SelectedValue+"' where MSSV = '" + txt_MaSV.Text +
                    "' ";
                //string sqlCapNhat = "update SINHVIEN set HOTEN = N'" + txt_HoTen.Text + "', NGAYNHAPHOC = Convert(datetime, '" + dateTimePicker1.Text + "', 103), MAKHOA = '" + cb_Khoa.SelectedValue + "', HINHANH = '" + txt_Hinh.Text + "', NAMTHU = '" + txt_Nam.Text + "', GHICHU = N'" + richTextBox1.Text + "', QUEQUAN = '" + lb_TinhThanh.SelectedValue + "' where MSSV = '" + txt_MaSV.Text + "' ";
                pictureBox1.Image.Save(duongdan + txt_Hinh.Text);   //Thêm ký tự TextBox hình ảnh chứ ko nên để trống. Hoặc để mặc định trong ô TextBox Hình Ảnh là jpg_png_jpeg 
                //pictureBox1.Image.Save(@"E:\Giang day\GiangDay_Winform CS_414_SL_Khuong_20170516\CS414 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\HINHANH\" + txt_Hinh.Text);
                SqlCommand comm = new SqlCommand(sqlCapNhat, conn);
                int ketqua = comm.ExecuteNonQuery();
                if (ketqua >= 1) MessageBox.Show("Sửa thành công");
                else MessageBox.Show("Sửa thất bại");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi catch, chưa nhập mã để sửa, Thiếu Ghi chú, phải nhập textbox hình ảnh hoặc có thể lỗi Sql");
            }
            finally
            { conn.Close(); }
           
            LoadData();
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            LoadData();
            //LoadReader();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_MaSV.Text = dataGridView1.CurrentRow.Cells["MSSV"].Value.ToString();
            txt_HoTen.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells["NGAYNHAPHOC"].Value.ToString();
            txt_Hinh.Text = dataGridView1.CurrentRow.Cells["HINHANH"].Value.ToString();
            pictureBox1.ImageLocation = duongdan + txt_Hinh.Text;
            
            richTextBox1.Text = dataGridView1.CurrentRow.Cells["GHICHU"].Value.ToString();
            txt_Nam.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            chon = 1;
            lb_TinhThanh.SelectedValue = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            chon = 1;
            cb_Khoa.SelectedValue = dataGridView1.CurrentRow.Cells[3].Value.ToString();//Phải dùng SelectedValue bởi vì để lấy giá trị, của MÃ KHOA chứ không phải giá trị của TÊN KHOA 
            //  Khi chạy tới dòng này thì chương trình sẽ chuyển qua hàm cb_NhaCungCap_SelectedIndexChanged, cho nên
            // chon = 1 phải đặt ở trên hàm này, đồng thời ở cb_Khoa_SelectedIndexChanged phải làm cái if(...) và gán chon = 0 cuối hàm
           // LoadData();  //Nếu dùng dòng này (cách này) thì khi click chọn Combobox để chọn khoa thì khi click vào lại gridview của khoa đó thì gridview vẫn hiện lên đầy đủ tất cả các khoa
            chon = 0;
        }
        int chon = 0;
       // bool landau = true;
        private void cb_Khoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(landau == true)
            //{
            //    string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //    SqlConnection conn3 = new SqlConnection(chuoiketnoi);
            //    string sqlLoad = "select * from SINHVIEN";                          // Load Form
            //    SqlDataAdapter da = new SqlDataAdapter(sqlLoad, conn3);

            //    //DataTable dt = new DataTable();
            //    //da.Fill(dt);
            //    //dataGridView1.DataSource = dt;

            //    DataSet ds = new DataSet();
            //    da.Fill(ds, "SINHVIEN");
            //    dataGridView1.DataSource = ds;
            //    dataGridView1.DataMember = "SINHVIEN";
            //    landau = false;
            //}
            //else if (landau == false)
            //{
            if (cb_Khoa.SelectedValue != null && chon == 0)
            {
                string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
                SqlConnection conn = new SqlConnection(chuoiketnoi);

                string text = cb_Khoa.SelectedValue.ToString();//cb_Khoa.GetItemText(cb_Khoa.SelectedValue); 
                //Phải dùng SelectedValue bởi vì để lấy giá trị
                //của MÃ KHOA chứ không phải giá trị của TÊN KHOA (SelectedItem)
                string sql = "select * from SINHVIEN where MAKHOA = N'" + text + "' ";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            chon = 0;
           // }
        }

        string duongdan = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\HINHANH\\";
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            //OFD.Title = "Hãy chọn File";
            OFD.Filter = "Tất cả file|*.*|JPG|*.jpg|PNG|*.png|JPEG|*.jpeg";
            if (OFD.ShowDialog() == DialogResult.OK)
                pictureBox1.Image = Image.FromFile(OFD.FileName);
        }

        private void btn_LoadNam_Click(object sender, EventArgs e)
        {
            // Cách 1 Connected
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);
            conn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DateTime LoadNam = DateTime.Parse(dataGridView1.Rows[i].Cells["NGAYNHAPHOC"].Value.ToString());
                string sqlSua = "Update SINHVIEN set NAMTHU = Convert(int,'" + (DateTime.Today.Year - LoadNam.Year + 1)
                    + "')  where MSSV = '" + dataGridView1.Rows[i].Cells["MSSV"].Value.ToString() + "'";
                SqlCommand comm = new SqlCommand(sqlSua, conn);
                comm.ExecuteNonQuery();
            }
            conn.Close();
            LoadData();


            // Cách 2   // Ngắt kết nối
            //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //SqlConnection conn = new SqlConnection(chuoiketnoi);
            //string sqlNgay = "select NGAYNHAPHOC from SINHVIEN";
            //SqlDataAdapter daNgay = new SqlDataAdapter(sqlNgay, conn);
            //DataTable dtNgay = new DataTable();
            //daNgay.Fill(dtNgay);

            //SqlDataAdapter daSV = new SqlDataAdapter("select * from SINHVIEN", conn);
            //DataTable dtSV = new DataTable();
            //daSV.Fill(dtSV);
            //for (int i = 0; i < dtNgay.Rows.Count; i++)
            //{
            //    int u = DateTime.Parse(dtNgay.Rows[i][0].ToString()).Year;
            //    txt_Nam.Text = (DateTime.Now.Year - u + 1).ToString();

            //    DataRow[] dataRows = dtSV.Select("MSSV = '" + dtSV.Rows[i][0].ToString() + "'");
            //    //int g = dataRows.Length;
            //    dataRows[0].SetModified();  // Cho phép điều chỉnh 1 dòng, vì trong trường hợp này chỉ có 1 dòng nên phải dùng [0] chứ ko phải là i
            //    string sqlSua = "Update SINHVIEN set NAMTHU = '" + int.Parse(txt_Nam.Text) + "' where MSSV = '" + dtSV.Rows[i][0].ToString() + "' ";
            //    SqlCommand comm = new SqlCommand(sqlSua, conn);
            //    daSV.UpdateCommand = comm;
            //    //daSV.UpdateCommand.Parameters.Add("@NAMTHU", SqlDbType.Int).Value = int.Parse(txt_Nam.Text);
            //    //       daSV.UpdateCommand.Parameters.Add("@NAMTHU", int.Parse(txt_Nam.Text));
            //    daSV.Update(dtSV);
            //}
            //dtSV.Clear();
            //daSV.Fill(dtSV);
            //dataGridView1.DataSource = dtSV;


            // Cách 3 
            //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //SqlConnection conn = new SqlConnection(chuoiketnoi);
            //string sql = "select MSSV, NGAYNHAPHOC from SINHVIEN";
            //SqlCommand comm = new SqlCommand(sql, conn);
            //conn.Open();
            //SqlDataReader reader = comm.ExecuteReader();
            //if (reader.HasRows) //Khi dữ liệu lấy về có ít nhất 1 bản ghi HasRows trả về true, ngược lại trả về false.
            //{
            //    while (reader.Read())
            //    {
            //        string masv = (string)reader.GetValue(0);
            //        DateTime ngayNH = (DateTime)reader.GetValue(1);
            //        string sqlCapNhat = "update SinhVien set NAMTHU = '" + (DateTime.Now.Year - ngayNH.Year + 1) + "' where MSSV = '" + masv + "' ";

            //        SqlConnection connn = new SqlConnection(chuoiketnoi);
            //        connn.Open();
            //        SqlCommand cmdCapNhat = new SqlCommand(sqlCapNhat, connn);
            //        cmdCapNhat.ExecuteNonQuery();
            //        connn.Close();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Không có dòng nào đc tìm thấy");
            //}
            //conn.Close();
            //LoadData();
        }

        private void rdo_Tang_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns["NAMTHU"], ListSortDirection.Ascending);
        }

        private void rdo_Giam_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns["NAMTHU"], ListSortDirection.Descending);
        }

        private void btn_Tim_Click(object sender, EventArgs e)
        {
            //String content = Interaction.InputBox("Bạn nhập vào ô bên dưới", "Thông báo", "Nhập vào đây");
            //if (content != "")
            //{
            //    MessageBox.Show(content);
            //}
            // Nhập vào hộp thoại, Phải có thư viện using Microsoft.VisualBasic;
            String content = Interaction.InputBox("Mời Bạn Nhập mã hoặc tên vào ô bên dưới"); // Nhập vào hộp thoại, Phải có thư viện using Microsoft.VisualBasic;
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);
            string sqlTim = "select * from SINHVIEN where MSSV like '%" + content + "%' or HOTEN like N'%" + content + "%' ";
            SqlDataAdapter da = new SqlDataAdapter(sqlTim, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GiangDay\GiangDay_WinNet\CS464 Lectures\Code cho cac Slide\Code_Slide_13_ADO_buoi1\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            //SqlConnection conn = new SqlConnection(chuoiketnoi);
            //string sqlTim = "select * from SINHVIEN where MSSV like '%" + txt_Tim.Text + "%' or HOTEN like N'%" + txt_Tim.Text + "%' "; //LIKE  ‘%”+…….+”%’  là gần giống    vd: Đức_Đ_ứ      ;  Nhật_N_h_ậ_t
            //SqlDataAdapter da = new SqlDataAdapter(sqlTim, conn);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //dataGridView1.DataSource = dt;
        }

        private void txt_Tim_TextChanged(object sender, EventArgs e)
        {
            AcceptButton = btn_Tim;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int namThu = DateTime.Now.Year - dateTimePicker1.Value.Year + 1;
            //int ngayDuTinh = 0;
            //if (tuoi > 60) hanGiamGia = tuoi + 5;
            //else hanGiamGia = tuoi + 2;
            // DateTime aTime = DateTime.Now;  
            DateTime aTime = dateTimePicker1.Value;
            DateTime timeDuTinh = aTime.AddYears(3);
            txt_Nam.Text = namThu.ToString();
            dateTimePicker2.Text = timeDuTinh.ToString();
        }

        private void frm_SinhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult DR;
            DR = MessageBox.Show("Bạn có muốn thoát không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DR == DialogResult.No)
                e.Cancel = true;
        }

        private void btn_LocNam_Click(object sender, EventArgs e)
        {
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);
            string locNam = Interaction.InputBox("Mời bạn nhập số năm bên dưới để lọc");
            int soNam = int.Parse(locNam);
            string sql = "select * from SINHVIEN where NAMTHU > '" + soNam + "' ";
            //string sql = "select * from SINHVIEN where NAMTHU > '" + int.Parse(txt_Nam.Text) + "' ";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            ExportToExcel excel = new ExportToExcel();
            // Lấy về nguồn dữ liệu cần Export là 1 DataTable
            string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn3 = new SqlConnection(chuoiketnoi);
            string sqlLoad = "select * from SINHVIEN";                          // Load Form
            SqlDataAdapter da = new SqlDataAdapter(sqlLoad, conn3);

            DataTable dt = new DataTable();
            da.Fill(dt);

            excel.Export(dt, "Danh sach", "DANH SÁCH CÁC ĐƠN VỊ"); //Danh sach : Sheet trong Excel 
            //"DANH SÁCH CÁC ĐƠN VỊ : Tiêu đề của sheet Danh sach
            //file sẽ đc export trực tiếp khi click vào sự kiện 
        }

        private void btn_HocPhi_Click(object sender, EventArgs e)
        {
             string chuoiketnoi = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\.NET\ADO_thuong\Code_Lab_9_ADO_buoi_1\SQL_SinhVien.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(chuoiketnoi);
            conn.Open();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                int namThu = int.Parse(dataGridView1.Rows[i].Cells["NAMTHU"].Value.ToString());
                float hocPhi1Nam = float.Parse(dataGridView1.Rows[i].Cells["HocPhi_1_Nam"].Value.ToString());
                float hocPhidaNop = namThu * hocPhi1Nam;
                string sqlSua = "Update SINHVIEN set HocPhiDaNop = '"+hocPhidaNop+"' " +
                    "where MSSV = '" + dataGridView1.Rows[i].Cells["MSSV"].Value.ToString() + "'";
                SqlCommand comm = new SqlCommand(sqlSua, conn);
                comm.ExecuteNonQuery();
            }
            conn.Close();
            LoadData();
        }
    }
}
