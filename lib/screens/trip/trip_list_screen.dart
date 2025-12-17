import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../../providers/booking_provider.dart';
import '../../models/models.dart';
import '../../models/time_filter.dart';
import '../seat/seat_selection_screen.dart';

class TripListScreen extends StatelessWidget {
  final TimeFilter filter;
  const TripListScreen({super.key, this.filter = TimeFilter.all});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade100,
      appBar: AppBar(
        title: const Text('Danh sách chuyến xe'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: Consumer<BookingProvider>(
        builder: (context, bookingProvider, child) {
          // Debug logging
          print('TripListScreen - isLoading: ${bookingProvider.isLoading}');
          print('TripListScreen - error: ${bookingProvider.error}');
          print('TripListScreen - searchResults count: ${bookingProvider.searchResults.length}');
          print('TripListScreen - filter: $filter');
          
          if (bookingProvider.isLoading) {
            return const Center(
              child: CircularProgressIndicator(),
            );
          }

          if (bookingProvider.error != null) {
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
                    bookingProvider.error!,
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: Colors.grey.shade600,
                    ),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 16),
                  ElevatedButton(
                    onPressed: () {
                      // Retry search
                      Navigator.of(context).pop();
                    },
                    child: const Text('Thử lại'),
                  ),
                ],
              ),
            );
          }

          if (bookingProvider.searchResults.isEmpty) {
            return Center(
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
                    'Không tìm thấy chuyến xe',
                    style: Theme.of(context).textTheme.titleLarge,
                  ),
                  const SizedBox(height: 8),
                  Text(
                    'Vui lòng thử lại với tuyến đường khác',
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: Colors.grey.shade600,
                    ),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 16),
                  ElevatedButton(
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                    child: const Text('Tìm lại'),
                  ),
                ],
              ),
            );
          }

          final trips = bookingProvider.searchResults.where((t) {
            print('Filtering trip: ${t.busName} at ${t.startTime}');
            switch (filter) {
              case TimeFilter.morning:
                final h = t.startTime.hour;
                final isMorning = h >= 5 && h <= 10;
                print('Morning filter: hour=$h, isMorning=$isMorning');
                return isMorning;
              case TimeFilter.afternoon:
                final h = t.startTime.hour;
                final isAfternoon = h >= 11 && h <= 16;
                print('Afternoon filter: hour=$h, isAfternoon=$isAfternoon');
                return isAfternoon;
              case TimeFilter.evening:
                final h = t.startTime.hour;
                final isEvening = h >= 17 && h <= 23;
                print('Evening filter: hour=$h, isEvening=$isEvening');
                return isEvening;
              case TimeFilter.all:
              default:
                print('All filter: returning true');
                return true;
            }
          }).toList();
          
          print('Filtered trips count: ${trips.length}');

          return ListView.builder(
            padding: const EdgeInsets.all(16),
            itemCount: trips.length,
            itemBuilder: (context, index) {
              final trip = trips[index];
              return TripCard(
                trip: trip,
              onTap: () {
                // Check if trip is paused
                if (trip.isPaused) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Chuyến này tạm thời ngưng hoạt động'),
                      backgroundColor: Colors.orange,
                      duration: Duration(seconds: 3),
                    ),
                  );
                  return;
                }
                
                bookingProvider.selectTrip(trip.tripId);
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => const SeatSelectionScreen(),
                  ),
                );
              },
              );
            },
          );
        },
      ),
    );
  }
}

class TripCard extends StatelessWidget {
  final Trip trip;
  final VoidCallback onTap;

  const TripCard({
    super.key,
    required this.trip,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      elevation: 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
      ),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(12),
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Header with bus name and status
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
                          Padding(
                            padding: const EdgeInsets.only(top: 4),
                            child: Text(
                              'Tài xế: ${trip.driverName}',
                              style: Theme.of(context).textTheme.bodySmall?.copyWith(
                                color: Colors.grey.shade600,
                              ),
                            ),
                          ),
                      ],
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 8,
                      vertical: 4,
                    ),
                    decoration: BoxDecoration(
                      color: trip.isPaused 
                          ? Colors.orange.shade100 
                          : (trip.isActive ? Colors.green.shade100 : Colors.red.shade100),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        if (trip.isPaused)
                          Icon(Icons.pause_circle, size: 14, color: Colors.orange.shade700)
                        else if (trip.isActive)
                          Icon(Icons.check_circle, size: 14, color: Colors.green.shade700)
                        else
                          Icon(Icons.cancel, size: 14, color: Colors.red.shade700),
                        const SizedBox(width: 4),
                        Text(
                          trip.isPaused ? 'Tạm ngưng' : (trip.isActive ? 'Hoạt động' : 'Đã hủy'),
                          style: TextStyle(
                            fontSize: 12,
                            fontWeight: FontWeight.w500,
                            color: trip.isPaused 
                                ? Colors.orange.shade700 
                                : (trip.isActive ? Colors.green.shade700 : Colors.red.shade700),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 12),
              
              // Route information
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
                        style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 8),
              ],
              
              // Departure time
              Row(
                children: [
                  Icon(
                    Icons.access_time,
                    color: Colors.orange.shade700,
                    size: 20,
                  ),
                  const SizedBox(width: 8),
                  Text(
                    DateFormat('HH:mm').format(trip.startTime),
                    style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                  const SizedBox(width: 16),
                  Text(
                    DateFormat('dd/MM/yyyy').format(trip.startTime),
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: Colors.grey.shade600,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 12),
              
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
                    'Còn ${trip.availableSeats} ghế trống',
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: Colors.green.shade700,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 16),
              
              // Select button
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: trip.isActive || trip.isPaused ? onTap : null,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: trip.isPaused 
                        ? Colors.orange.shade700
                        : (trip.isActive ? Colors.blue.shade700 : Colors.grey.shade400),
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(8),
                    ),
                  ),
                  child: Text(
                    trip.isPaused 
                        ? 'Tạm ngưng' 
                        : (trip.isActive ? 'Chọn chuyến' : 'Chuyến đã hủy'),
                    style: const TextStyle(
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
