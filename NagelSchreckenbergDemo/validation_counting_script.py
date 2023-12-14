import pathlib
import re

# Edge: number spawning Vehicle: number
# Vertex: number pedestrians crossing end
# Vertex: number pedestrians crossing start
# Edge: number removing Vehicle: number
# Vehicle: number on Vertex: number going: LEFT|RIGHT|STRAIGHT
# Vehicle: number from Edge: number to Edge: number going: LEFT|RIGHT|STRAIGHT

edges_names_map = {
    "from Edge: 2 to Edge: 4": "Juliusza Lea -> Gramatyka w prawo",
    "from Edge: 2 to Edge: 7": "Juliusza Lea -> Warmijska w lewo",
    "from Edge: 2 to Edge: 8": "Juliusza Lea -> Juliusza Lea (skrzyżowanie Gramatyka w kierunku Kijowskiej)",
    "from Edge: 5 to Edge: 3": "Gramatyka -> Lea w lewo",
    "from Edge: 5 to Edge: 8": "Gramatyka -> Lea w prawo",
    "from Edge: 6 to Edge: 3": "Warmijska -> Lea w prawo",
    "from Edge: 6 to Edge: 9": "Warmijska -> Lea w lewo",
    "from Edge: 10 to Edge: 13": "Lea -> Smoluchowskiego w lewo",
    "from Edge: 10 to Edge: 16": "Lea -> Lea (skrzyżowanie Smoluchowskiego w kierunku Kijowskiej)",
    "from Edge: 12 to Edge: 11": "Smoluchowskiego -> Lea w prawo",
    "from Edge: 12 to Edge: 16": "Smoluchowskiego -> Lea w lewo",
    "from Edge: 18 to Edge: 20": "Lea -> Bydgoska w prawo",
    "from Edge: 18 to Edge: 22": "Lea -> Lea (skrzyżowanie Bydgoska w kierunku Kijowskiej)",
    "from Edge: 24 to Edge: 28": "Lea -> Lea (skrzyżowanie Galla w kierunku Kijowskiej)",

    "from Edge: 29 to Edge: 25": "Lea -> Lea (skrzyżowanie Galla w kierunku Piastowskiej)",
    "from Edge: 26 to Edge: 28": "Galla -> Lea w lewo",
    "from Edge: 26 to Edge: 25": "Galla -> Lea w prawo",
    "from Edge: 23 to Edge: 20": "Lea -> Bydgoska w lewo",
    "from Edge: 23 to Edge: 19": "Lea -> Lea (skrzyżowanie Bydgoska w kierunku Piastowskiej)",
    "from Edge: 21 to Edge: 19": "Bydgoska -> Lea w lewo",
    "from Edge: 21 to Edge: 22": "Bydgoska -> Lea w prawo",
    "from Edge: 17 to Edge: 13": "Lea -> Smoluchowskiego w prawo",
    "from Edge: 17 to Edge: 11": "Lea -> Lea (skrzyżowanie Smoluchowskiego w kierunku Piastowskiej)",
    "from Edge: 9 to Edge: 7": "Lea -> Warmijska w prawo",
    "from Edge: 9 to Edge: 4": "Lea -> Gramatyka w lewo",
    "from Edge: 9 to Edge: 3": "Lea -> Lea (skrzyżowanie Gramatyka w kierunku Piastowskiej)"
}

spawned_vehicles_for_edge = {}
removed_vehicles_for_edge = {}
pedestrians_crossed_for_vertex = {}
vehicle_direction_for_vertex = {}
vehicle_direction_with_edges = {}

all_logs = pathlib.Path('logs.txt').read_text()
spawned_logs = re.findall(r"Vertex: \d+ spawning Vehicle: \d+", all_logs)
removed_logs = re.findall(r"Vertex: \d+ removing Vehicle: \d+", all_logs)
pedestrian_logs = re.findall(r"Vertex: \d+ pedestrians crossing start", all_logs)
direction_logs = re.findall(r"Vehicle: \d+ on Vertex: \d+ going: (?:LEFT|RIGHT|STRAIGHT)", all_logs)
direction_edges_logs = re.findall(r"Vehicle: \d+ from Edge: \d+ to Edge: \d+ going: (?:LEFT|RIGHT|STRAIGHT)", all_logs)

for spawned in spawned_logs:
    edge_id = int(spawned.split('Vertex: ')[-1].split(' ')[0])
    if edge_id in spawned_vehicles_for_edge:
        spawned_vehicles_for_edge[edge_id] += 1
    else:
        spawned_vehicles_for_edge[edge_id] = 1

for removed in removed_logs:
    edge_id = int(removed.split('Vertex: ')[-1].split(' ')[0])
    if edge_id in removed_vehicles_for_edge:
        removed_vehicles_for_edge[edge_id] += 1
    else:
        removed_vehicles_for_edge[edge_id] = 1

for pedestrian in pedestrian_logs:
    vertex_id = int(pedestrian.split('Vertex: ')[-1].split(' ')[0])
    if vertex_id in pedestrians_crossed_for_vertex:
        pedestrians_crossed_for_vertex[vertex_id] += 1
    else:
        pedestrians_crossed_for_vertex[vertex_id] = 1

for direction in direction_logs:
    vertex_id = int(direction.split('Vertex: ')[-1].split(' ')[0])
    vehicle_direction = direction.split('going: ')[-1]

    if vertex_id not in vehicle_direction_for_vertex:
        vehicle_direction_for_vertex[vertex_id] = {'STRAIGHT': 0, 'RIGHT': 0, 'LEFT': 0}

    vehicle_direction_for_vertex[vertex_id][vehicle_direction] += 1

for direction_edge in direction_edges_logs:
    edges, vehicle_direction = ' '.join(direction_edge.split('Vehicle: ')[-1].split(' ')[1:]).split(' going: ')

    if edges not in vehicle_direction_with_edges:
        vehicle_direction_with_edges[edges] = {'STRAIGHT': 0, 'RIGHT': 0, 'LEFT': 0}

    vehicle_direction_with_edges[edges][vehicle_direction] += 1

vehicle_direction_with_edges_mapped_to_roads = {}
for direction_edge in direction_edges_logs:
    edges, vehicle_direction = ' '.join(direction_edge.split('Vehicle: ')[-1].split(' ')[1:]).split(' going: ')

    if edges not in edges_names_map:
        continue

    if edges not in vehicle_direction_with_edges_mapped_to_roads:
        vehicle_direction_with_edges_mapped_to_roads[edges_names_map[edges]] = {'STRAIGHT': 0, 'RIGHT': 0, 'LEFT': 0}

    vehicle_direction_with_edges_mapped_to_roads[edges_names_map[edges]][vehicle_direction] += 1


print('Spawned:')
print(spawned_vehicles_for_edge)

print("Removed:")
print(removed_vehicles_for_edge)

print("Pedestrians:")
print(pedestrians_crossed_for_vertex)

print("Directions:")
print(vehicle_direction_for_vertex)

print("Directions with edges:")
print(vehicle_direction_with_edges)

print("Directions mapped to road names:")
print(vehicle_direction_with_edges_mapped_to_roads)
