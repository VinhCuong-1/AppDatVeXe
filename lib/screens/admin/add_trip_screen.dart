import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:intl/intl.dart';
import '../../providers/admin_provider.dart';

class AddTripScreen extends StatefulWidget {
  const AddTripScreen({super.key});

  @override
  State<AddTripScreen> createState() => _AddTripScreenState();
}

class _AddTripScreenState extends State<AddTripScreen> {
  final _formKey = GlobalKey<FormState>();
  final _driverNameController = TextEditingController();
  
  int? _selectedRouteId;
  String _selectedTimeSlot = 'Sáng';
  TimeOfDay _selectedTime = const TimeOfDay(hour: 6, minute: 30);
  bool _isLoading = false;

  // Danh sách khung giờ với giới hạn thời gian
  final Map<String, Map<String, int>> _timeSlotRanges = {
    'Sáng': {'start': 6, 'end': 11},    // 6:00 - 11:59
    'Chiều': {'start': 12, 'end': 17},  // 12:00 - 17:59
    'Tối': {'start': 18, 'end': 23},    // 18:00 - 23:59
  };

  bool _isTimeInSlot(TimeOfDay time, String slot) {
    final range = _timeSlotRanges[slot]!;
    final hour = time.hour;
    return hour >= range['start']! && hour <= range['end']!;
  }

  Future<void> _pickTime() async {
    final TimeOfDay? picked = await showTimePicker(
      context: context,
      initialTime: _selectedTime,
      builder: (context, child) {
        return MediaQuery(
          data: MediaQuery.of(context).copyWith(alwaysUse24HourFormat: true),
          child: child!,
        );
      },
    );

    if (picked != null) {
      if (_isTimeInSlot(picked, _selectedTimeSlot)) {
        setState(() {
          _selectedTime = picked;
        });
      } else {
        final range = _timeSlotRanges[_selectedTimeSlot]!;
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              'Giờ phải nằm trong khung $_selectedTimeSlot (${range['start']}:00 - ${range['end']}:59)',
            ),
            backgroundColor: Colors.orange,
          ),
        );
      }
    }
  }

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<AdminProvider>(context, listen: false).loadRoutes();
    });
  }

  @override
  void dispose() {
    _driverNameController.dispose();
    super.dispose();
  }

  Future<void> _submitForm() async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    setState(() {
      _isLoading = true;
    });

    try {
      // Lấy ngày hiện tại và kết hợp với giờ đã chọn
      final now = DateTime.now();
      final DateTime tripDateTime = DateTime(
        now.year,
        now.month,
        now.day,
        _selectedTime.hour,
        _selectedTime.minute,
      );

      final adminProvider = Provider.of<AdminProvider>(context, listen: false);
      final trip = await adminProvider.createTrip(
        routeId: _selectedRouteId!,
        busName: 'Ngũ An', // Mặc định Ngũ An
        driverName: _driverNameController.text.trim().isNotEmpty
            ? _driverNameController.text.trim()
            : null,
        startTime: tripDateTime,
        totalSeats: 40,
      );

      if (trip != null && mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Thêm chuyến xe thành công'),
            backgroundColor: Colors.green,
          ),
        );
        Navigator.of(context).pop(); // Quay lại trang quản lý chuyến
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi: ${e.toString()}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    } finally {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Thêm chuyến xe'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
      ),
      body: Consumer<AdminProvider>(
        builder: (context, adminProvider, child) {
          if (adminProvider.isLoading && adminProvider.routes.isEmpty) {
            return const Center(
              child: CircularProgressIndicator(),
            );
          }

          return SingleChildScrollView(
            padding: const EdgeInsets.all(16),
            child: Form(
              key: _formKey,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // Info card
                  Card(
                    color: Colors.blue.shade50,
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Row(
                        children: [
                          Icon(Icons.info_outline, color: Colors.blue.shade700),
                          const SizedBox(width: 12),
                          Expanded(
                            child: Text(
                              'Chuyến xe này sẽ chạy hàng ngày với giờ cố định',
                              style: TextStyle(
                                color: Colors.blue.shade900,
                                fontSize: 14,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 24),

                  // Route selection
                  DropdownButtonFormField<int>(
                    value: _selectedRouteId,
                    decoration: InputDecoration(
                      labelText: 'Tuyến đường',
                      border: const OutlineInputBorder(),
                      prefixIcon: const Icon(Icons.route),
                      filled: true,
                      fillColor: Colors.grey.shade50,
                    ),
                    items: adminProvider.routes
                        .map((route) => DropdownMenuItem<int>(
                              value: route.routeId,
                              child: Text(route.displayName),
                            ))
                        .toList(),
                    onChanged: (value) {
                      setState(() {
                        _selectedRouteId = value;
                      });
                    },
                    validator: (value) {
                      if (value == null) {
                        return 'Vui lòng chọn tuyến đường';
                      }
                      return null;
                    },
                  ),
                  const SizedBox(height: 16),

                  // Bus name (fixed, display only)
                  TextFormField(
                    initialValue: 'Ngũ An',
                    enabled: false,
                    decoration: InputDecoration(
                      labelText: 'Nhà xe',
                      border: const OutlineInputBorder(),
                      prefixIcon: const Icon(Icons.directions_bus),
                      filled: true,
                      fillColor: Colors.grey.shade100,
                      helperText: 'Nhà xe mặc định',
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Time slot selection
                  DropdownButtonFormField<String>(
                    value: _selectedTimeSlot,
                    decoration: InputDecoration(
                      labelText: 'Khung giờ',
                      border: const OutlineInputBorder(),
                      prefixIcon: const Icon(Icons.wb_sunny),
                      filled: true,
                      fillColor: Colors.grey.shade50,
                    ),
                    items: _timeSlotRanges.keys
                        .map((slot) {
                          final range = _timeSlotRanges[slot]!;
                          return DropdownMenuItem<String>(
                            value: slot,
                            child: Text('$slot (${range['start']}:00 - ${range['end']}:59)'),
                          );
                        })
                        .toList(),
                    onChanged: (value) {
                      setState(() {
                        _selectedTimeSlot = value!;
                        // Reset to start of new slot if current time is not in range
                        if (!_isTimeInSlot(_selectedTime, value)) {
                          final range = _timeSlotRanges[value]!;
                          _selectedTime = TimeOfDay(hour: range['start']!, minute: 0);
                        }
                      });
                    },
                  ),
                  const SizedBox(height: 16),

                  // Time picker
                  InkWell(
                    onTap: _pickTime,
                    child: Container(
                      padding: const EdgeInsets.all(16),
                      decoration: BoxDecoration(
                        border: Border.all(color: Colors.grey.shade300),
                        borderRadius: BorderRadius.circular(4),
                        color: Colors.grey.shade50,
                      ),
                      child: Row(
                        children: [
                          Icon(Icons.access_time, color: Colors.grey.shade700),
                          const SizedBox(width: 12),
                          Expanded(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  'Giờ khởi hành',
                                  style: TextStyle(
                                    fontSize: 12,
                                    color: Colors.grey.shade600,
                                  ),
                                ),
                                const SizedBox(height: 4),
                                Text(
                                  '${_selectedTime.hour.toString().padLeft(2, '0')}:${_selectedTime.minute.toString().padLeft(2, '0')}',
                                  style: const TextStyle(
                                    fontSize: 16,
                                    fontWeight: FontWeight.w500,
                                  ),
                                ),
                              ],
                            ),
                          ),
                          Icon(Icons.arrow_drop_down, color: Colors.grey.shade700),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Total seats (fixed, display only)
                  TextFormField(
                    initialValue: '40',
                    enabled: false,
                    decoration: InputDecoration(
                      labelText: 'Tổng số ghế',
                      border: const OutlineInputBorder(),
                      prefixIcon: const Icon(Icons.event_seat),
                      filled: true,
                      fillColor: Colors.grey.shade100,
                      helperText: 'Số ghế cố định cho tất cả chuyến',
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Driver name (optional)
                  TextFormField(
                    controller: _driverNameController,
                    decoration: InputDecoration(
                      labelText: 'Tên tài xế (tùy chọn)',
                      border: const OutlineInputBorder(),
                      prefixIcon: const Icon(Icons.person),
                      hintText: 'Nhập tên tài xế',
                      filled: true,
                      fillColor: Colors.grey.shade50,
                    ),
                  ),
                  const SizedBox(height: 32),

                  // Submit button
                  ElevatedButton(
                    onPressed: _isLoading ? null : _submitForm,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.blue.shade700,
                      foregroundColor: Colors.white,
                      padding: const EdgeInsets.symmetric(vertical: 16),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(8),
                      ),
                    ),
                    child: _isLoading
                        ? const SizedBox(
                            height: 20,
                            width: 20,
                            child: CircularProgressIndicator(
                              strokeWidth: 2,
                              color: Colors.white,
                            ),
                          )
                        : const Text(
                            'Thêm chuyến xe',
                            style: TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}

