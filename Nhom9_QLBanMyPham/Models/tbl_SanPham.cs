namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_SanPham()
        {
            tbl_CTDonHang = new HashSet<tbl_CTDonHang>();
        }

        [Key]
        [StringLength(10)]
        public string PK_sMaSP { get; set; }

        [Required]
        [StringLength(200)]
        public string sTenSP { get; set; }

        public string sMoTa { get; set; }

        [StringLength(50)]
        public string sDungTich { get; set; }

        public DateTime? dNgayHetHan { get; set; }

        public double? fGiaBan { get; set; }

        public int? iSoLuongTon { get; set; }

        [StringLength(10)]
        public string FK_sMaNCC_SanPham { get; set; }

        [StringLength(50)]
        public string sMauSac { get; set; }

        [StringLength(255)]
        public string sHinhAnh { get; set; }

        [StringLength(10)]
        public string FK_sMaDM_SanPham { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_CTDonHang> tbl_CTDonHang { get; set; }

        public virtual tbl_DanhMuc tbl_DanhMuc { get; set; }

        public virtual tbl_NhaCungCap tbl_NhaCungCap { get; set; }
    }
}
