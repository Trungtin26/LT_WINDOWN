using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using De01.DE01;

namespace De01
{
    public partial class b : Form
    {
        private Model1 dbContext;

        public b()
        {
            InitializeComponent();
            dbContext = new Model1();
            LoadData();
        }

        private void LoadData()
        {
            // Load danh sách sinh viên lên ListView
            lvSinhVien.Items.Clear();
            var sinhViens = dbContext.SINHVIENs.Include(s => s.LOP).ToList();

            foreach (var sv in sinhViens)
            {
                var item = new ListViewItem(sv.MaSV);
                item.SubItems.Add(sv.HotenSV);
                item.SubItems.Add(sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToString("yyyy-MM-dd") : "");
                item.SubItems.Add(sv.LOP?.TenLop);
                lvSinhVien.Items.Add(item);
            }

            // Load danh sách lớp vào ComboBox
            comboBox1.DataSource = dbContext.LOPs.ToList();
            comboBox1.DisplayMember = "TenLop";
            comboBox1.ValueMember = "MaLop";
        }

        private void ClearInput()
        {
            // Xóa dữ liệu trên các ô nhập liệu
            textBox1.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void BtTim_Click(object sender, EventArgs e)
        {
            // Tìm kiếm sinh viên
            string keyword = textBox2.Text.Trim();
            var results = dbContext.SINHVIENs
                .Include(s => s.LOP)
                .Where(s => s.MaSV.Contains(keyword) || s.HotenSV.Contains(keyword))
                .ToList();

            lvSinhVien.Items.Clear();
            foreach (var sv in results)
            {
                var item = new ListViewItem(sv.MaSV);
                item.SubItems.Add(sv.HotenSV);
                item.SubItems.Add(sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToString("yyyy-MM-dd") : "");
                item.SubItems.Add(sv.LOP?.TenLop);
                lvSinhVien.Items.Add(item);
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            // Thêm sinh viên mới
            try
            {
                var sinhVien = new SINHVIEN
                {
                    MaSV = textBox1.Text.Trim(),
                    HotenSV = textBox3.Text.Trim(),
                    NgaySinh = dateTimePicker1.Value,
                    MaLop = comboBox1.SelectedValue.ToString()
                };

                dbContext.SINHVIENs.Add(sinhVien);
                dbContext.SaveChanges();
                LoadData();
                ClearInput();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi");
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            // Xóa sinh viên được chọn
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                var selected = lvSinhVien.SelectedItems[0];
                string maSV = selected.SubItems[0].Text;

                var sinhVien = dbContext.SINHVIENs.Find(maSV);
                if (sinhVien != null)
                {
                    dbContext.SINHVIENs.Remove(sinhVien);
                    dbContext.SaveChanges();
                    LoadData();
                    MessageBox.Show("Xóa sinh viên thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!", "Thông báo");
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            // Sửa thông tin sinh viên
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                var selected = lvSinhVien.SelectedItems[0];
                string maSV = selected.SubItems[0].Text;

                var sinhVien = dbContext.SINHVIENs.Find(maSV);
                if (sinhVien != null)
                {
                    sinhVien.HotenSV = textBox2.Text.Trim();
                    sinhVien.NgaySinh = dateTimePicker1.Value;
                    sinhVien.MaLop = comboBox1.SelectedValue.ToString();

                    dbContext.SaveChanges();
                    LoadData();
                    ClearInput();
                    MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa!", "Thông báo");
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            // Lưu thay đổi (nếu cần)
            dbContext.SaveChanges();
            MessageBox.Show("Dữ liệu đã được lưu!", "Thông báo");
        }

        private void btKluu_Click(object sender, EventArgs e)
        {
            // Hủy thay đổi chưa lưu
            dbContext = new Model1();
            LoadData();
            ClearInput();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            // Thoát ứng dụng
            Application.Exit();
        }

        
    }
}


  