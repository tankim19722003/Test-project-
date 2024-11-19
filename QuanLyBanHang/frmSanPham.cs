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

namespace QuanLyBanHang
{
    public partial class frmSanPham : Form
    {
        //Khai báo biến kiểm tra việc Thêm hay Sửa dữ liệu
        bool Them;
        //Chuỗi kết nối
        //string strConnectionString = @"Server=.\SQLEXPRESS;Database=QuanLyBanHang;Integrated Security=True";
        //or
        string strConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=SSPI";

        //Đối tượng kết nối
        SqlConnection conn = null;
        //Đối tượng đưa dữ liệu vào DataTable dtHoaDon = null;
        SqlDataAdapter daSanPham = null;
        //Đối tượng hiển thị dữ liệu lên Form
        DataTable dtSanPham = null;

        //Thêm cho ví dụ 10.5
        //Đối tượng đưa dữ liệu vào DataTable dtKhachHang = null;
        //SqlDataAdapter daKhachHang = null;
        ////Đối tượng hiển thị dữ liệu lên Form
        //DataTable dtKhachHang = null;

        //SqlDataAdapter daNhanVien = null;
        ////Đối tượng hiển thị dữ liệu lên Form
        //DataTable dtNhanVien = null;

        public frmSanPham()
        {
            InitializeComponent();
        }

        void LoadData()
        {
            try
            {
                //Khởi động kết nối
                conn = new SqlConnection(strConnectionString);


                //Vận chuyển dữ liệu lên DataTable dtHoaDon
                daSanPham = new SqlDataAdapter("SELECT MaSP, TenSP, DonViTinh, DonGia FROM SanPham", conn);
                dtSanPham = new DataTable();
                dtSanPham.Clear();
                daSanPham.Fill(dtSanPham);
                //Đưa dữ liệu lên DataGridView
                this.dgvSanPham.DataSource = dtSanPham;

                //Bổ sung thêm cho ví dụ 10.5
                //Vận chuyển dữ liệu lên DataTable dtKhachHang dùng cho combobox
                //daKhachHang = new SqlDataAdapter("SELECT * FROM KhachHang", conn);
                //dtKhachHang = new DataTable();
                //dtKhachHang.Clear();
                //daKhachHang.Fill(dtKhachHang);

                /*
                //Vận chuyển dữ liệu lên DataTable dtNhanVien dùng cho combobox
                daNhanVien = new SqlDataAdapter("SELECT * FROM NhanVien", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);
                */
                //Vận chuyển dữ liệu lên DataTable dtHoaDon
                //daNhanVien = new SqlDataAdapter("SELECT MaNV, (Ho + ' ' + Ten) AS HoTen FROM NhanVien", conn);
                //dtNhanVien = new DataTable();
                //dtNhanVien.Clear();
                //daNhanVien.Fill(dtNhanVien);

                //  Đưa dữ liệu lên ComboBox trong DataGridView   
                (dgvSanPham.Columns["TenSP"] as DataGridViewComboBoxColumn).DataSource = dtSanPham;
                (dgvSanPham.Columns["TenSP"] as DataGridViewComboBoxColumn).DisplayMember = "TenSP";
                (dgvSanPham.Columns["TenSP"] as DataGridViewComboBoxColumn).ValueMember = "TenSP";

                //  Đưa dữ liệu lên ComboBox trong DataGridView   
                (dgvSanPham.Columns["DonViTinh"] as DataGridViewComboBoxColumn).DataSource = dtSanPham;
                (dgvSanPham.Columns["DonViTinh"] as DataGridViewComboBoxColumn).DisplayMember = "DonViTinh";
                (dgvSanPham.Columns["DonViTinh"] as DataGridViewComboBoxColumn).ValueMember = "DonViTinh";


                //Xóa các đối tượng trong Panel
                this.txtMaSP.ResetText();
                this.txtDonGia.ResetText();
                this.txtTenSP.ResetText();
                this.cbDonViTinh.ResetText();
                //Không cho thao tác trên các nút Lưu / Hủy
                this.btnLuu.Enabled = false;
                this.btnHuy.Enabled = false;
                this.pnlThongTinHD.Enabled = false;
                //Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
                this.btnThem.Enabled = true;
                this.btnSua.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnThoat.Enabled = true;
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong table. Lỗi rồi!!!");
            }
        }
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void frmSanPham_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Giải phóng tài nguyên
            dtSanPham.Dispose();
            dtSanPham = null;
            //hủy kết nối
            conn = null;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            try
            {
                // Lấy thứ tự record hiện hành
                int r = dgvSanPham.CurrentCell.RowIndex;
                // Lấy MaSP của record hiện hành
                string strMASP = dgvSanPham.Rows[r].Cells[0].Value.ToString();

                // Thực hiện lệnh
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM SanPham WHERE MaSP = @MaSP"
                };

                // Thêm tham số
                cmd.Parameters.AddWithValue("@MaSP", strMASP);

                // Thực hiện câu lệnh SQL
                cmd.ExecuteNonQuery();

                // Cập nhật lại DataGridView
                LoadData();

                // Thông báo
                MessageBox.Show("Đã xóa xong!");
            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
            finally
            {
                // Đóng kết nối
                conn.Close();
            }
        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
            //Xóa trống các đối tượng trong panel
            this.txtMaSP.ResetText();
            this.txtDonGia.ResetText();
            this.txtTenSP.ResetText();
            this.cbDonViTinh.ResetText();

            //Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = true;
            this.btnSua.Enabled = true;
            this.btnXoa.Enabled = true;
            this.btnThoat.Enabled = true;
            //Không cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = false;
            this.btnHuy.Enabled = false;
            this.pnlThongTinHD.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //Kích hoạt biến Them
            Them = true;
            //Xóa trống các đối tượng trong Panel
            this.txtMaSP.ResetText();
            this.txtDonGia.ResetText();

            //Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuy.Enabled = true;
            this.pnlThongTinHD.Enabled = true;
            //Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnThoat.Enabled = false;
            //Đưa dữ liệu lên ComboBox
            //this.cbTenSP.DataSource = dtSanPham;
            //this.cbTenSP.DisplayMember = "TenSP";
            //this.cbTenSP.ValueMember = "TenSP";

            //this.cbDonViTinh.DataSource = dtSanPham;
            //this.cbDonViTinh.DisplayMember = "DonViTinh";
            //this.cbDonViTinh.ValueMember = "DonViTinh";

            //Đưa con trỏ đến TextField txtMaHD
            this.txtMaSP.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            //Kích hoạt biến Sửa
            Them = false;
            //Đưa dữ liệu lên 2 ComboBox
            //this.cbTenSP.DataSource = dtSanPham;
            ////this.cbTenSP.DisplayMember = "TenSP";
            //this.cbTenSP.ValueMember = "TenSP";

            //this.cbDonViTinh.DataSource = dtSanPham;
            //this.cbDonViTinh.DisplayMember = "DonViTinh";
            //this.cbDonViTinh.ValueMember = "DonViTinh";

            //Cho phép thao tác trên Panel
            this.pnlThongTinHD.Enabled = true;
            //Thứ tự dòng hiện hành
            int r = dgvSanPham.CurrentCell.RowIndex;
            //Chuyển thông tin lên panel
            this.txtMaSP.Text = dgvSanPham.Rows[r].Cells[0].Value.ToString();
            this.txtTenSP.Text = dgvSanPham.Rows[r].Cells[1].Value.ToString();
            this.cbDonViTinh.Text = dgvSanPham.Rows[r].Cells[2].Value.ToString();
            this.txtDonGia.Text = dgvSanPham.Rows[r].Cells[3].Value.ToString();

            //Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuy.Enabled = true;
            this.pnlThongTinHD.Enabled = true;
            //Không cho thao tác trên các nút thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnThoat.Enabled = false;
            //Đưa con trỏ đến TextField txtMaKH
            this.txtMaSP.Focus();

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            try
            {
                // Kiểm tra thông tin cần thiết
                if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text) || string.IsNullOrEmpty(cbDonViTinh.Text) || string.IsNullOrEmpty(txtDonGia.Text))
                {
                    MessageBox.Show("Không thêm/sửa được. Vui lòng điền đầy đủ thông tin.");
                    return;
                }

                // Tạo đối tượng SqlCommand
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                if (Them) // Thêm mới
                {
                    cmd.CommandText = "INSERT INTO SanPham (MaSP, TenSP, DonViTinh, DonGia, Hinh) VALUES (@MaSP, @TenSP, @DonViTinh, @DonGia, null)";

                    cmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text);
                    cmd.Parameters.AddWithValue("@TenSP", txtTenSP.Text);
                    cmd.Parameters.AddWithValue("@DonViTinh", cbDonViTinh.Text);
                    cmd.Parameters.AddWithValue("@DonGia", float.Parse(txtDonGia.Text)); // Chuyển đổi thành float

                    // Thực hiện lệnh
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm xong!");
                }
                else // Cập nhật
                {
                    int r = dgvSanPham.CurrentCell.RowIndex;
                    string strMaSP = dgvSanPham.Rows[r].Cells[0].Value.ToString();

                    cmd.CommandText = "UPDATE SanPham SET TenSP=@TenSP, DonViTinh=@DonViTinh, DonGia=@DonGia WHERE MaSP=@MaSP";

                    cmd.Parameters.AddWithValue("@MaSP", strMaSP);
                    cmd.Parameters.AddWithValue("@TenSP", txtTenSP.Text);
                    cmd.Parameters.AddWithValue("@DonViTinh", cbDonViTinh.Text);
                    cmd.Parameters.AddWithValue("@DonGia", float.Parse(txtDonGia.Text)); // Chuyển đổi thành float

                    // Cập nhật
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


        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
