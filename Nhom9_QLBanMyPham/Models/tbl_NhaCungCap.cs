namespace Nhom9_QLBanMyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_NhaCungCap()
        {
            tbl_SanPham = new HashSet<tbl_SanPham>();
        }

        [Key]
        [StringLength(10)]
        public string PK_sMaNCC { get; set; }

        [Required]
        [StringLength(200)]
        public string sTenNCC { get; set; }

        [StringLength(255)]
        public string sDiaChi { get; set; }

        [StringLength(15)]
        public string sSDT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SanPham> tbl_SanPham { get; set; }
    }
}
