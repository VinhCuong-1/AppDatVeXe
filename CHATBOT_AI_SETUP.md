# 🤖 Hướng dẫn Setup Chatbot AI với Gemini API

## 📋 Tổng quan

Chatbot AI được tích hợp vào ứng dụng Đặt Vé Xe để hỗ trợ người dùng trả lời các câu hỏi thường gặp (FAQ) một cách tự động và thông minh.

### ✨ Tính năng:

- **6 danh mục câu hỏi chính**: Đặt vé, Hủy vé, Check-in, Tài khoản, Chuyến xe, App
- **Giao diện thân thiện**: Chat bubbles, categories buttons, quick questions
- **Trả lời thông minh**: Sử dụng Gemini AI để trả lời câu hỏi tùy chỉnh
- **Truy cập nhanh**: Floating Action Button trên màn hình tìm kiếm
- **Hoàn toàn miễn phí**: Gemini API Free tier - 60 requests/phút

---

## 🔑 Bước 1: Lấy Gemini API Key

### 1. Truy cập Google AI Studio:

👉 https://makersuite.google.com/app/apikey

### 2. Đăng nhập bằng tài khoản Google

### 3. Click "Create API Key"

### 4. Chọn project hoặc tạo mới

### 5. Copy API Key (dạng: `AIzaSy...`)

---

## ⚙️ Bước 2: Cấu hình API Key

### Mở file: `lib/services/gemini_service.dart`

Tìm dòng:

```dart
static const String _apiKey = 'YOUR_GEMINI_API_KEY_HERE';
```

Thay thế bằng API Key của bạn:

```dart
static const String _apiKey = 'AIzaSyXXXXXXXXXXXXXXXXXXXXXXXXXX';
```

⚠️ **LƯU Ý**:

- Không commit API key lên Git/GitHub (thêm vào `.gitignore`)
- Trong production, nên dùng environment variables hoặc backend proxy

---

## 📦 Bước 3: Cài đặt Dependencies

Chạy lệnh:

```bash
flutter pub get
```

Package đã được thêm vào `pubspec.yaml`:

```yaml
dependencies:
  google_generative_ai: ^0.4.3
```

---

## 🚀 Bước 4: Chạy ứng dụng

```bash
flutter run
```

---

## 🎯 Cách sử dụng Chatbot

### Cách 1: Từ màn hình Tài khoản

1. Vào tab **Tài khoản** (Profile)
2. Chọn **Trợ lý AI 🤖**

### Cách 2: Từ màn hình Tìm chuyến

1. Ở màn hình **Tìm chuyến**
2. Bấm vào nút **Floating Action Button** (Trợ lý AI)

### Cách 3: Chat với AI

1. **Chọn danh mục** (6 chủ đề có sẵn)
2. **Chọn câu hỏi nhanh** hoặc **Nhập câu hỏi tùy chỉnh**
3. AI sẽ trả lời dựa trên FAQ hoặc kiến thức chung

---

## 📊 Cấu trúc Files

```
lib/
├── models/
│   └── faq_data.dart           # Dữ liệu FAQ (6 categories, 24 questions)
├── services/
│   └── gemini_service.dart     # Gemini AI service
└── screens/
    └── chatbot/
        └── chatbot_screen.dart # UI chatbot
```

---

## 🎨 Customization

### Thêm câu hỏi FAQ mới:

**File**: `lib/models/faq_data.dart`

```dart
FAQCategory(
  id: 'new_category',
  title: 'Danh mục mới',
  emoji: '🎉',
  items: [
    FAQItem(
      question: 'Câu hỏi mới?',
      answer: 'Câu trả lời chi tiết...',
    ),
  ],
),
```

### Thay đổi model AI:

**File**: `lib/services/gemini_service.dart`

```dart
model: 'gemini-1.5-pro',  // Model mạnh hơn (có thể tốn phí)
temperature: 0.7,          // Độ sáng tạo (0.0 - 1.0)
maxOutputTokens: 1024,     // Độ dài câu trả lời
```

---

## 🔒 Bảo mật API Key

### Development (Tạm thời):

```dart
// gemini_service.dart
static const String _apiKey = 'AIza...'; // OK for testing
```

### Production (Khuyến nghị):

#### Option 1: Environment Variables

```dart
import 'package:flutter_dotenv/flutter_dotenv.dart';

static String get _apiKey => dotenv.env['GEMINI_API_KEY'] ?? '';
```

#### Option 2: Backend Proxy (Tốt nhất)

```dart
// Gọi API qua backend của bạn
static Future<String> askQuestion(String question) async {
  final response = await http.post(
    Uri.parse('https://your-backend.com/api/chatbot'),
    body: {'question': question},
  );
  return response.body;
}
```

---

## 🧪 Testing

### Test với các câu hỏi:

✅ **Có trong FAQ**:

- "Làm sao để đặt vé xe?"
- "Hủy vé có mất phí không?"
- "Check-in là gì?"

✅ **Không có trong FAQ** (AI sẽ tự trả lời):

- "Xe có điểm dừng nghỉ giữa đường không?"
- "Tôi có thể mang thú cưng lên xe không?"

✅ **Ngoài phạm vi** (AI sẽ gợi ý hotline):

- "Thời tiết hôm nay thế nào?"
- "Cách nấu phở?"

---

## 📈 Giới hạn API (Free Tier)

| Metric          | Limit          |
| --------------- | -------------- |
| Requests/phút   | 60             |
| Requests/ngày   | 1,500          |
| Tokens/request  | 32,000 (input) |
| Tokens/response | 8,192 (output) |

👉 **Chi tiết**: https://ai.google.dev/pricing

### Khi vượt quota:

```dart
// Error handling có sẵn trong gemini_service.dart
catch (e) {
  return 'Đã có lỗi xảy ra: ${e.toString()}. Vui lòng thử lại sau.';
}
```

---

## 🆘 Troubleshooting

### Lỗi: "API key not valid"

- Kiểm tra API key đã đúng chưa
- Kiểm tra đã enable Gemini API chưa
- Thử tạo lại API key mới

### Lỗi: "Resource has been exhausted"

- Đã vượt quota 60 requests/phút
- Chờ 1 phút rồi thử lại
- Xem xét upgrade lên paid plan

### Lỗi: "Network error"

- Kiểm tra kết nối internet
- Kiểm tra firewall/VPN
- Thử lại sau vài giây

### Chatbot không hiển thị câu trả lời:

- Mở DevTools → Console để xem log
- Kiểm tra API key đã setup đúng
- Kiểm tra kết nối mạng

---

## 📞 Liên hệ hỗ trợ

- **Hotline**: 1900 1199
- **Email**: support@datvexe.com
- **Gemini API Docs**: https://ai.google.dev/docs

---

## 🎉 Hoàn tất!

Bây giờ bạn đã có một Chatbot AI thông minh để hỗ trợ khách hàng 24/7! 🚀

### Các bước tiếp theo:

1. ✅ Setup API Key
2. ✅ Test chatbot
3. ✅ Thu thập feedback từ người dùng
4. 🔄 Cập nhật FAQ dựa trên câu hỏi thực tế
5. 🚀 Deploy lên production
