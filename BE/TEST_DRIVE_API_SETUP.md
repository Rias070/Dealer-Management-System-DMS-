# 🚀 Test Drive API - Backend Setup Guide

## ✅ Đã hoàn thành:

### 1. **Models**
- ✅ Cập nhật `TestDriveRecord.cs` với enum `TestDriveStatus` và các trường workflow
  - Status, CreatedBy, CreatedByName, CreatedAt
  - ApprovedBy, ApprovedByName, ApprovedAt
  - RejectionReason, RejectedAt

### 2. **DTOs** (5 files)
- ✅ `CreateTestDriveRequest.cs` - Tạo lịch hẹn mới
- ✅ `UpdateTestDriveRequest.cs` - Cập nhật lịch hẹn
- ✅ `ApproveTestDriveRequest.cs` - Phê duyệt
- ✅ `RejectTestDriveRequest.cs` - Từ chối với lý do
- ✅ `TestDriveResponse.cs` - Response với đầy đủ thông tin

### 3. **Repository**
- ✅ `ITestDriveRepository.cs` - Interface
- ✅ `TestDriveRepository.cs` - Implementation với:
  - CRUD operations
  - Filter by dealer, vehicle, status, date range
  - Schedule conflict detection (2 hours window)

### 4. **Service**
- ✅ `ITestDriveService.cs` - Interface
- ✅ `TestDriveService.cs` - Business logic với:
  - Validation (future date, vehicle availability, dealer active)
  - Schedule conflict checking
  - Status workflow management
  - Approve/Reject logic

### 5. **Controller**
- ✅ `TestDriveController.cs` - API endpoints:
  - `GET /api/TestDrive` - Lấy tất cả (với filters)
  - `GET /api/TestDrive/{id}` - Chi tiết
  - `POST /api/TestDrive` - Tạo mới
  - `PUT /api/TestDrive/{id}` - Cập nhật
  - `DELETE /api/TestDrive/{id}` - Xóa
  - `POST /api/TestDrive/{id}/approve` - Phê duyệt
  - `POST /api/TestDrive/{id}/reject` - Từ chối
  - `GET /api/TestDrive/dealer/{dealerId}` - By dealer
  - `GET /api/TestDrive/vehicle/{vehicleId}` - By vehicle

### 6. **Dependency Injection**
- ✅ Đã đăng ký trong `Program.cs`:
  - TestDriveRepository
  - TestDriveService
  - VehicleRepository (đã tạo interface)

---

## 🔧 Cần thực hiện tiếp:

### **Bước 1: Tạo Migration**

Mở terminal trong thư mục solution và chạy:

```powershell
# Chuyển đến thư mục DAL
cd BE/CompanyDealer

# Tạo migration mới
dotnet ef migrations add AddTestDriveWorkflowFields --project CompanyDealer.DAL --startup-project CompanyDealer

# Áp dụng migration vào database
dotnet ef database update --project CompanyDealer.DAL --startup-project CompanyDealer
```

**Migration này sẽ thêm các cột:**
- Status (enum → int trong DB)
- CreatedBy (Guid?)
- CreatedByName (string)
- CreatedAt (DateTime)
- ApprovedBy (Guid?)
- ApprovedByName (string)
- ApprovedAt (DateTime?)
- RejectionReason (string)
- RejectedAt (DateTime?)

---

### **Bước 2: Kiểm tra Program.cs**

Đảm bảo các dòng sau đã có:

```csharp
// Trong phần DI
builder.Services.AddScoped<CompanyDealer.DAL.Repository.TestDriveRepo.ITestDriveRepository, 
    CompanyDealer.DAL.Repository.TestDriveRepo.TestDriveRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.VehicleRepo.IVehicleRepository, 
    CompanyDealer.DAL.Repository.VehicleRepo.VehicleRepository>();
builder.Services.AddScoped<CompanyDealer.BLL.Services.ITestDriveService, 
    CompanyDealer.BLL.Services.TestDriveService>();
```

---

### **Bước 3: Build & Run**

```powershell
# Build project
dotnet build

# Run
dotnet run --project CompanyDealer
```

Backend sẽ chạy tại: `http://localhost:5000` hoặc `https://localhost:5001`

---

### **Bước 4: Test API với Swagger**

1. Mở browser: `http://localhost:5000/swagger`
2. Test các endpoints:

#### **Test Create:**
```json
POST /api/TestDrive
{
  "testDate": "2025-10-25T09:00:00Z",
  "customerName": "Nguyễn Văn A",
  "customerContact": "0912345678",
  "notes": "Khách muốn lái thử buổi sáng",
  "dealerId": "guid-của-dealer",
  "vehicleId": "guid-của-vehicle"
}
```

#### **Test Approve:**
```json
POST /api/TestDrive/{id}/approve
{
  "approvedBy": "guid-của-manager"
}
```

#### **Test Reject:**
```json
POST /api/TestDrive/{id}/reject
{
  "rejectedBy": "guid-của-manager",
  "rejectionReason": "Xe không có sẵn vào thời gian này. Vui lòng chọn thời gian khác."
}
```

---

### **Bước 5: Cập nhật Frontend**

Trong file `testDriveService.ts`:

```typescript
// Dòng 150
private useMockData = false; // ← Đổi thành false
```

---

## 🔐 Authorization Rules:

| Endpoint | Roles | Mô tả |
|----------|-------|-------|
| GET All/ById | All authenticated | Xem danh sách |
| POST Create | DealerStaff, DealerAdmin | Tạo mới |
| PUT Update | DealerStaff, DealerAdmin | Sửa |
| DELETE | DealerStaff, DealerAdmin | Xóa |
| POST Approve | **DealerAdmin only** | Phê duyệt |
| POST Reject | **DealerAdmin only** | Từ chối |

---

## 🎯 Business Rules Implemented:

1. ✅ **Test date validation** - Phải là ngày trong tương lai
2. ✅ **Vehicle availability check** - Xe phải có sẵn (IsAvailable = true)
3. ✅ **Dealer active check** - Đại lý phải đang hoạt động
4. ✅ **Schedule conflict detection** - Không trùng lịch trong vòng 2 giờ
5. ✅ **Status workflow** - Chỉ Pending mới được approve/reject
6. ✅ **Rejected → Pending** - Khi update lịch bị từ chối, status reset về Pending
7. ✅ **Auto timestamps** - CreatedAt, ApprovedAt, RejectedAt tự động

---

## 📊 Response Format:

```json
{
  "success": true,
  "message": "Test drive created successfully",
  "data": {
    "id": "guid",
    "testDate": "2025-10-25T09:00:00Z",
    "customerName": "Nguyễn Văn A",
    "customerContact": "0912345678",
    "notes": "...",
    "status": "Pending",
    "createdBy": "guid",
    "createdByName": "Nhân viên Nguyễn Thị C",
    "createdAt": "2025-10-21T10:30:00Z",
    "approvedBy": null,
    "approvedByName": "",
    "approvedAt": null,
    "rejectionReason": "",
    "rejectedAt": null,
    "dealerId": "guid",
    "dealer": {
      "id": "guid",
      "name": "Đại lý Hà Nội",
      "location": "Hà Nội"
    },
    "vehicleId": "guid",
    "vehicle": {
      "id": "guid",
      "make": "VinFast",
      "model": "VF e34",
      "year": 2024,
      "color": "Đỏ",
      "vin": "VIN001"
    }
  }
}
```

---

## 🐛 Troubleshooting:

### **Lỗi: "Vehicle not found"**
- Kiểm tra VehicleId có tồn tại trong DB không
- Seed data vehicles nếu chưa có

### **Lỗi: "Schedule conflict"**
- Đổi thời gian khác (cách ít nhất 2 giờ)
- Hoặc chọn xe khác

### **Lỗi: "Only pending test drives can be approved"**
- Kiểm tra status hiện tại
- Chỉ status = Pending mới approve/reject được

### **Lỗi 401 Unauthorized**
- Đảm bảo đã login và có JWT token
- Token phải có role DealerStaff hoặc DealerAdmin

---

## ✅ Checklist:

- [x] Model với enum status
- [x] DTOs cho tất cả operations
- [x] Repository với conflict detection
- [x] Service với business logic
- [x] Controller với authorization
- [x] DI registration
- [x] IVehicleRepository interface
- [ ] **Migration (cần chạy)**
- [ ] **Test API với Swagger**
- [ ] **Kết nối Frontend**
- [ ] **Test end-to-end**

---

## 🎉 Kết luận:

Backend API cho Test Drive đã **HOÀN THÀNH 100%** về code!

Chỉ cần:
1. Chạy migration
2. Test với Swagger
3. Kết nối Frontend

**Happy Coding!** 🚀
