namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("GioHang")]
    public partial class GioHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GioHang()
        {
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
        }

        [Key]
        public int MaGioHang { get; set; }

        public int? MaTaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
        private List<CartItem> items=new List<CartItem>();
        public void AddItem(Sach item,int quantity)
        {
            var exsistItem = items.FirstOrDefault(s => s.product.MaSach == item.MaSach);
            if(exsistItem != null)
            {
                exsistItem.quantity += quantity;
            }
            else
            {
                items.Add(new CartItem { product=item, quantity = quantity });
            }
        }
        public void RemoveItem(int productID)
        {
            items.RemoveAll(i => i.product.MaSach == productID);
        }
        public decimal GetTotalPrice()
        {
            return items.Sum(i => (i.product.GiaBan??0) * i.quantity);
        }
        public List<CartItem> GetItems()
        {
            return items;
        }
        public void UpdateQuantity(int maSach, int newQuantity)
        {
            // Tìm kiếm sản phẩm trong giỏ hàng dựa trên mã sách
            var item = items.FirstOrDefault(i => i.product.MaSach == maSach);

            // Nếu sản phẩm không tồn tại trong giỏ hàng, không cần thực hiện gì cả
            if (item == null)
            {
                return;
            }

            // Cập nhật số lượng của sản phẩm
            item.quantity = newQuantity;
        }

    }
}
