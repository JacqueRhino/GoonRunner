//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoonRunner.MVVM.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHUONGTRINHKHUYENMAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHUONGTRINHKHUYENMAI()
        {
            this.DANHSACHMAGIAMGIAs = new HashSet<DANHSACHMAGIAMGIA>();
        }
    
        public int MaKM { get; set; }
        public string TenKM { get; set; }
        public Nullable<System.DateTime> NgayBatDau { get; set; }
        public Nullable<System.DateTime> NgayKetThuc { get; set; }
        public int MaSP { get; set; }
    
        public virtual SANPHAM SANPHAM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DANHSACHMAGIAMGIA> DANHSACHMAGIAMGIAs { get; set; }
    }
}
