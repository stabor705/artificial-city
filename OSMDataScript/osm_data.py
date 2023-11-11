import sys
import pathlib
import argparse
import matplotlib.pyplot as plt
import shapely
from shapely.geometry import box
import json

from utils import *


parser = argparse.ArgumentParser(
                    prog='osm_data',
                    description='This program collects data using pyosm.'
)
parser.add_argument('command', choices=('get_all', 'get_roads', 'get_walkways'))
parser.add_argument('-c', '--city', help='Name of the city you want to download.')
parser.add_argument('-b', '--boundaries', help='Json file containing points being boundaries.')
parser.add_argument('-p', '--plot', action='store_true', help='Plot network.')
parser.add_argument('-o', '--output', action='store_true', help='Output data.')
parser.add_argument('-s', '--save', help='Save data to file in json format.')


def parse_boundaries(boundaries_filename: str) -> shapely.Polygon:
    with open(boundaries_filename, 'r') as fp:
        points = json.load(fp)
    return shapely.Polygon(points)


def handle_output(data: gpd.GeoDataFrame):
    args = parser.parse_args()
    if args.plot:
        data.plot(figsize=(15, 10))
        plt.show()
    if args.output:
        print(data)
    if args.save:
        with open(args.save, 'w') as fp:
            fp.write(data.to_json(indent=4))


def main():
    command = parser.parse_args().command
    args = parser.parse_args()
    bounding_box = parse_boundaries(args.boundaries)

    if command == 'get_all':
        data = get_all(args.city, bounding_box)
    elif command == 'get_roads':
        data = get_roads(args.city, bounding_box)
    elif command == 'get_walkways':
        data = get_walkways(args.city, bounding_box)
    handle_output(data)


if __name__ == '__main__':
    main()
