namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_KhachHang()
        {
            tbl_DonHang = new HashSet<tbl_DonHang>();
        }

        [Key]
        [StringLength(10)]
        public string PK_sMaKH { get; set; }

        [Required]
        [StringLength(100)]
        public string sTenKH { get; set; }

        [StringLength(15)]
        public string sSDT { get; set; }

        [StringLength(100)]
        public string sEmail { get; set; }

        public DateTime? dNgaySinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DonHang> tbl_DonHang { get; set; }
    }
}
