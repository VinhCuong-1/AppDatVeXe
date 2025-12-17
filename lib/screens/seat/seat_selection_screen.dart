import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../../providers/booking_provider.dart';
import '../../models/models.dart';
import '../booking/booking_confirmation_screen.dart';

class SeatSelectionScreen extends StatelessWidget {
  const SeatSelectionScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade100,
      appBar: AppBar(
        title: const Text('Chọn ghế'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: Consumer<BookingProvider>(
        builder: (context, bookingProvider, child) {
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
                      Navigator.of(context).pop();
                    },
                    child: const Text('Quay lại'),
                  ),
                ],
              ),
            );
          }

          if (bookingProvider.selectedTrip == null) {
            return const Center(
              child: Text('Không có thông tin chuyến xe'),
            );
          }

          return Column(
            children: [
              // Trip Info Header
              Container(
                width: double.infinity,
                padding: const EdgeInsets.all(16),
                color: Colors.white,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      bookingProvider.selectedTrip!.busName,
                      style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 8),
                    if (bookingProvider.selectedTrip!.route != null)
                      Text(
                        bookingProvider.selectedTrip!.route!.displayName,
                        style: Theme.of(context).textTheme.bodyLarge,
                      ),
                    const SizedBox(height: 4),
                    Text(
                      'Khởi hành: ${DateFormat('HH:mm dd/MM/yyyy').format(bookingProvider.selectedTrip!.startTime)}',
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                        color: Colors.grey.shade600,
                      ),
                    ),
                  ],
                ),
              ),
              
              // Seat Legend
              Container(
                width: double.infinity,
                padding: const EdgeInsets.all(16),
                color: Colors.white,
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: [
                    _buildLegendItem(
                      Colors.green.shade300,
                      'Ghế trống',
                    ),
                    _buildLegendItem(
                      Colors.red.shade300,
                      'Ghế đã đặt',
                    ),
                    _buildLegendItem(
                      Colors.blue.shade300,
                      'Ghế đã chọn',
                    ),
                  ],
                ),
              ),
              
              // Seat Grid
              Expanded(
                child: SingleChildScrollView(
                  padding: const EdgeInsets.all(16),
                  child: Column(
                    children: [
                      // Driver's seat (front)
                      Container(
                        padding: const EdgeInsets.all(16),
                        decoration: BoxDecoration(
                          color: Colors.white,
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: Column(
                          children: [
                            Icon(
                              Icons.directions_bus,
                              size: 32,
                              color: Colors.grey.shade600,
                            ),
                            const SizedBox(height: 8),
                            Text(
                              'Tài xế',
                              style: TextStyle(
                                color: Colors.grey.shade600,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                          ],
                        ),
                      ),
                      
                      const SizedBox(height: 20),
                      
                      // Seat Grid
                      Container(
                        padding: const EdgeInsets.all(16),
                        decoration: BoxDecoration(
                          color: Colors.white,
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: SeatGrid(
                          seats: bookingProvider.availableSeats,
                          selectedSeat: bookingProvider.selectedSeat,
                          onSeatSelected: (seat) {
                            bookingProvider.selectSeat(seat);
                          },
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              
              // Continue Button
              if (bookingProvider.selectedSeat != null)
                Container(
                  width: double.infinity,
                  padding: const EdgeInsets.all(16),
                  color: Colors.white,
                  child: ElevatedButton(
                    onPressed: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(
                          builder: (context) => const BookingConfirmationScreen(),
                        ),
                      );
                    },
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.blue.shade700,
                      foregroundColor: Colors.white,
                      padding: const EdgeInsets.symmetric(vertical: 16),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                    ),
                    child: Text(
                      'Tiếp tục với ghế ${bookingProvider.selectedSeat!.seatNumber}',
                      style: const TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
            ],
          );
        },
      ),
    );
  }

  Widget _buildLegendItem(Color color, String label) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          width: 16,
          height: 16,
          decoration: BoxDecoration(
            color: color,
            borderRadius: BorderRadius.circular(4),
          ),
        ),
        const SizedBox(width: 8),
        Text(
          label,
          style: const TextStyle(fontSize: 12),
        ),
      ],
    );
  }
}

class SeatGrid extends StatelessWidget {
  final List<Seat> seats;
  final Seat? selectedSeat;
  final Function(Seat) onSeatSelected;

  const SeatGrid({
    super.key,
    required this.seats,
    required this.selectedSeat,
    required this.onSeatSelected,
  });

  @override
  Widget build(BuildContext context) {
    // Group seats by row (assuming seat numbers like A1, A2, B1, B2, etc.)
    final Map<String, List<Seat>> seatsByRow = {};
    for (final seat in seats) {
      final row = seat.seatNumber.substring(0, 1);
      seatsByRow.putIfAbsent(row, () => []).add(seat);
    }

    return Column(
      children: seatsByRow.entries.map((entry) {
        final row = entry.key;
        final rowSeats = entry.value;
        
        return Padding(
          padding: const EdgeInsets.symmetric(vertical: 8),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              // Row label
              SizedBox(
                width: 30,
                child: Text(
                  row,
                  style: const TextStyle(
                    fontWeight: FontWeight.bold,
                  ),
                  textAlign: TextAlign.center,
                ),
              ),
              
              // Seats in this row
              ...rowSeats.map((seat) => SeatWidget(
                seat: seat,
                isSelected: selectedSeat?.seatId == seat.seatId,
                onTap: () => onSeatSelected(seat),
              )),
            ],
          ),
        );
      }).toList(),
    );
  }
}

class SeatWidget extends StatelessWidget {
  final Seat seat;
  final bool isSelected;
  final VoidCallback onTap;

  const SeatWidget({
    super.key,
    required this.seat,
    required this.isSelected,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    Color seatColor;
    if (seat.isBooked) {
      seatColor = Colors.red.shade300;
    } else if (isSelected) {
      seatColor = Colors.blue.shade300;
    } else {
      seatColor = Colors.green.shade300;
    }

    return GestureDetector(
      onTap: seat.isBooked ? null : onTap,
      child: Container(
        width: 40,
        height: 40,
        margin: const EdgeInsets.symmetric(horizontal: 4),
        decoration: BoxDecoration(
          color: seatColor,
          borderRadius: BorderRadius.circular(8),
          border: Border.all(
            color: isSelected ? Colors.blue.shade700 : Colors.grey.shade300,
            width: isSelected ? 2 : 1,
          ),
        ),
        child: Center(
          child: Text(
            seat.seatNumber.substring(1), // Remove row letter, show only number
            style: TextStyle(
              fontWeight: FontWeight.bold,
              color: seat.isBooked ? Colors.white : Colors.black87,
            ),
          ),
        ),
      ),
    );
  }
}
