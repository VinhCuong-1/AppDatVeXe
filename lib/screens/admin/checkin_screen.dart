import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:mobile_scanner/mobile_scanner.dart';
import '../../providers/admin_provider.dart';
import '../../models/models.dart';
import '../../services/api_service.dart';

class CheckinScreen extends StatefulWidget {
  const CheckinScreen({super.key});

  @override
  State<CheckinScreen> createState() => _CheckinScreenState();
}

class _CheckinScreenState extends State<CheckinScreen> with SingleTickerProviderStateMixin {
  late TabController _tabController;
  final TextEditingController _phoneController = TextEditingController();
  MobileScannerController? _scannerController;
  bool _isProcessing = false;
  List<Booking> _searchResults = [];

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 2, vsync: this);
  }

  @override
  void dispose() {
    _tabController.dispose();
    _phoneController.dispose();
    _scannerController?.dispose();
    super.dispose();
  }

  Future<void> _searchByPhone() async {
    if (_phoneController.text.isEmpty) {
      _showErrorDialog('Vui lòng nhập số điện thoại');
      return;
    }

    setState(() {
      _isProcessing = true;
      _searchResults = [];
    });

    try {
      // Call API to search bookings by phone
      final results = await ApiService.searchBookingsByPhone(_phoneController.text);

      setState(() {
        _searchResults = results;
      });

      if (results.isEmpty) {
        _showErrorDialog('Không tìm thấy vé với số điện thoại này');
      }
    } catch (e) {
      _showErrorDialog('Lỗi khi tìm kiếm: ${e.toString()}');
    } finally {
      setState(() {
        _isProcessing = false;
      });
    }
  }

  Future<void> _processQRCode(String qrData) async {
    if (_isProcessing) return;

    setState(() {
      _isProcessing = true;
    });

    try {
      final adminProvider = Provider.of<AdminProvider>(context, listen: false);
      
      // Verify the booking
      final booking = await adminProvider.verifyBooking(qrData);
      
      if (booking != null) {
        if (mounted) {
          _showBookingDetailDialog(booking);
        }
      } else {
        _showErrorDialog('Không tìm thấy vé với mã QR này');
      }
    } catch (e) {
      _showErrorDialog('Lỗi khi xử lý mã QR: ${e.toString()}');
    } finally {
      setState(() {
        _isProcessing = false;
      });
    }
  }

  void _showBookingDetailDialog(Booking booking) {
    // Create a local variable to track check-in status
    bool isCheckedIn = booking.isCheckedIn;
    
    showDialog(
      context: context,
      barrierDismissible: false, // Prevent dismiss by tapping outside
      builder: (dialogContext) => StatefulBuilder(
        builder: (builderContext, setDialogState) {
          final canCheckIn = !isCheckedIn && !booking.isCancelled && !booking.isExpired;
          
          return AlertDialog(
            title: const Text('Thông tin vé'),
            content: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize: MainAxisSize.min,
                children: [
                  _buildInfoRow('Mã vé', '#${booking.bookingId}'),
                  _buildInfoRow('Hành khách', booking.holderName),
                  _buildInfoRow('Số điện thoại', booking.phone),
                  _buildInfoRow('Ghế', booking.seatNumber),
                  if (booking.trip != null) ...[
                    _buildInfoRow('Nhà xe', booking.trip!.busName),
                    if (booking.trip!.route != null)
                      _buildInfoRow('Tuyến', booking.trip!.route!.displayName),
                    _buildInfoRow(
                      'Giờ khởi hành',
                      booking.trip!.startTime.toString().substring(0, 16),
                    ),
                  ],
                  _buildInfoRow(
                    'Trạng thái',
                    isCheckedIn ? 'Đã check-in' : _getStatusText(booking),
                  ),
                ],
              ),
            ),
            actions: [
              TextButton(
                onPressed: () {
                  if (Navigator.canPop(dialogContext)) {
                    Navigator.pop(dialogContext);
                  }
                },
                child: const Text('Đóng'),
              ),
              if (canCheckIn)
                ElevatedButton(
                  onPressed: _isProcessing ? null : () async {
                    setDialogState(() {
                      _isProcessing = true;
                    });
                    
                    try {
                      final adminProvider = Provider.of<AdminProvider>(context, listen: false);
                      
                      final success = await adminProvider.checkinBooking(
                        bookingId: booking.bookingId,
                        qrToken: booking.qrToken,
                      );

                      if (success) {
                        // Update local state
                        setDialogState(() {
                          isCheckedIn = true;
                          _isProcessing = false;
                        });
                        
                        // Show success message
                        if (mounted) {
                          ScaffoldMessenger.of(context).showSnackBar(
                            const SnackBar(
                              content: Row(
                                children: [
                                  Icon(Icons.check_circle, color: Colors.white),
                                  SizedBox(width: 12),
                                  Text('Check-in thành công!'),
                                ],
                              ),
                              backgroundColor: Colors.green,
                              duration: Duration(seconds: 2),
                            ),
                          );
                        }
                        
                        // Refresh search results if we have phone number
                        if (_phoneController.text.isNotEmpty && mounted) {
                          await _searchByPhone();
                        }
                      } else {
                        setDialogState(() {
                          _isProcessing = false;
                        });
                        
                        if (mounted) {
                          ScaffoldMessenger.of(context).showSnackBar(
                            const SnackBar(
                              content: Text('Không thể check-in vé này'),
                              backgroundColor: Colors.red,
                            ),
                          );
                        }
                      }
                    } catch (e) {
                      setDialogState(() {
                        _isProcessing = false;
                      });
                      
                      if (mounted) {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(
                            content: Text('Lỗi khi check-in: ${e.toString()}'),
                            backgroundColor: Colors.red,
                          ),
                        );
                      }
                    }
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.green,
                    foregroundColor: Colors.white,
                  ),
                  child: _isProcessing 
                    ? const SizedBox(
                        width: 20,
                        height: 20,
                        child: CircularProgressIndicator(
                          strokeWidth: 2,
                          color: Colors.white,
                        ),
                      )
                    : const Text('Check-in'),
                ),
              if (isCheckedIn)
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                  decoration: BoxDecoration(
                    color: Colors.green.shade50,
                    borderRadius: BorderRadius.circular(8),
                    border: Border.all(color: Colors.green),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(Icons.check_circle, color: Colors.green.shade700, size: 20),
                      const SizedBox(width: 8),
                      Text(
                        'Đã check-in',
                        style: TextStyle(
                          color: Colors.green.shade700,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ],
                  ),
                ),
            ],
          );
        },
      ),
    );
  }



  Widget _buildInfoRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 100,
            child: Text(
              '$label:',
              style: const TextStyle(fontWeight: FontWeight.bold),
            ),
          ),
          Expanded(
            child: Text(value),
          ),
        ],
      ),
    );
  }

  void _showErrorDialog(String message) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Row(
          children: [
            Icon(Icons.error, color: Colors.red),
            SizedBox(width: 8),
            Text('Lỗi'),
          ],
        ),
        content: Text(message),
        actions: [
          ElevatedButton(
            onPressed: () => Navigator.of(context).pop(),
            child: const Text('OK'),
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

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Kiểm tra vé'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        bottom: TabBar(
          controller: _tabController,
          indicatorColor: Colors.white,
          labelColor: Colors.white,
          unselectedLabelColor: Colors.white70,
          tabs: const [
            Tab(
              icon: Icon(Icons.phone),
              text: 'Tìm theo SĐT',
            ),
            Tab(
              icon: Icon(Icons.qr_code_scanner),
              text: 'Quét QR',
            ),
          ],
        ),
      ),
      body: TabBarView(
        controller: _tabController,
        children: [
          _buildPhoneSearchTab(),
          _buildQRScanTab(),
        ],
      ),
    );
  }

  Widget _buildPhoneSearchTab() {
    return Padding(
      padding: const EdgeInsets.all(16),
      child: Column(
        children: [
          // Search Box
          Row(
            children: [
              Expanded(
                child: TextField(
                  controller: _phoneController,
                  decoration: InputDecoration(
                    labelText: 'Số điện thoại',
                    hintText: 'Nhập số điện thoại',
                    prefixIcon: const Icon(Icons.phone),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                  ),
                  keyboardType: TextInputType.phone,
                  onSubmitted: (_) => _searchByPhone(),
                ),
              ),
              const SizedBox(width: 12),
              ElevatedButton(
                onPressed: _isProcessing ? null : _searchByPhone,
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.blue.shade700,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.all(16),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                ),
                child: _isProcessing
                    ? const SizedBox(
                        width: 20,
                        height: 20,
                        child: CircularProgressIndicator(
                          strokeWidth: 2,
                          color: Colors.white,
                        ),
                      )
                    : const Icon(Icons.search),
              ),
            ],
          ),
          
          const SizedBox(height: 24),
          
          // Search Results
          Expanded(
            child: _searchResults.isEmpty
                ? Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.search_off,
                          size: 64,
                          color: Colors.grey.shade400,
                        ),
                        const SizedBox(height: 16),
                        Text(
                          'Nhập số điện thoại để tìm kiếm',
                          style: TextStyle(
                            color: Colors.grey.shade600,
                            fontSize: 16,
                          ),
                        ),
                      ],
                    ),
                  )
                : ListView.builder(
                    itemCount: _searchResults.length,
                    itemBuilder: (context, index) {
                      final booking = _searchResults[index];
                      return Card(
                        margin: const EdgeInsets.only(bottom: 12),
                        child: ListTile(
                          leading: CircleAvatar(
                            backgroundColor: Colors.blue.shade700,
                            child: Text(
                              booking.seatNumber,
                              style: const TextStyle(
                                color: Colors.white,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ),
                          title: Text(booking.holderName),
                          subtitle: Text(
                            '${booking.trip?.route?.displayName ?? 'N/A'}\n'
                            '${booking.trip?.startTime.toString().substring(0, 16) ?? 'N/A'}',
                          ),
                          trailing: Chip(
                            label: Text(
                              _getStatusText(booking),
                              style: const TextStyle(fontSize: 12),
                            ),
                            backgroundColor: _getStatusColor(booking),
                          ),
                          onTap: () => _showBookingDetailDialog(booking),
                        ),
                      );
                    },
                  ),
          ),
        ],
      ),
    );
  }

  Widget _buildQRScanTab() {
    return Stack(
      children: [
        MobileScanner(
          controller: _scannerController ??= MobileScannerController(
            detectionSpeed: DetectionSpeed.noDuplicates,
          ),
          onDetect: (capture) {
            final List<Barcode> barcodes = capture.barcodes;
            for (final barcode in barcodes) {
              if (barcode.rawValue != null) {
                _processQRCode(barcode.rawValue!);
                break;
              }
            }
          },
        ),
        // Overlay with scan area
        Container(
          decoration: BoxDecoration(
            color: Colors.black.withOpacity(0.5),
          ),
          child: Column(
            children: [
              const Spacer(),
              Center(
                child: Container(
                  width: 250,
                  height: 250,
                  decoration: BoxDecoration(
                    border: Border.all(color: Colors.white, width: 3),
                    borderRadius: BorderRadius.circular(12),
                  ),
                ),
              ),
              const SizedBox(height: 24),
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
                decoration: BoxDecoration(
                  color: Colors.black54,
                  borderRadius: BorderRadius.circular(24),
                ),
                child: const Text(
                  'Đưa mã QR vào khung để quét',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 16,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
              const Spacer(),
            ],
          ),
        ),
        // Processing indicator
        if (_isProcessing)
          Container(
            color: Colors.black54,
            child: const Center(
              child: CircularProgressIndicator(),
            ),
          ),
      ],
    );
  }

  Color _getStatusColor(Booking booking) {
    if (booking.isCheckedIn) {
      return Colors.green.shade100;
    } else if (booking.isCancelled) {
      return Colors.red.shade100;
    } else if (booking.isExpired) {
      return Colors.orange.shade100;
    } else {
      return Colors.blue.shade100;
    }
  }
}

