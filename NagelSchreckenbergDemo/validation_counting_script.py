import pathlib
import re

# Edge: number spawning Vehicle: number
# Vertex: number pedestrians crossing end
# Vertex: number pedestrians crossing start
# Edge: number removing Vehicle: number
# Vehicle: number on Vertex: number going: LEFT|RIGHT|STRAIGHT
# Vehicle: number from Edge: number to Edge: number going: LEFT|RIGHT|STRAIGHT

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
