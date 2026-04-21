namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_DonHang()
        {
            tbl_CTDonHang = new HashSet<tbl_CTDonHang>();
            tbl_HoaDon = new HashSet<tbl_HoaDon>();
        }

        [Key]
        public int PK_iMaDH { get; set; }

        public DateTime? dNgayLap { get; set; }

        [StringLength(10)]
        public string FK_sMaKH_DonHang { get; set; }

        [StringLength(10)]
        public string FK_sMaNV_DonHang { get; set; }

        public double? fTongTien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_CTDonHang> tbl_CTDonHang { get; set; }

        public virtual tbl_KhachHang tbl_KhachHang { get; set; }

        public virtual tbl_NguoiDung tbl_NguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_HoaDon> tbl_HoaDon { get; set; }
    }
}
