namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_HoaDon
    {
        [Key]
        public int PK_iMaHD { get; set; }

        public int? FK_iMaDH { get; set; }

        public DateTime? dNgayXuat { get; set; }

        public double? fTongTien { get; set; }

        [StringLength(100)]
        public string sPTThanhToan { get; set; }

        [StringLength(50)]
        public string sTrangThai { get; set; }

        public virtual tbl_DonHang tbl_DonHang { get; set; }
    }
}
