import 'package:flutter/material.dart';
import '../../services/api_service.dart';
import '../../services/auth_service.dart';
import '../../services/biometric_service.dart';
import '../../services/sms_service.dart';
import '../home_screen.dart';
import 'register_screen.dart';
import 'forgot_password_screen.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> with SingleTickerProviderStateMixin {
  late TabController _tabController;
  
  // Password Login
  final _passwordFormKey = GlobalKey<FormState>();
  final _passwordPhoneController = TextEditingController();
  final _passwordController = TextEditingController();
  bool _obscurePassword = true;
  bool _isPasswordLoading = false;
  
  // Password + OTP (2FA)
  final _password2faOtpController = TextEditingController();
  bool _isPassword2faOtpSent = false;
  bool _isPassword2faOtpLoading = false;
  int _password2faOtpCountdown = 0;
  String _tempPasswordPhone = '';
  
  // Auto-fill OTP in TEST MODE (Bật/tắt dễ dàng)
  static const bool AUTO_FILL_TEST_OTP = true; // Đổi thành false để tắt auto-fill
  
  // Biometric
  bool _isBiometricAvailable = false;
  bool _isBiometricEnabled = false;
  String _biometricType = '';

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 2, vsync: this);
    _checkBiometric();
    
    // Pre-fill for testing
    _passwordPhoneController.text = '0123456789';
    _passwordController.text = 'Admin123';
  }

  @override
  void dispose() {
    _tabController.dispose();
    _passwordPhoneController.dispose();
    _passwordController.dispose();
    _password2faOtpController.dispose();
    super.dispose();
  }

  Future<void> _checkBiometric() async {
    final available = await BiometricService.isBiometricAvailable();
    final enabled = await BiometricService.isBiometricEnabled();
    
    String type = '';
    if (available) {
      final biometrics = await BiometricService.getAvailableBiometrics();
      if (biometrics.isNotEmpty) {
        type = BiometricService.getBiometricTypeName(biometrics.first);
      }
    }
    
    if (mounted) {
      setState(() {
        _isBiometricAvailable = available;
        _isBiometricEnabled = enabled;
        _biometricType = type;
      });
    }
  }

  /// Tự động điền OTP từ TEST MODE response
  /// Returns: OTP code nếu tìm thấy, null nếu không
  String? _autoFillOtpFromTestMode(String responseMessage) {
    if (!AUTO_FILL_TEST_OTP) return null;
    
    // Extract OTP từ format: [TEST MODE: 123456]
    final testModeMatch = RegExp(r'\[TEST MODE:\s*(\d{6})\]').firstMatch(responseMessage);
    
    if (testModeMatch != null) {
      final otpCode = testModeMatch.group(1)!;
      print('🔍 [AUTO-FILL] Phát hiện TEST MODE OTP: $otpCode');
      
      // Tự động điền OTP vào text field
      _password2faOtpController.text = otpCode;
      
      // Hiển thị thông báo
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('✅ Đã tự động điền OTP: $otpCode (TEST MODE)'),
            backgroundColor: Colors.orange,
            duration: const Duration(seconds: 2),
          ),
        );
      }
      
      return otpCode;
    }
    
    return null;
  }

  // Password Login - Step 1: Verify password and send OTP
  Future<void> _loginWithPassword() async {
    if (!_passwordFormKey.currentState!.validate()) return;

    FocusScope.of(context).unfocus();
    setState(() => _isPasswordLoading = true);

    try {
      // Verify password first
      final user = await ApiService.login(
        _passwordPhoneController.text.trim(),
        _passwordController.text,
      );

      // Password correct → Send OTP for 2FA
      _tempPasswordPhone = user.phone;
      
      final otpResponse = await ApiService.sendOtp(user.phone);
      
      if (otpResponse['success'] == true) {
        setState(() {
          _isPassword2faOtpSent = true;
          _password2faOtpCountdown = 300; // 5 phút
          _isPasswordLoading = false;
        });
        
        _startPassword2faOtpCountdown();
        
        // Tự động điền OTP nếu ở TEST MODE
        final message = otpResponse['message'] ?? '';
        _autoFillOtpFromTestMode(message);
        
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(otpResponse['message'] ?? 'Mã OTP đã được gửi'),
              backgroundColor: Colors.green,
            ),
          );
        }
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Đăng nhập thất bại: ${e.toString()}'),
            backgroundColor: Colors.red,
          ),
        );
        setState(() => _isPasswordLoading = false);
      }
    }
  }

  void _startPassword2faOtpCountdown() {
    Future.doWhile(() async {
      await Future.delayed(const Duration(seconds: 1));
      if (mounted && _password2faOtpCountdown > 0) {
        setState(() => _password2faOtpCountdown--);
        return true;
      }
      return false;
    });
  }

  // Password Login - Step 2: Verify OTP and complete login
  Future<void> _verifyPassword2faOtp() async {
    if (_password2faOtpController.text.trim().length != 6) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Vui lòng nhập mã OTP 6 số')),
      );
      return;
    }

    setState(() => _isPassword2faOtpLoading = true);

    try {
      final user = await ApiService.verifyOtp(
        _tempPasswordPhone,
        _password2faOtpController.text.trim(),
      );

      await AuthService.saveUser(user);

      // Check biometric credentials
      final currentlyEnabled = await BiometricService.isBiometricEnabled();
      final savedCredentials = await BiometricService.getBiometricCredentials();
      
      // Kiểm tra xem credentials đã lưu có phải của user này không
      final isSameUser = savedCredentials != null && 
                         savedCredentials['phone'] == user.phone;
      
      // Hỏi bật biometric nếu:
      // 1. Thiết bị hỗ trợ biometric
      // 2. CHƯA lưu credentials HOẶC credentials cũ là của user khác
      if (_isBiometricAvailable && (!currentlyEnabled || !isSameUser) && mounted) {
        final enable = await _showEnableBiometricDialog();
        if (enable == true) {
          await BiometricService.saveBiometricCredentials(
            phone: user.phone,
            userId: user.userId,
          );
          if (mounted) setState(() => _isBiometricEnabled = true);
        } else if (!isSameUser && currentlyEnabled) {
          // Nếu từ chối và đang lưu credentials của user khác → Xóa credentials cũ
          await BiometricService.disableBiometric();
          if (mounted) setState(() => _isBiometricEnabled = false);
        }
      }

      if (mounted) {
        Navigator.of(context).pushReplacement(
          MaterialPageRoute(builder: (context) => const HomeScreen()),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Xác thực OTP thất bại: ${e.toString()}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    } finally {
      if (mounted) setState(() => _isPassword2faOtpLoading = false);
    }
  }

  // Biometric Login
  Future<void> _loginWithBiometric() async {
    try {
      final authenticated = await BiometricService.authenticate(
        reason: 'Xác thực để đăng nhập vào ứng dụng',
      );

      if (!authenticated) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Xác thực sinh trắc học thất bại')),
          );
        }
        return;
      }

      // Lấy thông tin đã lưu
      final credentials = await BiometricService.getBiometricCredentials();
      if (credentials == null || credentials['phone'] == null) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Chưa có thông tin đăng nhập được lưu')),
          );
        }
        return;
      }

      // Kiểm tra xem có user data hợp lệ không (token còn hiệu lực)
      try {
        final existingUser = await AuthService.getCurrentUser();
        if (existingUser != null && existingUser.phone == credentials['phone']) {
          // Token còn hiệu lực VÀ là đúng user → Đăng nhập ngay
          print('✅ [BIOMETRIC] Token còn hiệu lực, đăng nhập ngay');
          if (mounted) {
            Navigator.of(context).pushReplacement(
              MaterialPageRoute(builder: (context) => const HomeScreen()),
            );
          }
          return;
        } else {
          // Token hết hạn hoặc sai user → Clear và gửi OTP
          print('⏰ [BIOMETRIC] Token hết hạn hoặc sai user, cần OTP');
          await AuthService.clearAuth();
        }
      } catch (e) {
        // Lỗi khi get user (token hết hạn) → Clear và gửi OTP
        print('❌ [BIOMETRIC] Token invalid: $e');
        await AuthService.clearAuth();
      }

      // Token hết hạn hoặc đã logout → Cần đăng nhập lại bằng OTP
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Đang tự động đăng nhập...'),
            backgroundColor: Colors.blue,
          ),
        );
      }

      // Tự động gửi OTP
      final phone = credentials['phone']!;
      final otpResponse = await ApiService.sendOtp(phone);
      
      if (otpResponse['success'] != true) {
        throw Exception('Không thể gửi OTP');
      }

      // Trích xuất OTP từ response
      final message = otpResponse['message'] ?? '';
      final testModeMatch = RegExp(r'\[TEST MODE:\s*(\d{6})\]').firstMatch(message);
      
      if (testModeMatch != null) {
        // Số test - Auto-verify OTP (không cần nhập thủ công)
        final otpCode = testModeMatch.group(1)!;
        print('🔐 [BIOMETRIC] Auto-verify OTP in TEST MODE: $otpCode');
        final user = await ApiService.verifyOtp(phone, otpCode);
        await AuthService.saveUser(user);
        
        if (mounted) {
          Navigator.of(context).pushReplacement(
            MaterialPageRoute(builder: (context) => const HomeScreen()),
          );
        }
      } else {
        // Số thật - Chuyển sang màn hình nhập OTP thủ công
        if (mounted) {
          // Điền số điện thoại vào field
          _passwordPhoneController.text = phone;
          
          setState(() {
            _tempPasswordPhone = phone;
            _isPassword2faOtpSent = true;
            _password2faOtpCountdown = 300; // 5 phút
            _tabController.animateTo(0); // Chuyển sang tab Mật khẩu
          });
          
          _startPassword2faOtpCountdown();
          
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('✅ Xác thực vân tay thành công!\n📱 Mã OTP đã được gửi đến số điện thoại của bạn.\nVui lòng nhập OTP để hoàn tất đăng nhập.'),
              backgroundColor: Colors.green,
              duration: Duration(seconds: 4),
            ),
          );
        }
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Đăng nhập sinh trắc học thất bại: ${e.toString()}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    }
  }

  Future<bool?> _showEnableBiometricDialog() {
    return showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Bật đăng nhập bằng vân tay?'),
        content: const Text('Bạn có muốn sử dụng vân tay để đăng nhập nhanh hơn không?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Để sau'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text('Bật ngay'),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.blue.shade50,
      body: SafeArea(
        child: Column(
          children: [
            const SizedBox(height: 16),
            // Company Logo & Name (compact version)
            Container(
              padding: const EdgeInsets.symmetric(vertical: 12),
              child: Column(
                children: [
                  // Logo với gradient background (nhỏ hơn)
                  Container(
                    padding: const EdgeInsets.all(12),
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [Colors.blue.shade700, Colors.blue.shade900],
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight,
                      ),
                      shape: BoxShape.circle,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.blue.shade300,
                          blurRadius: 10,
                          offset: const Offset(0, 3),
                        ),
                      ],
                    ),
                    child: const Icon(
                      Icons.directions_bus,
                      size: 36,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 10),
                  // Company Name với style nổi bật (nhỏ hơn)
                  ShaderMask(
                    shaderCallback: (bounds) => LinearGradient(
                      colors: [Colors.blue.shade700, Colors.blue.shade900],
                    ).createShader(bounds),
                    child: const Text(
                      'NHÀ XE NGŨ AN',
                      style: TextStyle(
                        fontSize: 24,
                        fontWeight: FontWeight.w900,
                        letterSpacing: 1.2,
                        color: Colors.white,
                      ),
                    ),
                  ),
                  const SizedBox(height: 2),
                  Text(
                    'Đặt vé nhanh - An tâm đi xa',
                    style: TextStyle(
                      fontSize: 12,
                      color: Colors.grey.shade600,
                      fontStyle: FontStyle.italic,
                    ),
                  ),
                ],
              ),
            ),
            const SizedBox(height: 12),
            
            // Tabs
            Container(
              margin: const EdgeInsets.symmetric(horizontal: 24),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(12),
              ),
              child: TabBar(
                controller: _tabController,
                indicator: BoxDecoration(
                  color: Colors.blue.shade700,
                  borderRadius: BorderRadius.circular(12),
                ),
                labelColor: Colors.white,
                unselectedLabelColor: Colors.grey,
                tabs: const [
                  Tab(icon: Icon(Icons.lock), text: 'Mật khẩu'),
                  Tab(icon: Icon(Icons.fingerprint), text: 'Sinh trắc'),
                ],
              ),
            ),
            
            const SizedBox(height: 20),
            
            // Tab Content
            Expanded(
              child: TabBarView(
                controller: _tabController,
                children: [
                  _buildPasswordTab(),
                  _buildBiometricTab(),
                ],
              ),
            ),
            
            // Register Link
            Padding(
              padding: const EdgeInsets.all(16.0),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Text('Chưa có tài khoản? '),
                  TextButton(
                    onPressed: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(builder: (context) => const RegisterScreen()),
                      );
                    },
                    child: Text(
                      'Đăng ký',
                      style: TextStyle(
                        color: Colors.blue.shade700,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPasswordTab() {
    return SingleChildScrollView(
      padding: const EdgeInsets.all(24.0),
      child: Form(
        key: _passwordFormKey,
        child: Column(
          children: [
            // Step 1: Phone + Password
            TextFormField(
              controller: _passwordPhoneController,
              keyboardType: TextInputType.phone,
              enabled: !_isPassword2faOtpSent,
              decoration: InputDecoration(
                labelText: 'Số điện thoại',
                prefixIcon: const Icon(Icons.phone),
                border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                filled: true,
                fillColor: Colors.white,
              ),
              validator: (value) {
                if (value == null || value.isEmpty) return 'Vui lòng nhập số điện thoại';
                if (value.length < 10) return 'Số điện thoại không hợp lệ';
                return null;
              },
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _passwordController,
              obscureText: _obscurePassword,
              enabled: !_isPassword2faOtpSent,
              decoration: InputDecoration(
                labelText: 'Mật khẩu',
                prefixIcon: const Icon(Icons.lock),
                suffixIcon: IconButton(
                  icon: Icon(_obscurePassword ? Icons.visibility : Icons.visibility_off),
                  onPressed: () => setState(() => _obscurePassword = !_obscurePassword),
                ),
                border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                filled: true,
                fillColor: Colors.white,
              ),
              validator: (value) {
                if (value == null || value.isEmpty) return 'Vui lòng nhập mật khẩu';
                return null;
              },
            ),
            
            // Forgot Password Link
            if (!_isPassword2faOtpSent)
              Align(
                alignment: Alignment.centerRight,
                child: TextButton(
                  onPressed: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(
                        builder: (context) => const ForgotPasswordScreen(),
                      ),
                    );
                  },
                  child: Text(
                    'Quên mật khẩu?',
                    style: TextStyle(
                      color: Colors.blue.shade700,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ),
              ),
            const SizedBox(height: 16),
            
            // Step 2: OTP (shown after password verified)
            if (_isPassword2faOtpSent) ...[
              Container(
                padding: const EdgeInsets.all(16),
                decoration: BoxDecoration(
                  color: Colors.blue.shade50,
                  borderRadius: BorderRadius.circular(12),
                  border: Border.all(color: Colors.blue.shade200),
                ),
                child: Row(
                  children: [
                    Icon(Icons.info_outline, color: Colors.blue.shade700),
                    const SizedBox(width: 12),
                    Expanded(
                      child: Text(
                        'Mã OTP đã được gửi đến số điện thoại của bạn',
                        style: TextStyle(
                          fontSize: 14,
                          color: Colors.blue.shade700,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _password2faOtpController,
                keyboardType: TextInputType.number,
                maxLength: 6,
                decoration: InputDecoration(
                  labelText: 'Mã OTP',
                  prefixIcon: const Icon(Icons.pin),
                  border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                  filled: true,
                  fillColor: Colors.white,
                  helperText: _password2faOtpCountdown > 0 
                      ? 'Mã có hiệu lực trong ${_password2faOtpCountdown}s'
                      : 'Mã đã hết hạn',
                ),
              ),
              const SizedBox(height: 16),
              Row(
                children: [
                  Expanded(
                    child: OutlinedButton(
                      onPressed: _password2faOtpCountdown > 0 ? null : () {
                        setState(() {
                          _isPassword2faOtpSent = false;
                          _password2faOtpController.clear();
                          _passwordController.clear();
                        });
                      },
                      style: OutlinedButton.styleFrom(
                        padding: const EdgeInsets.symmetric(vertical: 16),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                      ),
                      child: const Text('Nhập lại'),
                    ),
                  ),
                  const SizedBox(width: 16),
                  Expanded(
                    flex: 2,
                    child: ElevatedButton(
                      onPressed: _isPassword2faOtpLoading ? null : _verifyPassword2faOtp,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.blue.shade700,
                        foregroundColor: Colors.white,
                        padding: const EdgeInsets.symmetric(vertical: 16),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                      ),
                      child: _isPassword2faOtpLoading
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(color: Colors.white, strokeWidth: 2),
                            )
                          : const Text('Xác thực OTP', style: TextStyle(fontWeight: FontWeight.bold)),
                    ),
                  ),
                ],
              ),
            ],
            
            // Step 1: Login button
            if (!_isPassword2faOtpSent)
              SizedBox(
                width: double.infinity,
                height: 50,
                child: ElevatedButton(
                  onPressed: _isPasswordLoading ? null : _loginWithPassword,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.blue.shade700,
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                  ),
                  child: _isPasswordLoading
                      ? const SizedBox(
                          height: 20,
                          width: 20,
                          child: CircularProgressIndicator(
                            strokeWidth: 2,
                            valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                          ),
                        )
                      : const Text('Tiếp tục', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
                ),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildBiometricTab() {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            if (!_isBiometricAvailable) ...[
              const Icon(Icons.warning, size: 64, color: Colors.orange),
              const SizedBox(height: 16),
              const Text(
                'Thiết bị không hỗ trợ sinh trắc học',
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 16),
              ),
            ] else if (!_isBiometricEnabled) ...[
              const Icon(Icons.fingerprint, size: 64, color: Colors.grey),
              const SizedBox(height: 16),
              const Text(
                'Chưa bật đăng nhập sinh trắc học',
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 16),
              ),
              const SizedBox(height: 8),
              const Text(
                'Vui lòng đăng nhập bằng mật khẩu lần đầu',
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 14, color: Colors.grey),
              ),
            ] else ...[
              Icon(Icons.fingerprint, size: 100, color: Colors.blue.shade700),
              const SizedBox(height: 24),
              Text(
                'Đăng nhập bằng $_biometricType',
                style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
              const SizedBox(height: 32),
              SizedBox(
                width: double.infinity,
                height: 50,
                child: ElevatedButton.icon(
                  onPressed: _loginWithBiometric,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.blue.shade700,
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                  ),
                  icon: const Icon(Icons.fingerprint),
                  label: const Text('Xác thực', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}

