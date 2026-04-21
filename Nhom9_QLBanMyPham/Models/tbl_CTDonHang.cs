namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_CTDonHang
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PK_iMaDH_CTDonHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string PK_sMaSP_CTDonHang { get; set; }

        public int iSoLuongBan { get; set; }

        public double fGiaBanLucDo { get; set; }

        public virtual tbl_DonHang tbl_DonHang { get; set; }

        public virtual tbl_SanPham tbl_SanPham { get; set; }
    }
}
