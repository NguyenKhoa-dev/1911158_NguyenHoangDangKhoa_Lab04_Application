using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frmMain : Form
    {
        QuanLySinhVien qlsv;
        bool checkedChange = false;
        public frmMain()
        {
            InitializeComponent();
        }

        #region PhuongThucBoTro

        private bool Check(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return true;
            return false;
        }

        private SinhVien LaySinhVien()
        {
            SinhVien sv = new SinhVien();
            sv.MSSV = txtMSSV.Text;
            sv.HoTen = txtHoTen.Text;
            sv.Email = txtEmail.Text;
            sv.DiaChi = txtDiaChi.Text;
            sv.NgaySinh = dtpNgaySinh.Value;
            sv.Phai = (rdNam.Checked) ? "Nam" : "Nữ";
            sv.Lop = cbbLop.Text;
            sv.SDT = txtSDT.Text;
            sv.Hinh = txtHinh.Text;
            return sv;
        }

        private SinhVien LaySinhVienTuListView(ListViewItem lvItem)
        {
            SinhVien sv = new SinhVien();
            sv.MSSV = lvItem.SubItems[0].Text;
            sv.HoTen = lvItem.SubItems[1].Text;
            sv.Phai = lvItem.SubItems[2].Text;
            sv.NgaySinh = DateTime.Parse(lvItem.SubItems[3].Text);
            sv.Lop = lvItem.SubItems[4].Text;
            sv.SDT = lvItem.SubItems[5].Text;
            sv.Email = lvItem.SubItems[6].Text;
            sv.DiaChi = lvItem.SubItems[7].Text;
            sv.Hinh = lvItem.SubItems[8].Text;
            return sv;
        }

        private void HienThiThongTinSV(SinhVien sv)
        {
            txtMSSV.Text = sv.MSSV;
            txtHoTen.Text = sv.HoTen;
            txtEmail.Text = sv.Email;
            txtDiaChi.Text = sv.DiaChi;
            txtHinh.Text = sv.Hinh;
            dtpNgaySinh.Value = sv.NgaySinh;
            if (sv.Phai == "Nam")
                rdNam.Checked = true;
            else
                rdNu.Checked = true;
            cbbLop.Text = sv.Lop;
            txtSDT.Text = sv.SDT;
            pbHinh.ImageLocation = txtHinh.Text;
        }

        private void ThemSV(SinhVien sv)
        {
            ListViewItem lvItem = new ListViewItem(sv.MSSV);
            lvItem.SubItems.Add(sv.HoTen);
            lvItem.SubItems.Add(sv.Phai);
            lvItem.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lvItem.SubItems.Add(sv.Lop);
            lvItem.SubItems.Add(sv.SDT);
            lvItem.SubItems.Add(sv.Email);
            lvItem.SubItems.Add(sv.DiaChi);
            lvItem.SubItems.Add(sv.Hinh);
            lvDanhSachSV.Items.Add(lvItem);
        }

        private void CapNhatSV(SinhVien sv)
        {
            sv.HoTen = txtHoTen.Text;
            sv.Email = txtEmail.Text;
            sv.DiaChi = txtDiaChi.Text;
            sv.NgaySinh = dtpNgaySinh.Value;
            sv.Phai = (rdNam.Checked) ? "Nam" : "Nữ";
            sv.Lop = cbbLop.Text;
            sv.SDT = txtSDT.Text;
            sv.Hinh = txtHinh.Text;
        }

        private void LoadListView()
        {
            lvDanhSachSV.Items.Clear();
            foreach (var sv in qlsv.LayDSSV())
            {
                ThemSV(sv);
            }
        }

        #endregion

        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Chọn hình sinh viên";
            
            dlg.Filter = "Image Files (JPEG, PNG)|"
            + "*.jpg;*.jpeg;"
            + "*.png|"
            + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
            + "PNG files (*.png)|*.png|"
            + "All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileName = dlg.FileName;
                txtHinh.Text = fileName;
                pbHinh.Load(fileName);
                checkedChange = true;
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile();
            LoadListView();
        }      

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            txtMSSV.Text = "";
            txtHoTen.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            rdNam.Checked = true;
            cbbLop.SelectedIndex = 0;
            txtSDT.Text = "";
            txtHinh.Text = "D:\\Images";
            pbHinh.ImageLocation = txtHinh.Text;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (Check(txtMSSV.Text) || Check(txtHoTen.Text) || Check(txtEmail.Text) 
                || Check(txtDiaChi.Text) || Check(cbbLop.Text) || Check(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin của sinh viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SinhVien sv = LaySinhVien();
            SinhVien svCheck = qlsv.LayDSSV().Find(p => p.MSSV == sv.MSSV);
            if (svCheck != null)
            {
                DialogResult result = MessageBox.Show("Đã tồn tại sinh viên trong danh sách, bạn có muốn cập nhật hay không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    CapNhatSV(svCheck);
                    LoadListView();
                }                    
            }
            else
            {
                MessageBox.Show("Đã thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                qlsv.Them(sv);
                LoadListView();
            }
            checkedChange = true;                            
        }        

        private void lvDanhSachSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = lvDanhSachSV.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem lvItem = lvDanhSachSV.SelectedItems[0];
                SinhVien sv = LaySinhVienTuListView(lvItem);
                HienThiThongTinSV(sv);
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = lvDanhSachSV.SelectedItems.Count;
            SinhVien sv;
            if (count > 0)
            {
                foreach (ListViewItem item in lvDanhSachSV.SelectedItems)
                {
                    sv = LaySinhVienTuListView(item);
                    qlsv.Xoa(sv);
                }
                LoadListView();
                btnMacDinh_Click(sender, e);
            }
            checkedChange = true;
        }

        private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            qlsv.DocTuFile();
            LoadListView();
            checkedChange = false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkedChange)
            {
                DialogResult result = MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Thông báo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
                if (result == DialogResult.Yes)
                    qlsv.GhiVaoFile(qlsv.LayDSSV());
            }
        }
    }
}
