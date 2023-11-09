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
parser.add_argument('command', choices=('get_all', 'get_roads'))
parser.add_argument('-c', '--city', help='Name of the city you want to download.')
parser.add_argument('-b', '--boundaries', help='Json file containing points being boundaries.')
parser.add_argument('-p', '--plot', action='store_true', help='Plot network.')
parser.add_argument('-o', '--output', action='store_true', help='Output data.')
parser.add_argument('-s', '--save', help='Save data to file in json format.')


def parse_area(boundaries: str) -> shapely.Polygon:
    with open(boundaries, 'r') as fp:
        points = json.load(fp)

    print(points)
    print(shapely.Polygon(points))
    return shapely.Polygon(points)


def main():
    command = parser.parse_args().command
    if command == 'get_all':
        args = parser.parse_args()
        bounding_box = parse_area(args.boundaries)
        x, y = bounding_box.exterior.xy
        plt.plot(x, y)
        plt.show()
        data = get_all(args.city, bounding_box)
        if args.plot:
            data.plot(figsize=(15, 10))
            plt.show()


if __name__ == '__main__':
    main()
