using BanSachWeb.Models;
using BanSachWeb.Models.Payments;
using BanSachWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BanSachWeb.Controllers
{
    public class CartController : Controller
    {
        QuanLyBanSachModel db=new QuanLyBanSachModel();

        private static decimal amount = 0;
        private static int id = 0;
        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);

        }

        private JsonResult AddToCartLogic(int maSach, int quantity, bool isSelected)
        {
            var sach = GetSachById(maSach);
            var cart = GetCart();
            if (cart == null)
            {
                Session["ReturnUrl"] = Request.UrlReferrer?.ToString();
                
                return Json(new { success = false, notLoggedIn = true, message = "Vui lòng đăng nhập." });
            }
            else
            {
                var cartItems = db.ChiTietGioHangs.Where(c => c.MaGioHang == cart.MaGioHang).ToList();
                foreach (var item in cartItems)
                {
                    item.IsSelected = false;
                }
                var cartItem = cartItems.FirstOrDefault(c => c.MaSach == maSach);

                if (cartItem != null)
                {
                    cartItem.SoLuong += quantity;
                    cartItem.ThanhTien += quantity * db.Saches.Find(maSach).GiaBan;
                    cartItem.IsSelected = isSelected;
                }
                else
                {
                    cartItem = new ChiTietGioHang
                    {
                        MaGioHang = cart.MaGioHang,
                        MaSach = maSach,
                        SoLuong = quantity,
                        ThanhTien = quantity * db.Saches.Find(maSach).GiaBan,
                        IsSelected = isSelected
                    };
                    db.ChiTietGioHangs.Add(cartItem);
                }

                db.SaveChanges();
                return Json(new { success = true, message = "Thêm vào giỏ hàng thành công." });
            }
        }

        [HttpPost]
        public ActionResult AddToCart(int maSach, int quantity, bool isSelected = false)
        {
            return AddToCartLogic(maSach, quantity, isSelected);
        }

        [HttpPost]
        public ActionResult BuyNow(int maSach, int quantity = 1)
        {
            var result = AddToCartLogic(maSach, quantity, true); 
            var jsonResult = result as JsonResult;
            if (jsonResult != null)
            {
                var jsonData = jsonResult.Data as dynamic;
                if (jsonData.success == false && jsonData.message == "Vui lòng đăng nhập.")
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Index", "Cart");
        }


        public ActionResult RemoveFromCart(int maSach)
        {
            var cart = GetCart(); 
            var itemToRemove = cart.ChiTietGioHangs.FirstOrDefault(i => i.MaSach == maSach);
            if (itemToRemove != null)
            {
                cart.ChiTietGioHangs.Remove(itemToRemove);
            }
            db.SaveChanges();

            return RedirectToAction("Index", "Cart"); 

            
        }

        public ActionResult ClearCart()
        {
            var cart = GetCart();
            cart.ChiTietGioHangs.Clear();
            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        private GioHang GetCart()
        {

            if (Session["MaTaiKhoan"] != null)
            {
                int maTaiKhoan = (int)Session["MaTaiKhoan"];
                var cart= db.GioHangs.FirstOrDefault(g => g.MaTaiKhoan == maTaiKhoan);
                if (cart != null)
                {
                    return cart;
                }
                else
                {
                    cart = new GioHang { MaTaiKhoan = maTaiKhoan };
                    db.GioHangs.Add(cart);
                    db.SaveChanges();
                    return cart;
                }
            }
            return null;


        }
       


        private Sach GetSachById(int id)
        {
           
                return db.Saches.Find(id);
         
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int maSach, int newQuantity)
        {
            var sach = GetSachById(maSach);
            if (sach == null)
            {
                return HttpNotFound();
            }

            if (newQuantity < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cart = GetCart();

            var cartItem = cart.ChiTietGioHangs.FirstOrDefault(item => item.MaSach == maSach);

            if (cartItem != null)
            {
                cartItem.SoLuong = newQuantity;
                cartItem.ThanhTien = sach.GiaBan * newQuantity; 

                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }


        public int CountItemInCart()
        {
            var cart = GetCart();
            int countItem = 0;
            if (cart != null && cart.ChiTietGioHangs != null)
            {
                foreach (var item in cart.ChiTietGioHangs)
                {
                    countItem += item.SoLuong ?? 0; 
                }
            }
            return countItem;
        }
        public int CreateOrder(string paymentMethod, List<int> selectedProducts)
        {
            var username = Session["emailOrPhone"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                System.Diagnostics.Debug.WriteLine("CreateOrder failed: Missing user session.");
                return -1; 
            }

            var account = db.TaiKhoans.FirstOrDefault(s => s.Email == username);
            if (account == null)
            {
                System.Diagnostics.Debug.WriteLine("CreateOrder failed: Invalid account.");
                return -1; 
            }

            var cart = GetCart();
            if (cart == null || !cart.ChiTietGioHangs.Any())
            {
                System.Diagnostics.Debug.WriteLine("CreateOrder failed: Empty cart.");
                return -1; 
            }

            var selectedItems = cart.ChiTietGioHangs
                .Where(item => item.MaSach.HasValue && selectedProducts.Contains(item.MaSach.Value))
                .ToList();

            if (!selectedItems.Any())
            {
                System.Diagnostics.Debug.WriteLine("CreateOrder failed: No selected items.");
                return -1; 
            }

            var defaultAddress = db.DiaChis.FirstOrDefault(d => d.MaTaiKhoan == account.MaTaiKhoan && d.MacDinh == true);
            
           

            decimal totalOrderPrice = selectedItems.Sum(item => item.ThanhTien ?? 0);
            amount = totalOrderPrice;
            var order = new DonHang
            {
                ThoiGianDatHang = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                TongGiaTri = totalOrderPrice,
                MaTaiKhoan = account.MaTaiKhoan,
                PhuongThucThanhToan = paymentMethod,

                MaDiaChi = defaultAddress?.MaDiaChi,
                ChiTietDonHangs = selectedItems.Select(item => new ChiTietDonHang
                {
                    MaSach = item.MaSach,
                    SoLuong = item.SoLuong,
                    GiaBan = item.Sach.GiaBan,
                    ThanhTien = item.ThanhTien
                }).ToList()
            };

            try
            {
                db.DonHangs.Add(order);
                foreach (var item in selectedItems)
                {
                    var product = db.Saches.Find(item.MaSach);
                    if (product != null)
                    {
                        product.SoLuongDaBan = (product.SoLuongDaBan ?? 0) + item.SoLuong;
                        System.Diagnostics.Debug.WriteLine("New sold quantity: " + product.SoLuongDaBan);

                    }
                }
                db.SaveChanges();
              
                System.Diagnostics.Debug.WriteLine("CreateOrder succeeded: Order ID " + order.MaDonHang);
                return order.MaDonHang;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CreateOrder failed: Exception " + ex.Message);
                return -1;
            }
        }


        [HttpPost]
        public ActionResult CheckOut(string paymentMethod, List<int> selectedProducts)
        {
            System.Diagnostics.Debug.WriteLine("Payment Method: " + paymentMethod);
            System.Diagnostics.Debug.WriteLine("Selected Products: " + string.Join(", ", selectedProducts));

            int orderId = CreateOrder(paymentMethod, selectedProducts);
            if (orderId == -1)
            {
                return Json(new { success = false, message = "Order creation failed" });
            }
            Session["oderID"]=orderId;

            return Json(new { success = true, orderId = orderId });
        }




        public ActionResult OrderConfirmation(int orderId)
        {
            var order = db.DonHangs.Include("ChiTietDonHangs").FirstOrDefault(s => s.MaDonHang == orderId);
            if(Session["madiachi"]!=null)
            {
                var MaDiaChi = (int)Session["madiachi"];
                order.DiaChi=db.DiaChis.Where(t=> t.MaDiaChi==MaDiaChi).FirstOrDefault();
                System.Diagnostics.Debug.WriteLine("New address " + order.MaDiaChi);

            }
            
            if (order == null)
            {
                System.Diagnostics.Debug.WriteLine("Errors");
                return HttpNotFound();
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(order);
        }

        public string CreatePaymentUrl(PaymentInformationModel model)
        {
            string vnp_Version = "2.1.0";
            string vnp_Command = "pay";
            string orderType = "other";
            string bankCode = "NCB";

            string vnp_TxnRef = GetRandomNumber(8);
            string vnp_IpAddr = "127.0.0.1";
            string vnp_TmnCode = "0S7T01T8";

            var vnp_Params = new Dictionary<string, string>
    {
        { "vnp_Version", vnp_Version },
        { "vnp_Command", vnp_Command },
        { "vnp_TmnCode", vnp_TmnCode },
        { "vnp_Amount", (Math.Floor(model.Amount * 100)).ToString() },

        { "vnp_CurrCode", "VND" },
        { "vnp_BankCode", bankCode },
        { "vnp_TxnRef", vnp_TxnRef },
        { "vnp_OrderInfo", "Thanhtoan" },
        { "vnp_OrderType", model.OrderType },
        { "vnp_Locale", "vn" },
        { "vnp_ReturnUrl", "https://localhost:44379/Cart/PaymentCallback" },
        { "vnp_IpAddr", vnp_IpAddr },
        { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
    };

            DateTime expireDate = DateTime.Now.AddMinutes(15);
            vnp_Params.Add("vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss"));

            var fieldNames = vnp_Params.Keys.ToList();
            fieldNames.Sort();

            StringBuilder hashData = new StringBuilder();
            StringBuilder query = new StringBuilder();

            foreach (var fieldName in fieldNames)
            {
                string fieldValue = vnp_Params[fieldName];
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    hashData.Append(fieldName).Append('=').Append(Uri.EscapeDataString(fieldValue));
                    query.Append(Uri.EscapeDataString(fieldName))
                         .Append('=')
                         .Append(Uri.EscapeDataString(fieldValue));

                    if (fieldNames.IndexOf(fieldName) < fieldNames.Count - 1)
                    {
                        query.Append('&');
                        hashData.Append('&');
                    }
                }
            }

            string secretKey = "BEZLUPOPOTXTDYZHCBGDJBHFJPBLSARL";
            string secureHash = HmacSHA512(secretKey, hashData.ToString());

            query.Append("&vnp_SecureHash=").Append(secureHash);

            string paymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?" + query.ToString();

            System.Diagnostics.Debug.WriteLine($"Payment URL: {paymentUrl}");

            return paymentUrl;
        }


        public string GetRandomNumber(int length)
        {
            Random rnd = new Random();
            const string chars = "0123456789";
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
            }

            return sb.ToString();
        }


        public static string HmacSHA512(string key, string data)
        {
            if (key == null || data == null)
            {
                throw new ArgumentNullException("Key and data must not be null.");
            }

            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashBytes = hmac.ComputeHash(dataBytes);

                StringBuilder sb = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                {
                    sb.AppendFormat("{0:x2}", b);
                }

                return sb.ToString();
            }
        }

        public ActionResult OnlinePayment(int typePaymentVN, int orderId)
        {
            var model = new PaymentInformationModel()
            {
                OrderType = "other",
                Amount = amount,
                OrderDescription = "Thanh toán đơn hàng",
                Name = "User",
            };

            var paymentUrl = CreatePaymentUrl(model);
            return Redirect(paymentUrl);
        }
        public ActionResult PaymentReturn()
        {
            var vnpayData = Request.QueryString;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData)
            {
                vnpay.AddResponseData(s, vnpayData[s]);
            }
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            bool checkSignature = vnpay.ValidateSignature(vnpay.GetResponseData("vnp_SecureHash"), vnp_HashSecret);
            if (checkSignature)
            {
                return RedirectToAction("CheckOut");
            }
            else
            {
                return View("PaymentError");
            }
        }

        public ActionResult PaymentCallback()
        {
            try
            {
                TempData["SuccessMessage"] = "Your order has been placed successfully!";
                return RedirectToAction("OrderConfirmation", new { orderId = id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ViewBag.Message = "Đã xảy ra lỗi trong quá trình xử lý thanh toán.";
                return View("Error");
            }
        }

        public ActionResult ConfirmOrder(int orderId, string paymentMethod)
        {
            id = orderId;
            if (paymentMethod.Equals("cash", StringComparison.OrdinalIgnoreCase))
            {
                TempData["SuccessMessage"] = "Your order has been placed successfully!";
                return RedirectToAction("OrderConfirmation", new { orderId = orderId });
            }
            else if (paymentMethod.Equals("online", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("OnlinePayment", new { typePaymentVN = 1, orderId = orderId }); 
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid payment method.";
                return RedirectToAction("OrderConfirmation", new { orderId = orderId });
            }
        }
        public ActionResult UserOrders()
        {
            var maTaiKhoan = Session["MaTaiKhoan"] as int?; ;

            if (maTaiKhoan == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            var orders = db.DonHangs.Where(o => o.MaTaiKhoan== maTaiKhoan).ToList(); 

            return View(orders); 
        }
        public ActionResult OrderDetails(int id)
        {
            var order = db.DonHangs.Include("ChiTietDonHangs")
                                   .FirstOrDefault(o => o.MaDonHang == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
        public ActionResult CancelOrder(int id)
        {
            var order = db.DonHangs.FirstOrDefault(o => o.MaDonHang == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            

            order.TrangThai = "cancelled"; 
            db.SaveChanges();

            return RedirectToAction("UserOrders");
        }

    }
}
