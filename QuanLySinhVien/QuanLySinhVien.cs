using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien
{
    public class QuanLySinhVien
    {
        readonly string filename = "Data.txt";
        List<SinhVien> DSSV;

        public QuanLySinhVien()
        {
            DSSV = new List<SinhVien>();
        }

        public void Them(SinhVien sv)
        {
            DSSV.Add(sv);
        }

        public void Xoa(SinhVien sv)
        {
            DSSV.RemoveAll(p => p.MSSV == sv.MSSV);
        }

        public void DocTuFile()
        {
            DSSV.Clear();
            string line;
            string[] s;
            SinhVien sv;
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(stream))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        s = line.Split('^');
                        sv = new SinhVien();
                        sv.MSSV = s[0];
                        sv.HoTen = s[1];
                        sv.Phai = s[2];
                        sv.NgaySinh = DateTime.Parse(s[3]);
                        sv.Lop = s[4];
                        sv.SDT = s[5];
                        sv.Email = s[6];
                        sv.DiaChi = s[7];
                        sv.Hinh = s[8];
                        Them(sv);
                    }
                }
            }            
        }

        public List<SinhVien> LayDSSV()
        {
            return DSSV;
        }

        public void GhiVaoFile(List<SinhVien> list)
        {            
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(stream))
                {
                    foreach (var sv in list)
                    {
                        sw.WriteLine(sv.ToString());
                    }
                }
            }            
        }
    }
}
