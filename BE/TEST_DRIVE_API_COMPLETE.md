# ✅ Test Drive API - HOÀN THÀNH

## 🎯 Tóm tắt

Backend API cho Test Drive đã được hoàn thành 100% với đầy đủ chức năng theo yêu cầu.

---

## 📦 Các thành phần đã tạo

### 1. **Model** ✅
- `TestDriveRecord.cs` - Đã cập nhật với đầy đủ trường:
  - `Status` (enum: Pending, Approved, Rejected, Completed, Cancelled)
  - `CreatedBy`, `CreatedByName`, `CreatedAt`
  - `ApprovedBy`, `ApprovedByName`, `ApprovedAt`
  - `RejectionReason`, `RejectedAt`

### 2. **DTOs** ✅
- `CreateTestDriveRequest.cs` - Tạo lịch hẹn mới
- `UpdateTestDriveRequest.cs` - Cập nhật lịch hẹn
- `TestDriveResponse.cs` - Response với đầy đủ thông tin
- `ApproveTestDriveRequest.cs` - Phê duyệt
- `RejectTestDriveRequest.cs` - Từ chối với lý do

### 3. **Repository** ✅
- `ITestDriveRepository.cs` - Interface
- `TestDriveRepository.cs` - Implementation với:
  - GetAllAsync (có filters)
  - GetByIdAsync
  - CreateAsync
  - UpdateAsync
  - DeleteAsync

### 4. **Service** ✅
- `ITestDriveService.cs` - Interface
- `TestDriveService.cs` - Business logic với:
  - GetAllAsync (filters: dealerId, vehicleId, status, date range)
  - GetByIdAsync
  - CreateAsync (auto set CreatedBy, CreatedAt, Status=Pending)
  - UpdateAsync
  - DeleteAsync
  - ApproveAsync (Manager only)
  - RejectAsync (Manager only, với rejection reason)

### 5. **Controller** ✅
- `TestDriveController.cs` với các endpoints:
  - `GET /api/TestDrive` - Lấy danh sách (có filters)
  - `GET /api/TestDrive/{id}` - Lấy chi tiết
  - `POST /api/TestDrive` - Tạo mới [DealerStaff, DealerAdmin]
  - `PUT /api/TestDrive/{id}` - Cập nhật [DealerStaff, DealerAdmin]
  - `DELETE /api/TestDrive/{id}` - Xóa [DealerStaff, DealerAdmin]
  - `POST /api/TestDrive/{id}/approve` - Phê duyệt [DealerAdmin]
  - `POST /api/TestDrive/{id}/reject` - Từ chối [DealerAdmin]
  - `GET /api/TestDrive/dealer/{dealerId}` - Theo đại lý
  - `GET /api/TestDrive/vehicle/{vehicleId}` - Theo xe

### 6. **Database Migration** ✅
- Migration `UpdateTestDriveRecordFields` đã được apply thành công
- Các cột mới đã được tạo trong PostgreSQL

---

## 🔐 Authentication & Authorization

### Roles được hỗ trợ:
- **DealerStaff**: Tạo, sửa (Pending/Rejected), xóa (Pending)
- **DealerAdmin**: Toàn quyền + Approve/Reject

### JWT Token Required:
- Tất cả endpoints yêu cầu `Authorization: Bearer <token>`
- Token lấy từ `/api/Auth/login`

---

## 🧪 Test API với Postman/Thunder Client

### Base URL
```
http://localhost:5232/api
```

### 1. Login (Lấy Token)
```http
POST /api/Auth/login
Content-Type: application/json

{
  "userName": "dealerstaff1",
  "password": "password123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successfully",
  "token": "eyJhbGc...",
  "refreshToken": "abc123..."
}
```

---

### 2. Lấy danh sách Test Drives
```http
GET /api/TestDrive
Authorization: Bearer <your_token>
```

**Filters (optional):**
```http
GET /api/TestDrive?dealerId=<guid>&vehicleId=<guid>&status=Pending&fromDate=2025-10-01&toDate=2025-10-31
```

**Response:**
```json
{
  "success": true,
  "message": "Test drives retrieved successfully",
  "data": [
    {
      "id": "guid",
      "testDate": "2025-10-25T10:00:00Z",
      "customerName": "Nguyễn Văn A",
      "customerContact": "0912345678",
      "notes": "Khách muốn thử vào sáng",
      "dealerId": "guid",
      "dealerName": "Đại lý Hà Nội",
      "dealerLocation": "Hà Nội",
      "vehicleId": "guid",
      "vehicleMake": "VinFast",
      "vehicleModel": "VF e34",
      "vehicleYear": 2024,
      "vehicleColor": "Đỏ",
      "status": "Pending",
      "createdBy": "guid",
      "createdByName": "Nhân viên A",
      "createdAt": "2025-10-20T08:00:00Z"
    }
  ]
}
```

---

### 3. Lấy Test Drive theo ID
```http
GET /api/TestDrive/{id}
Authorization: Bearer <your_token>
```

---

### 4. Tạo Test Drive mới
```http
POST /api/TestDrive
Authorization: Bearer <your_token>
Content-Type: application/json

{
  "testDate": "2025-10-25T10:00:00Z",
  "customerName": "Trần Thị B",
  "customerContact": "0987654321",
  "notes": "Khách hàng quan tâm đến tính năng tự lái",
  "dealerId": "guid-of-dealer",
  "vehicleId": "guid-of-vehicle"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Test drive created successfully",
  "data": {
    "id": "new-guid",
    "status": "Pending",
    "createdBy": "current-user-id",
    "createdByName": "Current User Name",
    "createdAt": "2025-10-22T...",
    ...
  }
}
```

---

### 5. Cập nhật Test Drive
```http
PUT /api/TestDrive/{id}
Authorization: Bearer <your_token>
Content-Type: application/json

{
  "testDate": "2025-10-26T14:00:00Z",
  "customerName": "Trần Thị B",
  "customerContact": "0987654321",
  "notes": "Đã thay đổi thời gian",
  "dealerId": "guid-of-dealer",
  "vehicleId": "guid-of-vehicle"
}
```

---

### 6. Xóa Test Drive
```http
DELETE /api/TestDrive/{id}
Authorization: Bearer <your_token>
```

---

### 7. Phê duyệt Test Drive (Manager only)
```http
POST /api/TestDrive/{id}/approve
Authorization: Bearer <your_token>
Content-Type: application/json

{
  "approvedBy": "manager-user-id"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Test drive approved successfully",
  "data": {
    "id": "guid",
    "status": "Approved",
    "approvedBy": "manager-id",
    "approvedByName": "Manager Name",
    "approvedAt": "2025-10-22T...",
    ...
  }
}
```

---

### 8. Từ chối Test Drive (Manager only)
```http
POST /api/TestDrive/{id}/reject
Authorization: Bearer <your_token>
Content-Type: application/json

{
  "rejectedBy": "manager-user-id",
  "rejectionReason": "Xe không có sẵn vào thời gian này. Vui lòng chọn thời gian khác."
}
```

**Response:**
```json
{
  "success": true,
  "message": "Test drive rejected successfully",
  "data": {
    "id": "guid",
    "status": "Rejected",
    "approvedBy": "manager-id",
    "approvedByName": "Manager Name",
    "rejectionReason": "Xe không có sẵn...",
    "rejectedAt": "2025-10-22T...",
    ...
  }
}
```

---

### 9. Lấy Test Drives theo Dealer
```http
GET /api/TestDrive/dealer/{dealerId}
Authorization: Bearer <your_token>
```

---

### 10. Lấy Test Drives theo Vehicle
```http
GET /api/TestDrive/vehicle/{vehicleId}
Authorization: Bearer <your_token>
```

---

## 📊 Status Enum

```csharp
public enum TestDriveStatus
{
    Pending = 0,        // Chờ phê duyệt
    Approved = 1,       // Đã phê duyệt
    Rejected = 2,       // Đã từ chối
    Completed = 3,      // Đã hoàn thành
    Cancelled = 4       // Đã hủy
}
```

---

## ✅ Validation Rules

### CreateTestDriveRequest:
- `TestDate`: Required, phải là ngày trong tương lai
- `CustomerName`: Required, max 200 characters
- `CustomerContact`: Required, max 20 characters
- `DealerId`: Required, phải tồn tại trong DB
- `VehicleId`: Required, phải tồn tại trong DB

### UpdateTestDriveRequest:
- Tương tự CreateTestDriveRequest
- Chỉ cho phép update nếu:
  - DealerStaff: Status = Pending hoặc Rejected
  - DealerAdmin: Mọi status

### ApproveTestDriveRequest:
- Chỉ approve được nếu Status = Pending
- Chỉ DealerAdmin mới có quyền

### RejectTestDriveRequest:
- `RejectionReason`: Required, max 500 characters
- Chỉ reject được nếu Status = Pending
- Chỉ DealerAdmin mới có quyền

---

## 🔧 Các tính năng đặc biệt

### 1. Auto-populate relations
- Khi query, tự động include `Dealer` và `Vehicle` information
- Response trả về đầy đủ thông tin không cần query thêm

### 2. Soft tracking
- `CreatedBy`, `CreatedByName` tự động set khi tạo
- `ApprovedBy`, `ApprovedByName`, `ApprovedAt` tự động set khi approve
- `RejectedAt` tự động set khi reject

### 3. Business logic validation
- Không cho phép approve/reject nếu không phải Pending
- Không cho phép update/delete sau khi approved (trừ Admin)
- Validate tồn tại của Dealer và Vehicle

---

## 🚀 Trạng thái hiện tại

### ✅ Đã hoàn thành:
- [x] Model với đầy đủ trường
- [x] DTOs cho tất cả operations
- [x] Repository pattern
- [x] Service layer với business logic
- [x] Controller với authorization
- [x] Database migration applied
- [x] Build successful
- [x] Server running on http://localhost:5232

### 🔄 Kết nối Frontend:
1. Cập nhật `testDriveService.ts`:
   ```typescript
   private useMockData = false; // Chuyển sang false để dùng API thật
   ```

2. Cập nhật `api.ts` nếu cần:
   ```typescript
   export const API_BASE_URL = 'http://localhost:5232/api';
   ```

3. Test từ Frontend để xác nhận kết nối

---

## 📝 Notes

- **CORS**: Đã được cấu hình trong `Program.cs` để cho phép frontend connect
- **Swagger**: Có thể truy cập tại `http://localhost:5232/swagger` để xem API documentation
- **Database**: PostgreSQL trên localhost:5432, database `CompanyDealerDb`
- **JWT**: Token có thời gian sống 15 phút, refresh token 7 ngày

---

## 🎉 Kết luận

Backend API cho Test Drive đã hoàn thành 100% với:
- ✅ 10 Endpoints đầy đủ
- ✅ Role-based authorization
- ✅ Validation đầy đủ
- ✅ Business logic hoàn chỉnh
- ✅ Database migration thành công
- ✅ Server đang chạy

**Sẵn sàng kết nối với Frontend!** 🚀
