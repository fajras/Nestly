import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

class SystemManagementScreen extends StatefulWidget {
  const SystemManagementScreen({super.key});

  @override
  State<SystemManagementScreen> createState() => _SystemManagementScreenState();
}

class _SystemManagementScreenState extends State<SystemManagementScreen> {
  List<CategoryRow> roles = [];
  List<CategoryRow> foodTypes = [];
  List<CategoryRow> blogCategories = [];
  List<RecommendationRow> recommendations = [];
  bool loading = true;

  @override
  void initState() {
    super.initState();
    loadAll();
  }

  void openRecommendationDialog({RecommendationRow? item}) async {
    final foodTypes = item == null
        ? await getFoodTypesWithoutRecommendation()
        : await getAllFoodTypes();

    int? selectedFoodType = item?.foodTypeId;
    final weekController = TextEditingController(
      text: item?.weekNumber.toString() ?? "",
    );

    showDialog(
      context: context,
      builder: (_) => AlertDialog(
        title: Text(item == null ? "Dodaj preporuku" : "Uredi preporuku"),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            DropdownButtonFormField<int>(
              value: selectedFoodType,
              hint: const Text("Vrsta namirnice"),
              items: foodTypes.map((f) {
                return DropdownMenuItem(value: f.id, child: Text(f.name));
              }).toList(),
              onChanged: (v) => selectedFoodType = v,
            ),
            const SizedBox(height: 12),
            TextField(
              controller: weekController,
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(labelText: "Sedmica"),
            ),
          ],
        ),
        actions: [
          TextButton(
            child: const Text("Otkaži"),
            onPressed: () => Navigator.pop(context),
          ),
          ElevatedButton(
            child: const Text("Sačuvaj"),
            onPressed: () async {
              final week = int.tryParse(weekController.text);

              if (selectedFoodType == null || week == null) {
                NestlyToast.error(context, "Popuni sva polja");
                return;
              }

              try {
                if (item == null) {
                  await createRecommendation(selectedFoodType!, week);

                  NestlyToast.success(
                    context,
                    "Preporuka uspješno dodana",
                    accentColor: AppColors.seed,
                  );
                } else {
                  await updateRecommendation(item.id, selectedFoodType!, week);

                  NestlyToast.success(
                    context,
                    "Preporuka uspješno ažurirana",
                    accentColor: AppColors.seed,
                  );
                }

                Navigator.pop(context);
                loadAll();
              } catch (e) {
                NestlyToast.error(
                  context,
                  e.toString().replaceFirst("Exception: ", ""),
                );
              }
            },
          ),
        ],
      ),
    );
  }

  Widget buildRecommendationTable() {
    return Expanded(
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  const Text(
                    "Preporuke za ishranu",
                    style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                  ),
                  ElevatedButton(
                    onPressed: () => openRecommendationDialog(),
                    child: const Text("Dodaj"),
                  ),
                ],
              ),
              const SizedBox(height: 16),

              Expanded(
                child: ListView.builder(
                  itemCount: recommendations.length,
                  itemBuilder: (_, i) {
                    final item = recommendations[i];

                    return ListTile(
                      title: Text(item.foodName),
                      subtitle: Text("Week ${item.weekNumber}"),
                      trailing: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          IconButton(
                            icon: const Icon(Icons.edit),
                            onPressed: () =>
                                openRecommendationDialog(item: item),
                          ),
                          IconButton(
                            icon: const Icon(Icons.delete),
                            onPressed: () {
                              showDialog(
                                context: context,
                                builder: (_) => AlertDialog(
                                  title: const Text("Potvrda brisanja"),
                                  content: const Text(
                                    "Da li ste sigurni da želite obrisati ovu preporuku za namirnicu?",
                                  ),
                                  actions: [
                                    TextButton(
                                      child: const Text("Otkaži"),
                                      onPressed: () => Navigator.pop(context),
                                    ),
                                    ElevatedButton(
                                      child: const Text("Obriši"),
                                      onPressed: () async {
                                        Navigator.pop(context);

                                        try {
                                          await deleteRecommendation(item.id);

                                          NestlyToast.success(
                                            context,
                                            "Preporuka uspješno obrisana",
                                            accentColor: AppColors.seed,
                                          );

                                          loadAll();
                                        } catch (e) {
                                          NestlyToast.error(
                                            context,
                                            e.toString().replaceFirst(
                                              "Exception: ",
                                              "",
                                            ),
                                          );
                                        }
                                      },
                                    ),
                                  ],
                                ),
                              );
                            },
                          ),
                        ],
                      ),
                    );
                  },
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> loadAll() async {
    setState(() => loading = true);

    try {
      final r = await ApiClient.get('/api/Role');
      final f = await ApiClient.get('/api/FoodType');
      final b = await ApiClient.get('/api/BlogCategory');
      final rec = await ApiClient.get('/api/MealPlan/Recommendation');

      if (r.statusCode != 200 ||
          f.statusCode != 200 ||
          b.statusCode != 200 ||
          rec.statusCode != 200) {
        throw Exception('Greška pri učitavanju podataka');
      }

      if (!mounted) return;

      final recList = (jsonDecode(rec.body) as List)
          .map((e) => RecommendationRow.fromJson(e))
          .toList();

      setState(() {
        roles = parse(r.body);
        foodTypes = parse(f.body);
        blogCategories = parse(b.body);
        recommendations = recList;
      });
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju podataka');
    } finally {
      if (mounted) setState(() => loading = false);
    }
  }

  List<CategoryRow> parse(String body) {
    final List<dynamic> data = jsonDecode(body);
    return data.map((e) => CategoryRow.fromJson(e)).toList();
  }

  Future<void> delete(String path, int id) async {
    try {
      final res = await ApiClient.delete('$path/$id');

      if (res.statusCode == 204) {
        NestlyToast.success(
          context,
          'Uspješno obrisano',
          accentColor: AppColors.seed,
        );
        loadAll();
        return;
      }

      final message = res.body;

      if (message.contains("System categories")) {
        NestlyToast.error(
          context,
          "Ovo je sistemska kategorija i ne može se obrisati",
        );
        return;
      }

      if (message.contains("used by existing blog posts")) {
        NestlyToast.error(
          context,
          "Kategorija se ne može obrisati jer postoje blog članci koji je koriste",
        );
        return;
      }

      NestlyToast.error(context, "Greška pri brisanju");
    } catch (_) {
      NestlyToast.error(context, "Greška pri brisanju");
    }
  }

  Future<void> save({
    required String path,
    required String name,
    int? id,
  }) async {
    try {
      final body = {"name": name};

      final res = id == null
          ? await ApiClient.post(path, body: body)
          : await ApiClient.patch('$path/$id', body: body);

      if (res.statusCode == 200 || res.statusCode == 201) {
        final rootContext = Navigator.of(context, rootNavigator: true).context;

        Navigator.pop(context);

        NestlyToast.success(
          rootContext,
          'Uspješno sačuvano',
          accentColor: AppColors.seed,
        );

        loadAll();
      }
    } catch (_) {
      NestlyToast.error(
        Navigator.of(context, rootNavigator: true).context,
        'Greška pri spremanju',
      );
    }
  }

  void openDialog({
    required String title,
    required String path,
    CategoryRow? item,
  }) {
    final controller = TextEditingController(text: item?.name);

    showDialog(
      context: context,
      builder: (_) => AlertDialog(
        title: Text(title),
        content: TextField(
          controller: controller,
          decoration: const InputDecoration(labelText: 'Naziv'),
        ),
        actions: [
          TextButton(
            child: const Text('Otkaži'),
            onPressed: () => Navigator.pop(context),
          ),

          ElevatedButton(
            child: const Text('Sačuvaj'),
            onPressed: () {
              final text = controller.text.trim();

              if (text.isEmpty) {
                NestlyToast.error(context, 'Naziv ne može biti prazan');
                return;
              }

              save(path: path, name: text, id: item?.id);
            },
          ),
        ],
      ),
    );
  }

  Widget buildTable({
    required String title,
    required String path,
    required List<CategoryRow> data,
  }) {
    return Expanded(
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    title,
                    style: const TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                    ),
                  ),

                  ElevatedButton(
                    onPressed: () {
                      openDialog(title: 'Dodaj $title', path: path);
                    },
                    child: const Text("Dodaj"),
                  ),
                ],
              ),

              const SizedBox(height: 16),

              Expanded(
                child: ListView.builder(
                  itemCount: data.length,
                  itemBuilder: (_, i) {
                    final item = data[i];
                    final systemItem = isSystemItem(path, item.id);

                    return ListTile(
                      title: Text(item.name),

                      trailing: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          IconButton(
                            icon: const Icon(Icons.edit),
                            onPressed: systemItem
                                ? () {
                                    NestlyToast.error(
                                      context,
                                      "Ovo je sistemska postavka i ne može se uređivati",
                                    );
                                  }
                                : () {
                                    openDialog(
                                      title: 'Uredi $title',
                                      path: path,
                                      item: item,
                                    );
                                  },
                          ),

                          IconButton(
                            icon: const Icon(Icons.delete),
                            onPressed: systemItem
                                ? () {
                                    NestlyToast.error(
                                      context,
                                      "Ovo je sistemska postavka i ne može se brisati",
                                    );
                                  }
                                : () {
                                    showDialog(
                                      context: context,
                                      builder: (_) => AlertDialog(
                                        title: const Text("Potvrda brisanja"),
                                        content: const Text(
                                          "Da li ste sigurni da želite obrisati ovaj zapis?",
                                        ),
                                        actions: [
                                          TextButton(
                                            child: const Text("Otkaži"),
                                            onPressed: () =>
                                                Navigator.pop(context),
                                          ),
                                          ElevatedButton(
                                            child: const Text("Obriši"),
                                            onPressed: () {
                                              Navigator.pop(context);
                                              delete(path, item.id);
                                            },
                                          ),
                                        ],
                                      ),
                                    );
                                  },
                          ),
                        ],
                      ),
                    );
                  },
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (loading) {
      return const Center(child: CircularProgressIndicator());
    }

    return Padding(
      padding: const EdgeInsets.all(24),
      child: Column(
        children: [
          const Align(
            alignment: Alignment.centerLeft,
            child: Text(
              "Upravljanje sistemom",
              style: TextStyle(fontSize: 26, fontWeight: FontWeight.bold),
            ),
          ),

          const SizedBox(height: 24),

          Expanded(
            child: Row(
              children: [
                buildTable(title: "Uloge", path: "/api/role", data: roles),

                const SizedBox(width: 16),

                buildTable(
                  title: "Vrste namjernica",
                  path: "/api/foodtype",
                  data: foodTypes,
                ),

                const SizedBox(width: 16),

                buildTable(
                  title: "Kategorije blogova",
                  path: "/api/blogcategory",
                  data: blogCategories,
                ),

                const SizedBox(width: 16),

                buildRecommendationTable(),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class CategoryRow {
  final int id;
  final String name;

  CategoryRow({required this.id, required this.name});

  factory CategoryRow.fromJson(Map<String, dynamic> json) {
    return CategoryRow(id: json['id'], name: json['name']);
  }
}

Future<List<CategoryRow>> getFoodTypesWithoutRecommendation() async {
  try {
    final res = await ApiClient.get(
      '/api/MealPlan/Recommendation/AvailableFoodTypes',
    );

    if (res.statusCode != 200) {
      throw Exception(
        "Failed to load available food types for recommendations.",
      );
    }

    final List data = jsonDecode(res.body);

    return data.map((e) => CategoryRow.fromJson(e)).toList();
  } catch (e) {
    throw Exception(
      "Unable to retrieve available food types for recommendations.",
    );
  }
}

Future<void> createRecommendation(int foodTypeId, int weekNumber) async {
  try {
    final body = {"foodTypeId": foodTypeId, "weekNumber": weekNumber};

    final res = await ApiClient.post(
      '/api/MealPlan/Recommendation',
      body: body,
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception("Failed to create a new meal recommendation.");
    }
  } catch (e) {
    throw Exception(
      "An error occurred while creating the meal recommendation.",
    );
  }
}

bool isSystemItem(String path, int id) {
  if (path == "/api/role") {
    return id == 1 || id == 2;
  }

  if (path == "/api/blogcategory") {
    return id <= 6;
  }

  return false;
}

class RecommendationRow {
  final int id;
  final int foodTypeId;
  final int weekNumber;
  final String foodName;

  RecommendationRow({
    required this.id,
    required this.foodTypeId,
    required this.weekNumber,
    required this.foodName,
  });

  factory RecommendationRow.fromJson(Map<String, dynamic> json) {
    return RecommendationRow(
      id: json['id'],
      foodTypeId: json['foodTypeId'],
      weekNumber: json['weekNumber'],
      foodName: json['foodName'],
    );
  }
}

Future<List<RecommendationRow>> getRecommendations() async {
  try {
    final res = await ApiClient.get('/api/MealPlan/Recommendation');

    if (res.statusCode != 200) {
      throw Exception("Failed to load meal recommendations.");
    }

    final List data = jsonDecode(res.body);

    return data.map((e) => RecommendationRow.fromJson(e)).toList();
  } catch (e) {
    throw Exception("Unable to retrieve meal recommendations from the server.");
  }
}

Future<void> deleteRecommendation(int id) async {
  try {
    final res = await ApiClient.delete('/api/MealPlan/Recommendation/$id');

    if (res.statusCode != 204) {
      throw Exception("Failed to delete the meal recommendation.");
    }
  } catch (e) {
    throw Exception(
      "An error occurred while deleting the meal recommendation.",
    );
  }
}

Future<void> updateRecommendation(
  int id,
  int foodTypeId,
  int weekNumber,
) async {
  try {
    final body = {"foodTypeId": foodTypeId, "weekNumber": weekNumber};

    final res = await ApiClient.patch(
      '/api/MealPlan/Recommendation/$id',
      body: body,
    );

    if (res.statusCode != 200) {
      throw Exception("Failed to update the meal recommendation.");
    }
  } catch (e) {
    throw Exception(
      "An error occurred while updating the meal recommendation.",
    );
  }
}

Future<List<CategoryRow>> getAllFoodTypes() async {
  try {
    final res = await ApiClient.get('/api/FoodType');

    if (res.statusCode != 200) {
      throw Exception("Failed to load food types.");
    }

    final List data = jsonDecode(res.body);

    return data.map((e) => CategoryRow.fromJson(e)).toList();
  } catch (e) {
    throw Exception("Unable to retrieve food types from the server.");
  }
}
