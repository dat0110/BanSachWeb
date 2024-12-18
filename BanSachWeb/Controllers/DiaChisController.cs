using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanSachWeb.Models;

namespace BanSachWeb.Controllers
{
    public class DiaChisController : Controller
    {
        private QuanLyBanSachModel db = new QuanLyBanSachModel();

        // GET: DiaChis
        public ActionResult Index()
        {
            var userId = (int)Session["MaTaiKhoan"];
            var diaChis = db.DiaChis.Where(d => d.MaTaiKhoan == userId).ToList();
            return View(diaChis);
        }

        

        // GET: DiaChis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiaChis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDiaChi,DiaChiCuThe,MacDinh,SoDienThoaiNhanHang,TenNguoiNhan")] DiaChi diaChi)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user's account information from the session
                var userEmailOrPhone = Session["emailOrPhone"] as string;
                var user = db.TaiKhoans.FirstOrDefault(u => u.Email == userEmailOrPhone || u.SoDienThoai == userEmailOrPhone);

                if (user != null)
                {
                    // Set the MaTaiKhoan property of the new address
                    diaChi.MaTaiKhoan = user.MaTaiKhoan;
                    // If this address is set as default, reset the MacDinh field for other addresses
                    if (diaChi.MacDinh.Value==true)
                    {
                        var existingAddresses = db.DiaChis.Where(d => d.MaTaiKhoan == user.MaTaiKhoan);
                        foreach (var address in existingAddresses)
                        {
                            address.MacDinh = false;
                        }
                    }

                    db.DiaChis.Add(diaChi);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "User not found. Please log in again.");
                }
            }

            return View(diaChi);
        }
        // GET: DiaChis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaChi diaChi = db.DiaChis.Find(id);
            if (diaChi == null || diaChi.MaTaiKhoan != (int)Session["MaTaiKhoan"]) // Ensure it belongs to the logged-in user
            {
                return HttpNotFound();
            }
            return View(diaChi);
        }

        // POST: DiaChis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit([Bind(Include = "MaDiaChi,DiaChiCuThe,MacDinh,SoDienThoaiNhanHang,TenNguoiNhan")] DiaChi diaChi)
        {
            if (ModelState.IsValid)
            {
                diaChi.MaTaiKhoan = (int)Session["MaTaiKhoan"];

                // Check if the address exists in the database
                var existingDiaChi = db.DiaChis.SingleOrDefault(d => d.MaDiaChi == diaChi.MaDiaChi && d.MaTaiKhoan == diaChi.MaTaiKhoan);
                if (existingDiaChi == null)
                {
                    return HttpNotFound();
                }

                // If this address is set as default, reset the MacDinh field for other addresses
                if (diaChi.MacDinh.Value == true)
                {
                    var existingAddresses = db.DiaChis.Where(d => d.MaTaiKhoan == diaChi.MaTaiKhoan && d.MaDiaChi != diaChi.MaDiaChi).ToList();
                    foreach (var address in existingAddresses)
                    {
                        address.MacDinh = false;
                        db.Entry(address).State = EntityState.Modified;
                    }
                }

                // Update the current address
                existingDiaChi.DiaChiCuThe = diaChi.DiaChiCuThe;
                existingDiaChi.MacDinh = diaChi.MacDinh;
                existingDiaChi.SoDienThoaiNhanHang = diaChi.SoDienThoaiNhanHang;
                existingDiaChi.TenNguoiNhan = diaChi.TenNguoiNhan;

                db.Entry(existingDiaChi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(diaChi);
        }


        // GET: DiaChis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaChi diaChi = db.DiaChis.Find(id);
            if (diaChi == null)
            {
                return HttpNotFound();
            }
            return View(diaChi);
        }

        // POST: DiaChis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaChi diaChi = db.DiaChis.Find(id);
            db.DiaChis.Remove(diaChi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
