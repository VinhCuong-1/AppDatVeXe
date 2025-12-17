import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';
import 'package:qr_flutter/qr_flutter.dart';
import 'package:url_launcher/url_launcher.dart';
import '../../models/models.dart';

class TicketDetailScreen extends StatelessWidget {
  final Booking booking;

  const TicketDetailScreen({
    super.key,
    required this.booking,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade100,
      appBar: AppBar(
        title: const Text('Chi tiết vé'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
        actions: [
          IconButton(
            onPressed: () => _shareTicket(context),
            icon: const Icon(Icons.share),
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            // QR Code Card
            Card(
              elevation: 8,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16),
              ),
              child: Container(
                width: double.infinity,
                padding: const EdgeInsets.all(24),
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(16),
                  gradient: LinearGradient(
                    colors: [Colors.blue.shade700, Colors.blue.shade900],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                ),
                child: Column(
                  children: [
                    Text(
                      'VÉ ĐIỆN TỬ',
                      style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                        letterSpacing: 2,
                      ),
                    ),
                    const SizedBox(height: 16),
                    
                    // QR Code
                    Container(
                      padding: const EdgeInsets.all(16),
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: QrImageView(
                        data: booking.qrToken, // Using qrToken from backend
                        version: QrVersions.auto,
                        size: 200.0,
                        backgroundColor: Colors.white,
                        errorCorrectionLevel: QrErrorCorrectLevel.H,
                      ),
                    ),
                    const SizedBox(height: 16),
                    
                    Text(
                      'Mã vé: #${booking.bookingId}',
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 8),
                    
                    Text(
                      'Quét mã QR để check-in',
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                        color: Colors.white70,
                      ),
                    ),
                  ],
                ),
              ),
            ),
            
            const SizedBox(height: 16),
            
            // Trip Information Card
            Card(
              elevation: 4,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12),
              ),
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Thông tin chuyến xe',
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 16),
                    
                    if (booking.trip != null) ...[
                      _buildInfoRow(
                        context,
                        Icons.directions_bus,
                        'Nhà xe',
                        booking.trip!.busName,
                        Colors.blue.shade700,
                      ),
                      const SizedBox(height: 12),
                      
                      if (booking.trip!.route != null) ...[
                        _buildInfoRow(
                          context,
                          Icons.location_on,
                          'Tuyến đường',
                          booking.trip!.route!.displayName,
                          Colors.green.shade700,
                        ),
                        const SizedBox(height: 12),
                      ],
                      
                      _buildInfoRow(
                        context,
                        Icons.access_time,
                        'Giờ khởi hành',
                        DateFormat('HH:mm dd/MM/yyyy').format(booking.trip!.startTime),
                        Colors.orange.shade700,
                      ),
                      const SizedBox(height: 12),
                    ],
                    
                    _buildInfoRow(
                      context,
                      Icons.event_seat,
                      'Ghế',
                      booking.seatNumber,
                      Colors.purple.shade700,
                    ),
                  ],
                ),
              ),
            ),
            
            const SizedBox(height: 16),
            
            // Passenger Information Card
            Card(
              elevation: 4,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12),
              ),
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Thông tin hành khách',
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 16),
                    
                    _buildInfoRow(
                      context,
                      Icons.person,
                      'Họ và tên',
                      booking.holderName,
                      Colors.indigo.shade700,
                    ),
                    const SizedBox(height: 12),
                    
                    _buildInfoRow(
                      context,
                      Icons.phone,
                      'Số điện thoại',
                      booking.phone,
                      Colors.teal.shade700,
                    ),
                    
                    // Pickup point
                    if (booking.pickupPoint != null) ...[
                      const SizedBox(height: 12),
                      _buildInfoRow(
                        context,
                        Icons.place,
                        'Nơi đón',
                        booking.pickupPoint!,
                        Colors.red.shade700,
                      ),
                      
                      // Google Maps button for "Bến xe miền đông"
                      if (booking.pickupPoint == 'Bến xe miền đông') ...[
                        const SizedBox(height: 12),
                        SizedBox(
                          width: double.infinity,
                          child: ElevatedButton.icon(
                            onPressed: () => _openGoogleMaps(context),
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.green.shade700,
                              foregroundColor: Colors.white,
                              padding: const EdgeInsets.symmetric(vertical: 12),
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(8),
                              ),
                            ),
                            icon: const Icon(Icons.map),
                            label: const Text('Xem địa điểm'),
                          ),
                        ),
                      ],
                    ],
                  ],
                ),
              ),
            ),
            
            const SizedBox(height: 16),
            
            // Booking Information Card
            Card(
              elevation: 4,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12),
              ),
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Thông tin đặt vé',
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 16),
                    
                    _buildInfoRow(
                      context,
                      Icons.schedule,
                      'Thời gian đặt',
                      DateFormat('HH:mm dd/MM/yyyy').format(booking.bookingTime),
                      Colors.grey.shade700,
                    ),
                    
                    if (booking.expiresAt != null) ...[
                      const SizedBox(height: 12),
                      _buildInfoRow(
                        context,
                        Icons.timer,
                        'Hết hạn',
                        DateFormat('HH:mm dd/MM/yyyy').format(booking.expiresAt!),
                        Colors.red.shade700,
                      ),
                    ],
                    
                    const SizedBox(height: 12),
                    _buildInfoRow(
                      context,
                      Icons.info,
                      'Trạng thái',
                      _getStatusText(booking),
                      _getStatusColor(booking),
                    ),
                    
                    const SizedBox(height: 12),
                    _buildInfoRow(
                      context,
                      Icons.payment,
                      'Thanh toán',
                      booking.isPaid ? 'Đã thanh toán' : 'Chưa thanh toán',
                      booking.isPaid ? Colors.green.shade700 : Colors.orange.shade700,
                    ),
                  ],
                ),
              ),
            ),
            
            const SizedBox(height: 16),
            
            // Instructions Card
            Card(
              elevation: 4,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12),
              ),
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Hướng dẫn sử dụng',
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 12),
                    
                    _buildInstructionItem(
                      context,
                      '1',
                      'Có mặt tại bến xe trước giờ khởi hành 30 phút',
                    ),
                    _buildInstructionItem(
                      context,
                      '2',
                      'Mang theo giấy tờ tùy thân khi lên xe',
                    ),
                    _buildInstructionItem(
                      context,
                      '3',
                      'Xuất trình mã QR hoặc mã vé cho nhân viên',
                    ),
                    _buildInstructionItem(
                      context,
                      '4',
                      'Ngồi đúng ghế đã đặt',
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildInfoRow(
    BuildContext context,
    IconData icon,
    String label,
    String value,
    Color color,
  ) {
    return Row(
      children: [
        Icon(
          icon,
          color: color,
          size: 20,
        ),
        const SizedBox(width: 12),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                label,
                style: Theme.of(context).textTheme.bodySmall?.copyWith(
                  color: Colors.grey.shade600,
                ),
              ),
              Text(
                value,
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildInstructionItem(
    BuildContext context,
    String number,
    String text,
  ) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 8),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            width: 24,
            height: 24,
            decoration: BoxDecoration(
              color: Colors.blue.shade700,
              borderRadius: BorderRadius.circular(12),
            ),
            child: Center(
              child: Text(
                number,
                style: const TextStyle(
                  color: Colors.white,
                  fontSize: 12,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Text(
              text,
              style: Theme.of(context).textTheme.bodyMedium,
            ),
          ),
        ],
      ),
    );
  }

  String _getStatusText(Booking booking) {
    if (booking.isCheckedIn) {
      return 'Đã check-in';
    } else if (booking.isCancelled) {
      return 'Đã hủy';
    } else if (booking.isExpired) {
      return 'Hết hạn';
    } else {
      return 'Đã đặt';
    }
  }

  Color _getStatusColor(Booking booking) {
    if (booking.isCheckedIn) {
      return Colors.green.shade700;
    } else if (booking.isCancelled) {
      return Colors.red.shade700;
    } else if (booking.isExpired) {
      return Colors.orange.shade700;
    } else {
      return Colors.blue.shade700;
    }
  }

  void _shareTicket(BuildContext context) {
    final String shareText = '''
Vé xe điện tử
Mã vé: #${booking.bookingId}
Nhà xe: ${booking.trip?.busName ?? 'N/A'}
Tuyến: ${booking.trip?.route?.displayName ?? 'N/A'}
Ghế: ${booking.seatNumber}
Hành khách: ${booking.holderName}
SĐT: ${booking.phone}
Giờ khởi hành: ${booking.trip != null ? DateFormat('HH:mm dd/MM/yyyy').format(booking.trip!.startTime) : 'N/A'}
${booking.pickupPoint != null ? 'Nơi đón: ${booking.pickupPoint}' : ''}
''';

    Clipboard.setData(ClipboardData(text: shareText));
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(
        content: Text('Đã sao chép thông tin vé vào clipboard'),
        duration: Duration(seconds: 2),
      ),
    );
  }

  Future<void> _openGoogleMaps(BuildContext context) async {
    // Địa chỉ Bến xe miền đông: 292 Đ. Đinh Bộ Lĩnh, Phường 26, Bình Thạnh, TP. Hồ Chí Minh
    const double lat = 10.8142; // Latitude của Bến xe miền đông
    const double lng = 106.7106; // Longitude của Bến xe miền đông
    const String placeName = 'Bến xe miền đông';
    
    // Thử nhiều URL schemes khác nhau
    final List<Uri> urlsToTry = [
      // 1. Google Maps app với tọa độ (ưu tiên)
      Uri.parse('google.navigation:q=$lat,$lng&mode=d'),
      
      // 2. Geo URI (Universal)
      Uri.parse('geo:0,0?q=$lat,$lng($placeName)'),
      
      // 3. Google Maps app với địa chỉ
      Uri.parse('https://www.google.com/maps/search/?api=1&query=$lat,$lng'),
      
      // 4. Google Maps web (fallback)
      Uri.parse('https://www.google.com/maps/dir/?api=1&destination=$lat,$lng&travelmode=driving'),
    ];
    
    bool launched = false;
    
    for (final url in urlsToTry) {
      try {
        if (await canLaunchUrl(url)) {
          await launchUrl(url, mode: LaunchMode.externalApplication);
          launched = true;
          break;
        }
      } catch (e) {
        // Thử URL tiếp theo
        continue;
      }
    }
    
    if (!launched && context.mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Không thể mở Google Maps. Vui lòng cài đặt Google Maps.'),
          backgroundColor: Colors.red,
          duration: Duration(seconds: 3),
        ),
      );
    }
  }
}
