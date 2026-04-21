namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_NguoiDung()
        {
            tbl_DonHang = new HashSet<tbl_DonHang>();
        }

        [Key]
        [StringLength(10)]
        public string PK_sMaNV { get; set; }

        [Required]
        [StringLength(100)]
        public string sTenNV { get; set; }

        [StringLength(15)]
        public string sSDT { get; set; }

        [StringLength(50)]
        public string sVaiTro { get; set; }

        [StringLength(20)]
        public string sMatKhau { get; set; }

        [StringLength(50)]
        public string sPhanQuyen { get; set; }

        public double? fLuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DonHang> tbl_DonHang { get; set; }
    }
}
