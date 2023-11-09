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
    return download_data(city, bounding_box).get_network()


def get_roads(bounding_box: Polygon) -> gpd.GeoDataFrame:
    pass
