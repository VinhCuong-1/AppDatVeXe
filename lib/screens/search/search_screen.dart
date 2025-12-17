import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../../config/provinces.dart';
import '../../providers/booking_provider.dart';
import '../trip/trip_list_screen.dart';
import '../../models/time_filter.dart';
import '../chatbot/chatbot_screen.dart';

class SearchScreen extends StatefulWidget {
  const SearchScreen({super.key});

  @override
  State<SearchScreen> createState() => _SearchScreenState();
}

class _SearchScreenState extends State<SearchScreen> {
  final _formKey = GlobalKey<FormState>();
  String? _fromProvince;
  String? _toProvince;
  DateTime _selectedDate = DateTime.now();
  bool _isLoading = false;
  TimeFilter _timeFilter = TimeFilter.all;

  @override
  void dispose() {
    super.dispose();
  }

  Future<void> _searchTrips() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _isLoading = true;
    });

    try {
      final bookingProvider = Provider.of<BookingProvider>(context, listen: false);
      await bookingProvider.searchTrips(
        from: _fromProvince!.trim(),
        to: _toProvince!.trim(),
        date: _selectedDate,
        timeFilter: _timeFilter,
      );

      if (mounted) {
        Navigator.of(context).push(
          MaterialPageRoute(
            builder: (context) => TripListScreen(filter: _timeFilter),
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Tìm kiếm thất bại: ${e.toString()}'),
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

  Widget _buildFilterChip(String label, TimeFilter value) {
    final isSelected = _timeFilter == value;
    return ChoiceChip(
      label: Text(label),
      selected: isSelected,
      onSelected: (_) {
        setState(() {
          _timeFilter = value;
        });
      },
      selectedColor: Colors.blue.shade100,
      labelStyle: TextStyle(
        color: isSelected ? Colors.blue.shade800 : Colors.black,
        fontWeight: isSelected ? FontWeight.w600 : FontWeight.w400,
      ),
    );
  }

  Future<void> _selectDate() async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(const Duration(days: 30)),
    );
    if (picked != null && picked != _selectedDate) {
      setState(() {
        _selectedDate = picked;
      });
    }
  }

  void _swapLocations() {
    final temp = _fromProvince;
    setState(() {
      _fromProvince = _toProvince;
      _toProvince = temp;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.blue.shade50,
      appBar: AppBar(
        title: const Text('Tìm chuyến xe'),
        backgroundColor: Colors.blue.shade700,
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SafeArea(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              // Search Form Card
              Card(
                elevation: 4,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
                child: Padding(
                  padding: const EdgeInsets.all(20.0),
                  child: Form(
                    key: _formKey,
                    child: Column(
                      children: [
                        // From Province
                        DropdownButtonFormField<String>(
                          value: _fromProvince,
                          items: vietnamProvinces
                              .map(
                                (p) => DropdownMenuItem<String>(
                                  value: p,
                                  child: Text(p),
                                ),
                              )
                              .toList(),
                          onChanged: (value) {
                            setState(() {
                              _fromProvince = value;
                            });
                          },
                          decoration: InputDecoration(
                            labelText: 'Điểm đi',
                            hintText: 'Chọn điểm đi',
                            prefixIcon: const Icon(Icons.location_on, color: Colors.blue),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                            filled: true,
                            fillColor: Colors.grey.shade50,
                          ),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Vui lòng chọn điểm đi';
                            }
                            if (_toProvince != null && value == _toProvince) {
                              return 'Điểm đi và điểm đến không được trùng nhau';
                            }
                            return null;
                          },
                        ),
                        const SizedBox(height: 16),
                        // Time of day filter
                        Row(
                          children: [
                            _buildFilterChip('Sáng', TimeFilter.morning),
                            const SizedBox(width: 8),
                            _buildFilterChip('Chiều', TimeFilter.afternoon),
                            const SizedBox(width: 8),
                            _buildFilterChip('Tối', TimeFilter.evening),
                          ],
                        ),
                        const SizedBox(height: 16),
                        
                        // Swap Button
                        Center(
                          child: IconButton(
                            onPressed: _swapLocations,
                            icon: Container(
                              padding: const EdgeInsets.all(8),
                              decoration: BoxDecoration(
                                color: Colors.blue.shade100,
                                borderRadius: BorderRadius.circular(20),
                              ),
                              child: Icon(
                                Icons.swap_vert,
                                color: Colors.blue.shade700,
                              ),
                            ),
                          ),
                        ),
                        
                        // To Province
                        DropdownButtonFormField<String>(
                          value: _toProvince,
                          items: vietnamProvinces
                              .map(
                                (p) => DropdownMenuItem<String>(
                                  value: p,
                                  child: Text(p),
                                ),
                              )
                              .toList(),
                          onChanged: (value) {
                            setState(() {
                              _toProvince = value;
                            });
                          },
                          decoration: InputDecoration(
                            labelText: 'Điểm đến',
                            hintText: 'Chọn điểm đến',
                            prefixIcon: const Icon(Icons.location_on, color: Colors.red),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                            filled: true,
                            fillColor: Colors.grey.shade50,
                          ),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Vui lòng chọn điểm đến';
                            }
                            if (_fromProvince != null && value == _fromProvince) {
                              return 'Điểm đi và điểm đến không được trùng nhau';
                            }
                            return null;
                          },
                        ),
                        const SizedBox(height: 16),
                        
                        // Date Selection
                        InkWell(
                          onTap: _selectDate,
                          child: Container(
                            padding: const EdgeInsets.symmetric(
                              horizontal: 16,
                              vertical: 12,
                            ),
                            decoration: BoxDecoration(
                              border: Border.all(color: Colors.grey.shade300),
                              borderRadius: BorderRadius.circular(12),
                              color: Colors.grey.shade50,
                            ),
                            child: Row(
                              children: [
                                Icon(
                                  Icons.calendar_today,
                                  color: Colors.blue.shade700,
                                ),
                                const SizedBox(width: 12),
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      'Ngày khởi hành',
                                      style: TextStyle(
                                        fontSize: 12,
                                        color: Colors.grey.shade600,
                                      ),
                                    ),
                                    Text(
                                      DateFormat('dd/MM/yyyy').format(_selectedDate),
                                      style: const TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w500,
                                      ),
                                    ),
                                  ],
                                ),
                              ],
                            ),
                          ),
                        ),
                        const SizedBox(height: 24),
                        
                        // Search Button
                        SizedBox(
                          width: double.infinity,
                          height: 50,
                          child: ElevatedButton(
                            onPressed: _isLoading ? null : _searchTrips,
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.blue.shade700,
                              foregroundColor: Colors.white,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                            ),
                            child: _isLoading
                                ? const SizedBox(
                                    height: 20,
                                    width: 20,
                                    child: CircularProgressIndicator(
                                      strokeWidth: 2,
                                      valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                                    ),
                                  )
                                : const Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: [
                                      Icon(Icons.search),
                                      SizedBox(width: 8),
                                      Text(
                                        'Tìm chuyến xe',
                                        style: TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ],
                                  ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
              
              const SizedBox(height: 24),
              
              // Popular Routes
              Card(
                elevation: 2,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Tuyến phổ biến',
                        style: Theme.of(context).textTheme.titleMedium?.copyWith(
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      const SizedBox(height: 12),
                      _buildPopularRoute('Hà Nội', 'TP. Hồ Chí Minh'),
                      _buildPopularRoute('Hà Nội', 'Đà Nẵng'),
                      _buildPopularRoute('TP. Hồ Chí Minh', 'Lâm Đồng'),
                      _buildPopularRoute('Hà Nội', 'Hải Phòng'),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => const ChatbotScreen(),
            ),
          );
        },
        backgroundColor: Colors.blue.shade700,
        icon: const Icon(Icons.smart_toy),
        label: const Text('Trợ lý AI'),
      ),
    );
  }

  Widget _buildPopularRoute(String from, String to) {
    return InkWell(
      onTap: () {
        setState(() {
          _fromProvince = from;
          _toProvince = to;
        });
      },
      child: Container(
        padding: const EdgeInsets.symmetric(vertical: 8),
        child: Row(
          children: [
            Icon(
              Icons.directions_bus,
              color: Colors.blue.shade700,
              size: 20,
            ),
            const SizedBox(width: 8),
            Text('$from - $to'),
            const Spacer(),
            Icon(
              Icons.arrow_forward_ios,
              size: 16,
              color: Colors.grey.shade400,
            ),
          ],
        ),
      ),
    );
  }
}
