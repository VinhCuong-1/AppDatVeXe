# 🚀 Quick Start - Chatbot AI

## 🎯 Cách test NGAY (không cần API key)

### Option 1: Demo Mode (Offline - Không cần internet)

Mở file `lib/screens/chatbot/chatbot_screen.dart` và thay đổi dòng import:

**Từ:**

```dart
import '../../services/gemini_service.dart';
```

**Thành:**

```dart
import '../../services/gemini_service_demo.dart';
```

**Và thay:**

```dart
final answer = await GeminiService.askCustomQuestion(question);
```

**Thành:**

```dart
final answer = await GeminiServiceDemo.askCustomQuestion(question);
```

✅ Chạy app → Vào Chatbot → Test ngay!

---

### Option 2: Gemini API (Online - Cần API key)

#### 1️⃣ Lấy API Key (30 giây)

👉 https://makersuite.google.com/app/apikey

- Đăng nhập Google
- Click "Create API Key"
- Copy key

#### 2️⃣ Setup API Key

Mở `lib/services/gemini_service.dart`, dòng 7:

```dart
static const String _apiKey = 'PASTE_API_KEY_VÀO_ĐÂY';
```

#### 3️⃣ Chạy app

```bash
flutter run
```

✅ Done! Chatbot AI đã sẵn sàng! 🎉

---

## 📱 Cách sử dụng trong App

### Truy cập Chatbot (3 cách):

#### 1. Từ màn hình Tìm chuyến:

- Bấm nút **Floating Button "Trợ lý AI"** (góc dưới bên phải)

#### 2. Từ màn hình Tài khoản:

- Tab **Tài khoản** → **Trợ lý AI 🤖**

#### 3. Direct Navigation:

```dart
Navigator.push(
  context,
  MaterialPageRoute(builder: (context) => const ChatbotScreen()),
);
```

---

## 💬 Test với các câu hỏi

### ✅ Câu hỏi có sẵn (FAQ):

1. **Đặt vé & Tìm kiếm** 🎫

   - "Làm sao để đặt vé xe?"
   - "Có chuyến nào từ Hà Nội đi TP.HCM không?"
   - "Tôi có thể đặt vé cho nhiều người không?"

2. **Hủy vé & Đổi vé** 🔄

   - "Tôi có thể hủy vé không?"
   - "Hủy vé có mất phí không?"

3. **Check-in & Lên xe** ✅

   - "Check-in là gì?"
   - "Phải check-in khi nào?"

4. **Tài khoản & Bảo mật** 🔐

   - "Làm sao để đăng ký tài khoản?"
   - "Quên mật khẩu thì làm sao?"

5. **Chuyến xe & Lịch trình** 🚌

   - "Xe chạy mấy giờ?"
   - "Xe có wifi không?"

6. **App & Tính năng** 📱
   - "App này miễn phí không?"
   - "Tôi có thể xem lại vé đã đặt không?"

### 🤖 Câu hỏi tùy chỉnh (với Gemini AI):

- "Xe có ghế massage không?"
- "Tôi có thể thanh toán bằng thẻ visa không?"
- "Xe có dừng nghỉ giữa đường không?"

---

## 🎨 UI Features

### 1. Categories Selector

- 6 nút danh mục với emoji
- Click vào để xem câu hỏi trong danh mục

### 2. Quick Questions

- Các câu hỏi phổ biến hiển thị dạng chips
- Click để xem ngay câu trả lời

### 3. Custom Input

- Nhập câu hỏi bất kỳ
- AI sẽ trả lời dựa trên context

### 4. Chat Interface

- Bubbles cho user (bên phải, màu xanh)
- Bubbles cho bot (bên trái, màu xám)
- Avatar để phân biệt

### 5. Navigation

- Nút "Quay lại" để về trang chủ
- Nút "Bắt đầu lại" để reset chat

---

## 🔧 Customization nhanh

### Thay đổi màu sắc:

**File**: `lib/screens/chatbot/chatbot_screen.dart`

```dart
// Bot bubble color
color: Colors.grey.shade200,  // Đổi màu bot

// User bubble color
color: Colors.blue.shade700,  // Đổi màu user

// FAB color
backgroundColor: Colors.blue.shade700,  // Đổi màu nút floating
```

### Thêm emoji vào câu trả lời:

**File**: `lib/models/faq_data.dart`

```dart
answer: '✅ Bạn chọn tab "Tìm chuyến"... 🚌',
```

---

## 📊 Giám sát Performance

### Trong Developer Console:

```dart
// gemini_service.dart đã có error handling
try {
  final response = await model.generateContent(content);
  print('✅ AI Response: ${response.text}');
} catch (e) {
  print('❌ Error: $e');
}
```

### Metrics cần theo dõi:

- Response time (thường < 2s)
- Error rate (nên < 1%)
- API quota usage (60 req/min)

---

## 🆘 Common Issues

### 1. "API key not valid"

**Fix**: Kiểm tra lại API key trong `gemini_service.dart`

### 2. Chatbot không hiển thị

**Fix**:

```dart
// Kiểm tra import
import '../chatbot/chatbot_screen.dart'; // ✅
import 'chatbot/chatbot_screen.dart';   // ❌ (nếu không đúng path)
```

### 3. Categories không hiển thị

**Fix**: Scroll xuống dưới, categories ở phía dưới chat messages

### 4. Lỗi network

**Fix**:

- Kiểm tra internet
- Dùng Demo Mode để test offline

---

## 🎯 Next Steps

1. ✅ Test chatbot với 6 danh mục FAQ
2. ✅ Test với câu hỏi tùy chỉnh
3. 📝 Thu thập feedback từ người dùng
4. 🔄 Cập nhật thêm FAQ dựa trên câu hỏi thực tế
5. 🚀 Deploy lên production

---

## 📞 Support

Cần hỗ trợ? Đọc file **CHATBOT_AI_SETUP.md** để biết thêm chi tiết!

**Hotline**: 1900 1199
