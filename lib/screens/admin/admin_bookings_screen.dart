import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../../providers/admin_provider.dart';
import '../../models/models.dart';

class AdminBookingsScreen extends StatefulWidget {
  const AdminBookingsScreen({super.key});

  @override
  State<AdminBookingsScreen> createState() => _AdminBookingsScreenState();
}

class _AdminBookingsScreenState extends State<AdminBookingsScreen> {
  String _selectedFilter = 'all';
  int? _selectedRouteId; // Filter by route
  String? _selectedTimeSlot; // Filter by time slot (Sáng/Chiều/Tối)

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
      Provider.of<AdminProvider>(context, listen: false).loadBookings();
      Provider.of<AdminProvider>(context, listen: false).loadTodayTrips();
      Provider.of<AdminProvider>(context, listen: false).loadRoutes();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade100,
      appBar: AppBar(
        title: const Text('Quản lý vé'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
        actions: [
          IconButton(
            onPressed: () {
              Provider.of<AdminProvider>(context, listen: false).loadBookings();
            },
            icon: const Icon(Icons.refresh),
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

          // Filter bookings
          List<Booking> filteredBookings = adminProvider.bookings;
          
          // Filter by status
          if (_selectedFilter != 'all') {
            filteredBookings = filteredBookings.where((booking) {
              switch (_selectedFilter) {
                case 'reserved':
                  return booking.isReserved;
                case 'checkedin':
                  return booking.isCheckedIn;
                case 'cancelled':
                  return booking.isCancelled;
                default:
                  return true;
              }
            }).toList();
          }

          // Filter by route
          if (_selectedRouteId != null) {
            filteredBookings = filteredBookings.where((booking) {
              return booking.trip?.route?.routeId == _selectedRouteId;
            }).toList();
          }

          // Filter by time slot
          if (_selectedTimeSlot != null) {
            final range = _timeSlotRanges[_selectedTimeSlot]!;
            filteredBookings = filteredBookings.where((booking) {
              if (booking.trip == null) return false;
              final hour = booking.trip!.startTime.hour;
              return hour >= range['start']! && hour <= range['end']!;
            }).toList();
          }

          return Column(
            children: [
              // Filter Section
              Container(
                padding: const EdgeInsets.all(16),
                color: Colors.white,
                child: Column(
                  children: [
                    // Status Filter
                    SingleChildScrollView(
                      scrollDirection: Axis.horizontal,
                      child: Row(
                        children: [
                          _buildFilterChip('all', 'Tất cả'),
                          const SizedBox(width: 8),
                          _buildFilterChip('reserved', 'Đã đặt'),
                          const SizedBox(width: 8),
                          _buildFilterChip('checkedin', 'Đã check-in'),
                          const SizedBox(width: 8),
                          _buildFilterChip('cancelled', 'Đã hủy'),
                        ],
                      ),
                    ),
                    const SizedBox(height: 16),
                    
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
              ),
              
              // Bookings List
              Expanded(
                child: filteredBookings.isEmpty
                    ? Center(
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            Icon(
                              Icons.confirmation_number_outlined,
                              size: 64,
                              color: Colors.grey.shade400,
                            ),
                            const SizedBox(height: 16),
                            Text(
                              'Không có vé nào',
                              style: Theme.of(context).textTheme.titleLarge,
                            ),
                            const SizedBox(height: 8),
                            Text(
                              'Hãy thay đổi bộ lọc để xem vé',
                              style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                                color: Colors.grey.shade600,
                              ),
                              textAlign: TextAlign.center,
                            ),
                          ],
                        ),
                      )
                    : ListView.builder(
                        padding: const EdgeInsets.all(16),
                        itemCount: filteredBookings.length,
                        itemBuilder: (context, index) {
                          final booking = filteredBookings[index];
                          return BookingManagementCard(booking: booking);
                        },
                      ),
              ),
            ],
          );
        },
      ),
    );
  }

  Widget _buildFilterChip(String value, String label) {
    final isSelected = _selectedFilter == value;
    return FilterChip(
      label: Text(label),
      selected: isSelected,
      onSelected: (selected) {
        setState(() {
          _selectedFilter = value;
        });
      },
      selectedColor: Colors.blue.shade100,
      checkmarkColor: Colors.blue.shade700,
    );
  }
}

class BookingManagementCard extends StatelessWidget {
  final Booking booking;

  const BookingManagementCard({
    super.key,
    required this.booking,
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
            // Header
            Row(
              children: [
                Expanded(
                  child: Text(
                    '#${booking.bookingId}',
                    style: Theme.of(context).textTheme.titleMedium?.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                _buildStatusChip(context),
              ],
            ),
            const SizedBox(height: 12),
            
            // Passenger info
            Row(
              children: [
                Icon(
                  Icons.person,
                  color: Colors.blue.shade700,
                  size: 20,
                ),
                const SizedBox(width: 8),
                Expanded(
                  child: Text(
                    booking.holderName,
                    style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 8),
            
            // Phone
            Row(
              children: [
                Icon(
                  Icons.phone,
                  color: Colors.green.shade700,
                  size: 20,
                ),
                const SizedBox(width: 8),
                Text(
                  booking.phone,
                  style: Theme.of(context).textTheme.bodyMedium,
                ),
              ],
            ),
            const SizedBox(height: 8),
            
            // Trip info
            if (booking.trip != null) ...[
              Row(
                children: [
                  Icon(
                    Icons.directions_bus,
                    color: Colors.orange.shade700,
                    size: 20,
                  ),
                  const SizedBox(width: 8),
                  Expanded(
                    child: Text(
                      booking.trip!.busName,
                      style: Theme.of(context).textTheme.bodyMedium,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 8),
              
              if (booking.trip!.route != null) ...[
                Row(
                  children: [
                    Icon(
                      Icons.location_on,
                      color: Colors.purple.shade700,
                      size: 20,
                    ),
                    const SizedBox(width: 8),
                    Expanded(
                      child: Text(
                        booking.trip!.route!.displayName,
                        style: Theme.of(context).textTheme.bodyMedium,
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 8),
              ],
              
              Row(
                children: [
                  Icon(
                    Icons.access_time,
                    color: Colors.indigo.shade700,
                    size: 20,
                  ),
                  const SizedBox(width: 8),
                  Text(
                    DateFormat('HH:mm dd/MM/yyyy').format(booking.trip!.startTime),
                    style: Theme.of(context).textTheme.bodyMedium,
                  ),
                ],
              ),
              const SizedBox(height: 8),
            ],
            
            // Seat info
            Row(
              children: [
                Icon(
                  Icons.event_seat,
                  color: Colors.teal.shade700,
                  size: 20,
                ),
                const SizedBox(width: 8),
                Text(
                  'Ghế ${booking.seatNumber}',
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 8),
            
            // Booking time
            Row(
              children: [
                Icon(
                  Icons.schedule,
                  color: Colors.grey.shade600,
                  size: 20,
                ),
                const SizedBox(width: 8),
                Text(
                  'Đặt lúc: ${DateFormat('HH:mm dd/MM/yyyy').format(booking.bookingTime)}',
                  style: Theme.of(context).textTheme.bodySmall?.copyWith(
                    color: Colors.grey.shade600,
                  ),
                ),
              ],
            ),
            
            // Expiry time if applicable
            if (booking.expiresAt != null && booking.isReserved) ...[
              const SizedBox(height: 4),
              Row(
                children: [
                  Icon(
                    Icons.timer,
                    color: Colors.red.shade600,
                    size: 20,
                  ),
                  const SizedBox(width: 8),
                  Text(
                    'Hết hạn: ${DateFormat('HH:mm dd/MM/yyyy').format(booking.expiresAt!)}',
                    style: Theme.of(context).textTheme.bodySmall?.copyWith(
                      color: Colors.red.shade600,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                ],
              ),
            ],
            
            const SizedBox(height: 12),
            
            // QR Token (for reference)
            Container(
              width: double.infinity,
              padding: const EdgeInsets.all(8),
              decoration: BoxDecoration(
                color: Colors.grey.shade100,
                borderRadius: BorderRadius.circular(8),
              ),
              child: Text(
                'QR Token: ${booking.qrToken}',
                style: Theme.of(context).textTheme.bodySmall?.copyWith(
                  fontFamily: 'monospace',
                  color: Colors.grey.shade700,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatusChip(BuildContext context) {
    Color backgroundColor;
    Color textColor;
    String text;

    if (booking.isCheckedIn) {
      backgroundColor = Colors.green.shade100;
      textColor = Colors.green.shade700;
      text = 'Đã check-in';
    } else if (booking.isCancelled) {
      backgroundColor = Colors.red.shade100;
      textColor = Colors.red.shade700;
      text = 'Đã hủy';
    } else if (booking.isExpired) {
      backgroundColor = Colors.orange.shade100;
      textColor = Colors.orange.shade700;
      text = 'Hết hạn';
    } else {
      backgroundColor = Colors.blue.shade100;
      textColor = Colors.blue.shade700;
      text = 'Đã đặt';
    }

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: backgroundColor,
        borderRadius: BorderRadius.circular(8),
      ),
      child: Text(
        text,
        style: TextStyle(
          fontSize: 12,
          fontWeight: FontWeight.w500,
          color: textColor,
        ),
      ),
    );
  }
}
