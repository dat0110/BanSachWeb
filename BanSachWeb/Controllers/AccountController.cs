using BanSachWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSachWeb.ViewModels;

namespace BanSachWeb.Controllers
{
    public class AccountController : Controller
    {

        private QuanLyBanSachModel db = new QuanLyBanSachModel();// tạo đối tượng dể tương tác với CSDL thông qua linq


        //Use Case Đăng nhập
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]

        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = db.TaiKhoans
                    .FirstOrDefault(p => (p.Email == model.EmailOrPhone || p.SoDienThoai == model.EmailOrPhone) && p.MatKhau == model.Password);

                if (user != null)
                {
                    Session["emailOrPhone"] = user.Email;
                    Session["user"] = user.TenDangNhap;
                    Session["username"] = user.HoTen;
                    Session["MaTaiKhoan"] = user.MaTaiKhoan;

                    string redirectUrl = Session["ReturnUrl"] as string;
                    if (!string.IsNullOrEmpty(redirectUrl))
                    {
                        Session["ReturnUrl"] = null; // Clear the return URL
                        return Redirect(redirectUrl); // Redirect to the stored URL
                    }
                    return RedirectToAction("MainContent", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials. Please check and try again.");
                    return View(model);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "An error occurred: " + e.Message);
                return View(model);
            }

        }
        // Use case Đăng Xuất
        public ActionResult Logout()
        {
            Session.Remove("emailOrPhone");
            Session.Remove("username");
            Session.Remove("MaTaiKhoan");
            return RedirectToAction("Login", "Account");
        }
        //Use case Quên mật khẩu

        public ActionResult ForgetPass()
        {
            return View();
        }

        // POST: /Account/SendOtp
        [HttpPost]
        public ActionResult SendOtp(string emailOrPhoneNumber)
        {
            // Kiểm tra xem email hoặc số điện thoại có tồn tại trong cơ sở dữ liệu không
            var user = db.TaiKhoans.FirstOrDefault(u => u.Email == emailOrPhoneNumber || u.SoDienThoai == emailOrPhoneNumber);
            if (user == null)
            {
                ViewBag.Error = "Email hoặc số điện thoại không tồn tại.";
                return View("ForgetPass");
            }

            // Sinh mã OTP và gửi nó đến email hoặc số điện thoại của người dùng (bạn có thể sử dụng thư viện gửi email hoặc SMS tương ứng)

            TempData["UserId"] = user.MaTaiKhoan;
            TempData["OtpCode"] = "123456"; // Thay "123456" bằng mã OTP thực tế

            return RedirectToAction("VerifyOtp");
        }

        // GET: /Account/VerifyOtp
        public ActionResult VerifyOtp()
        {
            return View();
        }

        // POST: /Account/VerifyOtp
        [HttpPost]
        public ActionResult VerifyOtp(string otp)
        {
            var userId = (int)TempData["UserId"];
            var savedOtp = TempData["OtpCode"].ToString();

            if (otp != savedOtp)
            {
                ViewBag.Error = "Mã OTP không hợp lệ.";
                return View("VerifyOtp");
            }

            return RedirectToAction("SetNewPassword");
        }

        // GET: /Account/SetNewPassword
        public ActionResult SetNewPassword()
        {
            return View();
        }

        // POST: /Account/SetNewPassword
        [HttpPost]
        public ActionResult SetNewPassword(string newPassword)
        {
            var userId = (int)TempData["UserId"];
            var user = db.TaiKhoans.Find(userId);

            if (user == null)
            {
                ViewBag.Error = "Người dùng không tồn tại.";
                return View("SetNewPassword");
            }

            // Cập nhật mật khẩu mới cho người dùng
            user.MatKhau = newPassword;
            db.SaveChanges();

            return RedirectToAction("Login");
        }
        [HttpGet]
        // Use case Đổi mật khẩu
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(string newPass, string confirmPass)
        {
            if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                ViewBag.mess = "Vui lòng nhập đầy đủ thông tin!";
                return View("ChangePass");
            }
            if (Session["emailOrPhone"] != null)
            {
                var email = Session["emailOrPhone"].ToString();
                var user = db.TaiKhoans.SingleOrDefault(p => p.Email == email);
                if (user != null)
                {
                    if (newPass == confirmPass)
                    {
                        user.MatKhau = newPass;
                        db.SaveChanges();
                        return RedirectToAction("MainContent", "Home");
                    }

                    else if (newPass != confirmPass)
                    {
                        ViewBag.mess = "Nhập sai xác nhận mật khẩu!";
                    }

                }
            }
            return View("ChangePass");
        }
        [HttpGet]
        // use case Cập nhật thông tin tài khoản
        public ActionResult UpdateAccountInfo()
        {
            if(Session["emailOrPhone"] != null)
            {
                string userEmail = Session["emailOrPhone"].ToString();
                var user = db.TaiKhoans.FirstOrDefault(p => p.Email == userEmail);
                if (user != null)
                {
                    var address = db.DiaChis.FirstOrDefault(d => d.MaTaiKhoan == user.MaTaiKhoan && d.MacDinh == true);
                    
                    var model = new UpdateAccountInfoViewModel
                    {
                        TenDangNhap = user.TenDangNhap,
                        Email = user.Email,
                        HoTen = user.HoTen,
                        SoDienThoai = user.SoDienThoai,
                        ThongTinNhanHang = address != null ? address.DiaChiCuThe + " - " + address.TenNguoiNhan + " - " + address.SoDienThoaiNhanHang : ""

                    };
                    return View(model);
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public ActionResult UpdateAccountInfo(UpdateAccountInfoViewModel model)
        {
            if (!ModelState.IsValid)//checks whether the model binding and validation processes have succeeded.
            {
                return View(model);
            }
            else
            {
                if (Session["emailOrPhone"] != null)
                {
                    string userEmail = Session["emailOrPhone"].ToString();
                    var user = db.TaiKhoans.FirstOrDefault(p => p.Email == userEmail);
                    if (user != null)
                    {

                        user.TenDangNhap = model.TenDangNhap;
                        user.Email = model.Email;
                        user.HoTen = model.HoTen;
                        bool phoneNumberExists = db.TaiKhoans.Any(t => t.SoDienThoai == model.SoDienThoai && t.Email != userEmail);
                        if (phoneNumberExists)
                        {
                            ModelState.AddModelError("SoDienThoai", "Số điện thoại đã tồn tại trong hệ thống!");
                            return View(model);// Xử lý check xem số điện thoại mới đã tồn tại trong database hay chưa?
                        }
                        user.SoDienThoai = model.SoDienThoai;
                        Session["username"] = user.HoTen;
                        db.SaveChanges();
                        return RedirectToAction("MainContent", "Home");
                    }
                }
            }

            
            return RedirectToAction("Error", "Home");
        }
       
        // use case Dang Ky
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (db.TaiKhoans.Any(t => t.Email == model.Email || t.SoDienThoai == model.SoDienThoai))
                    {
                        ViewBag.err = "Email hoặc số điện thoại đã tồn tại trong hệ thống!";
                        return View(model);
                    }

                    var user = new TaiKhoan
                    {
                        TenDangNhap = model.TenDangNhap,
                        MatKhau = model.MatKhau,
                        Email = model.Email,
                        HoTen = model.HoTen,
                        SoDienThoai = model.SoDienThoai,
                        VaiTro = "User", // Assuming a default role
                        DiemThuong = 0 // Assuming default points
                    };

                    db.TaiKhoans.Add(user);
                    db.SaveChanges();

                    // Redirect to login or home page after successful registration
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return View(model);

                }
            }
            catch(Exception ex)
            {
                ViewBag.err = ex.Message;
                return View(model);
            }
               
                   
               
        
            
            
        }



    }
}
