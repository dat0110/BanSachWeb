USE master
GO

IF EXISTS (SELECT name FROM sys.sysdatabases WHERE name = 'CuaHangSachDB')
    DROP DATABASE CuaHangSachDB

CREATE DATABASE CuaHangSachDB
GO

USE CuaHangSachDB
GO

-- Bảng Tác giả
CREATE TABLE TacGia (
    MaTacGia INT PRIMARY KEY IDENTITY(1,1),
    TenTacGia NVARCHAR(255),
    
    MoTa NVARCHAR(MAX),
    Visible BIT
);
-- Bảng Danh mục chính
CREATE TABLE DanhMucChinh (
    MaDanhMucChinh INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(255),
    MoTa NVARCHAR(MAX),
    Visible BIT
);






-- Bảng Danh mục phụ
CREATE TABLE DanhMucPhu (
    MaDanhMucPhu INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(255),
    MoTa NVARCHAR(MAX),
    Visible BIT,
    MaDanhMucChinh INT,
    FOREIGN KEY (MaDanhMucChinh) REFERENCES DanhMucChinh(MaDanhMucChinh)
);
-- Bảng Sách
CREATE TABLE Sach (
    MaSach INT PRIMARY KEY IDENTITY(1,1),
    TenSach NVARCHAR(255),
    AnhSach NVARCHAR(255),
    GiaGoc DECIMAL(10, 2),
    GiaBan DECIMAL(10, 2),
    SoLuongDaBan INT,
    SoLuongConDu INT,
    TomTat NVARCHAR(MAX),
    NhaXuatBan NVARCHAR(255),
    NamXuatBan INT,
    HinhThuc NVARCHAR(255),
    SoTrang INT,
    KichThuoc NVARCHAR(50),
    TrongLuong FLOAT,
    MaTacGia INT,
    MaDanhMuc INT,
    Visible BIT,
	FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucPhu(MaDanhMucPhu),
	FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia)
);






-- Bảng Chi nhánh
CREATE TABLE ChiNhanh (
    MaChiNhanh INT PRIMARY KEY IDENTITY(1,1),
    TenChiNhanh NVARCHAR(255),
    DiaChi NVARCHAR(MAX),
    GioiThieu NVARCHAR(MAX)
);

-- Bảng Sách - Chi nhánh
CREATE TABLE Sach_ChiNhanh (
    MaSach INT,
    MaChiNhanh INT,
    PRIMARY KEY (MaSach, MaChiNhanh),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    FOREIGN KEY (MaChiNhanh) REFERENCES ChiNhanh(MaChiNhanh)
);

-- Bảng Tài khoản
CREATE TABLE TaiKhoan (
    MaTaiKhoan INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap NVARCHAR(255),
    MatKhau NVARCHAR(255),
    Email NVARCHAR(255),
    HoTen NVARCHAR(255),
    SoDienThoai NVARCHAR(15),
    VaiTro NVARCHAR(50),
    DiemThuong INT
);

-- Bảng Địa chỉ
CREATE TABLE DiaChi (
    MaDiaChi INT PRIMARY KEY IDENTITY(1,1),
    DiaChiCuThe NVARCHAR(MAX),
    MacDinh BIT,
    SoDienThoaiNhanHang NVARCHAR(15),
    TenNguoiNhan NVARCHAR(255),
    MaTaiKhoan INT,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);

-- Bảng Đơn hàng
CREATE TABLE DonHang (
    MaDonHang INT PRIMARY KEY IDENTITY(1,1),
    ThoiGianDatHang DATETIME,
    TrangThai NVARCHAR(100),
    DonViVanChuyen NVARCHAR(255),
    ThoiGianGiaoHangDuKien DATETIME,
    PhuongThucThanhToan NVARCHAR(100),
    MaQR NVARCHAR(255),
    PhiVanChuyen DECIMAL(10, 2),
    TongGiaTri DECIMAL(10, 2),
    LoiNhuan DECIMAL(10, 2),
    DaThanhToan BIT,
    MaTaiKhoan INT,
    MaDiaChi INT,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan),
    FOREIGN KEY (MaDiaChi) REFERENCES DiaChi(MaDiaChi)
);

-- Bảng Chi tiết đơn hàng
CREATE TABLE ChiTietDonHang (
    MaChiTietDonHang INT PRIMARY KEY IDENTITY(1,1),
    SoLuong INT,
    GiaBan DECIMAL(10, 2),
    ThanhTien DECIMAL(10, 2),
    MaDonHang INT,
    MaSach INT,
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng Khuyến mãi
CREATE TABLE KhuyenMai (
    MaKhuyenMai INT PRIMARY KEY IDENTITY(1,1),
    MoTa NVARCHAR(MAX),
    MucGiam DECIMAL(5, 2),
    DieuKienApDung NVARCHAR(MAX),
    ThoiGianBatDau DATETIME,
    ThoiGianKetThuc DATETIME,
    MaChiNhanh INT,
    FOREIGN KEY (MaChiNhanh) REFERENCES ChiNhanh(MaChiNhanh)
);

-- Bảng Khuyến mãi - Tài khoản
CREATE TABLE KhuyenMai_TaiKhoan (
    MaKhuyenMai INT,
    MaTaiKhoan INT,
    PRIMARY KEY (MaKhuyenMai, MaTaiKhoan),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai),
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);

-- Bảng Khuyến mãi - Sách
CREATE TABLE KhuyenMai_Sach (
    MaKhuyenMai INT,
    MaSach INT,
    PRIMARY KEY (MaKhuyenMai, MaSach),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng Giỏ hàng
CREATE TABLE GioHang (
    MaGioHang INT PRIMARY KEY IDENTITY(1,1),
    MaTaiKhoan INT,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);

-- Bảng Chi tiết giỏ hàng
CREATE TABLE ChiTietGioHang (
    MaChiTietGioHang INT PRIMARY KEY IDENTITY(1,1),
    MaGioHang INT,
    MaSach INT,
    SoLuong INT,
    ThanhTien DECIMAL(10, 2),
    FOREIGN KEY (MaGioHang) REFERENCES GioHang(MaGioHang),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- Bảng Đơn hàng - Khuyến mãi
CREATE TABLE DonHang_KhuyenMai (
    MaKhuyenMai INT,
    MaDonHang INT,
    PRIMARY KEY (MaKhuyenMai, MaDonHang),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai),
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
);

-- Bảng Phản hồi
CREATE TABLE PhanHoi (
    MaPhanHoi INT PRIMARY KEY IDENTITY(1,1),
    NoiDung NVARCHAR(MAX),
    DiemDanhGia INT,
    MaSach INT,
    MaTaiKhoan INT,
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);

-- insert data
-- Account 
INSERT INTO TaiKhoan VALUES ('HaoMB', 'Leo15823', 'manbahao1508@gmail.com', 'Man Ba Hao', '0968518163', 'admin', 0);

-- Insert sample data into DanhMucChinh
INSERT INTO DanhMucChinh (TenDanhMuc, MoTa, Visible) VALUES
(N'Văn học', N'Danh mục sách văn học', 1),
(N'Khoa học', N'Danh mục sách khoa học', 1),
(N'Lịch sử', N'Danh mục sách lịch sử', 1),
(N'Thiếu nhi', N'Danh mục sách thiếu nhi', 1),
(N'Ngoại ngữ', N'Danh mục sách ngoại ngữ', 1);

-- Insert sample data into DanhMucPhu
INSERT INTO DanhMucPhu (TenDanhMuc, MoTa, Visible, MaDanhMucChinh) VALUES
(N'Tiểu thuyết', N'Sách tiểu thuyết', 1, 1), -- Assuming 200 is the ID for Văn học
(N'Truyện ngắn', N'Sách truyện ngắn', 1, 1),
(N'Vật lý', N'Sách vật lý', 1, 2),           -- Assuming 201 is the ID for Khoa học
(N'Hóa học', N'Sách hóa học', 1, 2),
(N'Tiếng Anh', N'Sách tiếng Anh', 1, 5);     -- Assuming 204 is the ID for Ngoại ngữ

-- Inserting authors into the TacGia table without AnhMinhHoa
INSERT INTO TacGia (TenTacGia, MoTa, Visible) VALUES
(N'Gabriel Garcia Marquez', N'Tác giả nổi tiếng của Colombia.', 1),
(N'Victor Hugo', N'Tác giả nổi tiếng của Pháp.', 1),
(N'Nam Cao', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'Haruki Murakami', N'Tác giả nổi tiếng của Nhật Bản.', 1),
(N'Ngô Tất Tố', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'Tô Hoài', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'Nguyễn Minh Châu', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'Nguyễn Tuân', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'Maxim Gorky', N'Tác giả nổi tiếng của Nga.', 1),
(N'Stephen Hawking', N'Tác giả nổi tiếng của Anh.', 1),
(N'Albert Einstein', N'Tác giả nổi tiếng của Đức.', 1),
(N'Nguyễn Bính', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'J.K. Rowling', N'Tác giả nổi tiếng của Anh.', 1),
(N'Mark Twain', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'Nguyễn Du', N'Tác giả nổi tiếng của Việt Nam.', 1);

INSERT INTO TacGia (TenTacGia, MoTa, Visible) VALUES
(N'Ernest Hemingway', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'George Orwell', N'Tác giả nổi tiếng của Anh.', 1),
(N'Franz Kafka', N'Tác giả nổi tiếng của Tiệp Khắc.', 1),
(N'Hồ Chí Minh', N'Tác giả nổi tiếng của Việt Nam.', 1),
(N'Agatha Christie', N'Tác giả nổi tiếng của Anh.', 1),
(N'Leonardo da Vinci', N'Tác giả nổi tiếng của Ý.', 1),
(N'Miguel de Cervantes', N'Tác giả nổi tiếng của Tây Ban Nha.', 1),
(N'William Shakespeare', N'Tác giả nổi tiếng của Anh.', 1),
(N'Leo Tolstoy', N'Tác giả nổi tiếng của Nga.', 1),
(N'Jules Verne', N'Tác giả nổi tiếng của Pháp.', 1),
(N'Marcel Proust', N'Tác giả nổi tiếng của Pháp.', 1),
(N'Lu Xun', N'Tác giả nổi tiếng của Trung Quốc.', 1),
(N'Gabriela Mistral', N'Tác giả nổi tiếng từ Chile.', 1),
(N'Yukio Mishima', N'Tác giả nổi tiếng của Nhật Bản.', 1),
(N'Orhan Pamuk', N'Tác giả nổi tiếng của Thổ Nhĩ Kỳ.', 1);

INSERT INTO TacGia (TenTacGia, MoTa, Visible) VALUES
(N'J.R.R. Tolkien', N'Tác giả nổi tiếng của Anh.', 1),
(N'Antoine de Saint-Exupéry', N'Tác giả nổi tiếng của Pháp.', 1),
(N'Dante Alighieri', N'Tác giả nổi tiếng của Ý.', 1),
(N'Virginia Woolf', N'Tác giả nổi tiếng của Anh.', 1),
(N'Jane Austen', N'Tác giả nổi tiếng của Anh.', 1),
(N'Charles Dickens', N'Tác giả nổi tiếng của Anh.', 1),
(N'Arthur Conan Doyle', N'Tác giả nổi tiếng của Scotland.', 1),
(N'Emily Dickinson', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'Edgar Allan Poe', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'Fyodor Dostoevsky', N'Tác giả nổi tiếng của Nga.', 1),
(N'Henry David Thoreau', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'Jack London', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'James Joyce', N'Tác giả nổi tiếng của Ireland.', 1),
(N'John Steinbeck', N'Tác giả nổi tiếng của Mỹ.', 1),
(N'Kazuo Ishiguro', N'Tác giả nổi tiếng của Anh.', 1);
 -- Insert Books into the Sach Table
-- Tiểu thuyết
INSERT INTO Sach (TenSach, AnhSach, GiaGoc, GiaBan, SoLuongDaBan, SoLuongConDu, TomTat, NhaXuatBan, NamXuatBan, HinhThuc, SoTrang, KichThuoc, TrongLuong, MaTacGia, MaDanhMuc, Visible)
VALUES 
(N'Trăm năm cô đơn', N'tram-nam-co-don.jpg', 150000, 120000, 500, 200, N'Một tiểu thuyết nổi tiếng của Gabriel Garcia Marquez.', N'NXB Văn Học', 1982, N'Bìa mềm', 500, N'13x20 cm', 0.5, 1, 1, 1),
(N'Những người khốn khổ', N'nhung-nguoi-khon-kho.jpg', 200000, 180000, 300, 150, N'Tiểu thuyết của Victor Hugo.', N'NXB Trẻ', 2005, N'Bìa cứng', 1000, N'15x24 cm', 1.2, 2, 1, 1),
(N'Chí Phèo', N'chi-pheo.jpg', 100000, 90000, 200, 100, N'Tiểu thuyết của Nam Cao.', N'NXB Hội Nhà Văn', 1941, N'Bìa mềm', 200, N'12x19 cm', 0.3, 3, 1, 1),
(N'Rừng Na Uy', N'rung-na-uy.jpg', 180000, 160000, 150, 100, N'Tiểu thuyết của Haruki Murakami.', N'NXB Văn Học', 1987, N'Bìa mềm', 450, N'14x21 cm', 0.6, 4, 1, 1),
(N'Tắt đèn', N'tat-den.jpg', 120000, 110000, 300, 200, N'Tiểu thuyết của Ngô Tất Tố.', N'NXB Hội Nhà Văn', 1939, N'Bìa mềm', 250, N'13x20 cm', 0.4, 5, 1, 1),

-- Truyện ngắn
(N'Vợ chồng A Phủ', N'vo-chong-a-phu.jpg', 90000, 80000, 400, 250, N'Truyện ngắn của Tô Hoài.', N'NXB Văn Học', 1952, N'Bìa mềm', 150, N'13x20 cm', 0.2, 6, 2, 1),
(N'Chiếc thuyền ngoài xa', N'chiec-thuyen-ngoai-xa.jpg', 110000, 100000, 350, 200, N'Truyện ngắn của Nguyễn Minh Châu.', N'NXB Trẻ', 1983, N'Bìa mềm', 180, N'14x21 cm', 0.3, 7, 2, 1),
(N'Người lái đò sông Đà', N'nguoi-lai-do-song-da.jpg', 95000, 85000, 300, 150, N'Truyện ngắn của Nguyễn Tuân.', N'NXB Văn Học', 1960, N'Bìa mềm', 200, N'13x20 cm', 0.3, 8, 2, 1),
(N'Thời thơ ấu', N'thoi-tho-au.jpg', 130000, 120000, 150, 100, N'Truyện ngắn của Maxim Gorky.', N'NXB Trẻ', 1913, N'Bìa mềm', 250, N'14x21 cm', 0.4, 9, 2, 1),
(N'Đôi mắt', N'doi-mat.jpg', 100000, 90000, 200, 100, N'Truyện ngắn của Nam Cao.', N'NXB Hội Nhà Văn', 1950, N'Bìa mềm', 180, N'12x19 cm', 0.2, 3, 2, 1),

-- Vật lý
(N'Vũ trụ trong vỏ hạt dẻ', N'vu-tru-trong-vo-hat-de.jpg', 250000, 220000, 100, 50, N'Sách khoa học nổi tiếng của Stephen Hawking.', N'NXB Tri Thức', 2001, N'Bìa cứng', 320, N'16x24 cm', 0.8, 10, 3, 1),
(N'Bí mật của ánh sáng', N'bi-mat-cua-anh-sang.jpg', 180000, 150000, 120, 80, N'Sách về vật lý ánh sáng.', N'NXB Khoa Học', 2010, N'Bìa mềm', 280, N'14x21 cm', 0.6, 11, 3, 1),
(N'Lý thuyết tương đối', N'ly-thuyet-tuong-doi.jpg', 200000, 180000, 150, 100, N'Sách về lý thuyết tương đối của Albert Einstein.', N'NXB Giáo Dục', 1995, N'Bìa mềm', 300, N'15x23 cm', 0.7, 12, 3, 1),
(N'Vật lý lượng tử', N'vat-ly-luong-tu.jpg', 220000, 200000, 130, 90, N'Sách về vật lý lượng tử.', N'NXB Khoa Học', 2005, N'Bìa mềm', 350, N'16x24 cm', 0.9, 13, 3, 1),
(N'Bách khoa toàn thư vật lý', N'bach-khoa-toan-thu-vat-ly.jpg', 300000, 280000, 170, 120, N'Sách bách khoa toàn thư về vật lý.', N'NXB Tri Thức', 2010, N'Bìa cứng', 500, N'17x25 cm', 1.1, 14, 3, 1),

-- Hóa học
(N'Hóa học đại cương', N'hoa-hoc-dai-cuong.jpg', 210000, 190000, 140, 100, N'Sách hóa học cơ bản.', N'NXB Giáo Dục', 2000, N'Bìa mềm', 400, N'15x22 cm', 0.8, 15, 4, 1),
(N'Hóa học hữu cơ', N'hoa-hoc-huu-co.jpg', 250000, 230000, 160, 110, N'Sách về hóa học hữu cơ.', N'NXB Khoa Học', 2005, N'Bìa mềm', 450, N'16x23 cm', 0.9, 16, 4, 1),
(N'Hóa học vô cơ', N'hoa-hoc-vo-co.jpg', 220000, 200000, 130, 90, N'Sách về hóa học vô cơ.', N'NXB Tri Thức', 2010, N'Bìa mềm', 350, N'15x22 cm', 0.7, 17, 4, 1),
(N'Hóa học polymer', N'hoa-hoc-polymer.jpg', 280000, 260000, 110, 70, N'Sách về hóa học polymer.', N'NXB Giáo Dục', 2015, N'Bìa mềm', 500, N'16x24 cm', 1.0, 18, 4, 1),
(N'Hóa học phân tích', N'hoa-hoc-phan-tich.jpg', 230000, 210000, 120, 80, N'Sách về hóa học phân tích.', N'NXB Khoa Học', 2020, N'Bìa mềm', 380, N'15x23 cm', 0.9, 19, 4, 1),

-- Tiếng Anh
(N'Tiếng Anh cơ bản', N'tieng-anh-co-ban.jpg', 180000, 160000, 200, 150, N'Sách học tiếng Anh cơ bản.', N'NXB Giáo Dục', 2015, N'Bìa mềm', 300, N'14x21 cm', 0.6, 20, 5, 1),
(N'Tiếng Anh giao tiếp', N'tieng-anh-giao-tiep.jpg', 190000, 170000, 220, 160, N'Sách học tiếng Anh giao tiếp.', N'NXB Trẻ', 2018, N'Bìa mềm', 320, N'14x22 cm', 0.7, 21, 5, 1),
(N'Từ vựng tiếng Anh', N'tu-vung-tieng-anh.jpg', 160000, 140000, 180, 130, N'Sách từ vựng tiếng Anh.', N'NXB Tri Thức', 2020, N'Bìa mềm', 280, N'13x20 cm', 0.5, 22, 5, 1),
(N'Tiếng Anh cho người đi làm', N'tieng-anh-cho-nguoi-di-lam.jpg', 200000, 180000, 150, 100, N'Sách tiếng Anh chuyên dụng cho người đi làm.', N'NXB Giáo Dục', 2021, N'Bìa mềm', 350, N'15x23 cm', 0.8, 23, 5, 1),
(N'Luyện thi TOEIC', N'luyen-thi-toeic.jpg', 220000, 200000, 170, 120, N'Sách luyện thi TOEIC.', N'NXB Khoa Học', 2019, N'Bìa mềm', 400, N'16x24 cm', 0.9, 24, 5, 1);

SELECT s.MaSach, s.TenSach, s.SoLuongDaBan, s.AnhSach, s.GiaBan, s.GiaGoc, t.TenTacGia
FROM Sach s
JOIN TacGia t ON s.MaTacGia = t.MaTacGia
-- Inserting 100 additional books into the Sach table

INSERT INTO Sach (TenSach, AnhSach, GiaGoc, GiaBan, SoLuongDaBan, SoLuongConDu, TomTat, NhaXuatBan, NamXuatBan, HinhThuc, SoTrang, KichThuoc, TrongLuong, MaTacGia, MaDanhMuc, Visible)
VALUES 
-- Sách Văn học - Tiểu thuyết
(N'Bão ngầm', N'bao-ngam.jpg', 140000, 130000, 450, 220, N'Một tiểu thuyết trinh thám.', N'NXB Văn Học', 2000, N'Bìa mềm', 520, N'13x20 cm', 0.5, 1, 1, 1),
(N'Gió mùa thu', N'gio-mua-thu.jpg', 130000, 120000, 420, 210, N'Tiểu thuyết tình cảm.', N'NXB Trẻ', 2001, N'Bìa cứng', 480, N'14x21 cm', 0.6, 2, 1, 1),
(N'Nắng tháng Tư', N'nang-thang-tu.jpg', 120000, 110000, 410, 200, N'Tiểu thuyết đời sống.', N'NXB Hội Nhà Văn', 2002, N'Bìa mềm', 450, N'13x20 cm', 0.5, 3, 1, 1),
(N'Mưa tháng Năm', N'mua-thang-nam.jpg', 110000, 100000, 390, 190, N'Tiểu thuyết tâm lý.', N'NXB Văn Học', 2003, N'Bìa mềm', 420, N'13x20 cm', 0.5, 4, 1, 1),
(N'Băng tuyết', N'bang-tuyet.jpg', 100000, 90000, 370, 180, N'Tiểu thuyết phiêu lưu.', N'NXB Trẻ', 2004, N'Bìa mềm', 400, N'13x20 cm', 0.5, 5, 1, 1),

-- Sách Văn học - Truyện ngắn
(N'Tình người', N'tinh-nguoi.jpg', 90000, 80000, 350, 170, N'Truyện ngắn cảm động.', N'NXB Hội Nhà Văn', 2005, N'Bìa mềm', 380, N'13x20 cm', 0.4, 6, 2, 1),
(N'Hương đêm', N'huong-dem.jpg', 85000, 75000, 330, 160, N'Truyện ngắn lãng mạn.', N'NXB Văn Học', 2006, N'Bìa mềm', 360, N'13x20 cm', 0.4, 7, 2, 1),
(N'Gió đầu mùa', N'gio-dau-mua.jpg', 80000, 70000, 310, 150, N'Truyện ngắn mùa thu.', N'NXB Trẻ', 2007, N'Bìa mềm', 340, N'13x20 cm', 0.4, 8, 2, 1),
(N'Mưa ngâu', N'mua-ngau.jpg', 75000, 65000, 290, 140, N'Truyện ngắn tình cảm.', N'NXB Hội Nhà Văn', 2008, N'Bìa mềm', 320, N'13x20 cm', 0.4, 9, 2, 1),
(N'Trăng rằm', N'trang-ram.jpg', 70000, 60000, 270, 130, N'Truyện ngắn truyền thống.', N'NXB Văn Học', 2009, N'Bìa mềm', 300, N'13x20 cm', 0.4, 10, 2, 1),

-- Sách Khoa học - Vật lý
(N'Lực hấp dẫn', N'luc-hap-dan.jpg', 150000, 140000, 250, 120, N'Sách về lực hấp dẫn.', N'NXB Khoa Học', 2010, N'Bìa cứng', 420, N'15x23 cm', 0.6, 11, 3, 1),
(N'Năng lượng tối', N'nang-luong-toi.jpg', 160000, 150000, 260, 130, N'Sách về năng lượng tối.', N'NXB Tri Thức', 2011, N'Bìa mềm', 440, N'16x24 cm', 0.7, 12, 3, 1),
(N'Vũ trụ học', N'vu-tru-hoc.jpg', 170000, 160000, 270, 140, N'Sách về vũ trụ học.', N'NXB Giáo Dục', 2012, N'Bìa mềm', 460, N'14x21 cm', 0.6, 13, 3, 1),
(N'Động lực học', N'dong-luc-hoc.jpg', 180000, 170000, 280, 150, N'Sách về động lực học.', N'NXB Khoa Học', 2013, N'Bìa mềm', 480, N'15x23 cm', 0.6, 14, 3, 1),
(N'Nhiệt động lực học', N'niet-dong-luc-hoc.jpg', 190000, 180000, 290, 160, N'Sách về nhiệt động lực học.', N'NXB Tri Thức', 2014, N'Bìa mềm', 500, N'16x24 cm', 0.7, 15, 3, 1),

-- Sách Khoa học - Hóa học
(N'Phản ứng hóa học', N'phan-ung-hoa-hoc.jpg', 140000, 130000, 240, 120, N'Sách về phản ứng hóa học.', N'NXB Giáo Dục', 2015, N'Bìa mềm', 380, N'14x21 cm', 0.5, 16, 4, 1),
(N'Hóa học chất lỏng', N'hoa-hoc-chat-long.jpg', 150000, 140000, 250, 130, N'Sách về hóa học chất lỏng.', N'NXB Khoa Học', 2016, N'Bìa mềm', 400, N'15x23 cm', 0.6, 17, 4, 1),
(N'Hóa học khí', N'hoa-hoc-khi.jpg', 160000, 150000, 260, 140, N'Sách về hóa học khí.', N'NXB Tri Thức', 2017, N'Bìa mềm', 420, N'16x24 cm', 0.6, 18, 4, 1),
(N'Hóa học rắn', N'hoa-hoc-ran.jpg', 170000, 160000, 270, 150, N'Sách về hóa học rắn.', N'NXB Giáo Dục', 2018, N'Bìa mềm', 440, N'14x21 cm', 0.5, 19, 4, 1),
(N'Hóa học phức hợp', N'hoa-hoc-phuc-hop.jpg', 180000, 170000, 280, 160, N'Sách về hóa học phức hợp.', N'NXB Khoa Học', 2019, N'Bìa mềm', 460, N'15x23 cm', 0.6, 20, 4, 1),

-- Sách Ngoại ngữ - Tiếng Anh
(N'Tiếng Anh cho trẻ em', N'tieng-anh-cho-tre-em.jpg', 120000, 110000, 150, 100, N'Sách học tiếng Anh cho trẻ em.', N'NXB Giáo Dục', 2020, N'Bìa mềm', 300, N'13x20 cm', 0.4, 21, 5, 1),
(N'Tiếng Anh trung cấp', N'tieng-anh-trung-cap.jpg', 130000, 120000, 160, 110, N'Sách học tiếng Anh trung cấp.', N'NXB Trẻ', 2021, N'Bìa mềm', 320, N'14x21 cm', 0.5, 22, 5, 1),
(N'Ngữ pháp tiếng Anh', N'ngu-phap-tieng-anh.jpg', 140000, 130000, 170, 120, N'Sách ngữ pháp tiếng Anh.', N'NXB Tri Thức', 2022, N'Bìa mềm', 340, N'15x23 cm', 0.6, 23, 5, 1),
(N'Tiếng Anh nâng cao', N'tieng-anh-nang-cao.jpg', 150000, 140000, 180, 130, N'Sách học tiếng Anh nâng cao.', N'NXB Giáo Dục', 2023, N'Bìa mềm', 360, N'13x20 cm', 0.4, 24, 5, 1),
(N'Luyện thi IELTS', N'luyen-thi-ielts.jpg', 160000, 150000, 190, 140, N'Sách luyện thi IELTS.', N'NXB Khoa Học', 2024, N'Bìa mềm', 380, N'14x21 cm', 0.5, 25, 5, 1),

-- Sách Văn học - Tiểu thuyết tiếp theo
(N'Góc khuất của ánh sáng', N'goc-khuat-cua-anh-sang.jpg', 140000, 130000, 450, 220, N'Một tiểu thuyết lãng mạn.', N'NXB Văn Học', 2011, N'Bìa mềm', 520, N'13x20 cm', 0.5, 1, 1, 1),
(N'Kẻ săn đuổi', N'ke-san-duoi.jpg', 130000, 120000, 420, 210, N'Tiểu thuyết trinh thám.', N'NXB Trẻ', 2012, N'Bìa cứng', 480, N'14x21 cm', 0.6, 2, 1, 1),
(N'Tình yêu và chiến tranh', N'tinh-yeu-va-chien-tranh.jpg', 120000, 110000, 410, 200, N'Tiểu thuyết lịch sử.', N'NXB Hội Nhà Văn', 2013, N'Bìa mềm', 450, N'13x20 cm', 0.5, 3, 1, 1),
(N'Bên kia bầu trời', N'ben-kia-bau-troi.jpg', 110000, 100000, 390, 190, N'Tiểu thuyết phiêu lưu.', N'NXB Văn Học', 2014, N'Bìa mềm', 420, N'13x20 cm', 0.5, 4, 1, 1),
(N'Thế giới song song', N'the-gioi-song-song.jpg', 100000, 90000, 370, 180, N'Tiểu thuyết khoa học viễn tưởng.', N'NXB Trẻ', 2015, N'Bìa mềm', 400, N'13x20 cm', 0.5, 5, 1, 1),

-- Sách Văn học - Truyện ngắn tiếp theo
(N'Cuộc đời và những nỗi đau', N'cuoc-doi-va-nhung-noi-dau.jpg', 90000, 80000, 350, 170, N'Truyện ngắn xã hội.', N'NXB Hội Nhà Văn', 2016, N'Bìa mềm', 380, N'13x20 cm', 0.4, 6, 2, 1),
(N'Những kỷ niệm', N'nhung-ky-niem.jpg', 85000, 75000, 330, 160, N'Truyện ngắn về kỷ niệm.', N'NXB Văn Học', 2017, N'Bìa mềm', 360, N'13x20 cm', 0.4, 7, 2, 1),
(N'Hoa nở muộn', N'hoa-no-muon.jpg', 80000, 70000, 310, 150, N'Truyện ngắn về tuổi trẻ.', N'NXB Trẻ', 2018, N'Bìa mềm', 340, N'13x20 cm', 0.4, 8, 2, 1),
(N'Những đêm mưa', N'nhung-dem-mua.jpg', 75000, 65000, 290, 140, N'Truyện ngắn về mưa.', N'NXB Hội Nhà Văn', 2019, N'Bìa mềm', 320, N'13x20 cm', 0.4, 9, 2, 1),
(N'Cuộc hành trình', N'cuoc-hanh-trinh.jpg', 70000, 60000, 270, 130, N'Truyện ngắn về cuộc sống.', N'NXB Văn Học', 2020, N'Bìa mềm', 300, N'13x20 cm', 0.4, 10, 2, 1),

-- Sách Khoa học - Vật lý tiếp theo
(N'Lực lượng và chuyển động', N'luc-luong-va-chuyen-dong.jpg', 150000, 140000, 250, 120, N'Sách về lực lượng và chuyển động.', N'NXB Khoa Học', 2021, N'Bìa cứng', 420, N'15x23 cm', 0.6, 11, 3, 1),
(N'Cơ học lượng tử', N'co-hoc-luong-tu.jpg', 160000, 150000, 260, 130, N'Sách về cơ học lượng tử.', N'NXB Tri Thức', 2022, N'Bìa mềm', 440, N'16x24 cm', 0.7, 12, 3, 1),
(N'Vật lý thiên văn', N'vat-ly-thien-van.jpg', 170000, 160000, 270, 140, N'Sách về vật lý thiên văn.', N'NXB Giáo Dục', 2023, N'Bìa mềm', 460, N'14x21 cm', 0.6, 13, 3, 1),
(N'Điện động lực học', N'dien-dong-luc-hoc.jpg', 180000, 170000, 280, 150, N'Sách về điện động lực học.', N'NXB Khoa Học', 2024, N'Bìa mềm', 480, N'15x23 cm', 0.6, 14, 3, 1),
(N'Thuyết tương đối rộng', N'thuyet-tuong-doi-rong.jpg', 190000, 180000, 290, 160, N'Sách về thuyết tương đối rộng.', N'NXB Tri Thức', 2025, N'Bìa mềm', 500, N'16x24 cm', 0.7, 15, 3, 1),

-- Sách Khoa học - Hóa học tiếp theo
(N'Hóa học bề mặt', N'hoa-hoc-be-mat.jpg', 140000, 130000, 240, 120, N'Sách về hóa học bề mặt.', N'NXB Giáo Dục', 2026, N'Bìa mềm', 380, N'14x21 cm', 0.5, 16, 4, 1),
(N'Hóa học môi trường', N'hoa-hoc-moi-truong.jpg', 150000, 140000, 250, 130, N'Sách về hóa học môi trường.', N'NXB Khoa Học', 2027, N'Bìa mềm', 400, N'15x23 cm', 0.6, 17, 4, 1),
(N'Hóa học sinh học', N'hoa-hoc-sinh-hoc.jpg', 160000, 150000, 260, 140, N'Sách về hóa học sinh học.', N'NXB Tri Thức', 2028, N'Bìa mềm', 420, N'16x24 cm', 0.6, 18, 4, 1),
(N'Hóa học hữu cơ nâng cao', N'hoa-hoc-huu-co-nang-cao.jpg', 170000, 160000, 270, 150, N'Sách về hóa học hữu cơ nâng cao.', N'NXB Giáo Dục', 2029, N'Bìa mềm', 440, N'14x21 cm', 0.5, 19, 4, 1),
(N'Phương pháp phân tích hóa học', N'phuong-phap-phan-tich-hoa-hoc.jpg', 180000, 170000, 280, 160, N'Sách về phương pháp phân tích hóa học.', N'NXB Khoa Học', 2030, N'Bìa mềm', 460, N'15x23 cm', 0.6, 20, 4, 1),

-- Sách Ngoại ngữ - Tiếng Anh tiếp theo
(N'Từ điển Anh-Việt', N'tu-dien-anh-viet.jpg', 140000, 130000, 370, 180, N'Từ điển tiếng Anh-Việt.', N'NXB Giáo Dục', 2021, N'Bìa cứng', 520, N'14x21 cm', 0.5, 21, 5, 1),
(N'Luyện nghe tiếng Anh', N'luyen-nghe-tieng-anh.jpg', 150000, 140000, 380, 190, N'Sách luyện nghe tiếng Anh.', N'NXB Khoa Học', 2022, N'Bìa mềm', 540, N'15x23 cm', 0.6, 22, 5, 1),
(N'Giao tiếp tiếng Anh nơi công sở', N'giao-tiep-tieng-anh-noi-cong-so.jpg', 160000, 150000, 390, 200, N'Sách giao tiếp tiếng Anh nơi công sở.', N'NXB Trẻ', 2023, N'Bìa mềm', 560, N'16x24 cm', 0.7, 23, 5, 1),
(N'Luyện viết tiếng Anh', N'luyen-viet-tieng-anh.jpg', 170000, 160000, 400, 210, N'Sách luyện viết tiếng Anh.', N'NXB Giáo Dục', 2024, N'Bìa mềm', 580, N'14x21 cm', 0.6, 24, 5, 1),
(N'Tiếng Anh cho trẻ em', N'tieng-anh-cho-tre-em.jpg', 180000, 170000, 410, 220, N'Sách học tiếng Anh cho trẻ em.', N'NXB Tri Thức', 2025, N'Bìa mềm', 600, N'13x20 cm', 0.5, 25, 5, 1),

-- Sách Văn học - Tiểu thuyết tiếp theo
(N'Mùa thu vàng', N'mua-thu-vang.jpg', 190000, 180000, 370, 200, N'Tiểu thuyết tình cảm.', N'NXB Văn Học', 2016, N'Bìa mềm', 420, N'14x21 cm', 0.5, 1, 1, 1),
(N'Kẻ săn bóng tối', N'ke-san-bong-toi.jpg', 200000, 190000, 380, 210, N'Tiểu thuyết kinh dị.', N'NXB Trẻ', 2017, N'Bìa mềm', 440, N'14x21 cm', 0.5, 2, 1, 1),
(N'Biên niên sử côn trùng', N'bien-nien-su-con-trung.jpg', 210000, 200000, 390, 220, N'Tiểu thuyết kỳ ảo.', N'NXB Hội Nhà Văn', 2018, N'Bìa mềm', 460, N'14x21 cm', 0.6, 3, 1, 1),
(N'Thế giới động vật', N'the-gioi-dong-vat.jpg', 220000, 210000, 400, 230, N'Tiểu thuyết phiêu lưu.', N'NXB Văn Học', 2019, N'Bìa mềm', 480, N'14x21 cm', 0.6, 4, 1, 1),
(N'Người trở về từ cõi chết', N'nguoi-tro-ve-tu-coi-chet.jpg', 230000, 220000, 410, 240, N'Tiểu thuyết trinh thám.', N'NXB Trẻ', 2020, N'Bìa mềm', 500, N'14x21 cm', 0.7, 5, 1, 1),

-- Sách Văn học - Truyện ngắn tiếp theo
(N'Những mùa xuân đã qua', N'nhung-mua-xuan-da-qua.jpg', 130000, 120000, 290, 150, N'Truyện ngắn về mùa xuân.', N'NXB Hội Nhà Văn', 2021, N'Bìa mềm', 320, N'13x20 cm', 0.4, 6, 2, 1),
(N'Mùa hè rực rỡ', N'mua-he-ruc-ro.jpg', 140000, 130000, 300, 160, N'Truyện ngắn về mùa hè.', N'NXB Văn Học', 2022, N'Bìa mềm', 340, N'13x20 cm', 0.4, 7, 2, 1),
(N'Tháng năm và những kỷ niệm', N'thang-nam-va-nhung-ky-niem.jpg', 150000, 140000, 310, 170, N'Truyện ngắn về tháng năm.', N'NXB Trẻ', 2023, N'Bìa mềm', 360, N'13x20 cm', 0.4, 8, 2, 1),
(N'Mùa đông lạnh giá', N'mua-dong-lanh-gia.jpg', 160000, 150000, 320, 180, N'Truyện ngắn về mùa đông.', N'NXB Hội Nhà Văn', 2024, N'Bìa mềm', 380, N'13x20 cm', 0.4, 9, 2, 1),
(N'Những ngày mưa', N'nhung-ngay-mua.jpg', 170000, 160000, 330, 190, N'Truyện ngắn về ngày mưa.', N'NXB Văn Học', 2025, N'Bìa mềm', 400, N'13x20 cm', 0.4, 10, 2, 1),
-- Sách Khoa học - Vật lý tiếp theo
(N'Vật lý nhiệt động', N'vat-ly-nhiet-dong.jpg', 190000, 180000, 300, 170, N'Sách về vật lý nhiệt động.', N'NXB Tri Thức', 2026, N'Bìa mềm', 500, N'15x23 cm', 0.7, 21, 3, 1),
(N'Vật lý hạt nhân', N'vat-ly-hat-nhan.jpg', 200000, 190000, 310, 180, N'Sách về vật lý hạt nhân.', N'NXB Giáo Dục', 2027, N'Bìa mềm', 520, N'16x24 cm', 0.7, 22, 3, 1),
(N'Cơ học chất rắn', N'co-hoc-chat-ran.jpg', 210000, 200000, 320, 190, N'Sách về cơ học chất rắn.', N'NXB Khoa Học', 2028, N'Bìa mềm', 540, N'15x23 cm', 0.8, 23, 3, 1),
(N'Quang học lượng tử', N'quang-hoc-luong-tu.jpg', 220000, 210000, 330, 200, N'Sách về quang học lượng tử.', N'NXB Tri Thức', 2029, N'Bìa mềm', 560, N'16x24 cm', 0.8, 24, 3, 1),
(N'Vật lý chất lỏng', N'vat-ly-chat-long.jpg', 230000, 220000, 340, 210, N'Sách về vật lý chất lỏng.', N'NXB Giáo Dục', 2030, N'Bìa mềm', 580, N'15x23 cm', 0.9, 25, 3, 1),

-- Sách Khoa học - Hóa học tiếp theo
(N'Phản ứng hóa học', N'phan-ung-hoa-hoc.jpg', 190000, 180000, 300, 170, N'Sách về phản ứng hóa học.', N'NXB Khoa Học', 2031, N'Bìa mềm', 500, N'14x21 cm', 0.7, 21, 4, 1),
(N'Hoá học phân tích', N'hoa-hoc-phan-tich.jpg', 200000, 190000, 310, 180, N'Sách về hóa học phân tích.', N'NXB Tri Thức', 2032, N'Bìa mềm', 520, N'15x23 cm', 0.8, 22, 4, 1),
(N'Hoá học vô cơ', N'hoa-hoc-vo-co.jpg', 210000, 200000, 320, 190, N'Sách về hóa học vô cơ.', N'NXB Giáo Dục', 2033, N'Bìa mềm', 540, N'16x24 cm', 0.8, 23, 4, 1),
(N'Hoá học hữu cơ', N'hoa-hoc-huu-co.jpg', 220000, 210000, 330, 200, N'Sách về hóa học hữu cơ.', N'NXB Khoa Học', 2034, N'Bìa mềm', 560, N'14x21 cm', 0.9, 24, 4, 1),
(N'Hoá học vật liệu', N'hoa-hoc-vat-lieu.jpg', 230000, 220000, 340, 210, N'Sách về hóa học vật liệu.', N'NXB Tri Thức', 2035, N'Bìa mềm', 580, N'15x23 cm', 0.9, 25, 4, 1),

-- Sách Ngoại ngữ - Tiếng Anh tiếp theo
(N'Ngữ âm tiếng Anh', N'ngu-am-tieng-anh.jpg', 190000, 180000, 370, 190, N'Sách về ngữ âm tiếng Anh.', N'NXB Giáo Dục', 2026, N'Bìa cứng', 600, N'14x21 cm', 0.7, 21, 5, 1),
(N'Tiếng Anh thương mại', N'tieng-anh-thuong-mai.jpg', 200000, 190000, 380, 200, N'Sách tiếng Anh thương mại.', N'NXB Khoa Học', 2027, N'Bìa mềm', 620, N'15x23 cm', 0.8, 22, 5, 1),
(N'Tiếng Anh du lịch', N'tieng-anh-du-lich.jpg', 210000, 200000, 390, 210, N'Sách tiếng Anh du lịch.', N'NXB Trẻ', 2028, N'Bìa mềm', 640, N'16x24 cm', 0.8, 23, 5, 1),
(N'Tiếng Anh giao tiếp', N'tieng-anh-giao-tiep.jpg', 220000, 210000, 400, 220, N'Sách tiếng Anh giao tiếp.', N'NXB Giáo Dục', 2029, N'Bìa mềm', 660, N'14x21 cm', 0.9, 24, 5, 1),
(N'Phương pháp dạy tiếng Anh', N'phuong-phap-day-tieng-anh.jpg', 230000, 220000, 410, 230, N'Sách về phương pháp dạy tiếng Anh.', N'NXB Tri Thức', 2030, N'Bìa mềm', 680, N'15x23 cm', 0.9, 25, 5, 1),

-- Sách Văn học - Tiểu thuyết tiếp theo
(N'Chân trời mới', N'chan-troi-moi.jpg', 240000, 230000, 420, 230, N'Tiểu thuyết phiêu lưu.', N'NXB Văn Học', 2021, N'Bìa mềm', 500, N'14x21 cm', 0.6, 6, 1, 1),
(N'Người lữ hành', N'nguoi-lu-hanh.jpg', 250000, 240000, 430, 240, N'Tiểu thuyết kỳ ảo.', N'NXB Trẻ', 2022, N'Bìa mềm', 520, N'15x23 cm', 0.7, 7, 1, 1),
(N'Khát vọng sống', N'khat-vong-song.jpg', 260000, 250000, 440, 250, N'Tiểu thuyết xã hội.', N'NXB Hội Nhà Văn', 2023, N'Bìa mềm', 540, N'14x21 cm', 0.8, 8, 1, 1),
(N'Giấc mơ lớn', N'giac-mo-lon.jpg', 270000, 260000, 450, 260, N'Tiểu thuyết lãng mạn.', N'NXB Văn Học', 2024, N'Bìa mềm', 560, N'15x23 cm', 0.8, 9, 1, 1),
(N'Ngọn đèn trước gió', N'ngon-den-truoc-gio.jpg', 280000, 270000, 460, 270, N'Tiểu thuyết tâm lý.', N'NXB Trẻ', 2025, N'Bìa mềm', 580, N'14x21 cm', 0.9, 10, 1, 1),

-- Sách Văn học - Truyện ngắn tiếp theo
(N'Mùa thu và em', N'mua-thu-va-em.jpg', 180000, 170000, 340, 180, N'Truyện ngắn về mùa thu.', N'NXB Hội Nhà Văn', 2026, N'Bìa mềm', 400, N'13x20 cm', 0.5, 11, 2, 1),
(N'Mùa đông ấm áp', N'mua-dong-am-ap.jpg', 190000, 180000, 350, 190, N'Truyện ngắn về mùa đông.', N'NXB Văn Học', 2027, N'Bìa mềm', 420, N'13x20 cm', 0.5, 12, 2, 1),
(N'Mùa xuân tươi đẹp', N'mua-xuan-tuoi-dep.jpg', 200000, 190000, 360, 200, N'Truyện ngắn về mùa xuân.', N'NXB Trẻ', 2028, N'Bìa mềm', 440, N'13x20 cm', 0.6, 13, 2, 1),
(N'Mùa hè trong mắt em', N'mua-he-trong-mat-em.jpg', 210000, 200000, 370, 210, N'Truyện ngắn về mùa hè.', N'NXB Hội Nhà Văn', 2029, N'Bìa mềm', 460, N'13x20 cm', 0.6, 14, 2, 1),
(N'Những cơn mưa chiều', N'nhung-con-mua-chieu.jpg', 220000, 210000, 380, 220, N'Truyện ngắn về cơn mưa.', N'NXB Văn Học', 2030, N'Bìa mềm', 480, N'13x20 cm', 0.6, 15, 2, 1),

-- Sách Khoa học - Vật lý tiếp theo
(N'Vật lý sóng cơ', N'vat-ly-song-co.jpg', 240000, 230000, 400, 230, N'Sách về vật lý sóng cơ.', N'NXB Tri Thức', 2031, N'Bìa mềm', 600, N'15x23 cm', 0.7, 26, 3, 1),
(N'Vật lý sóng điện từ', N'vat-ly-song-dien-tu.jpg', 250000, 240000, 410, 240, N'Sách về vật lý sóng điện từ.', N'NXB Giáo Dục', 2032, N'Bìa mềm', 620, N'16x24 cm', 0.8, 27, 3, 1),
(N'Vật lý chất bán dẫn', N'vat-ly-chat-ban-dan.jpg', 260000, 250000, 420, 250, N'Sách về vật lý chất bán dẫn.', N'NXB Khoa Học', 2033, N'Bìa mềm', 640, N'15x23 cm', 0.8, 28, 3, 1),
(N'Vật lý plasma', N'vat-ly-plasma.jpg', 270000, 260000, 430, 260, N'Sách về vật lý plasma.', N'NXB Tri Thức', 2034, N'Bìa mềm', 660, N'16x24 cm', 0.9, 29, 3, 1),
(N'Thuyết tương đối rộng', N'thuyet-tuong-doi-rong.jpg', 280000, 270000, 440, 270, N'Sách về thuyết tương đối rộng.', N'NXB Giáo Dục', 2035, N'Bìa mềm', 680, N'15x23 cm', 0.9, 30, 3, 1),
-- Sách Khoa học - Vật lý tiếp theo
(N'Vật lý điện', N'vat-ly-dien.jpg', 290000, 280000, 450, 280, N'Sách về vật lý điện.', N'NXB Khoa Học', 2036, N'Bìa mềm', 700, N'14x21 cm', 0.9, 31, 3, 1),
(N'Vật lý từ trường', N'vat-ly-tu-truong.jpg', 300000, 290000, 460, 290, N'Sách về vật lý từ trường.', N'NXB Tri Thức', 2037, N'Bìa mềm', 720, N'15x23 cm', 1.0, 32, 3, 1),
(N'Vật lý điện tử', N'vat-ly-dien-tu.jpg', 310000, 300000, 470, 300, N'Sách về vật lý điện tử.', N'NXB Giáo Dục', 2038, N'Bìa mềm', 740, N'16x24 cm', 1.0, 33, 3, 1),
(N'Vật lý nguyên tử', N'vat-ly-nguyen-to.jpg', 320000, 310000, 480, 310, N'Sách về vật lý nguyên tử.', N'NXB Tri Thức', 2039, N'Bìa mềm', 760, N'15x23 cm', 1.1, 34, 3, 1),
(N'Vật lý hạt nhân', N'vat-ly-hat-nhan2.jpg', 330000, 320000, 490, 320, N'Sách về vật lý hạt nhân.', N'NXB Giáo Dục', 2040, N'Bìa mềm', 780, N'16x24 cm', 1.1, 35, 3, 1),

-- Sách Khoa học - Hóa học tiếp theo
(N'Hóa học hữu cơ cơ bản', N'hoa-hoc-huu-co-co-ban.jpg', 240000, 230000, 400, 230, N'Sách về hóa học hữu cơ cơ bản.', N'NXB Khoa Học', 2036, N'Bìa mềm', 600, N'14x21 cm', 0.7, 26, 4, 1),
(N'Hóa học hữu cơ nâng cao', N'hoa-hoc-huu-co-nang-cao.jpg', 250000, 240000, 410, 240, N'Sách về hóa học hữu cơ nâng cao.', N'NXB Tri Thức', 2037, N'Bìa mềm', 620, N'15x23 cm', 0.8, 27, 4, 1),
(N'Hóa học vô cơ cơ bản', N'hoa-hoc-vo-co-co-ban.jpg', 260000, 250000, 420, 250, N'Sách về hóa học vô cơ cơ bản.', N'NXB Giáo Dục', 2038, N'Bìa mềm', 640, N'16x24 cm', 0.8, 28, 4, 1),
(N'Hóa học vô cơ nâng cao', N'hoa-hoc-vo-co-nang-cao.jpg', 270000, 260000, 430, 260, N'Sách về hóa học vô cơ nâng cao.', N'NXB Khoa Học', 2039, N'Bìa mềm', 660, N'15x23 cm', 0.9, 29, 4, 1)


ALTER TABLE ChiTietGioHang ADD IsSelected BIT NOT NULL DEFAULT 0;