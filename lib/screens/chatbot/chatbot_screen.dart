import 'package:flutter/material.dart';
import '../../models/faq_data.dart';
import '../../services/gemini_service.dart';
// import '../../test_gemini_api.dart';
// import '../../services/gemini_service_demo.dart';

class ChatbotScreen extends StatefulWidget {
  const ChatbotScreen({super.key});

  @override
  State<ChatbotScreen> createState() => _ChatbotScreenState();
}

class _ChatbotScreenState extends State<ChatbotScreen> {
  final List<ChatMessage> _messages = [];
  final TextEditingController _textController = TextEditingController();
  final ScrollController _scrollController = ScrollController();
  bool _isLoading = false;
  bool _showCategories = true;
  FAQCategory? _selectedCategory;

  @override
  void initState() {
    super.initState();
    _addBotMessage(
      'Xin chào! 👋 Tôi là trợ lý AI của ứng dụng Đặt Vé Xe.\n\nBạn cần hỗ trợ về vấn đề gì?',
    );
    
    // Test Gemini API - REMOVED
    // testGeminiApi();
  }

  @override
  void dispose() {
    _textController.dispose();
    _scrollController.dispose();
    super.dispose();
  }

  void _addBotMessage(String text) {
    setState(() {
      _messages.add(ChatMessage(
        text: text,
        isUser: false,
        timestamp: DateTime.now(),
      ));
    });
    _scrollToBottom();
  }

  void _addUserMessage(String text) {
    setState(() {
      _messages.add(ChatMessage(
        text: text,
        isUser: true,
        timestamp: DateTime.now(),
      ));
    });
    _scrollToBottom();
  }

  void _scrollToBottom() {
    Future.delayed(const Duration(milliseconds: 100), () {
      if (_scrollController.hasClients) {
        _scrollController.animateTo(
          _scrollController.position.maxScrollExtent,
          duration: const Duration(milliseconds: 300),
          curve: Curves.easeOut,
        );
      }
    });
  }

  void _handleCategorySelected(FAQCategory category) {
    setState(() {
      _selectedCategory = category;
      _showCategories = false;
    });
    _addUserMessage('${category.emoji} ${category.title}');
    _addBotMessage(
      'Bạn có thể chọn một trong các câu hỏi sau hoặc nhập câu hỏi của bạn:',
    );
  }

  void _handleQuestionSelected(FAQItem item) {
    _addUserMessage(item.question);
    _addBotMessage(item.answer);
  }

  Future<void> _handleCustomQuestion(String question) async {
    if (question.trim().isEmpty) return;

    _addUserMessage(question);
    _textController.clear();

    setState(() {
      _isLoading = true;
    });

    try {
      final answer = await GeminiService.askCustomQuestion(question);
      _addBotMessage(answer);
    } catch (e) {
      _addBotMessage(
        'Xin lỗi, đã có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ hotline 1900 1199.',
      );
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _handleBackToCategories() {
    setState(() {
      _showCategories = true;
      _selectedCategory = null;
    });
    _addUserMessage('Quay lại danh mục');
    _addBotMessage('Bạn cần hỗ trợ về vấn đề gì?');
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Trợ lý AI'),
        actions: [
          if (!_showCategories)
            IconButton(
              icon: const Icon(Icons.home),
              onPressed: _handleBackToCategories,
              tooltip: 'Về trang chủ',
            ),
          IconButton(
            icon: const Icon(Icons.refresh),
            onPressed: () {
              setState(() {
                _messages.clear();
                _showCategories = true;
                _selectedCategory = null;
              });
              _addBotMessage(
                'Xin chào! 👋 Tôi là trợ lý AI của ứng dụng Đặt Vé Xe.\n\nBạn cần hỗ trợ về vấn đề gì?',
              );
            },
            tooltip: 'Bắt đầu lại',
          ),
        ],
      ),
      body: Column(
        children: [
          // Messages list
          Expanded(
            child: ListView.builder(
              controller: _scrollController,
              padding: const EdgeInsets.all(16),
              itemCount: _messages.length,
              itemBuilder: (context, index) {
                final message = _messages[index];
                return ChatBubble(message: message);
              },
            ),
          ),

          // Categories or Questions
          if (_showCategories)
            _buildCategoriesWidget()
          else if (_selectedCategory != null)
            _buildQuestionsWidget()
          else
            // Show expand button when collapsed
            Container(
              padding: const EdgeInsets.all(16),
              decoration: BoxDecoration(
                color: Colors.grey.shade50,
                border: Border(top: BorderSide(color: Colors.grey.shade300)),
              ),
              child: Center(
                child: TextButton.icon(
                  onPressed: () {
                    setState(() {
                      _showCategories = true;
                    });
                  },
                  icon: const Icon(Icons.keyboard_arrow_up),
                  label: const Text('Xem chủ đề'),
                  style: TextButton.styleFrom(
                    foregroundColor: Colors.blue.shade700,
                  ),
                ),
              ),
            ),

          // Loading indicator
          if (_isLoading)
            Container(
              padding: const EdgeInsets.all(16),
              child: const Row(
                children: [
                  SizedBox(
                    width: 20,
                    height: 20,
                    child: CircularProgressIndicator(strokeWidth: 2),
                  ),
                  SizedBox(width: 12),
                  Text('Đang suy nghĩ...'),
                ],
              ),
            ),

          // Input field
          _buildInputWidget(),
        ],
      ),
    );
  }

  Widget _buildCategoriesWidget() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.grey.shade50,
        border: Border(top: BorderSide(color: Colors.grey.shade300)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text(
                'Chọn chủ đề:',
                style: TextStyle(
                  fontWeight: FontWeight.bold,
                  fontSize: 16,
                ),
              ),
              TextButton.icon(
                onPressed: () {
                  setState(() {
                    _showCategories = false;
                  });
                },
                icon: const Icon(Icons.keyboard_arrow_down, size: 20),
                label: const Text('Thu gọn'),
                style: TextButton.styleFrom(
                  foregroundColor: Colors.blue.shade700,
                ),
              ),
            ],
          ),
          const SizedBox(height: 12),
          Wrap(
            spacing: 8,
            runSpacing: 8,
            children: FAQData.categories.map((category) {
              return ElevatedButton(
                onPressed: () => _handleCategorySelected(category),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.blue.shade50,
                  foregroundColor: Colors.blue.shade700,
                  elevation: 0,
                  padding: const EdgeInsets.symmetric(
                    horizontal: 16,
                    vertical: 12,
                  ),
                ),
                child: Text(
                  '${category.emoji} ${category.title}',
                  style: const TextStyle(fontSize: 13),
                ),
              );
            }).toList(),
          ),
        ],
      ),
    );
  }

  Widget _buildQuestionsWidget() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.grey.shade50,
        border: Border(top: BorderSide(color: Colors.grey.shade300)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Expanded(
                child: Text(
                  '${_selectedCategory!.emoji} ${_selectedCategory!.title}',
                  style: const TextStyle(
                    fontWeight: FontWeight.bold,
                    fontSize: 16,
                  ),
                ),
              ),
              TextButton.icon(
                onPressed: _handleBackToCategories,
                icon: const Icon(Icons.arrow_back, size: 16),
                label: const Text('Quay lại'),
                style: TextButton.styleFrom(
                  foregroundColor: Colors.blue.shade700,
                ),
              ),
            ],
          ),
          const SizedBox(height: 8),
          SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            child: Row(
              children: _selectedCategory!.items.map((item) {
                return Padding(
                  padding: const EdgeInsets.only(right: 8),
                  child: OutlinedButton(
                    onPressed: () => _handleQuestionSelected(item),
                    style: OutlinedButton.styleFrom(
                      foregroundColor: Colors.blue.shade700,
                      side: BorderSide(color: Colors.blue.shade300),
                      padding: const EdgeInsets.symmetric(
                        horizontal: 12,
                        vertical: 8,
                      ),
                    ),
                    child: Text(
                      item.question,
                      style: const TextStyle(fontSize: 12),
                    ),
                  ),
                );
              }).toList(),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildInputWidget() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        boxShadow: [
          BoxShadow(
            color: Colors.grey.shade300,
            blurRadius: 4,
            offset: const Offset(0, -2),
          ),
        ],
      ),
      child: Row(
        children: [
          Expanded(
            child: TextField(
              controller: _textController,
              decoration: InputDecoration(
                hintText: 'Nhập câu hỏi của bạn...',
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(24),
                ),
                contentPadding: const EdgeInsets.symmetric(
                  horizontal: 16,
                  vertical: 12,
                ),
                filled: true,
                fillColor: Colors.grey.shade100,
              ),
              onSubmitted: _handleCustomQuestion,
              enabled: !_isLoading,
            ),
          ),
          const SizedBox(width: 8),
          CircleAvatar(
            backgroundColor: Colors.blue.shade700,
            child: IconButton(
              icon: const Icon(Icons.send, color: Colors.white),
              onPressed: _isLoading
                  ? null
                  : () => _handleCustomQuestion(_textController.text),
            ),
          ),
        ],
      ),
    );
  }
}

class ChatMessage {
  final String text;
  final bool isUser;
  final DateTime timestamp;

  ChatMessage({
    required this.text,
    required this.isUser,
    required this.timestamp,
  });
}

class ChatBubble extends StatelessWidget {
  final ChatMessage message;

  const ChatBubble({super.key, required this.message});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: Row(
        mainAxisAlignment:
            message.isUser ? MainAxisAlignment.end : MainAxisAlignment.start,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          if (!message.isUser) ...[
            CircleAvatar(
              backgroundColor: Colors.blue.shade100,
              child: Icon(Icons.smart_toy, color: Colors.blue.shade700),
            ),
            const SizedBox(width: 8),
          ],
          Flexible(
            child: Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: message.isUser
                    ? Colors.blue.shade700
                    : Colors.grey.shade200,
                borderRadius: BorderRadius.only(
                  topLeft: const Radius.circular(16),
                  topRight: const Radius.circular(16),
                  bottomLeft:
                      Radius.circular(message.isUser ? 16 : 4),
                  bottomRight:
                      Radius.circular(message.isUser ? 4 : 16),
                ),
              ),
              child: Text(
                message.text,
                style: TextStyle(
                  color: message.isUser ? Colors.white : Colors.black87,
                  fontSize: 15,
                ),
              ),
            ),
          ),
          if (message.isUser) ...[
            const SizedBox(width: 8),
            CircleAvatar(
              backgroundColor: Colors.green.shade100,
              child: Icon(Icons.person, color: Colors.green.shade700),
            ),
          ],
        ],
      ),
    );
  }
}

