using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBanHang
{
    public partial class frmNhanVien : Form
    {
        bool Them;
        //Chuỗi kết nối
        string strConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=SSPI";
        //Đối tượng kết nối
        SqlConnection conn = null;
        //Đối tượng đưa dữ liệu vào Datatable dtNhanVien
        SqlDataAdapter daNhanVien = null;
        //Đối tượng hiển thị dữ liệu lên form
        DataTable dtNhanVien = null;

        //Đối tượng đưa dữ liệu vào Datatable dtNhanVien
        SqlDataAdapter daGioiTinh = null;
        //Đối tượng hiển thị dữ liệu lên form
        DataTable dtGioiTinh = null;


        //Form Load
        void LoadData()
        {
            try
            {
                //Khởi động connection
                conn = new SqlConnection(strConnectionString);
                //Vận chuyển dữ liệu lên DataTable dtNhanVien
                daNhanVien = new SqlDataAdapter("select MaNV, Ho, Ten, Nu, NgayNV, DiaChi, DienThoai from NHANVIEN", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);

                this.dgvNhanVien.DataSource = dtNhanVien;
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong table NHANVIEN. Lỗi rồi!!!");
            }

            //Xóa trống và vô hiệu hóa các đối tượng trong Panel
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.pnlThongTinNV.Enabled = false;
            //this.comboBox1.Items.Clear();
            this.txtNgayNV.ResetText();
            this.txtDiachi.ResetText();
            this.txtDienthoai.ResetText();
            //Không cho thao tác trên các nút Lưu / Hủy
            this.btnLuu.Enabled = false;
            this.btnHuy.Enabled = false;
            //Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = true;
            this.btnSua.Enabled = true;
            this.btnXoa.Enabled = true;
            this.btnThoat.Enabled = true;

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
            comboBox1.SelectedItem = "Nam";


        }


        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            //DialogResult traloi = MessageBox.Show("Thật không?", "Trả lời", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (traloi == DialogResult.OK)
            //    Application.Exit();
            this.Close();
        }

        private void frmNhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Giải phóng tài nguyên
            dtNhanVien.Dispose();
            dtNhanVien = null;
            //Hủy kết nối
            conn = null;
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Mở kết nối
            conn.Open();
            try
            {
                //Thực hiện lệnh
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Lấy thứ tự record hiện hành
                int r = dgvNhanVien.CurrentCell.RowIndex;
                //Lấy MaKH của record hiện hành
                string strMANV = dgvNhanVien.Rows[r].Cells[0].Value.ToString();
                //Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete from NhanVien where MaNV='" + strMANV + "'");
                //cmd.CommandType = CommandType.Text;
                //Thực hiện câu lệnh SQL
                cmd.ExecuteNonQuery();
                //Cập nhật lại DataGridView
                LoadData();
                //Thông báo
                MessageBox.Show("Đã xóa xong!");

            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
            //Đóng kết nối
            conn.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Đặt biến Them thành false, chỉ thực hiện cập nhật dữ liệu
            Them = false;

            // Kiểm tra xem có bản ghi nào được chọn trong DataGridView không
            if (dgvNhanVien.CurrentRow != null)
            {
                // Thứ tự dòng hiện hành
                int r = dgvNhanVien.CurrentCell.RowIndex;

                // Chuyển thông tin từ DataGridView lên các ô nhập liệu
                this.txtMaNV.Text = dgvNhanVien.Rows[r].Cells[0].Value.ToString();
                this.txtHo.Text = dgvNhanVien.Rows[r].Cells[1].Value.ToString();
                this.txtTen.Text = dgvNhanVien.Rows[r].Cells[2].Value.ToString();
                this.comboBox1.SelectedItem = dgvNhanVien.Rows[r].Cells[3].Value.ToString() == "0" ? "Nam" : "Nữ";
                this.txtNgayNV.Text = dgvNhanVien.Rows[r].Cells[4].Value.ToString();
                this.txtDiachi.Text = dgvNhanVien.Rows[r].Cells[5].Value.ToString();
                this.txtDienthoai.Text = dgvNhanVien.Rows[r].Cells[6].Value.ToString();

                // Bật các nút Lưu và Hủy, tắt các nút Thêm, Sửa, Xóa, Thoát
                this.btnLuu.Enabled = true;
                this.btnHuy.Enabled = true;
                this.btnThem.Enabled = false;
                this.btnSua.Enabled = false;
                this.btnXoa.Enabled = false;
                this.btnThoat.Enabled = false;
                this.pnlThongTinNV.Enabled = true;


                // Đưa con trỏ đến ô nhập liệu Mã NV
                this.txtMaNV.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn bản ghi để sửa.");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //Kích hoạt biến Them
            Them = true;

            //Xóa trống và cho phép chỉnh sửa các đối tượng trong Panel
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.pnlThongTinNV.Enabled = true;
            this.txtNgayNV.ResetText();
            this.txtDiachi.ResetText();
            this.txtDienthoai.ResetText();

            //Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuy.Enabled = true;

            //Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnThoat.Enabled = false;
            //Đưa con trỏ đến TextField txtMaNV
            this.txtMaNV.Focus();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                comboBox1.SelectedItem.ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            try
            {
                // Kiểm tra thông tin cần thiết
                if (string.IsNullOrEmpty(this.txtMaNV.Text)
                || string.IsNullOrEmpty(this.txtHo.Text)
                || string.IsNullOrEmpty(this.txtTen.Text)
                || string.IsNullOrEmpty(this.txtNgayNV.Text)
                || string.IsNullOrEmpty(this.txtDiachi.Text)
                || string.IsNullOrEmpty(this.txtDienthoai.Text)
                )
                {
                    MessageBox.Show("Không thể thêm/sửa được. Vui lòng điền đầy đủ thông tin");
                    return;
                }

                // Tạo đối tượng SqlCommand
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                if (Them) // Thêm mới
                {
                    //Đổi Nam thành 0 Nữ 1
                    int gioiTinhValue = comboBox1.SelectedItem.ToString() == "Nam" ? 0 : 1;

                    cmd.CommandText = "INSERT INTO NhanVien (MaNV, Ho, Ten, Nu, NgayNV, DiaChi, DienThoai) " +
                  "VALUES (@MaNV, @Ho, @Ten, @Nu, @NgayNV, @DiaChi, @DienThoai)";

                    cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    cmd.Parameters.AddWithValue("@Ho", txtHo.Text);
                    cmd.Parameters.AddWithValue("@Ten", txtTen.Text);
                    cmd.Parameters.AddWithValue("@Nu", gioiTinhValue);
                    cmd.Parameters.AddWithValue("@NgayNV", DateTime.Parse(txtNgayNV.Text));
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiachi.Text);
                    cmd.Parameters.AddWithValue("@DienThoai", txtDienthoai.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm xong!");
                }
                else // Cập nhật
                {
                    // Lấy thông tin từ ô nhập liệu để tạo câu lệnh cập nhật
                    int r = dgvNhanVien.CurrentCell.RowIndex;
                    string strMANV = dgvNhanVien.Rows[r].Cells[0].Value.ToString();
                    int gioiTinhValue = comboBox1.SelectedItem.ToString() == "Nam" ? 0 : 1;

                    // Câu lệnh SQL để cập nhật
                    cmd.CommandText = $"Update NHANVIEN Set Ho = N'{this.txtHo.Text}', Ten = N'{this.txtTen.Text}', " +
                                      $"Nu = {gioiTinhValue}, NgayNV = '{this.txtNgayNV.Text}', " +
                                      $"DiaChi = N'{this.txtDiachi.Text}', DienThoai = '{this.txtDienthoai.Text}' " +
                                      $"where MaNV = '{strMANV}'";

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã sửa xong!");
                }

                // Load lại dữ liệu trên DataGridView
                LoadData();
            }
            catch (FormatException)
            {
                MessageBox.Show("Đơn giá không hợp lệ. Vui lòng nhập số.");
            }
            catch (SqlException)
            {
                MessageBox.Show("Không thêm/sửa được. Lỗi rồi!");
            }
            finally
            {
                // Đóng kết nối
                conn.Close();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            //Xóa trống và vô hiệu hóa các đối tượng trong panel
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.txtNgayNV.ResetText();
            this.txtDiachi.ResetText();
            this.txtDienthoai.ResetText();
            this.pnlThongTinNV.Enabled = false;
            //Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = true;
            this.btnSua.Enabled = true;
            this.btnXoa.Enabled = true;
            this.btnThoat.Enabled = true;
            //Không cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = false;
            this.btnHuy.Enabled = false;
        }
    }
}
