import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/faq_data.dart';

class GeminiService {
  // API Key
  static const String _apiKey = 'AIzaSyCp9rDco53tJdc7w9zHulbXw1ST666ZuEY';
  // ✅ API v1 - gemini-2.5-flash (stable, miễn phí)
  static const String _baseUrl =
      'https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent';

  static Future<String> _callGeminiAPI(String prompt) async {
    try {
      final url = Uri.parse(_baseUrl);
      final response = await http.post(
        url,
        headers: {
          'Content-Type': 'application/json',
          'x-goog-api-key': _apiKey,
        },
        body: jsonEncode({
          'contents': [
            {
              'parts': [
                {'text': prompt}
              ]
            }
          ],
          'generationConfig': {
            'temperature': 0.7,
            'topK': 40,
            'topP': 0.95,
            'maxOutputTokens': 1024,
          }
        }),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final text = data['candidates']?[0]?['content']?['parts']?[0]?['text'];
        return text?.toString().trim() ?? 'Không nhận được câu trả lời từ AI.';
      } else {
        return 'Lỗi API (${response.statusCode}): ${response.body}';
      }
    } catch (e) {
      return 'Đã có lỗi xảy ra: ${e.toString()}';
    }
  }

  static Future<String> askQuestion(String question) async {
    final faqContext = FAQData.getAllFAQsAsContext();
    final prompt = '''
Bạn là trợ lý AI của ứng dụng "Đặt Vé Xe".
Nhiệm vụ của bạn là:

1. Trả lời dựa trên thông tin FAQ
2. Nếu không có câu trả lời, gợi ý liên hệ hotline 1900 1199
3. Lịch sự, thân thiện, chuyên nghiệp
4. Trả lời bằng tiếng Việt, tối đa 4 câu

$faqContext

Câu hỏi của khách hàng: $question

Trả lời:''';
    return await _callGeminiAPI(prompt);
  }

  static Future<String> askCustomQuestion(String question) async {
    final prompt = '''
Bạn là trợ lý AI của ứng dụng "Đặt Vé Xe". 
Khách hàng hỏi: $question

Hãy trả lời ngắn gọn, thân thiện. Nếu không biết hoặc không liên quan đến đặt vé xe, hãy gợi ý liên hệ hotline 1900 1199.
Trả lời bằng tiếng Việt, tối đa 4 câu.

Trả lời:''';

    return await _callGeminiAPI(prompt);
  }
}
