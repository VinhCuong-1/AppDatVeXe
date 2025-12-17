import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../providers/admin_provider.dart';
import '../../models/models.dart';
import 'admin_trips_screen.dart';
import 'admin_bookings_screen.dart';
import 'user_management_screen.dart';

class AdminDashboardScreen extends StatefulWidget {
  const AdminDashboardScreen({super.key});

  @override
  State<AdminDashboardScreen> createState() => _AdminDashboardScreenState();
}

class _AdminDashboardScreenState extends State<AdminDashboardScreen> {
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      Provider.of<AdminProvider>(context, listen: false).loadTodayTrips();
      Provider.of<AdminProvider>(context, listen: false).loadBookings();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade100,
      appBar: AppBar(
        title: const Text('Admin Dashboard'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
        actions: [
          IconButton(
            onPressed: () {
              Provider.of<AdminProvider>(context, listen: false).loadTodayTrips();
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

          return SingleChildScrollView(
            padding: const EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Quick Actions
                Text(
                  'Thao tác nhanh',
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 16),
                
                      GridView.count(
                        crossAxisCount: 2,
                        shrinkWrap: true,
                        physics: const NeverScrollableScrollPhysics(),
                        crossAxisSpacing: 16,
                        mainAxisSpacing: 16,
                        childAspectRatio: 1.5,
                        children: [
                          _buildActionCard(
                            context,
                            'Quản lý chuyến',
                            Icons.directions_bus,
                            Colors.green.shade700,
                            () {
                              Navigator.of(context).push(
                                MaterialPageRoute(
                                  builder: (context) => const AdminTripsScreen(),
                                ),
                              );
                            },
                          ),
                          _buildActionCard(
                            context,
                            'Quản lý vé',
                            Icons.confirmation_number,
                            Colors.orange.shade700,
                            () {
                              Navigator.of(context).push(
                                MaterialPageRoute(
                                  builder: (context) => const AdminBookingsScreen(),
                                ),
                              );
                            },
                          ),
                          _buildActionCard(
                            context,
                            'Quản lý người dùng',
                            Icons.people,
                            Colors.purple.shade700,
                            () {
                              Navigator.of(context).push(
                                MaterialPageRoute(
                                  builder: (context) => const UserManagementScreen(),
                                ),
                              );
                            },
                          ),
                        ],
                      ),
                
                const SizedBox(height: 24),
                
                // Today's Statistics
                Text(
                  'Thống kê hôm nay',
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 16),
                
                Row(
                  children: [
                    Expanded(
                      child: _buildStatCard(
                        context,
                        'Tổng chuyến',
                        adminProvider.todayTrips.length.toString(),
                        Icons.directions_bus,
                        Colors.blue.shade700,
                      ),
                    ),
                    const SizedBox(width: 16),
                    Expanded(
                      child: _buildStatCard(
                        context,
                        'Tổng vé',
                        adminProvider.bookings.length.toString(),
                        Icons.confirmation_number,
                        Colors.green.shade700,
                      ),
                    ),
                  ],
                ),
                
                const SizedBox(height: 16),
                
                Row(
                  children: [
                    Expanded(
                      child: _buildStatCard(
                        context,
                        'Đã check-in',
                        adminProvider.bookings
                            .where((b) => b.isCheckedIn)
                            .length
                            .toString(),
                        Icons.check_circle,
                        Colors.orange.shade700,
                      ),
                    ),
                    const SizedBox(width: 16),
                    Expanded(
                      child: _buildStatCard(
                        context,
                        'Chưa check-in',
                        adminProvider.bookings
                            .where((b) => b.isReserved)
                            .length
                            .toString(),
                        Icons.schedule,
                        Colors.red.shade700,
                      ),
                    ),
                  ],
                ),
                
                const SizedBox(height: 24),
                
                // Recent Bookings
                Text(
                  'Vé gần đây',
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 16),
                
                if (adminProvider.bookings.isEmpty)
                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(32),
                      child: Center(
                        child: Column(
                          children: [
                            Icon(
                              Icons.confirmation_number_outlined,
                              size: 64,
                              color: Colors.grey.shade400,
                            ),
                            const SizedBox(height: 16),
                            Text(
                              'Chưa có vé nào',
                              style: Theme.of(context).textTheme.titleMedium,
                            ),
                          ],
                        ),
                      ),
                    ),
                  )
                else
                  ...adminProvider.bookings.take(5).map((booking) =>
                      _buildBookingCard(context, booking)),
              ],
            ),
          );
        },
      ),
    );
  }

  Widget _buildActionCard(
    BuildContext context,
    String title,
    IconData icon,
    Color color,
    VoidCallback onTap,
  ) {
    return Card(
      elevation: 4,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
      ),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(12),
        child: Container(
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(12),
            gradient: LinearGradient(
              colors: [color.withOpacity(0.1), color.withOpacity(0.05)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                icon,
                size: 32,
                color: color,
              ),
              const SizedBox(height: 8),
              Text(
                title,
                style: Theme.of(context).textTheme.titleSmall?.copyWith(
                  fontWeight: FontWeight.bold,
                  color: color,
                  fontSize: 13,
                ),
                textAlign: TextAlign.center,
                maxLines: 2,
                overflow: TextOverflow.ellipsis,
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildStatCard(
    BuildContext context,
    String title,
    String value,
    IconData icon,
    Color color,
  ) {
    return Card(
      elevation: 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            Icon(
              icon,
              size: 32,
              color: color,
            ),
            const SizedBox(height: 8),
            Text(
              value,
              style: Theme.of(context).textTheme.headlineMedium?.copyWith(
                fontWeight: FontWeight.bold,
                color: color,
              ),
            ),
            Text(
              title,
              style: Theme.of(context).textTheme.bodySmall?.copyWith(
                color: Colors.grey.shade600,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildBookingCard(BuildContext context, Booking booking) {
    return Card(
      margin: const EdgeInsets.only(bottom: 8),
      child: ListTile(
        leading: CircleAvatar(
          backgroundColor: booking.isCheckedIn 
              ? Colors.green.shade100 
              : Colors.blue.shade100,
          child: Icon(
            booking.isCheckedIn ? Icons.check : Icons.schedule,
            color: booking.isCheckedIn 
                ? Colors.green.shade700 
                : Colors.blue.shade700,
          ),
        ),
        title: Text(booking.holderName),
        subtitle: Text('Ghế ${booking.seatNumber} - #${booking.bookingId}'),
        trailing: Text(
          booking.isCheckedIn ? 'Đã check-in' : 'Chưa check-in',
          style: TextStyle(
            color: booking.isCheckedIn ? Colors.green.shade700 : Colors.orange.shade700,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }

}
