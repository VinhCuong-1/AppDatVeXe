// DEMO MODE - Không cần API key để test
// Thay thế import trong chatbot_screen.dart nếu muốn test offline

import '../models/faq_data.dart';

class GeminiServiceDemo {
  static Future<String> askQuestion(String question) async {
    // Simulate API delay
    await Future.delayed(const Duration(seconds: 1));
    
    // Tìm câu trả lời trong FAQ
    final lowerQuestion = question.toLowerCase();
    
    for (var category in FAQData.categories) {
      for (var item in category.items) {
        if (item.question.toLowerCase().contains(lowerQuestion) ||
            lowerQuestion.contains(item.question.toLowerCase().substring(0, 10))) {
          return item.answer;
        }
      }
    }
    
    // Trả lời mặc định nếu không tìm thấy
    return 'Xin lỗi, tôi không thể trả lời câu hỏi này. '
        'Vui lòng liên hệ hotline 1900 1199 để được hỗ trợ.\n\n'
        '💡 Gợi ý: Hãy chọn một trong các danh mục phía dưới để xem '
        'các câu hỏi thường gặp!';
  }

  static Future<String> askCustomQuestion(String question) async {
    return askQuestion(question);
  }
}

