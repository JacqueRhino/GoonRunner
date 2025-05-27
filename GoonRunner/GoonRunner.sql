/* Tạo Database */
USE [master]

CREATE DATABASE [GoonRunner]
    ON PRIMARY
    ( NAME = N'GoonRunner', FILENAME = N'E:\Khang\Database\GoonRunner.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
    LOG ON
    ( NAME = N'GoonRunner_log', FILENAME = N'E:\Khang\Database\GoonRunner_log.ldf' , SIZE = 5184KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)

/* Ngắt kết nối và Drop Database */
USE [master]
ALTER DATABASE [GoonRunner] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; /* Force Disconnect */
Drop database [GoonRunner]

USE [GoonRunner]

/* Table NHANVIEN */
Create Table NHANVIEN (
                          MaNV int identity(0,1) primary key not null,
                          HoNV nvarchar(50) not null,
                          TenNV nvarchar(20) not null,
                          GioiTinh nvarchar(3) not null,
                          SdtNV varchar(20) not null,
                          NgaySinh smalldatetime not null,
                          DiaChiNV nvarchar(100) not null,
                          CMND char(12) unique not null,
                          ChucVu nvarchar(50) not null,
                          Luong int,
                          MaPB int not null,
);

/* Table ACCNHANVIEN */
Create Table ACCNHANVIEN (
                             DisplayName nvarchar(50) primary key not null,
                             UserName varchar(50),
                             Pass varchar(32), /* Mã hoá md5 sẽ có 32 ký tự nên varchar(32) */
                             Quyen nvarchar(50),
                             MaNV int not null,
);

/* Table HINHNHANVIEN */
Create Table HINHNHANVIEN (
                              MaNV int not null,
                              HinhNV varbinary(max)
);

/* Table PHONGBAN */
Create Table PHONGBAN (
                          MaPB int identity(1,1) primary key not null,
                          TenPB nvarchar(50) not null,
                          SoLuongNV int,
);

/* Table GIAOCA */
Create Table GIAOCA (
                        MaGC int identity(1,1) primary key not null,
                        MaNV int not null,
                        NgayLamViec smalldatetime,
                        NgayTrongTuan smalldatetime,
                        CaLamViec int,
                        TrangThaiCa nvarchar(20),
);

/* Table SANPHAM */
Create Table SANPHAM (
                         MaSP int identity(1,1) primary key not null,
                         TenSP nvarchar(200) not null,
                         LoaiSP nvarchar(50) not null,
                         ThoiGianBH int,
                         GiaSP int,
                         NhaSX nvarchar(100) not null,
                         CoKhuyenMai bit, /* 0 là false, 1 là true */
);

/* Table NHACUNGCAP */
Create Table NHACUNGCAP (
                            MaNCC int identity(1,1) primary key not null,
                            TenNCC nvarchar(100) not null
);

/* Table KHACHHANG */
Create Table KHACHHANG (
                           MaKH int identity(1,1) primary key not null,
                           HoKH nvarchar(50) not null,
                           TenKH nvarchar(20) not null,
                           SdtKH varchar(20) not null,
                           NgaySinh datetime,
                           DiaChi nvarchar(100) not null,
);

/* Table HOADON */
Create table HOADON (
                        MaHD int identity(1,1) primary key not null,
                        MaKH int not null,
                        HoKH nvarchar(50) not null,
                        TenKH nvarchar(20) not null,
                        SdtKH varchar(20) not null,
                        DiaChi nvarchar(100) not null,
                        MaNV int not null,
                        HoNV nvarchar(50) not null,
                        TenNV nvarchar(20) not null,
                        NgayMuaHang smalldatetime,
);

/* Table CHITIETHOADON */
Create Table CHITIETHOADON (
                               MaHD int not null,
                               MaSP int not null,
                               TenSP nvarchar(200) not null,
                               SoLuongDat int,
                               DonGia int,
                               ThanhTien int,
                               primary key (MaHD, MaSP)
);
alter table CHITIETHOADON
    drop column TongTien

/* Table PHIEUBAOHANH */
Create Table PHIEUBAOHANH (
                              MaBH int identity(1,1) primary key not null,
                              HoKH nvarchar(50) not null,
                              TenKH nvarchar(20) not null,
                              NgayMuaHang smalldatetime,
                              MaNV int not null,
);

/* Table CHITIETBAOHANH */
Create Table CHITIETBAOHANH (
                                MaBH int not null,
                                MaSP int not null,
                                TenSP nvarchar(200) not null,
                                SoLuongDat int,
                                ThoiGianBH int,
                                Primary key (MaBH, MaSP)
);

/* Table PHIEUNHAPHANG */
Create Table PHIEUNHAPHANG (
                               MaPNH int identity(1,1) primary key not null,
                               MaNCC int not null,
                               TenNCC nvarchar(100) not null,
                               NgayNhap smalldatetime,
                               MaNV int not null,
);

/* Table CHITIETPHIEUNHAPHANG */
Create Table CHITIETPHIEUNHAPHANG (
                                      MaPNH int not null,
                                      MaSP int not null,
                                      TenSP nvarchar(200) not null,
                                      SoLuongNhap int,
                                      DonGia int,
                                      Primary key (MaPNH, MaSP),
);

/* Table TONKHO */
/*Create Table TONKHO (
	MaSP int not null,
	TenSP nvarchar(200) not null,
	SoLuong int not null,
);*/

/* Table CHUONGTRINHKHUYENMAI */
Create Table CHUONGTRINHKHUYENMAI (
                                      MaKM int identity(1,1) primary key not null,
                                      TenKM nvarchar(100) not null,
                                      NgayBatDau smalldatetime,
                                      NgayKetThuc smalldatetime,
                                      MaSP int not null,
);

/* Table DANHSACHMAGIAMGIA */
Create Table DANHSACHMAGIAMGIA (
                                   MaGiamGia char(10) primary key not null,
                                   NgayHetHan smalldatetime,
                                   LoaiSP nvarchar(50) not null,
                                   MaKM int not null,
);

/* Tạo ràng buộc */
ALTER TABLE ACCNHANVIEN WITH CHECK ADD FOREIGN KEY (MaNV) REFERENCES NHANVIEN (MaNV)
ALTER TABLE HINHNHANVIEN WITH CHECK ADD FOREIGN KEY (MaNV) REFERENCES NHANVIEN (MaNV)
ALTER TABLE NHANVIEN WITH CHECK ADD FOREIGN KEY (MaPB) REFERENCES PHONGBAN (MaPB)
ALTER TABLE GIAOCA WITH CHECK ADD FOREIGN KEY (MaNV) REFERENCES NHANVIEN (MaNV)
ALTER TABLE HOADON WITH CHECK ADD FOREIGN KEY (MaKH) REFERENCES KHACHHANG (MaKH)
ALTER TABLE HOADON WITH CHECK ADD FOREIGN KEY (MaNV) REFERENCES NHANVIEN (MaNV)
ALTER TABLE CHITIETHOADON WITH CHECK ADD FOREIGN KEY (MaHD) REFERENCES HOADON (MaHD)
ALTER TABLE CHITIETHOADON WITH CHECK ADD FOREIGN KEY (MaSP) REFERENCES SANPHAM (MaSP)
ALTER TABLE PHIEUBAOHANH WITH CHECK ADD FOREIGN KEY (MaNV) REFERENCES NHANVIEN (MaNV)
ALTER TABLE CHITIETBAOHANH WITH CHECK ADD FOREIGN KEY (MaBH) REFERENCES PHIEUBAOHANH (MaBH)
ALTER TABLE PHIEUNHAPHANG WITH CHECK ADD FOREIGN KEY (MaNCC) REFERENCES NHACUNGCAP (MaNCC)
ALTER TABLE PHIEUNHAPHANG WITH CHECK ADD FOREIGN KEY (MaNV) REFERENCES NHANVIEN (MaNV)
ALTER TABLE CHITIETPHIEUNHAPHANG WITH CHECK ADD FOREIGN KEY (MaPNH) REFERENCES PHIEUNHAPHANG (MaPNH)
ALTER TABLE CHITIETPHIEUNHAPHANG WITH CHECK ADD FOREIGN KEY (MaSP) REFERENCES SANPHAM (MaSP)
--ALTER TABLE TONKHO WITH CHECK ADD FOREIGN KEY (MaSP) REFERENCES SANPHAM (MaSP)
ALTER TABLE CHUONGTRINHKHUYENMAI WITH CHECK ADD FOREIGN KEY (MaSP) REFERENCES SANPHAM (MaSP)
ALTER TABLE DANHSACHMAGIAMGIA WITH CHECK ADD FOREIGN KEY (MaKM) REFERENCES CHUONGTRINHKHUYENMAI (MaKM)

/* Drop nếu bị lỗi */
DROP TABLE ACCNHANVIEN
DROP TABLE HINHNHANVIEN
DROP TABLE CHITIETHOADON
DROP TABLE CHITIETBAOHANH
DROP TABLE CHUONGTRINHKHUYENMAI
DROP TABLE DANHSACHMAGIAMGIA
DROP TABLE KHACHHANG
DROP TABLE NHACUNGCAP
DROP TABLE NHANVIEN
DROP TABLE PHIEUNHAPHANG
DROP TABLE PHONGBAN
DROP TABLE SANPHAM
DROP TABLE TONKHO
DROP TABLE PHIEUBAOHANH
DROP TABLE CHITIETPHIEUNHAPHANG
DROP TABLE HOADON
DROP TABLE GIAOCA

/* Dữ liệu PHONGBAN */
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng bán hàng', 5);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng kế toán', 5);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng bảo hành', 5);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng marketing', 5);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng tạp vụ', 2);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng kho', 3);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng bảo vệ', 3);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng chăm sóc khách hàng', 3);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng vận chuyển', 3);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng bảo trì web', 3);
INSERT INTO PHONGBAN (TenPB, SoLuongNV)
VALUES (N'Phòng giám đốc', 1);

/* Dữ liệu NHANVIEN */
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Tạ Tích', N'Khang', N'Nam', '0980382308', convert(datetime,'13-08-1989',105), N'TPHCM', '076626719348', N'Chủ cửa hàng', 100000000, 11);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Đặng Quốc', N'Phong', N'Nam', '0980382213', convert(datetime,'13-08-1989',105), N'Cần Thơ', '076626719588', N'Nhân viên kiểm kho', 15000000, 6);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Nguyễn Thị', N'Lan', N'Nữ', '0912345678', convert(datetime,'25-12-1992',105), N'Hà Nội', '079812345678', N'Nhân viên kỹ thuật', 12000000, 3);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Trần Văn', N'Bình', N'Nam', '0908765432', convert(datetime,'05-06-1985',105), N'Đà Nẵng', '078976543210', N'Quản lý cửa hàng', 20000000, 1);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Phạm Minh', N'Hòa', N'Nam', '0976543210', convert(datetime,'14-02-1990',105), N'Hải Phòng', '077123456789', N'Nhân viên bán hàng', 12000000, 1);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Lê Thị', N'Hiền', N'Nữ', '0965123456', convert(datetime,'30-07-1995',105), N'Bình Dương', '078654123987', N'Nhân viên kiểm kho', 13000000, 6);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Hoàng Văn', N'Nam', N'Nam', '0934678921', convert(datetime,'18-09-1988',105), N'Quảng Ninh', '077654321098', N'Nhân viên bảo vệ', 9000000, 7);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Đỗ Thị', N'Hồng', N'Nữ', '0923456789', convert(datetime,'11-04-1993',105), N'Hưng Yên', '078321098765', N'Nhân viên marketing', 16000000, 4);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Bùi Thanh', N'Tùng', N'Nam', '0945678901', convert(datetime,'22-05-1987',105), N'Thái Nguyên', '079567890123', N'Nhân viên chăm sóc khách hàng', 17000000, 3);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Ngô Minh', N'Châu', N'Nữ', '0956789012', convert(datetime,'29-10-1991',105), N'Cà Mau', '077890123456', N'Nhân viên kế toán', 14000000, 2);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Vũ Thị', N'Mai', N'Nữ', '0912340987', convert(datetime,'06-12-1996',105), N'Thanh Hóa', '078098765432', N'Nhân viên chăm sóc khách hàng', 10000000, 8);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Đinh Văn', N'Thắng', N'Nam', '0987456123', convert(datetime,'17-07-1984',105), N'Nam Định', '077612345678', N'Nhân viên quản trị mạng', 15000000, 10);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Tạ Minh', N'Long', N'Nam', '0906789234', convert(datetime,'02-03-1990',105), N'Ninh Bình', '079234567890', N'Nhân viên kế toán', 13500000, 2);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Lương Thị', N'Ngọc', N'Nữ', '0978456098', convert(datetime,'15-05-1986',105), N'Vĩnh Phúc', '078765432109', N'Nhân viên quản trị mạng', 13000000, 10);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Trịnh Quang', N'Huy', N'Nam', '0926789012', convert(datetime,'24-11-1994',105), N'Bắc Giang', '077098765432', N'Nhân viên kỹ thuật', 12000000, 3);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Nguyễn Hoàng', N'An', N'Nam', '0935678901', convert(datetime,'05-09-1989',105), N'Bạc Liêu', '078890123456', N'Nhân viên bán hàng', 12000000, 1);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Phan Thị', N'Mỹ', N'Nữ', '0912345670', convert(datetime,'10-06-1992',105), N'Tây Ninh', '079123456789', N'Nhân viên bán hàng', 13000000, 1);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Huỳnh Văn', N'Phong', N'Nam', '0956789023', convert(datetime,'21-01-1985',105), N'Sóc Trăng', '077345678901', N'Nhân viên tạp vụ', 12000000, 5);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Quách Thị', N'Bích', N'Nữ', '0947890123', convert(datetime,'03-08-1997',105), N'Hậu Giang', '078901234567', N'Nhân viên tạp vụ', 12000000, 5);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Lý Minh', N'Trí', N'Nam', '0923456780', convert(datetime,'30-07-1983',105), N'An Giang', '077567890123', N'Nhân viên giao hàng', 13000000, 9);
INSERT INTO NHANVIEN (HoNV, TenNV, GioiTinh, SdtNV, NgaySinh, DiaChiNV, CMND, ChucVu, Luong, MaPB)
VALUES (N'Tống Hoài', N'Nam', N'Nam', '0989012345', convert(datetime,'12-04-1990',105), N'Kiên Giang', '078678901234', N'Nhân viên giao hàng', 13000000, 9);

/* Dữ liệu ACCNHANVIEN */
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Tạ Tích Khang', N'goonrunner', N'72122ce96bfec66e2396d2e25225d70a', N'Chủ cửa hàng', 0);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Đặng Quốc Phong', N'PhongLong', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên kiểm kho', 1);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Nguyễn Thị Lan', N'Lan', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên kỹ thuật', 2);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass admin */
VALUES (N'Trần Văn Bình', N'Binh', N'21232f297a57a5a743894a0e4a801fc3', N'Admin', 3);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Phạm Minh Hòa', N'Hoa', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên bán hàng', 4);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Lê Thị Hiền', N'Hien', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên kiểm kho', 5);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Đỗ Thị Hồng', N'Hong', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên marketing', 7);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Bùi Thanh Tùng', N'Tung', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên chăm sóc khách hàng', 8);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Ngô Minh Châu', N'Chau', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên kế toán', 9);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Vũ Thị Mai', N'Mai', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên chăm sóc khách hàng', 10);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Đinh Văn Thắng', N'Thang', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên quản trị mạng', 11);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Tạ Minh Long', N'Long', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên kế toán', 12);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Lương Thị Ngọc', N'Ngoc', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên quản trị mạng', 13);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Trịnh Quang Huy', N'Huy', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên kỹ thuật', 14);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Nguyễn Hoàng An', N'An', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên bán hàng', 15);
INSERT INTO ACCNHANVIEN (DisplayName, UserName, Pass, Quyen, MaNV) /* Pass 12345 */
VALUES (N'Phan Thị Mỹ', N'My', N'827ccb0eea8a706c4c34a16891f84e7b', N'Nhân viên bán hàng', 16);

/* Dữ liệu HINHNHANVIEN */
--INSERT INTO HINHNHANVIEN (MaNV, HinhNV) VALUES ('NV001', (SELECT * FROM OPENROWSET (BULK 'path\to\Database\HinhNhanVien\okarun.png', SINGLE_BLOB) AS Hinh))

/* Dữ liệu SANPHAM */
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Intel Core i9-13900K', N'CPU', 36, 14500000, N'Intel', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'AMD Ryzen 9 7950X', N'CPU', 36, 15000000, N'AMD', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Mainboard ASUS ROG Strix Z790-E', N'Mainboard', 36, 9000000, N'Asus', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Mainboard MSI MAG B660M Mortar', N'Mainboard', 24, 4500000, N'MSI', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'RAM Corsair Vengeance 32GB DDR5', N'RAM', 36, 4800000, N'Corsair', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'RAM Kingston Fury 16GB DDR4', N'RAM', 24, 2100000, N'Kingston', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'SSD Samsung 990 Pro 2TB NVMe', N'SSD', 36, 7500000, N'Samsung', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'SSD WD Black SN850X 1TB NVMe', N'SSD', 36, 4500000, N'Western Digital', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Card đồ họa NVIDIA RTX 4090', N'GPU', 36, 50000000, N'NVIDIA', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Card đồ họa AMD Radeon RX 7900 XTX', N'GPU', 36, 37000000, N'AMD', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Nguồn Corsair RM1000e 1000W', N'PSU', 36, 3800000, N'Corsair', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Case NZXT H7 Flow', N'Case', 24, 2500000, N'NZXT', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Màn hình LG UltraGear 32GQ950-B', N'Màn hình', 36, 18000000, N'LG', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Màn hình Dell U2723QE', N'Màn hình', 36, 13500000, N'Dell', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Bàn phím cơ Keychron K8 Pro', N'Bàn phím', 12, 3200000, N'Keychron', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Laptop Dell XPS 15', N'Laptop', 36, 35000000, N'Dell', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Intel Core i7-13700K', N'CPU', 36, 12500000, N'Intel', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'AMD Ryzen 7 7700X', N'CPU', 36, 10000000, N'AMD', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Mainboard Gigabyte B760 AORUS ELITE AX', N'Mainboard', 36, 6500000, N'Gigabyte', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'RAM G.Skill Trident Z5 32GB DDR5', N'RAM', 24, 5500000, N'G.Skill', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'SSD Kingston NV2 1TB NVMe', N'SSD', 36, 2500000, N'Kingston', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Card đồ họa ASUS TUF RTX 4070 Ti', N'GPU', 36, 22000000, N'Asus', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Intel Core i9-14900K', N'CPU', 36, 14500000, N'Intel', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'AMD Ryzen 9 7950X', N'CPU', 36, 13200000, N'AMD', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'NVIDIA GeForce RTX 4080', N'GPU', 36, 34000000, N'NVIDIA', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'ASUS ROG Strix RTX 4070 Ti', N'GPU', 36, 28500000, N'Asus', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Kingston Fury 32GB DDR5', N'RAM', 36, 4200000, N'Kingston', 0);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'Samsung 980 Pro 2TB NVMe', N'SSD', 36, 5500000, N'Samsung', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'WD Black SN850X 1TB NVMe', N'SSD', 36, 3900000, N'Western Digital', 1);
INSERT INTO SANPHAM (TenSP, LoaiSP, ThoiGianBH, GiaSP, NhaSX, CoKhuyenMai)
VALUES (N'MSI MPG A1000G 1000W', N'PSU', 36, 3800000, N'MSI', 1);

/* Dữ liệu KHACHHANG */
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Nguyễn Văn', N'Anh', '0901234567', convert(datetime,'15-01-1990',105), N'123 Nguyễn Thị Minh Khai, Phường Bến Nghé, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Trần Minh', N'Bảo', '0912345678', convert(datetime,'20-03-1985',105), N'456 Lý Tự Trọng, Phường Bến Thành, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Lê Thị', N'Hương', '0933456789', convert(datetime,'10-05-1992',105), N'789 Phạm Ngọc Thạch, Phường 6, Quận 3');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Phan Văn', N'Minh', '0944567890', convert(datetime,'25-07-1988',105), N'101 Hai Bà Trưng, Phường Đa Kao, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Hoàng Thị', N'Lan', '0975678901', convert(datetime,'01-12-1994',105), N'202 Nguyễn Đình Chiểu, Phường 5, Quận 3');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Vũ Tuấn', N'Sơn', '0906789012', convert(datetime,'22-11-1989',105), N'303 Trường Chinh, Phường 13, Quận Tân Bình');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Nguyễn Thị', N'Lý', '0917890123', convert(datetime,'30-06-1996',105), N'404 Cách Mạng Tháng 8, Phường 10, Quận 10');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Trần Duy', N'Tâm', '0938901234', convert(datetime,'12-09-1987',105), N'505 Phan Đình Phùng, Phường 4, Quận 5');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Lê Thị', N'Thảo', '0949012345', convert(datetime,'03-02-1993',105), N'606 Nguyễn Văn Cừ, Phường Cầu Kho, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Phan Thiều', N'Kiệt', '0970123456', convert(datetime,'17-04-1986',105), N'707 Lê Hồng Phong, Phường 1, Quận 10');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Hoàng Quang', N'Duy', '0901230987', convert(datetime,'19-08-1995',105), N'808 Trần Quang Khải, Phường Tân Định, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Vũ Thị', N'Thúy', '0912340987', convert(datetime,'25-10-1990',105), N'909 Võ Văn Tần, Phường 6, Quận 3');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Nguyễn Quang', N'Bảo', '0934567891', convert(datetime,'01-07-1988',105), N'1010 Nguyễn Thị Minh Khai, Phường Đa Kao, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Trần Thị', N'Vân', '0945678912', convert(datetime,'18-03-1992',105), N'1111 Lý Thường Kiệt, Phường 4, Quận 10');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Lê Vĩnh', N'Mai', '0976789013', convert(datetime,'29-06-1984',105), N'1212 Nguyễn Trãi, Phường Nguyễn Cư Trinh, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Phan Đình', N'Hà', '0907890124', convert(datetime,'09-01-1999',105), N'1313 Hồng Bàng, Phường 1, Quận 11');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Hoàng Thị', N'Thiện', '0918901235', convert(datetime,'21-02-1991',105), N'1414 Tân Kỳ Tân Quý, Phường Tân Quý, Quận Tân Phú');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Vũ Phương', N'Lan', '0939012346', convert(datetime,'15-05-1993',105), N'1515 Đường 3/2, Phường 12, Quận 10');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Nguyễn Thị', N'Linh', '0970123457', convert(datetime,'11-10-1987',105), N'1616 Lê Văn Sỹ, Phường 13, Quận 3');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Trần Quang', N'Tâm', '0901234678', convert(datetime,'17-01-1989',105), N'1717 Trường Chinh, Phường 14, Quận Tân Bình');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Lê Vĩnh', N'Vân', '0932345789', convert(datetime,'12-11-1994',105), N'1818 Nguyễn Văn Cừ, Phường 12, Quận 5');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Phan Ánh', N'Hồng', '0943456780', convert(datetime,'05-08-1992',105), N'1919 Tôn Đức Thắng, Phường 6, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Hoàng Thị', N'Hiền', '0974567891', convert(datetime,'29-12-1995',105), N'2020 Phú Nhuận, Phường 8, Quận Phú Nhuận');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Vũ Quang', N'Quân', '0905678902', convert(datetime,'08-06-1990',105), N'2121 Lê Thị Hồng Gấm, Phường 4, Quận 4');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Nguyễn Minh', N'Khoa', '0916789012', convert(datetime,'04-11-1986',105), N'2222 Đinh Tiên Hoàng, Phường 8, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Trần Thị', N'Ánh', '0937890123', convert(datetime,'22-09-1997',105), N'2323 Võ Thị Sáu, Phường 12, Quận 3');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Lê Thiên', N'Thiên', '0948901234', convert(datetime,'16-02-1994',105), N'2424 Cộng Hòa, Phường 13, Quận Tân Bình');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Phan Bích', N'Ngọc', '0979012345', convert(datetime,'28-05-1988',105), N'2525 Bùi Viện, Phường Phạm Ngũ Lão, Quận 1');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Hoàng An', N'Bình', '0902345678', convert(datetime,'10-12-1992',105), N'2626 Nam Kỳ Khởi Nghĩa, Phường 2, Quận 3');
INSERT INTO KHACHHANG (HoKH, TenKH, SdtKH, NgaySinh, DiaChi)
VALUES (N'Vũ Minh', N'Lan', '0913456789', convert(datetime,'22-07-1991',105), N'2727 Lê Lai, Phường 5, Quận 5');

/* Dữ liệu NHACUNGCAP */
INSERT INTO NHACUNGCAP (TenNCC)
VALUES (N'ProParts Solutions');
INSERT INTO NHACUNGCAP (TenNCC)
VALUES (N'CircuitWorks');
INSERT INTO NHACUNGCAP (TenNCC)
VALUES (N'NexSource Components');
INSERT INTO NHACUNGCAP (TenNCC)
VALUES (N'Global Circuit');
INSERT INTO NHACUNGCAP (TenNCC)
VALUES (N'Advanced Parts Co.');
INSERT INTO NHACUNGCAP (TenNCC)
VALUES (N'Meko Distributor');

/* Dữ liệu PHIEUNHAPHANG */
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (2, N'CircuitWorks', convert(datetime,'03-01-2024',105), 2)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (5, N'Advanced Parts Co.', convert(datetime,'19-01-2024',105), 2)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (1, N'ProParts Solutions', convert(datetime,'10-02-2024',105), 3)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (2, N'CircuitWorks', convert(datetime,'19-02-2024',105), 3)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (2, N'CircuitWorks', convert(datetime,'05-03-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (3, N'NexSource Components', convert(datetime,'15-03-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (3, N'NexSource Components', convert(datetime,'07-04-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (3,N'NexSource Components', convert(datetime,'21-04-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (5, N'Advanced Parts Co.', convert(datetime,'02-05-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (5, N'Advanced Parts Co.', convert(datetime,'07-05-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (5, N'Advanced Parts Co.', convert(datetime,'18-06-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (4, N'Global Circuit', convert(datetime,'23-06-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (6, N'Meko Distributor', convert(datetime,'09-07-2024',105), 5)
INSERT INTO PHIEUNHAPHANG (MaNCC, TenNCC, NgayNhap, MaNV)
VALUES (6, N'Meko Distributor', convert(datetime,'21-07-2024',105), 5)

/* Dữ liệu CHITIETPHIEUNHAPHANG */
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (1, 4, N'Mainboard MSI MAG B660M Mortar', 50, 4800000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (1, 5, N'RAM Corsair Vengeance 32GB DDR5', 50, 4800000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (2, 2, N'AMD Ryzen 9 7950X', 50, 15000000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (2, 4, N'Mainboard MSI MAG B660M Mortar', 100, 4800000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (2, 3, N'Mainboard ASUS ROG Strix Z790-E', 50, 9000000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (3, 6, N'RAM Kingston Fury 16GB DDR4', 50, 2100000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (3, 7, N'SSD Samsung 990 Pro 2TB NVMe', 50, 7500000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (4, 9, N'Card đồ họa NVIDIA RTX 4090', 50, 50000000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (4, 11, N'Nguồn Corsair RM1000e 1000W', 50, 3800000)
INSERT INTO CHITIETPHIEUNHAPHANG (MaPNH, MaSP, TenSP, SoLuongNhap, DonGia)
VALUES (4, 12, N'Case NZXT H7 Flow', 50, 2500000)

/* Dữ liệu HOADON */
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (1, N'Nguyễn Văn', N'Anh', '0901234567', N'123 Nguyễn Thị Minh Khai, Phường Bến Nghé, Quận 1', 4, N'Phạm Minh', N'Hòa', convert(datetime,'23-02-2025',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (2, N'Trần Minh', N'Bảo', '0912345678', N'456 Lý Tự Trọng, Phường Bến Thành, Quận 1', 4, N'Phạm Minh', N'Hòa', convert(datetime,'23-04-2025',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (3, N'Lê Thị', N'Hương', '0933456789', N'789 Phạm Ngọc Thạch, Phường 6, Quận 3', 4, N'Phạm Minh', N'Hòa', convert(datetime,'10-01-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (4, N'Phan Văn', N'Minh', '0944567890', N'101 Hai Bà Trưng, Phường Đa Kao, Quận 1', 4, N'Phạm Minh', N'Hòa', convert(datetime,'11-01-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (5, N'Hoàng Thị', N'Lan', '0975678901', N'202 Nguyễn Đình Chiểu, Phường 5, Quận 3', 4, N'Phạm Minh', N'Hòa', convert(datetime,'03-01-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (6, N'Vũ Tuấn', N'Sơn', '0906789012', N'303 Trường Chinh, Phường 13, Quận Tân Bình', 4, N'Phạm Minh', N'Hòa', convert(datetime,'05-01-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (7, N'Nguyễn Thị', N'Lý', '0917890123', N'404 Cách Mạng Tháng 8, Phường 10, Quận 10', 4, N'Phạm Minh', N'Hòa', convert(datetime,'20-01-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (8, N'Trần Duy', N'Tâm', '0938901234', N'505 Phan Đình Phùng, Phường 4, Quận 5', 4, N'Phạm Minh', N'Hòa', convert(datetime,'20-02-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (9, N'Lê Thị', N'Thảo', '0949012345', N'606 Nguyễn Văn Cừ, Phường Cầu Kho, Quận 1', 4, N'Phạm Minh', N'Hòa', convert(datetime,'10-02-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (10, N'Phan Thiều', N'Kiệt', '0970123456', N'707 Lê Hồng Phong, Phường 1, Quận 10', 4, N'Phạm Minh', N'Hòa', convert(datetime,'15-02-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (11, N'Hoàng Quang', N'Duy', '0901230987', N'808 Trần Quang Khải, Phường Tân Định, Quận 1', 4, N'Phạm Minh', N'Hòa', convert(datetime,'01-02-2024',105));
INSERT INTO HOADON (MaKH, HoKH, TenKH, SdtKH, DiaChi, MaNV, HoNV, TenNV, NgayMuaHang)
VALUES (12, N'Vũ Thị', N'Thúy', '0912340987', N'909 Võ Văn Tần, Phường 6, Quận 3', 4, N'Phạm Minh', N'Hòa', convert(datetime,'05-02-2024',105));

/* Dữ liệu CHITIETHOADON */
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (1, 4, N'Mainboard MSI MAG B660M Mortar', 5, 4800000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (1, 5, N'RAM Corsair Vengeance 32GB DDR5', 2, 4800000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (2, 3, N'Mainboard ASUS ROG Strix Z790-E', 5, 9000000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (2, 5, N'RAM Corsair Vengeance 32GB DDR5', 2, 4800000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (3, 6, N'RAM Kingston Fury 16GB DDR4', 4, 2100000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (3, 7, N'SSD Samsung 990 Pro 2TB NVMe', 1, 7500000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (4, 9, N'Card đồ họa NVIDIA RTX 4090', 1, 50000000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (4, 11, N'Nguồn Corsair RM1000e 1000W', 10, 3800000);
INSERT INTO CHITIETHOADON (MaHD, MaSP, TenSP, SoLuongDat, DonGia)
VALUES (4, 12, N'Case NZXT H7 Flow', 1, 2500000);

/* View Doanh Thu + Lợi nhuận */
CREATE VIEW DoanhThuTheoNgay AS
SELECT
    DAY(T.Ngay) AS Ngay,
    MONTH(T.Ngay) AS Thang,
    YEAR(T.Ngay) AS Nam,
    SP.MaSP,
    SP.TenSP,
    COALESCE(SUM(BH.SoLuongBan), 0) AS TongSoLuongBan,
    COALESCE(SUM(BH.ThanhTienBan), 0) AS TongDoanhThu,
    300000 * COALESCE(SUM(BH.SoLuongBan), 0) AS LoiNhuan
FROM (
         -- Lấy tất cả các ngày có giao dịch (nhập hoặc bán)
         SELECT DISTINCT CAST(PNH.NgayNhap AS DATE) AS Ngay
         FROM PHIEUNHAPHANG PNH
         WHERE PNH.NgayNhap IS NOT NULL
         UNION
         SELECT DISTINCT CAST(HD.NgayMuaHang AS DATE) AS Ngay
         FROM HOADON HD
         WHERE HD.NgayMuaHang IS NOT NULL
     ) T
         CROSS JOIN (
    -- Lấy danh sách tất cả sản phẩm
    SELECT DISTINCT MaSP, TenSP
    FROM CHITIETPHIEUNHAPHANG
    UNION
    SELECT DISTINCT MaSP, TenSP
    FROM CHITIETHOADON
) SP
         LEFT JOIN (
    -- Dữ liệu nhập hàng theo ngày và sản phẩm
    SELECT
        CTPNH.MaSP,
        CTPNH.TenSP,
        CAST(PNH.NgayNhap AS DATE) AS NgayNhap,
        SUM(CTPNH.SoLuongNhap) AS SoLuongNhap,
        SUM(CTPNH.SoLuongNhap * CTPNH.DonGia) AS ThanhTienNhap
    FROM CHITIETPHIEUNHAPHANG CTPNH
             INNER JOIN PHIEUNHAPHANG PNH ON CTPNH.MaPNH = PNH.MaPNH
    WHERE PNH.NgayNhap IS NOT NULL
    GROUP BY CTPNH.MaSP, CTPNH.TenSP, CAST(PNH.NgayNhap AS DATE)
) NH ON SP.MaSP = NH.MaSP AND T.Ngay = NH.NgayNhap
         LEFT JOIN (
    -- Dữ liệu bán hàng theo ngày và sản phẩm
    SELECT
        CTHD.MaSP,
        CTHD.TenSP,
        CAST(HD.NgayMuaHang AS DATE) AS NgayBan,
        SUM(CTHD.SoLuongDat) AS SoLuongBan,
        SUM(CTHD.SoLuongDat * CTHD.DonGia) AS ThanhTienBan
    FROM CHITIETHOADON CTHD
             INNER JOIN HOADON HD ON CTHD.MaHD = HD.MaHD
    WHERE HD.NgayMuaHang IS NOT NULL
    GROUP BY CTHD.MaSP, CTHD.TenSP, CAST(HD.NgayMuaHang AS DATE)
) BH ON SP.MaSP = BH.MaSP AND T.Ngay = BH.NgayBan
GROUP BY CAST(T.Ngay AS DATE), SP.MaSP, SP.TenSP;

Drop View DoanhThuTheoNgay

Select *
From DoanhThuTheoNgay
Where Thang = 1

-- Procedure kiểm tra login
CREATE PROCEDURE kiem_tra_login
    @UserName NVARCHAR(50),
    @PasswordHash NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        DisplayName,
        Quyen,
        MaNV
    FROM
        ACCNHANVIEN
    WHERE
        UserName COLLATE Latin1_General_CS_AS = @UserName AND Pass = @PasswordHash;
END

    EXEC kiem_tra_login 'Binh', '21232f297a57a5a743894a0e4a801fc3'

    -- Procedure quên mật khẩu
    CREATE PROCEDURE sp_ChangePassword
        @UserName VARCHAR(50),
        @NewPassword VARCHAR(32),
        @Result INT OUTPUT
    AS
    BEGIN
        SET NOCOUNT ON;

        BEGIN TRY
            IF EXISTS (SELECT 1 FROM ACCNHANVIEN WHERE UserName = @UserName)
                BEGIN
                    UPDATE ACCNHANVIEN
                    SET Pass = @NewPassword
                    WHERE UserName = @UserName;

                    SET @Result = 0;
                END
            ELSE
                BEGIN
                    SET @Result = 1;
                END
        END TRY
        BEGIN CATCH
            SET @Result = 2;
        END CATCH
    END
go