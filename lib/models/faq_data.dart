class FAQCategory {
  final String id;
  final String title;
  final String emoji;
  final List<FAQItem> items;

  FAQCategory({
    required this.id,
    required this.title,
    required this.emoji,
    required this.items,
  });
}

class FAQItem {
  final String question;
  final String answer;

  FAQItem({
    required this.question,
    required this.answer,
  });
}

class FAQData {
  static final List<FAQCategory> categories = [
    FAQCategory(
      id: 'booking',
      title: 'Đặt vé & Tìm kiếm',
      emoji: '🎫',
      items: [
        FAQItem(
          question: 'Làm sao để đặt vé xe?',
          answer:
              "Bạn chọn tab 'Tìm chuyến' → Nhập điểm đi/điểm đến → Chọn ngày → Chọn chuyến xe → Chọn ghế → Xác nhận đặt vé. Rất đơn giản!",
        ),
        FAQItem(
          question: 'Có chuyến nào từ Hà Nội đi TP.HCM không?',
          answer:
              "Có nhiều chuyến mỗi ngày! Bạn vào 'Tìm chuyến', chọn 'Hà Nội - TP.HCM' và chọn ngày muốn đi nhé.",
        ),
        FAQItem(
          question: 'Tôi có thể đặt vé cho nhiều người không?',
          answer:
              'Bạn cần đặt từng vé riêng cho mỗi hành khách. Mỗi vé tương ứng với 1 ghế ngồi.',
        ),
        FAQItem(
          question: 'Làm sao biết còn ghế trống không?',
          answer:
              "Khi tìm chuyến xe, bạn sẽ thấy số ghế trống còn lại. Ví dụ: 'Còn 25 ghế trống'.",
        ),
      ],
    ),
    FAQCategory(
      id: 'cancel',
      title: 'Hủy vé & Đổi vé',
      emoji: '🔄',
      items: [
        FAQItem(
          question: 'Tôi có thể hủy vé không?',
          answer:
              "Có, bạn vào 'Vé của tôi' → Chọn vé muốn hủy → Bấm 'Hủy vé'. Lưu ý: chỉ hủy được trước giờ xe chạy.",
        ),
        FAQItem(
          question: 'Hủy vé có mất phí không?',
          answer:
              'Chính sách hủy vé phụ thuộc vào thời gian hủy. Hủy trước 24h thường không mất phí. Liên hệ hotline 1900 1199 để biết chi tiết.',
        ),
        FAQItem(
          question: 'Tôi muốn đổi giờ xe, làm sao?',
          answer:
              'Bạn cần hủy vé cũ và đặt vé mới cho chuyến muốn đổi. Hoặc liên hệ hotline 1900 1199 để được hỗ trợ trực tiếp.',
        ),
      ],
    ),
    FAQCategory(
      id: 'checkin',
      title: 'Check-in & Lên xe',
      emoji: '✅',
      items: [
        FAQItem(
          question: 'Check-in là gì?',
          answer:
              'Check-in là xác nhận bạn đã có mặt để lên xe. Nhân viên sẽ quét mã QR của bạn trước khi xe chạy.',
        ),
        FAQItem(
          question: 'Phải check-in khi nào?',
          answer:
              'Bạn nên đến trước giờ xe chạy 15-30 phút để check-in và tìm chỗ ngồi.',
        ),
        FAQItem(
          question: 'Quên mang điện thoại thì sao?',
          answer:
              'Bạn nên chụp ảnh mã QR hoặc ghi lại mã đặt vé để nhân viên tra cứu thủ công.',
        ),
        FAQItem(
          question: 'Nơi đón xe ở đâu?',
          answer:
              "Khi đặt vé, bạn chọn 'Dọc tuyến đường' hoặc 'Bến xe miền đông'. Thông tin này có trong vé của bạn.",
        ),
      ],
    ),
    FAQCategory(
      id: 'account',
      title: 'Tài khoản & Bảo mật',
      emoji: '🔐',
      items: [
        FAQItem(
          question: 'Làm sao để đăng ký tài khoản?',
          answer:
              "Bấm 'Đăng ký' → Nhập thông tin (SĐT, Email, Mật khẩu) → Xác thực OTP → Hoàn tất!",
        ),
        FAQItem(
          question: 'Quên mật khẩu thì làm sao?',
          answer:
              "Bạn có thể đăng nhập bằng SMS OTP hoặc Vân tay (nếu đã bật). Sau đó vào 'Tài khoản' để đổi mật khẩu mới.",
        ),
        FAQItem(
          question: 'Đăng nhập bằng vân tay có an toàn không?',
          answer:
              'Rất an toàn! App sử dụng bảo mật 2 lớp: Vân tay + OTP SMS. Thông tin vân tay được lưu trên thiết bị của bạn, không gửi lên server.',
        ),
        FAQItem(
          question: 'Mã OTP là gì?',
          answer:
              'Mã OTP là mã xác thực 6 số gửi qua SMS để bảo vệ tài khoản của bạn. Chỉ nhập khi đăng nhập hoặc xác thực giao dịch quan trọng.',
        ),
      ],
    ),
    FAQCategory(
      id: 'schedule',
      title: 'Chuyến xe & Lịch trình',
      emoji: '🚌',
      items: [
        FAQItem(
          question: 'Xe chạy mấy giờ?',
          answer:
              'Mỗi tuyến có nhiều khung giờ: Sáng (6h-11h), Chiều (12h-17h), Tối (18h-22h). Bạn chọn khung giờ phù hợp khi tìm chuyến.',
        ),
        FAQItem(
          question: 'Xe có wifi không?',
          answer:
              'Thông tin tiện ích xe (wifi, điều hòa, toilet) phụ thuộc vào từng nhà xe. Bạn có thể hỏi khi check-in.',
        ),
        FAQItem(
          question: 'Xe có ghế nằm không?',
          answer:
              'Hiện tại app chỉ hỗ trợ xe ghế ngồi 40 chỗ. Xe giường nằm sẽ cập nhật sau.',
        ),
        FAQItem(
          question: 'Có chuyến đêm không?',
          answer:
              "Có! Bạn lọc theo khung giờ 'Tối' để xem các chuyến tối và đêm.",
        ),
      ],
    ),
    FAQCategory(
      id: 'app',
      title: 'App & Tính năng',
      emoji: '📱',
      items: [
        FAQItem(
          question: 'App này miễn phí không?',
          answer:
              'Hoàn toàn miễn phí! Bạn chỉ trả tiền vé xe, không có phí app.',
        ),
        FAQItem(
          question: 'Tôi có thể xem lại vé đã đặt không?',
          answer:
              "Có! Vào tab 'Vé của tôi' để xem tất cả vé: đã đặt, đã check-in, đã hủy.",
        ),
        FAQItem(
          question: 'Làm sao liên hệ với nhà xe?',
          answer:
              'Bạn có thể gọi hotline 1900 1199 hoặc đến trực tiếp quầy bán vé để nhân viên hỗ trợ.',
        ),
        FAQItem(
          question: 'App có trên iPhone không?',
          answer: 'Hiện app chỉ hỗ trợ Android.',
        ),
      ],
    ),
  ];

  static String getAllFAQsAsContext() {
    StringBuffer context = StringBuffer();
    context.writeln('Danh sách câu hỏi thường gặp về app Đặt Vé Xe:\n');

    for (var category in categories) {
      context.writeln('${category.emoji} ${category.title}:');
      for (var item in category.items) {
        context.writeln('Q: ${item.question}');
        context.writeln('A: ${item.answer}\n');
      }
    }

    return context.toString();
  }
}

