# Hướng Dẫn Sử Dụng Trang Quản Lý Xe (Products)

## 📦 Tổng Quan

Trang **Products** cho phép quản lý danh sách xe điện với đầy đủ các chức năng CRUD (Create, Read, Update, Delete).

## 🎯 Các Tính Năng Đã Hoàn Thiện

### 1. **Hiển Thị Danh Sách Xe**
- ✅ Load dữ liệu tự động từ API khi vào trang
- ✅ Hiển thị thông tin đầy đủ: Hãng xe, Mẫu xe, Năm SX, VIN, Màu sắc, Giá bán, Trạng thái
- ✅ Format giá tiền theo định dạng VND
- ✅ Badge trạng thái (Còn hàng/Hết hàng)
- ✅ Preview màu sắc với color dot
- ✅ Loading state với spinner
- ✅ Error handling với thông báo thân thiện
- ✅ Empty state khi chưa có dữ liệu
- ✅ Responsive design cho mobile/tablet/desktop
- ✅ Dark mode support

### 2. **Thêm Xe Mới**
- ✅ Modal form với đầy đủ các trường thông tin
- ✅ Validation form đầy đủ:
  - Hãng xe (bắt buộc)
  - Mẫu xe (bắt buộc)
  - Năm sản xuất (1900 - năm hiện tại + 1)
  - Số VIN (bắt buộc, tối thiểu 17 ký tự)
  - Màu sắc (bắt buộc)
  - Giá bán (phải > 0)
  - Mô tả (tùy chọn)
  - Trạng thái còn hàng (checkbox)
- ✅ Hiển thị lỗi validation real-time
- ✅ Auto uppercase cho số VIN
- ✅ Loading state khi submit
- ✅ Toast notification khi thành công

### 3. **Cập Nhật Thông Tin Xe**
- ✅ Mở form với dữ liệu xe hiện tại
- ✅ Validation tương tự form thêm mới
- ✅ Update API call
- ✅ Refresh danh sách sau khi cập nhật
- ✅ Toast notification

### 4. **Xóa Xe**
- ✅ Modal xác nhận xóa
- ✅ Hiển thị thông tin xe cần xóa
- ✅ Warning icon
- ✅ Loading state khi đang xóa
- ✅ Toast notification khi thành công/thất bại

### 5. **Các Tính Năng Khác**
- ✅ Nút "Làm mới" để reload dữ liệu
- ✅ Đếm tổng số xe
- ✅ Toast notification với animation
- ✅ Auto dismiss notification sau 3 giây
- ✅ Đóng modal bằng ESC key
- ✅ Click outside để đóng modal
- ✅ Prevent body scroll khi modal mở

## 📂 Các File Đã Tạo/Cập Nhật

### 1. **Types & Services**
```
src/types/vehicle.ts          - Interface Vehicle & VehicleCreateUpdate
src/services/vehicleService.ts - API service với 5 methods
```

### 2. **Components**
```
src/components/ecommerce/VehicleFormModal.tsx     - Form thêm/sửa xe
src/components/ecommerce/ConfirmDeleteModal.tsx   - Modal xác nhận xóa
```

### 3. **Pages**
```
src/pages/Ecommerce/Products.tsx - Trang chính hiển thị danh sách xe
```

### 4. **Styles**
```
src/index.css - Thêm animation cho toast notification
```

## 🚀 Cách Sử Dụng

### Khởi Động Backend
```powershell
cd "d:\EVM\NEW EVM_WEB\NEW BE_FE\Dealer-Management-System-DMS-\BE\CompanyDealer\CompanyDealer"
dotnet run
```

### Khởi Động Frontend
```powershell
cd "d:\EVM\NEW EVM_WEB\NEW BE_FE\Dealer-Management-System-DMS-\FE\DMS dashboard"
npm run dev
```

### Truy Cập
1. Mở browser và truy cập trang Products
2. Xem danh sách xe hiện có
3. Nhấn "Thêm xe mới" để thêm xe
4. Nhấn "Sửa" để chỉnh sửa thông tin xe
5. Nhấn "Xóa" để xóa xe (có xác nhận)

## 🎨 UI/UX Highlights

### Form Modal
- **Layout**: 2 cột responsive cho các trường liên quan
- **Input Types**: Text, Number, TextArea
- **Icons**: Success/Error cho validation
- **Buttons**: Primary (Submit) & Secondary (Cancel)
- **States**: Normal, Disabled, Loading

### Confirm Delete Modal
- **Icon**: Warning icon màu đỏ
- **Message**: Hiển thị tên xe cần xóa
- **Buttons**: Danger (Xóa) & Secondary (Hủy)
- **Safety**: Yêu cầu xác nhận trước khi xóa

### Toast Notification
- **Position**: Top-right, fixed
- **Animation**: Slide in from right
- **Types**: Success (green) & Error (red)
- **Icons**: Checkmark & X
- **Auto dismiss**: 3 seconds

### Table
- **Columns**: 8 cột với thông tin đầy đủ
- **Row Actions**: Sửa & Xóa buttons
- **Hover Effects**: Highlight row khi hover
- **Responsive**: Horizontal scroll trên mobile

## 📝 Validation Rules

| Field | Rules |
|-------|-------|
| Hãng xe | Bắt buộc, không được rỗng |
| Mẫu xe | Bắt buộc, không được rỗng |
| Năm SX | Bắt buộc, 1900 - (năm hiện tại + 1) |
| VIN | Bắt buộc, tối thiểu 17 ký tự |
| Màu sắc | Bắt buộc, không được rỗng |
| Giá bán | Bắt buộc, phải > 0 |
| Mô tả | Tùy chọn |

## 🔌 API Endpoints

### GET `/api/Vehicles`
Lấy danh sách tất cả xe

### GET `/api/Vehicles/{id}`
Lấy chi tiết 1 xe theo ID

### POST `/api/Vehicles`
Tạo xe mới
**Body**: VehicleCreateUpdate

### PUT `/api/Vehicles/{id}`
Cập nhật thông tin xe
**Body**: VehicleCreateUpdate

### DELETE `/api/Vehicles/{id}`
Xóa xe theo ID

## 🎯 Todo (Tính năng có thể mở rộng)

- [ ] Tìm kiếm xe theo hãng/mẫu/VIN
- [ ] Lọc theo năm sản xuất, giá bán, trạng thái
- [ ] Sắp xếp theo các cột
- [ ] Phân trang (pagination)
- [ ] Export danh sách ra Excel/PDF
- [ ] Upload hình ảnh xe
- [ ] Xem chi tiết xe (modal/trang riêng)
- [ ] Bulk actions (xóa nhiều, cập nhật nhiều)
- [ ] Category management
- [ ] Import từ Excel

## 🐛 Troubleshooting

### Backend không kết nối được
- Kiểm tra backend đã chạy chưa (port 5232)
- Xem file `api.ts` có đúng URL không
- Check CORS settings trong backend

### Form không submit được
- Kiểm tra console để xem lỗi validation
- Đảm bảo tất cả field bắt buộc đã điền
- Check network tab để xem response từ API

### Không thấy dữ liệu
- Kiểm tra database có dữ liệu không
- Xem console có lỗi API không
- Check network tab để xem response

## 💡 Tips

1. **Số VIN**: Tự động uppercase khi nhập
2. **Giá bán**: Có step 1,000,000 để dễ nhập
3. **ESC key**: Đóng modal nhanh chóng
4. **Dark mode**: Tự động theo system preference
5. **Validation**: Real-time, hiển thị ngay khi nhập

---

**Developed with ❤️ for EVM Dealer Management System**
