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
    
    public partial class DANHSACHMAGIAMGIA
    {
        public string MaGiamGia { get; set; }
        public Nullable<System.DateTime> NgayHetHan { get; set; }
        public string LoaiSP { get; set; }
        public int MaKM { get; set; }
    
        public virtual CHUONGTRINHKHUYENMAI CHUONGTRINHKHUYENMAI { get; set; }
    }
}
