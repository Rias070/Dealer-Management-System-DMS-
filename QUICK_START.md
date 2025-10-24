# 🚀 Hướng dẫn Khởi động và Kiểm tra DMS System

## 📋 Mục lục
1. [Kiểm tra kết nối](#kiểm-tra-kết-nối)
2. [Khởi động hệ thống](#khởi-động-hệ-thống)
3. [Tài khoản test](#tài-khoản-test)
4. [Troubleshooting](#troubleshooting)

---

## ✅ Kiểm tra kết nối

### Cách 1: Script tự động (Khuyến nghị)
```powershell
.\test-connection.ps1
```

### Cách 2: Kiểm tra thủ công

#### 1. Kiểm tra PostgreSQL
- Mở **pgAdmin 4**
- Kết nối vào server `localhost:5432`
- Kiểm tra database `CompanyDealerDb` tồn tại

#### 2. Kiểm tra Backend
```powershell
cd "BE\CompanyDealer\CompanyDealer"
dotnet run
```
Mở: http://localhost:5232/swagger

#### 3. Kiểm tra Frontend
```powershell
cd "FE\DMS dashboard"
npm run dev
```
Mở: http://localhost:5173

---

## 🚀 Khởi động hệ thống

### Cách 1: Script tự động (Khuyến nghị) ⭐
```powershell
.\start-system.ps1
```
Script này sẽ:
- Mở 2 cửa sổ PowerShell riêng biệt
- Khởi động Backend trong window 1
- Khởi động Frontend trong window 2
- Tự động mở browser

### Cách 2: Khởi động thủ công

#### Terminal 1 - Backend:
```powershell
cd "BE\CompanyDealer\CompanyDealer"
dotnet run
```

#### Terminal 2 - Frontend:
```powershell
cd "FE\DMS dashboard"
npm run dev
```

---

## 🔐 Tài khoản Test

Xem chi tiết trong: `FE/DMS dashboard/TEST_ACCOUNTS.md`

**Ví dụ tài khoản:**

| Role | Username | Password |
|------|----------|----------|
| Admin | admin | Admin@123 |
| Dealer Manager | dealer1 | Dealer@123 |
| Dealer Staff | staff1 | Staff@123 |
| EVM Staff | evm1 | Evm@123 |

---

## 🌐 URLs

| Service | URL | Mô tả |
|---------|-----|-------|
| Frontend | http://localhost:5173 | Giao diện chính |
| Backend API | http://localhost:5232 | API Server |
| Swagger UI | http://localhost:5232/swagger | API Documentation |
| pgAdmin | http://localhost (hoặc app) | Database Management |

---

## 🔧 Troubleshooting

### Lỗi: Port đã được sử dụng

**Backend (Port 5232):**
```powershell
# Tìm process
netstat -ano | findstr :5232

# Kill process (thay <PID>)
taskkill /PID <PID> /F
```

**Frontend (Port 5173):**
```powershell
# Tìm process
netstat -ano | findstr :5173

# Kill process
taskkill /PID <PID> /F
```

### Lỗi: Database connection failed

1. Kiểm tra PostgreSQL service đang chạy:
   - Win+R → `services.msc`
   - Tìm "postgresql"
   - Đảm bảo Status = "Running"

2. Kiểm tra connection string trong `appsettings.json`:
   ```json
   "DefaultConnection": "Host=localhost;Database=CompanyDealerDb;Username=postgres;Password=12345;Port=5432"
   ```

3. Test connection trong pgAdmin

### Lỗi: CORS Error

Đảm bảo:
- Backend đang chạy trước
- Frontend proxy đúng cấu hình trong `vite.config.ts`
- CORS policy trong Backend `Program.cs` cho phép port của Frontend

### Lỗi: npm dependencies

```powershell
cd "FE\DMS dashboard"
rm -r node_modules
rm package-lock.json
npm install
```

### Lỗi: Migration fails

```powershell
cd "BE\CompanyDealer\CompanyDealer"

# Xem migrations
dotnet ef migrations list

# Xóa database và tạo lại
dotnet ef database drop
dotnet ef database update
```

---

## 📊 Cấu trúc Dự án

```
DMS/
├── BE/
│   └── CompanyDealer/
│       ├── CompanyDealer/          # API Layer (Controllers)
│       ├── CompanyDealer.BLL/      # Business Logic (Services, DTOs)
│       └── CompanyDealer.DAL/      # Data Access (Models, Repository)
├── FE/
│   └── DMS dashboard/              # React + Vite + TypeScript
├── test-connection.ps1             # Script kiểm tra kết nối
├── start-system.ps1                # Script khởi động hệ thống
└── CONNECTION_TEST_REPORT.md       # Báo cáo kiểm tra chi tiết
```

---

## 📚 Tài liệu liên quan

- `CONNECTION_TEST_REPORT.md` - Báo cáo kiểm tra kết nối chi tiết
- `FE/DMS dashboard/TEST_ACCOUNTS.md` - Danh sách tài khoản test
- `FE/DMS dashboard/ROLE_USAGE_GUIDE.md` - Hướng dẫn sử dụng theo vai trò
- `FE/DMS dashboard/TEST_DRIVE_GUIDE.md` - Hướng dẫn tính năng lái thử
- `FE/DMS dashboard/PERMISSION_FIXES.md` - Sửa lỗi phân quyền

---

## 🎯 Workflow phát triển

1. **Khởi động hệ thống:**
   ```powershell
   .\start-system.ps1
   ```

2. **Làm việc với Frontend:**
   - Code trong `FE/DMS dashboard/src/`
   - Hot reload tự động
   - Check console và Network tab

3. **Làm việc với Backend:**
   - Code trong `BE/CompanyDealer/`
   - Restart backend khi thay đổi code
   - Test API trong Swagger

4. **Làm việc với Database:**
   - Thay đổi Models trong `CompanyDealer.DAL/Models/`
   - Tạo migration:
     ```powershell
     cd "BE\CompanyDealer\CompanyDealer"
     dotnet ef migrations add <TenMigration>
     dotnet ef database update
     ```

5. **Kiểm tra kết nối:**
   ```powershell
   .\test-connection.ps1
   ```

---

## ✨ Features chính

### Phía Đại lý (Dealer Staff/Manager):
- ✅ Xem danh mục xe
- ✅ Quản lý bán hàng (đơn hàng, báo giá)
- ✅ Quản lý khách hàng
- ✅ Quản lý lịch lái thử
- ✅ Báo cáo doanh số

### Phía Hãng xe (EVM Staff/Admin):
- ✅ Quản lý sản phẩm & tồn kho
- ✅ Quản lý đại lý & hợp đồng
- ✅ Phân phối xe cho đại lý
- ✅ Báo cáo & phân tích

---

**Happy Coding! 🎉**
