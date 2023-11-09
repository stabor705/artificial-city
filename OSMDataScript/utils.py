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


def get_all(city: str, bounding_box: Polygon) -> gpd.GeoDataFrame:
    return download_data(city, bounding_box).get_network(network_type='all')


def get_roads(city: str, bounding_box: Polygon) -> gpd.GeoDataFrame:
    return download_data(city, bounding_box).get_network(network_type="driving")


def get_walkways(city: str, bounding_box: Polygon):
    filters = ['foot']
    return download_data(city, bounding_box).get_network(network_type="walking", extra_attributes=filters)
