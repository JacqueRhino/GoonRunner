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
    
    public partial class TONKHO
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public Nullable<int> SoLuong { get; set; }
    
        public virtual SANPHAM SANPHAM { get; set; }
    }
}
