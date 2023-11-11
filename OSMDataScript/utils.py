from __future__ import annotations

import numpy as np
import pandas as pd
import geopandas as gpd
import pyrosm
import pyrosm
import osmnx
import shapely
from shapely.geometry import box, Polygon


def download_data(city: str, bounding_box: shapely.Polygon) -> pyrosm.OSM:
    fp = pyrosm.get_data(city)
    return pyrosm.OSM(fp, bounding_box)


def get_network(city: str, bounding_box: Polygon) -> gpd.GeoDataFrame:
    return download_data(city, bounding_box).get_network(network_type='all')


def get_roads(city: str, bounding_box: Polygon) -> gpd.GeoDataFrame:
    roads_filters = ['primary', 'secondary', 'tertiary', 'residential']
    column_filters = ['osm_type', 'tags', 'version', 'timestamp', 'width', 'tunnel', 'smoothness', 'access', 'bicycle', 'cycleway', 'motor_vehicle', 'surface', 'lit', 'foot', 'sidewalk', 'service']

    network = download_data(city, bounding_box).get_network(network_type="driving")
    network = network[network['highway'].isin(roads_filters)]
    network = network.drop(column_filters, axis=1)

    return network


def get_walkways(city: str, bounding_box: Polygon) -> gpd.GeoDataFrame:
    footway_filters = ['footway', 'path']
    column_filters = ['osm_type', 'tags', 'version', 'timestamp', 'width', 'tunnel', 'smoothness', 'access', 'bicycle', 'cycleway', 'motor_vehicle', 'maxspeed', 'name', 'surface', 'oneway', 'lit', 'foot', 'lanes', 'sidewalk', 'service', 'segregated', 'footway', 'path']

    network = download_data(city, bounding_box).get_network(network_type="walking")
    network = network[network['highway'].isin(footway_filters)]
    network['crossing'] = (network['footway'] == 'crossing') | (network['path'] == 'crossing')
    network = network.drop(column_filters, axis=1)

    return network


def get_buildings(city: str, bounding_box: Polygon) -> gpd.GeoDataFrame:
    column_filters = ['height', 'source', 'start_date', 'wikipedia', 'timestamp', 'version', 'tags', 'name', 'opening_hours', 'operator', 'ref', 'visible', 'website', 'addr:city', 'addr:street', 'addr:country', 'addr:housenumber', 'addr:postcode', 'addr:housename', 'osm_type', 'building:material', 'building:levels', 'amenity']

    network = download_data(city, bounding_box).get_buildings()
    network = network.drop(column_filters, axis=1)

    return network
