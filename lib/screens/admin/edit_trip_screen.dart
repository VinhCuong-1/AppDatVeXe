import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../../providers/admin_provider.dart';
import '../../models/models.dart';

class EditTripScreen extends StatefulWidget {
  final Trip trip;

  const EditTripScreen({
    super.key,
    required this.trip,
  });

  @override
  State<EditTripScreen> createState() => _EditTripScreenState();
}

class _EditTripScreenState extends State<EditTripScreen> {
  final _formKey = GlobalKey<FormState>();
  
  int? _selectedRouteId;
  String? _selectedTimeSlot;
  TimeOfDay? _selectedTime;
  String _busName = 'Ngũ An';
  String? _driverName;
  int _totalSeats = 40;
  
  // Định nghĩa khung giờ
  final Map<String, Map<String, int>> _timeSlotRanges = {
    'Sáng': {'start': 6, 'end': 11},    // 6:00 - 11:59
    'Chiều': {'start': 12, 'end': 17},  // 12:00 - 17:59
    'Tối': {'start': 18, 'end': 23},    // 18:00 - 23:59
  };

  @override
  void initState() {
    super.initState();
    
    // Load routes
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<AdminProvider>(context, listen: false).loadRoutes();
    });
    
    // Pre-fill data from existing trip
    _selectedRouteId = widget.trip.routeId;
    _busName = widget.trip.busName;
    _driverName = widget.trip.driverName;
    _totalSeats = widget.trip.totalSeats;
    _selectedTime = TimeOfDay.fromDateTime(widget.trip.startTime);
    
    // Determine time slot from trip start time
    final hour = widget.trip.startTime.hour;
    if (hour >= 6 && hour <= 11) {
      _selectedTimeSlot = 'Sáng';
    } else if (hour >= 12 && hour <= 17) {
      _selectedTimeSlot = 'Chiều';
    } else if (hour >= 18 && hour <= 23) {
      _selectedTimeSlot = 'Tối';
    }
  }

  Future<void> _selectTime() async {
    if (_selectedTimeSlot == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Vui lòng chọn khung giờ trước'),
          backgroundColor: Colors.orange,
        ),
      );
      return;
    }

    final TimeOfDay? picked = await showTimePicker(
      context: context,
      initialTime: _selectedTime ?? TimeOfDay.now(),
    );

    if (picked != null) {
      // Validate time is within selected time slot
      final range = _timeSlotRanges[_selectedTimeSlot]!;
      if (picked.hour < range['start']! || picked.hour > range['end']!) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Giờ khởi hành phải trong khung $_selectedTimeSlot (${range['start']}:00 - ${range['end']}:59)'),
              backgroundColor: Colors.red,
            ),
          );
        }
        return;
      }

      setState(() {
        _selectedTime = picked;
      });
    }
  }

  Future<void> _saveTrip() async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    if (_selectedRouteId == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Vui lòng chọn tuyến đường'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    if (_selectedTimeSlot == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Vui lòng chọn khung giờ'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    if (_selectedTime == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Vui lòng chọn giờ khởi hành'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    // Create start time from selected date and time
    final now = DateTime.now();
    final startTime = DateTime(
      now.year,
      now.month,
      now.day,
      _selectedTime!.hour,
      _selectedTime!.minute,
    );

    final adminProvider = Provider.of<AdminProvider>(context, listen: false);
    final success = await adminProvider.updateTrip(
      tripId: widget.trip.tripId,
      routeId: _selectedRouteId!,
      busName: _busName,
      driverName: _driverName,
      startTime: startTime,
      totalSeats: _totalSeats,
    );

    if (success && mounted) {
      Navigator.of(context).pop();
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Cập nhật chuyến xe thành công'),
          backgroundColor: Colors.green,
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Sửa chuyến xe'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: Consumer<AdminProvider>(
        builder: (context, adminProvider, child) {
          return SingleChildScrollView(
            padding: const EdgeInsets.all(16),
            child: Form(
              key: _formKey,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // Route dropdown
                  DropdownButtonFormField<int>(
                    value: _selectedRouteId,
                    decoration: InputDecoration(
                      labelText: 'Tuyến đường *',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      filled: true,
                      fillColor: Colors.grey.shade50,
                      prefixIcon: const Icon(Icons.route),
                    ),
                    items: adminProvider.routes.map((route) {
                      return DropdownMenuItem<int>(
                        value: route.routeId,
                        child: Text(route.displayName),
                      );
                    }).toList(),
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

                  // Time slot dropdown
                  DropdownButtonFormField<String>(
                    value: _selectedTimeSlot,
                    decoration: InputDecoration(
                      labelText: 'Khung giờ *',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      filled: true,
                      fillColor: Colors.grey.shade50,
                      prefixIcon: const Icon(Icons.wb_sunny),
                    ),
                    items: _timeSlotRanges.keys.map((slot) {
                      final range = _timeSlotRanges[slot]!;
                      return DropdownMenuItem<String>(
                        value: slot,
                        child: Text('$slot (${range['start']}:00 - ${range['end']}:59)'),
                      );
                    }).toList(),
                    onChanged: (value) {
                      setState(() {
                        _selectedTimeSlot = value;
                        // Reset time when time slot changes
                        _selectedTime = null;
                      });
                    },
                    validator: (value) {
                      if (value == null) {
                        return 'Vui lòng chọn khung giờ';
                      }
                      return null;
                    },
                  ),
                  const SizedBox(height: 16),

                  // Time picker
                  InkWell(
                    onTap: _selectTime,
                    child: Container(
                      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 16),
                      decoration: BoxDecoration(
                        border: Border.all(color: Colors.grey.shade300),
                        borderRadius: BorderRadius.circular(12),
                        color: Colors.grey.shade50,
                      ),
                      child: Row(
                        children: [
                          Icon(Icons.access_time, color: Colors.blue.shade700),
                          const SizedBox(width: 12),
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Giờ khởi hành *',
                                style: TextStyle(
                                  fontSize: 12,
                                  color: Colors.grey.shade600,
                                ),
                              ),
                              Text(
                                _selectedTime != null
                                    ? _selectedTime!.format(context)
                                    : 'Chọn giờ khởi hành',
                                style: TextStyle(
                                  fontSize: 16,
                                  fontWeight: _selectedTime != null ? FontWeight.w500 : FontWeight.w400,
                                  color: _selectedTime != null ? Colors.black : Colors.grey.shade600,
                                ),
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Bus name (read-only)
                  TextFormField(
                    initialValue: _busName,
                    readOnly: true,
                    decoration: InputDecoration(
                      labelText: 'Tên xe',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      filled: true,
                      fillColor: Colors.grey.shade200,
                      prefixIcon: const Icon(Icons.directions_bus),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Driver name (optional)
                  TextFormField(
                    initialValue: _driverName,
                    decoration: InputDecoration(
                      labelText: 'Tên tài xế (tùy chọn)',
                      hintText: 'Nhập tên tài xế',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      filled: true,
                      fillColor: Colors.grey.shade50,
                      prefixIcon: const Icon(Icons.person),
                    ),
                    onChanged: (value) {
                      _driverName = value.isEmpty ? null : value;
                    },
                  ),
                  const SizedBox(height: 16),

                  // Total seats (read-only)
                  TextFormField(
                    initialValue: _totalSeats.toString(),
                    readOnly: true,
                    decoration: InputDecoration(
                      labelText: 'Số ghế',
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      filled: true,
                      fillColor: Colors.grey.shade200,
                      prefixIcon: const Icon(Icons.event_seat),
                    ),
                  ),
                  const SizedBox(height: 24),

                  // Save button
                  SizedBox(
                    height: 50,
                    child: ElevatedButton(
                      onPressed: adminProvider.isLoading ? null : _saveTrip,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.blue.shade700,
                        foregroundColor: Colors.white,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(12),
                        ),
                      ),
                      child: adminProvider.isLoading
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(
                                strokeWidth: 2,
                                valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                              ),
                            )
                          : const Text(
                              'Lưu thay đổi',
                              style: TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                              ),
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

