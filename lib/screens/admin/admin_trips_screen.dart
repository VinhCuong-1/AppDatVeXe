import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../../providers/admin_provider.dart';
import '../../models/models.dart';
import 'add_trip_screen.dart';
import 'edit_trip_screen.dart';

class AdminTripsScreen extends StatefulWidget {
  const AdminTripsScreen({super.key});

  @override
  State<AdminTripsScreen> createState() => _AdminTripsScreenState();
}

class _AdminTripsScreenState extends State<AdminTripsScreen> {
  int? _selectedRouteId;
  String? _selectedTimeSlot;

  // Định nghĩa khung giờ
  final Map<String, Map<String, int>> _timeSlotRanges = {
    'Sáng': {'start': 6, 'end': 11},    // 6:00 - 11:59
    'Chiều': {'start': 12, 'end': 17},  // 12:00 - 17:59
    'Tối': {'start': 18, 'end': 23},    // 18:00 - 23:59
  };

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<AdminProvider>(context, listen: false).loadTodayTrips();
      Provider.of<AdminProvider>(context, listen: false).loadRoutes();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade100,
      appBar: AppBar(
        title: const Text('Quản lý chuyến xe'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
        actions: [
          IconButton(
            onPressed: () {
              Provider.of<AdminProvider>(context, listen: false).loadTodayTrips();
            },
            icon: const Icon(Icons.refresh),
          ),
          IconButton(
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (context) => const AddTripScreen(),
                ),
              );
            },
            icon: const Icon(Icons.add),
          ),
        ],
      ),
      body: Consumer<AdminProvider>(
        builder: (context, adminProvider, child) {
          if (adminProvider.isLoading) {
            return const Center(
              child: CircularProgressIndicator(),
            );
          }

          if (adminProvider.error != null) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.error_outline,
                    size: 64,
                    color: Colors.red.shade300,
                  ),
                  const SizedBox(height: 16),
                  Text(
                    'Có lỗi xảy ra',
                    style: Theme.of(context).textTheme.titleLarge,
                  ),
                  const SizedBox(height: 8),
                  Text(
                    adminProvider.error!,
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: Colors.grey.shade600,
                    ),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 16),
                  ElevatedButton(
                    onPressed: () {
                      adminProvider.loadTodayTrips();
                    },
                    child: const Text('Thử lại'),
                  ),
                ],
              ),
            );
          }

          // Apply filters
          var filteredTrips = adminProvider.todayTrips;

          // Filter by route
          if (_selectedRouteId != null) {
            filteredTrips = filteredTrips.where((trip) {
              return trip.route?.routeId == _selectedRouteId;
            }).toList();
          }

          // Filter by time slot
          if (_selectedTimeSlot != null) {
            final range = _timeSlotRanges[_selectedTimeSlot]!;
            filteredTrips = filteredTrips.where((trip) {
              final hour = trip.startTime.hour;
              return hour >= range['start']! && hour <= range['end']!;
            }).toList();
          }

          if (filteredTrips.isEmpty) {
            return Column(
              children: [
                // Filter section
                _buildFilterSection(adminProvider),
                
                Expanded(
                  child: Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.directions_bus_outlined,
                          size: 64,
                          color: Colors.grey.shade400,
                        ),
                        const SizedBox(height: 16),
                        Text(
                          'Chưa có chuyến xe nào',
                          style: Theme.of(context).textTheme.titleLarge,
                        ),
                        const SizedBox(height: 8),
                        Text(
                          _selectedRouteId != null || _selectedTimeSlot != null
                              ? 'Thay đổi bộ lọc để xem thêm chuyến'
                              : 'Hãy thêm chuyến xe để bắt đầu',
                          style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                            color: Colors.grey.shade600,
                          ),
                          textAlign: TextAlign.center,
                        ),
                        const SizedBox(height: 16),
                        ElevatedButton(
                          onPressed: () {
                            Navigator.of(context).push(
                              MaterialPageRoute(
                                builder: (context) => const AddTripScreen(),
                              ),
                            );
                          },
                          child: const Text('Thêm chuyến xe'),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            );
          }

          return Column(
            children: [
              // Filter section
              _buildFilterSection(adminProvider),
              
              // Trip list
              Expanded(
                child: ListView.builder(
                  padding: const EdgeInsets.all(16),
                  itemCount: filteredTrips.length,
                  itemBuilder: (context, index) {
                    final trip = filteredTrips[index];
                    return TripManagementCard(
                      trip: trip,
                      onEdit: () => _navigateToEditTrip(context, trip),
                      onPause: () => _showPauseDialog(context, trip),
                      onResume: () => _showResumeDialog(context, trip),
                      onDelete: () => _showDeleteDialog(context, trip),
                    );
                  },
                ),
              ),
            ],
          );
        },
      ),
    );
  }

  Widget _buildFilterSection(AdminProvider adminProvider) {
    return Container(
      padding: const EdgeInsets.all(16),
      color: Colors.white,
      child: Column(
        children: [
          // Route Filter
          DropdownButtonFormField<int>(
            value: _selectedRouteId,
            decoration: InputDecoration(
              labelText: 'Lọc theo tuyến đường',
              border: const OutlineInputBorder(),
              isDense: true,
              prefixIcon: const Icon(Icons.route),
              filled: true,
              fillColor: Colors.grey.shade50,
              suffixIcon: _selectedRouteId != null
                  ? IconButton(
                      icon: const Icon(Icons.clear, size: 18),
                      onPressed: () {
                        setState(() {
                          _selectedRouteId = null;
                        });
                      },
                    )
                  : null,
            ),
            items: [
              const DropdownMenuItem<int>(
                value: null,
                child: Text('Tất cả tuyến'),
              ),
              ...adminProvider.routes.map((route) => DropdownMenuItem<int>(
                    value: route.routeId,
                    child: Text(route.displayName),
                  )),
            ],
            onChanged: (value) {
              setState(() {
                _selectedRouteId = value;
              });
            },
          ),
          const SizedBox(height: 12),
          
          // Time Slot Filter
          DropdownButtonFormField<String>(
            value: _selectedTimeSlot,
            decoration: InputDecoration(
              labelText: 'Lọc theo khung giờ',
              border: const OutlineInputBorder(),
              isDense: true,
              prefixIcon: const Icon(Icons.access_time),
              filled: true,
              fillColor: Colors.grey.shade50,
              suffixIcon: _selectedTimeSlot != null
                  ? IconButton(
                      icon: const Icon(Icons.clear, size: 18),
                      onPressed: () {
                        setState(() {
                          _selectedTimeSlot = null;
                        });
                      },
                    )
                  : null,
            ),
            items: [
              const DropdownMenuItem<String>(
                value: null,
                child: Text('Tất cả khung giờ'),
              ),
              ..._timeSlotRanges.keys.map((slot) {
                final range = _timeSlotRanges[slot]!;
                return DropdownMenuItem<String>(
                  value: slot,
                  child: Text('$slot (${range['start']}:00 - ${range['end']}:59)'),
                );
              }),
            ],
            onChanged: (value) {
              setState(() {
                _selectedTimeSlot = value;
              });
            },
          ),
        ],
      ),
    );
  }

  void _navigateToEditTrip(BuildContext context, Trip trip) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (context) => EditTripScreen(trip: trip),
      ),
    ).then((_) {
      // Reload trips after edit (check if still mounted)
      if (mounted) {
        Provider.of<AdminProvider>(context, listen: false).loadTodayTrips();
      }
    });
  }

  void _showPauseDialog(BuildContext context, Trip trip) {
    showDialog(
      context: context,
      builder: (dialogContext) => AlertDialog(
        title: const Text('Tạm ngưng chuyến xe'),
        content: const Text('Bạn có chắc muốn tạm ngưng chuyến xe này? Khách hàng sẽ không thể đặt vé cho chuyến này.'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(dialogContext).pop(),
            child: const Text('Hủy'),
          ),
          ElevatedButton(
            onPressed: () async {
              Navigator.of(dialogContext).pop();
              final adminProvider = Provider.of<AdminProvider>(context, listen: false);
              await adminProvider.pauseTrip(trip.tripId);
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.orange,
            ),
            child: const Text('Tạm ngưng'),
          ),
        ],
      ),
    );
  }

  void _showResumeDialog(BuildContext context, Trip trip) {
    showDialog(
      context: context,
      builder: (dialogContext) => AlertDialog(
        title: const Text('Hủy tạm ngưng'),
        content: const Text('Bạn có chắc muốn hủy tạm ngưng chuyến xe này? Khách hàng sẽ có thể đặt vé trở lại.'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(dialogContext).pop(),
            child: const Text('Hủy'),
          ),
          ElevatedButton(
            onPressed: () async {
              Navigator.of(dialogContext).pop();
              final adminProvider = Provider.of<AdminProvider>(context, listen: false);
              await adminProvider.resumeTrip(trip.tripId);
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.green,
            ),
            child: const Text('Hủy tạm ngưng'),
          ),
        ],
      ),
    );
  }

  void _showDeleteDialog(BuildContext context, Trip trip) {
    showDialog(
      context: context,
      builder: (dialogContext) => AlertDialog(
        title: const Text('Xóa chuyến xe'),
        content: const Text('Bạn có chắc muốn XÓA VĨNH VIỄN chuyến xe này? Hành động này không thể hoàn tác!'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(dialogContext).pop(),
            child: const Text('Hủy'),
          ),
          ElevatedButton(
            onPressed: () async {
              Navigator.of(dialogContext).pop();
              final adminProvider = Provider.of<AdminProvider>(context, listen: false);
              await adminProvider.deleteTrip(trip.tripId);
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.red,
            ),
            child: const Text('Xóa'),
          ),
        ],
      ),
    );
  }
}

class TripManagementCard extends StatelessWidget {
  final Trip trip;
  final VoidCallback onEdit;
  final VoidCallback onPause;
  final VoidCallback onResume;
  final VoidCallback onDelete;

  const TripManagementCard({
    super.key,
    required this.trip,
    required this.onEdit,
    required this.onPause,
    required this.onResume,
    required this.onDelete,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      elevation: 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Header with status badge
            Row(
              children: [
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        trip.busName,
                        style: Theme.of(context).textTheme.titleMedium?.copyWith(
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      if (trip.driverName != null && trip.driverName!.isNotEmpty)
                        Text(
                          'Tài xế: ${trip.driverName}',
                          style: Theme.of(context).textTheme.bodySmall?.copyWith(
                            color: Colors.grey.shade600,
                          ),
                        ),
                    ],
                  ),
                ),
                _buildStatusBadge(context),
              ],
            ),
            const SizedBox(height: 12),
            
            // Route
            if (trip.route != null) ...[
              Row(
                children: [
                  Icon(
                    Icons.location_on,
                    color: Colors.blue.shade700,
                    size: 20,
                  ),
                  const SizedBox(width: 8),
                  Expanded(
                    child: Text(
                      trip.route!.displayName,
                      style: Theme.of(context).textTheme.bodyLarge,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 8),
            ],
            
            // Time (only show hour, as trips repeat daily)
            Row(
              children: [
                Icon(
                  Icons.access_time,
                  color: Colors.orange.shade700,
                  size: 20,
                ),
                const SizedBox(width: 8),
                Text(
                  'Khởi hành: ${DateFormat('HH:mm').format(trip.startTime)}',
                  style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 8),
            
            // Available seats
            Row(
              children: [
                Icon(
                  Icons.event_seat,
                  color: Colors.green.shade700,
                  size: 20,
                ),
                const SizedBox(width: 8),
                Text(
                  'Còn ${trip.availableSeats}/${trip.totalSeats} ghế',
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    color: Colors.green.shade700,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            
            // Action buttons
            Row(
              children: [
                // Edit button
                Expanded(
                  child: OutlinedButton.icon(
                    onPressed: onEdit,
                    icon: const Icon(Icons.edit, size: 18),
                    label: const Text('Sửa'),
                    style: OutlinedButton.styleFrom(
                      foregroundColor: Colors.blue.shade700,
                    ),
                  ),
                ),
                const SizedBox(width: 8),
                
                // Pause/Resume button
                if (trip.isActive)
                  Expanded(
                    child: OutlinedButton.icon(
                      onPressed: onPause,
                      icon: const Icon(Icons.pause, size: 18),
                      label: const Text('Tạm ngưng'),
                      style: OutlinedButton.styleFrom(
                        foregroundColor: Colors.orange.shade700,
                      ),
                    ),
                  )
                else if (trip.isPaused)
                  Expanded(
                    child: ElevatedButton.icon(
                      onPressed: onResume,
                      icon: const Icon(Icons.play_arrow, size: 18),
                      label: const Text('Hoạt động'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.green.shade700,
                        foregroundColor: Colors.white,
                      ),
                    ),
                  ),
                const SizedBox(width: 8),
                
                // Delete button
                IconButton(
                  onPressed: onDelete,
                  icon: const Icon(Icons.delete),
                  color: Colors.red.shade700,
                  tooltip: 'Xóa',
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatusBadge(BuildContext context) {
    Color backgroundColor;
    Color textColor;
    String text;
    IconData icon;

    if (trip.isActive) {
      backgroundColor = Colors.green.shade100;
      textColor = Colors.green.shade700;
      text = 'Hoạt động';
      icon = Icons.check_circle;
    } else if (trip.isPaused) {
      backgroundColor = Colors.orange.shade100;
      textColor = Colors.orange.shade700;
      text = 'Tạm ngưng';
      icon = Icons.pause_circle;
    } else {
      backgroundColor = Colors.red.shade100;
      textColor = Colors.red.shade700;
      text = 'Đã hủy';
      icon = Icons.cancel;
    }

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: backgroundColor,
        borderRadius: BorderRadius.circular(12),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 16, color: textColor),
          const SizedBox(width: 4),
          Text(
            text,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: textColor,
            ),
          ),
        ],
      ),
    );
  }
}
